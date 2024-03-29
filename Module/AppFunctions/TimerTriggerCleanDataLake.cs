using Bygdrift.Warehouse;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Module.AppFunctions
{
    public class TimerTriggerCleanDataLake
    {
        public TimerTriggerCleanDataLake(ILogger<TimerTriggerCleanDataLake> logger) => App = new AppBase<Settings>(logger);

        public AppBase<Settings> App { get; }

        [FunctionName(nameof(TimerTriggerCleanDataLake))]
        public async Task Run([TimerTrigger("0 0 0 * * 0")] TimerInfo myTimer)
        {
            var days = Convert.ToInt32((DateTime.Now - DateTime.Now.AddMonths(-App.Settings.MonthsToKeepDataInDataLake)).TotalDays);
            await App.DataLake.DeleteDirectoriesOlderThanDaysAsync("Refined", days);
        }
    }
}