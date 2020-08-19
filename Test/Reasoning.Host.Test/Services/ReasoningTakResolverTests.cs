using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Reasoning.Core.Contracts;
using Reasoning.Core.Models;
using Reasoning.Core.Services;
using Reasoning.Host.Repositories;
using Reasoning.Host.Resources;
using Reasoning.Host.Services;
using Reasoning.Host.Test.Mocks;
using Reasoning.MongoDb.Repositories;

namespace Reasoning.Host.Test.Services
{
    [TestClass]
    public class ReasoningTakResolverTests
    {
        [TestMethod]
        public void ProcessReasoningTask_AllFacts()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.WAITING);

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));
            var reasoningService = new ReasoningService();

            var service = MockReasoningTaskResolver(reasoningTaskRepository.Object, reasoningService);

            var result = service.ProcessReasoningTask("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.FINISHED, result.Status);
            Assert.IsTrue(result.ReasoningProcess.ReasonedItems.Count == 1);
        }

        [TestMethod]
        public void ProcessReasoningTask_PartialFacts_APIRespond()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.WAITING);
            reasoningTask.ReasoningProcess.KnowledgeBase.RuleSet[0].Predicates[0].LeftTerm.Value = null;
            reasoningTask.ReasoningProcess.KnowledgeBase.RuleSet[1].Predicates[0].LeftTerm.Value = null;
            reasoningTask.Sources = new List<IVariableSource>
            {
                new VariableSource
                {
                    VariableIds = new List<string> { "var1" },
                    Request = new ReasoningRequest
                    {
                        Uri = "http://localhost:8080",
                        Method = ReasoningRequestMethod.GET
                    }
                }
            };

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService.Setup(x => x.ExecuteTaskAsync<VariablesResource>(It.IsAny<IReasoningRequest>()))
                .Returns(Task.FromResult(new VariablesResource
                {
                    Variables = new List<IVariable>
                    {
                        new Variable("var1", 3)
                    }
                }));
            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));
            var reasoningService = new ReasoningService();

            var service = MockReasoningTaskResolver(reasoningTaskRepository.Object, reasoningService, null, httpClientService.Object);

            var result = service.ProcessReasoningTask("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.FINISHED, result.Status);
            Assert.IsTrue(result.ReasoningProcess.ReasonedItems.Count == 1);
        }

        [TestMethod]
        public void ProcessReasoningTask_PartialFacts_APIDoesNotRespond()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.WAITING);
            reasoningTask.ReasoningProcess.KnowledgeBase.RuleSet[0].Predicates[0].LeftTerm.Value = null;
            reasoningTask.ReasoningProcess.KnowledgeBase.RuleSet[1].Predicates[0].LeftTerm.Value = null;
            reasoningTask.Sources = new List<IVariableSource>
            {
                new VariableSource
                {
                    VariableIds = new List<string> { "var1" },
                    Request = new ReasoningRequest
                    {
                        Uri = "http://localhost:8080",
                        Method = ReasoningRequestMethod.GET
                    }
                }
            };

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));
            var reasoningService = new ReasoningService();

            var service = MockReasoningTaskResolver(reasoningTaskRepository.Object, reasoningService);

            var result = service.ProcessReasoningTask("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.STOPPED, result.Status);
            Assert.IsTrue(result.ReasoningProcess.ReasonedItems.Count == 0);
        }

        [TestMethod]
        public void ProcessReasoningTask_PartialFacts_APISendsRequestOnEnd()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.WAITING);
            reasoningTask.Actions = new List<IReasoningAction>
            {
                new ReasoningAction
                {
                    ReasoningItems = new List<IVariable>
                    {
                        new Variable("conclusion1", 1)
                    },
                    ReasoningRequests = new List<IReasoningRequest>
                    {
                        new ReasoningRequest
                        {
                            Uri = "http://localhost:8080",
                            Method = ReasoningRequestMethod.POST
                        }
                    }
                }
            };

            var httpClientService = new Mock<IHttpClientService>();
            httpClientService.Setup(x => x.ExecuteTaskAsync(It.IsAny<IReasoningRequest>()))
                .Returns(Task.CompletedTask);
            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));
            var reasoningService = new ReasoningService();

            var service = MockReasoningTaskResolver(reasoningTaskRepository.Object, reasoningService, null, httpClientService.Object);

            var result = service.ProcessReasoningTask("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.FINISHED, result.Status);
            Assert.IsTrue(result.ReasoningProcess.ReasonedItems.Count == 1);
        }

        [TestMethod]
        public void ProcessReasoningTask_PartialFacts_APISendsFailingRequestOnEnd()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.WAITING);
            reasoningTask.Actions = new List<IReasoningAction>
            {
                new ReasoningAction
                {
                    ReasoningItems = new List<IVariable>
                    {
                        new Variable("conclusion1", 1)
                    },
                    ReasoningRequests = new List<IReasoningRequest>
                    {
                        new ReasoningRequest
                        {
                            Uri = "http://localhost:8080",
                            Method = ReasoningRequestMethod.POST
                        }
                    }
                }
            };

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));
            var reasoningService = new ReasoningService();

            var service = MockReasoningTaskResolver(reasoningTaskRepository.Object, reasoningService);

            var result = service.ProcessReasoningTask("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.FINISHED, result.Status);
            Assert.IsTrue(result.ReasoningProcess.ReasonedItems.Count == 1);
        }

        private IReasoningTaskResolver MockReasoningTaskResolver(
            IReasoningTaskRepository reasoningTaskRepository = null,
            IReasoningService reasoningService = null,
            IBackgroundTaskQueue backgroundTaskQueue = null,
            IHttpClientService httpClientService = null
        )
        {
            return new ReasoningTaskResolver(
                reasoningTaskRepository ?? Mock.Of<IReasoningTaskRepository>(),
                reasoningService ?? Mock.Of<IReasoningService>(),
                backgroundTaskQueue ?? Mock.Of<IBackgroundTaskQueue>(),
                httpClientService ?? Mock.Of<IHttpClientService>(),
                Mock.Of<ILogger<ReasoningTaskResolver>>()
                );
        }
    }
}
