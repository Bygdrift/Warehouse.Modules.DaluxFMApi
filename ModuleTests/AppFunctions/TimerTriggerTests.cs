using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module.AppFunctions;
using Moq;
using System.Threading.Tasks;

namespace ModuleTests.AppFunctions
{
    [TestClass]
    public class TimerTriggerTests
    {
        private readonly Mock<IDurableOrchestrationContext> contextMock = new();
        private readonly Mock<ILogger<TimerTrigger>> loggerMock = new();
        private readonly Mock<IDurableClientFactory> clientFactory = new();
        private readonly string instanceId = "testId";
        private readonly TimerTrigger timerTrigger;

        public TimerTriggerTests()
        {
            contextMock.Setup(o => o.InstanceId).Returns(instanceId);
            timerTrigger = new TimerTrigger(loggerMock.Object, clientFactory.Object);
            TimerTrigger.App.Config["QualifiedInstanceId"] = instanceId;
        }

        [TestMethod]
        public async Task CallOrchestrator()
        {
            await timerTrigger.RunOrchestrator(contextMock.Object);
        }
    }
}
