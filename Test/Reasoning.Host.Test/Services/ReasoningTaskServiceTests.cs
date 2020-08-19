using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;

using Reasoning.Core.Contracts;
using Reasoning.Core.Services;
using Reasoning.Host.Resources;
using Reasoning.Host.Services;
using Reasoning.Host.Test.Mocks;
using Reasoning.MongoDb.Repositories;

namespace Reasoning.Host.Test.Services
{
    [TestClass]
    public class ReasoningTaskServiceTests
    {
        [TestMethod]
        public void CancelTaskAsync_ExistingTask()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.WAITING);
            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));

            var service = MockReasoningTaskService(reasoningTaskRepository.Object);

            var result = service.CancelTaskAsync("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.CANCELLED, result.Status);
        }

        [TestMethod]
        public void CancelTaskAsync_MissingTask()
        {
            var service = MockReasoningTaskService();

            var result = service.CancelTaskAsync("testId").Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CreateTaskAsync_ExistingKB()
        {
            var kb = ReasoningMocks.GetKnowledgeBase();
            var createResource = MockCreateResource();

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(kb));
            var reasoningService = new ReasoningService();

            var service = MockReasoningTaskService(null, knowledgeBaseRepository.Object, reasoningService);

            var result = service.CreateTaskAsync(createResource).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.WAITING, result.Status);
        }

        [TestMethod]
        public void DeleteAsync_ExistingTask()
        {
            var deleteResult = new DeleteResult.Acknowledged(1);

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((DeleteResult)deleteResult));

            var service = MockReasoningTaskService(reasoningTaskRepository.Object);

            var result = service.DeleteTaskAsync("testId").Result;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteAsync_MissingTask()
        {
            var deleteResult = new DeleteResult.Acknowledged(0);

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((DeleteResult)deleteResult));

            var service = MockReasoningTaskService(reasoningTaskRepository.Object);

            var result = service.DeleteTaskAsync("testId").Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetTaskAsync_ExistingTask()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.STOPPED);

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));

            var service = MockReasoningTaskService(reasoningTaskRepository.Object);

            var result = service.GetTaskAsync("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.STOPPED, result.Status);
        }

        [TestMethod]
        public void GetTaskDetailAsync_ExistingTask()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.STOPPED);

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));

            var service = MockReasoningTaskService(reasoningTaskRepository.Object);

            var result = service.GetTaskDetailAsync("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ReasoningTask);
            Assert.AreEqual(ReasoningTaskStatus.STOPPED, result.ReasoningTask.Status);
        }

        [TestMethod]
        public void ResumeTaskAsync_ExistingTask()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.STOPPED);

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));

            var service = MockReasoningTaskService(reasoningTaskRepository.Object);

            var result = service.ResumeTaskAsync("testId").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(ReasoningTaskStatus.WAITING, result.Status);
        }

        [TestMethod]
        public void ResumeTaskAsync_MissingTask()
        {
            var service = MockReasoningTaskService();

            var result = service.ResumeTaskAsync("testId").Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void SetVariablesAsync_ExistingTask()
        {
            var reasoningTask = ReasoningMocks.GetReasoningTask(ReasoningTaskStatus.STOPPED);
            var variablesResource = new VariablesResource { Variables = new List<IVariable>() };

            var reasoningTaskRepository = new Mock<IReasoningTaskRepository>();
            reasoningTaskRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(reasoningTask));
            var reasoningService = new ReasoningService();

            var service = MockReasoningTaskService(reasoningTaskRepository.Object, null, reasoningService);

            var result = service.SetVariablesAsync("testId", variablesResource).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.MissingVariableIds.ToList().Count == 0);
        }

        [TestMethod]
        public void SetVariablesAsync_MissingTask()
        {
            var variablesResource = new VariablesResource { Variables = new List<IVariable>() };

            var service = MockReasoningTaskService();

            var result = service.SetVariablesAsync("testId", variablesResource).Result;

            Assert.IsNull(result);
        }

        private CreateReasoningTaskResource MockCreateResource()
        {
            return new CreateReasoningTaskResource
            {
                Description = "test",
                KnowledgeBaseId = "testId",
                ReasoningMethod = ReasoningMethod.Deduction
            };
        }

        private ReasoningTaskService MockReasoningTaskService(
            IReasoningTaskRepository reasoningTaskRepository = null,
            IKnowledgeBaseRepository knowledgeBaseRepository = null,
            IReasoningService reasoningService = null,
            IReasoningTaskResolver reasoningTaskResolver = null
        )
        {
            return  new ReasoningTaskService(
                reasoningTaskRepository ?? Mock.Of<IReasoningTaskRepository>(),
                knowledgeBaseRepository ?? Mock.Of<IKnowledgeBaseRepository>(),
                reasoningService ?? Mock.Of<IReasoningService>(),
                reasoningTaskResolver ?? Mock.Of<IReasoningTaskResolver>(),
                Mock.Of<ILogger<ReasoningTaskService>>()
                );
        }
    }
}
