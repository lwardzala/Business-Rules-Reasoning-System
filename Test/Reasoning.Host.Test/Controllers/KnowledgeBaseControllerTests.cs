using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Reasoning.Core.Contracts;
using Reasoning.Host.Controllers;
using Reasoning.Host.Resources;
using Reasoning.Host.Services;
using Reasoning.Host.Test.Mocks;

namespace Reasoning.Host.Test.Controllers
{
    [TestClass]
    public class KnowledgeBaseControllerTests
    {
        [TestMethod]
        public void GetById_GetExistingKB()
        {
            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();
            knowledgeBaseService.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(GetKnowledgeBaseResource(ReasoningMocks.GetKnowledgeBase())));

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Get("testId").Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(KnowledgeBaseResource));
            Assert.IsNotNull(((KnowledgeBaseResource)result.Value).KnowledgeBase);
        }

        [TestMethod]
        public void GetById_GetMissingKB()
        {
            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Get("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Post_NewKB()
        {
            var knowledgeBaseResource = GetKnowledgeBaseResource(ReasoningMocks.GetKnowledgeBase());

            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();
            knowledgeBaseService.Setup(x => x.CreateAsync(It.IsAny<IKnowledgeBase>()))
                .Returns(Task.FromResult(knowledgeBaseResource));

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Post(knowledgeBaseResource).Result as CreatedResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(IKnowledgeBase));
        }

        [TestMethod]
        public void Post_ExistingKB()
        {
            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Post(GetKnowledgeBaseResource(ReasoningMocks.GetKnowledgeBase())).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ConflictResult));
        }

        [TestMethod]
        public void Put_ExistingKB()
        {
            var knowledgeBaseResource = GetKnowledgeBaseResource(ReasoningMocks.GetKnowledgeBase());

            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();
            knowledgeBaseService.Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<IKnowledgeBase>()))
                .Returns(Task.FromResult(knowledgeBaseResource));

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Put("testId", knowledgeBaseResource).Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOfType(result.Value, typeof(KnowledgeBaseResource));
            Assert.IsNotNull(((KnowledgeBaseResource)result.Value).KnowledgeBase);
        }

        [TestMethod]
        public void Put_MissingKB()
        {
            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Put("testId", GetKnowledgeBaseResource(ReasoningMocks.GetKnowledgeBase())).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Delete_ExistingKB()
        {
            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();
            knowledgeBaseService.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Delete("testId").Result as OkResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_MissingKB()
        {
            var knowledgeBaseService = new Mock<IKnowledgeBaseService>();

            var controller = new KnowledgeBaseController(knowledgeBaseService.Object);

            var result = controller.Delete("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        private KnowledgeBaseResource GetKnowledgeBaseResource(IKnowledgeBase knowledgeBase)
        {
            return new KnowledgeBaseResource
            {
                KnowledgeBase = knowledgeBase
            };
        }
    }
}
