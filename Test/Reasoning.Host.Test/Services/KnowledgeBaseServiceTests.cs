using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

using Reasoning.Core.Contracts;
using Reasoning.Core.Models;
using Reasoning.Host.Resources;
using Reasoning.Host.Services;
using Reasoning.MongoDb.Repositories;

namespace Reasoning.Host.Test.Services
{
    [TestClass]
    public class KnowledgeBaseServiceTests
    {
        [TestMethod]
        public void CreateAsync_NewKB()
        {
            var knowledgeBaseResource = new KnowledgeBaseResource {KnowledgeBase = new KnowledgeBase()};

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<IKnowledgeBase, bool>>>()))
                .Returns(Task.FromResult((long)0));

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.CreateAsync(knowledgeBaseResource.KnowledgeBase).Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.KnowledgeBase);
        }

        [TestMethod]
        public void CreateAsync_ExistingKB()
        {
            var knowledgeBaseResource = new KnowledgeBaseResource { KnowledgeBase = new KnowledgeBase() };

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<IKnowledgeBase, bool>>>()))
                .Returns(Task.FromResult((long)1));

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.CreateAsync(knowledgeBaseResource.KnowledgeBase).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteAsync_ExistingKB()
        {
            var deleteResult = new DeleteResult.Acknowledged(1);

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((DeleteResult)deleteResult));

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.DeleteAsync("testId").Result;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteAsync_MissingKB()
        {
            var deleteResult = new DeleteResult.Acknowledged(0);

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((DeleteResult)deleteResult));

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.DeleteAsync("testId").Result;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetAsync_NewKB()
        {
            var knowledgeBase = new KnowledgeBase();

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((IKnowledgeBase)knowledgeBase));

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.GetAsync("testId").Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.KnowledgeBase);
        }

        [TestMethod]
        public void GetAsync_ExistingKB()
        {
            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.GetAsync("testId").Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateAsync_ExistingKB()
        {
            var knowledgeBase = new KnowledgeBase { Id = "testId" };
            var updateResponse = new ReplaceOneResult.Acknowledged(1, 1, new BsonString("testId"));

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<IKnowledgeBase>()))
                .Returns(Task.FromResult((ReplaceOneResult)updateResponse));

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.UpdateAsync("testId", knowledgeBase).Result;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.KnowledgeBase);
            Assert.AreEqual("testId", result.KnowledgeBase.Id);
        }

        [TestMethod]
        public void UpdateAsync_MissingKB()
        {
            var knowledgeBase = new KnowledgeBase { Id = "testId" };
            var updateResponse = new ReplaceOneResult.Acknowledged(0, 0, new BsonString("testId"));

            var knowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();
            knowledgeBaseRepository.Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<IKnowledgeBase>()))
                .Returns(Task.FromResult((ReplaceOneResult)updateResponse));

            var service = new KnowledgeBaseService(knowledgeBaseRepository.Object, Mock.Of<ILogger<KnowledgeBaseService>>());

            var result = service.UpdateAsync("testId", knowledgeBase).Result;

            Assert.IsNull(result);
        }
    }
}
