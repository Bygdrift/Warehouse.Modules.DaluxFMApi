using System;
using System.Linq;
using System.Threading.Tasks;
using Bygdrift.Warehouse;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Options;
using Microsoft.Extensions.Logging;
using Module.AppFunctions.Models;

namespace Module.AppFunctions
{
    public class TimerTrigger
    {
        public TimerTrigger(ILogger<TimerTrigger> logger, IDurableClientFactory clientFactory)
        {
            if (App == null)
                App = new AppBase<Settings>(logger);

            if (Client == null)
                Client = clientFactory.CreateClient(new DurableClientOptions { TaskHub = App.ModuleName });
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

            var dataRoot = new DataRoot(App, App.Settings.DaluxFMDataToFetch);

            for (int i = 0; i <= App.Settings.MaxRuns; i++)
            {
                if (dataRoot.AllLoaded)
                    break;

                dataRoot = await context.CallActivityAsync<DataRoot>(nameof(HandleDataAsync), dataRoot);
            }

            if (!dataRoot.AllLoaded)
            {
                var finished = string.Join(',', dataRoot.Collections.Where(o => o.IsLoaded).Select(o => o.Name));
                var missing = string.Join(',', dataRoot.Collections.Where(o => !o.IsLoaded).Select(o => o.Name));
                App.Log.LogError("The orchestrator did not finish the run with the preestimated {Runs} runs. Has finished: {Finished}. Missing: {Missing}", App.Settings.MaxRuns, finished, missing);
            }

            App.Log.LogInformation($"Finished reading in {context.CurrentUtcDateTime.Subtract(App.LoadedUtc).TotalSeconds / 60} minutes.");
        }

        [FunctionName(nameof(HandleDataAsync))]
        public async Task<DataRoot> HandleDataAsync([ActivityTrigger] IDurableActivityContext context)
        {
            if (!Basic.IsQualifiedInstance(App, context.InstanceId)) return default;
            var dataRoot = context.GetInput<DataRoot>();
            return await dataRoot.RunAsync();
        }
    }
}