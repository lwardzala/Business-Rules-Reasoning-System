using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Reasoning.Host.Controllers;
using Reasoning.Host.Resources;
using Reasoning.Host.Services;

namespace Reasoning.Host.Test.Controllers
{
    [TestClass]
    public class ReasoningTaskControllerTests
    {
        [TestMethod]
        public void GetById_ExistingTask()
        {
            var resource = new ReasoningTaskResource();

            var reasoningTaskService = new Mock<IReasoningTaskService>();
            reasoningTaskService.Setup(x => x.GetTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(resource));

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Get("testId").Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(ReasoningTaskResource));
        }

        [TestMethod]
        public void GetById_MissingTask()
        {
            var reasoningTaskService = new Mock<IReasoningTaskService>();

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Get("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetDetail_ExistingTask()
        {
            var resource = new DetailedReasoningTaskResource();

            var reasoningTaskService = new Mock<IReasoningTaskService>();
            reasoningTaskService.Setup(x => x.GetTaskDetailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(resource));

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.GetDetail("testId").Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(DetailedReasoningTaskResource));
        }

        [TestMethod]
        public void GetDetail_MissingTask()
        {
            var reasoningTaskService = new Mock<IReasoningTaskService>();

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.GetDetail("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Post_NewTask()
        {
            var resource = new ReasoningTaskResource();

            var reasoningTaskService = new Mock<IReasoningTaskService>();
            reasoningTaskService.Setup(x => x.CreateTaskAsync(It.IsAny<CreateReasoningTaskResource>()))
                .Returns(Task.FromResult(resource));

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Post(new CreateReasoningTaskResource()).Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(ReasoningTaskResource));
        }

        [TestMethod]
        public void Delete_ExistingTask()
        {
            var reasoningTaskService = new Mock<IReasoningTaskService>();
            reasoningTaskService.Setup(x => x.DeleteTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Delete("testId").Result as OkResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_MissingTask()
        {
            var reasoningTaskService = new Mock<IReasoningTaskService>();

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Delete("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Resume_ExistingTask()
        {
            var resource = new ReasoningTaskResource();

            var reasoningTaskService = new Mock<IReasoningTaskService>();
            reasoningTaskService.Setup(x => x.ResumeTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(resource));

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Resume("testId").Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(ReasoningTaskResource));
        }

        [TestMethod]
        public void Resume_MissingTask()
        {
            var reasoningTaskService = new Mock<IReasoningTaskService>();

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Resume("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Cancel_ExistingTask()
        {
            var resource = new ReasoningTaskResource();

            var reasoningTaskService = new Mock<IReasoningTaskService>();
            reasoningTaskService.Setup(x => x.CancelTaskAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(resource));

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Cancel("testId").Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(ReasoningTaskResource));
        }

        [TestMethod]
        public void Cancel_MissingTask()
        {
            var reasoningTaskService = new Mock<IReasoningTaskService>();

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.Cancel("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void SetVariables_ExistingTask()
        {
            var resource = new MissingVariablesResource();

            var reasoningTaskService = new Mock<IReasoningTaskService>();
            reasoningTaskService.Setup(x => x.SetVariablesAsync(It.IsAny<string>(), It.IsAny<VariablesResource>()))
                .Returns(Task.FromResult(resource));

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.SetVariables("testId", new VariablesResource()).Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(MissingVariablesResource));
        }

        [TestMethod]
        public void SetVariables_MissingTask()
        {
            var reasoningTaskService = new Mock<IReasoningTaskService>();

            var controller = new ReasoningTaskController(reasoningTaskService.Object);

            var result = controller.SetVariables("testId", new VariablesResource()).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
