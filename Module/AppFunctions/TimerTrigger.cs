using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bygdrift.Warehouse;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Options;
using Microsoft.Extensions.Logging;
using Module.Services;
using Module.Services.Models.Helpers;
using Module.Services.Models;
using Bygdrift.DataLakeTools;
using Module.Refines;
using System.Threading;

namespace Module.AppFunctions
{
    public class TimerTrigger
    {
        public TimerTrigger(ILogger<TimerTrigger> logger, IDurableClientFactory clientFactory)
        {
            App ??= new AppBase<Settings>(logger);
            Client ??= clientFactory.CreateClient(new DurableClientOptions { TaskHub = App.ModuleName });
        }

        public static AppBase<Settings> App { get; private set; }
        public static IDurableClient Client { get; private set; }

        [FunctionName(nameof(Starter))]
        public async Task Starter([TimerTrigger("%ScheduleExpression%"
#if DEBUG
            ,RunOnStartup = true
#endif
            )] TimerInfo timerInfo)
        {
            if (await Basic.IsRunning(App, Client)) return;
            App.LoadedUtc = DateTime.UtcNow;
            App.Config["QualifiedInstanceId"] = await Client.StartNewAsync(nameof(RunOrchestrator));
        }

        [FunctionName(nameof(RunOrchestrator))]
        public async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            if (!Basic.IsQualifiedInstance(App, context.InstanceId)) return;
            if (!context.IsReplaying)
                App.Log.LogInformation("Has started instance {Instance}", context.InstanceId);

            var calls = InitCalls(App.Settings.DaluxFMDataToFetch);
            for (int i = 0; i <= App.Settings.MaxRuns; i++)
            {
                if (calls == null || calls.All(o => o.IsLoaded))
                    break;

                calls = await context.CallActivityAsync<List<Call>>(nameof(HandleDataAsync), calls);
            }

            if (calls != null && calls.All(o => !o.IsLoaded))
            {
                var finished = string.Join(',', calls.Where(o => o.IsLoaded).Select(o => o.Name));
                var missing = string.Join(',', calls.Where(o => !o.IsLoaded).Select(o => o.Name));
                App.Log.LogError("The orchestrator did not finish the run with the preestimated {Runs} runs. Has finished: {Finished}. Missing: {Missing}", App.Settings.MaxRuns, finished, missing);
            }

            App.Log.LogInformation($"Finished reading in {context.CurrentUtcDateTime.Subtract(App.LoadedUtc).TotalSeconds / 60} minutes.");
        }

        [FunctionName(nameof(HandleDataAsync))]
        public async Task<List<Call>> HandleDataAsync([ActivityTrigger] IDurableActivityContext context)
        {
            if (!Basic.IsQualifiedInstance(App, context.InstanceId)) return default;
            var calls = context.GetInput<List<Call>>();
            var service = new WebService(App);
            return await RunAsync(App, service, calls);
        }

        private static async Task<List<Call>> RunAsync(AppBase<Settings> app, WebService service, List<Call> calls)
        {
            var started = DateTime.Now;
            var apiName = new ApiName();
            foreach (var call in calls.Where(o => !o.IsLoaded))
            {
                if (apiName.Compare<Asset>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<Asset>(app, service, call)).IsLoaded;
                else if (apiName.Compare<AssetClassification>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<AssetClassification>(app, service, call)).IsLoaded;
                else if (apiName.Compare<Building>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<Building>(app, service, call)).IsLoaded;
                else if (apiName.Compare<Document>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<Document>(app, service, call)).IsLoaded;
                else if (apiName.Compare<Estate>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<Estate>(app, service, call)).IsLoaded;
                else if (apiName.Compare<Location>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<Location>(app, service, call)).IsLoaded;
                else if (apiName.Compare<Lot>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<Lot>(app, service, call)).IsLoaded;
                else if (apiName.Compare<Room>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<Room>(app, service, call)).IsLoaded;
                else if (apiName.Compare<WorkOrder>(call.Name)) call.IsLoaded = (await GetAndRefinAsync<WorkOrder>(app, service, call)).IsLoaded;
                else
                {
                    app.Log.LogError($"In the appSetting 'DaluxFMDataToFetch', there are used an unknown name: '{call.Name}'.");
                    call.IsLoaded = true;
                }
                if (started.AddMinutes(6) < DateTime.Now)
                    break;
            }
            return calls;
        }

        private static async Task<Call> GetAndRefinAsync<T>(AppBase<Settings> app, WebService service, Call call) where T : class
        {
            var multipleRuns = call.NextBookMark != null ? "Call is split over multiple runs. Bookmark: " + call.NextBookMark + "." : "";
            app.Log.LogInformation($"- Loading '{typeof(T).Name}' data from API. {multipleRuns}");
            var res = await service.GetDataAsync<T>(2, call.NextBookMark ?? 0);
            await GenericRefine.RefineAsync(app, res.Items, call.NextBookMark == null);  //Truncate table when it's the first time data gets loaded at this durableCall
            var fileName = $"{typeof(T).Name}_{DateTime.UtcNow.ToString("HH-mm-ss")}.json";
            await app.DataLake.SaveObjectAsync(res.Items, "Raw", fileName, FolderStructure.DatePath);
            call.NextBookMark = res.NextBookMark;
            if (res.NextBookMark == null)
                call.IsLoaded = true;

            return call;
        }

        private static IEnumerable<Call> InitCalls(string itemsToFetch)
        {
            foreach (var name in itemsToFetch.ToLower().Replace(" ", string.Empty).Split(','))
                yield return new Call(name);
        }
    }
}