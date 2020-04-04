using AutoMapper;
using InternationalBusinessMen.Api.Controllers;
using InternationalBusinessMen.Api.Dtos;
using InternationalBusinessMen.Api.Mapper;
using InternationalBusinessMen.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Test
{
    [TestClass]
    public class TransactionsControllerTest
    {
        private readonly Mock<ILogger<TransactionsController>> _mockLogger;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly IMapper _mockMapper;

        public TransactionsControllerTest()
        {
            _mockLogger = new Mock<ILogger<TransactionsController>>();
            _mockTransactionService = new Mock<ITransactionService>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProfileMapper());
            });
            _mockMapper = mockMapper.CreateMapper();
        }

        [TestMethod]
        public async Task TransactionsController_GetAll_Ok()
        {
            //Arrange
            _mockTransactionService.Setup(m => m.GetAll())
                                          .Returns(Task.FromResult(TestRepository.GetAllTransactions()));

            TransactionsController controller = new TransactionsController(_mockTransactionService.Object, _mockMapper, _mockLogger.Object);

            //Act
            IActionResult result = await controller.GetAll();
            ObjectResult dtoAsObjectResult = result as ObjectResult;
            var dtoResult = dtoAsObjectResult.Value as IEnumerable<TransactionDto>;

            // Assert
            Assert.IsNotNull(dtoAsObjectResult);
            Assert.AreEqual(StatusCodes.Status200OK, dtoAsObjectResult.StatusCode);
            Assert.AreEqual(5, dtoResult.Count());
        }

        [TestMethod]
        public async Task TransactionsController_GetAll_Error()
        {
            //Arrange
            _mockTransactionService.Setup(m => m.GetAll())
                                          .Throws(new Exception());

            TransactionsController controller = new TransactionsController(_mockTransactionService.Object, _mockMapper, _mockLogger.Object);

            //Act
            IActionResult result = await controller.GetAll();
            var internalServerErrorCode = result as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), typeof(StatusCodeResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, internalServerErrorCode.StatusCode);
        }
    }
}
