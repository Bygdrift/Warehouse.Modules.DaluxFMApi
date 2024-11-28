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
using Module.Refines;
using Bygdrift.Tools.DataLakeTool;
using Bygdrift.Tools.CsvTool;
using Bygdrift.Tools.LogTool.Models;

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

        [FunctionName(nameof(InititateOrchestrator))]
        public async Task InititateOrchestrator([TimerTrigger("%ScheduleExpression%"
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
            WriteStatusToSQL(context.InstanceId, calls, false);

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
            WriteStatusToSQL(context.InstanceId, calls, true);
        }

        [FunctionName(nameof(HandleDataAsync))]
        public async Task<List<Call>> HandleDataAsync([ActivityTrigger] IDurableActivityContext context)
        {
            if (!Basic.IsQualifiedInstance(App, context.InstanceId)) return default;
            var calls = context.GetInput<List<Call>>();
            var service = new WebService(App);

            var started = DateTime.Now;
            var apiName = new ApiName();
            foreach (var call in calls.Where(o => !o.IsLoaded))
            {
                if (apiName.Compare<Asset>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Asset>(service, call)).IsLoaded;
                else if (apiName.Compare<AssetClassification>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<AssetClassification>(service, call)).IsLoaded;
                else if (apiName.Compare<Building>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Building>(service, call)).IsLoaded;
                else if (apiName.Compare<Company>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Company>(service, call)).IsLoaded;
                else if (apiName.Compare<Document>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Document>(service, call)).IsLoaded;
                else if (apiName.Compare<Estate>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Estate>(service, call)).IsLoaded;
                else if (apiName.Compare<Floor>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Floor>(service, call)).IsLoaded;
                else if (apiName.Compare<Location>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Location>(service, call)).IsLoaded;
                else if (apiName.Compare<Lot>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Lot>(service, call)).IsLoaded;
                else if (apiName.Compare<Room>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Room>(service, call)).IsLoaded;
                else if (apiName.Compare<Ticket>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<Ticket>(service, call)).IsLoaded;
                else if (apiName.Compare<WorkOrder>(call.Name)) call.IsLoaded = (await GetAndRefineAsync<WorkOrder>(service, call)).IsLoaded;
                else
                {
                    App.Log.LogError($"In the appSetting 'DaluxFMDataToFetch', there are used an unknown name: '{call.Name}'.");
                    call.IsLoaded = true;
                }
                if (started.AddMinutes(6) < DateTime.Now)
                    break;
            }
            WriteStatusToSQL(context.InstanceId, calls, false);
            return calls;
        }

        private static async Task<Call> GetAndRefineAsync<T>(WebService service, Call call) where T : class
        {
            var multipleRuns = call.NextBookMark != null ? "Call is split over multiple runs. Bookmark: " + call.NextBookMark + "." : "";
            App.Log.LogInformation($"- Loading '{typeof(T).Name}' data from API. {multipleRuns}");
            var res = await service.GetDataAsync<T>(2, call.NextBookMark ?? 0);
            await GenericRefine.RefineAsync(App, res.Items, call.NextBookMark == null);  //Truncate table when it's the first time data gets loaded at this durableCall
            var fileName = $"{typeof(T).Name}_{DateTime.UtcNow.ToString("HH-mm-ss")}.json";
            await App.DataLake.SaveObjectAsync(res.ItemsRaw, "Raw", fileName, FolderStructure.DatePath);
            call.NextBookMark = res.NextBookmark;
            if (res.NextBookmark == null)
                call.IsLoaded = true;

            return call;
        }

        private static IEnumerable<Call> InitCalls(string itemsToFetch)
        {
            foreach (var name in itemsToFetch.ToLower().Replace(" ", string.Empty).Split(','))
                yield return new Call(name);
        }

        private static void WriteStatusToSQL(string instanceId, IEnumerable<Call> calls, bool finished)
        {
            var time = DateTime.UtcNow;
            var callsMissing = calls != null ? string.Join(',', calls.Where(o => !o.IsLoaded).Select(o => o.Name)) : null;
            var callsFinished = calls != null ? string.Join(',', calls.Where(o => o.IsLoaded).Select(o => o.Name)) : null;

            var csv = new Csv().AddRecord(1, "InstanceId", instanceId)
                .AddRecord(1, "Status", finished ? "Finished" : "Is working")
                .AddRecord(1, "Started", App.LoadedUtc.ToString("s"))
                .AddRecord(1, "Time", time.ToString("s"))
                .AddRecord(1, "ElapsedMinutes", time.Subtract(App.LoadedUtc).TotalSeconds / 60)
                .AddRecord(1, "Log", string.Join('\n', App.Log.GetLogs(LogType.Information)))
                .AddRecord(1, "LogErrors", string.Join('\n', App.Log.GetErrorsAndCriticals()))
                .AddRecord(1, "CallsMissing", callsMissing)
                .AddRecord(1, "CallsFinished", callsFinished);

            App.Mssql.MergeCsv(csv, "ImportLog", "InstanceId", false, false);
        }
    }
}