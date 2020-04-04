using InternationalBusinessMen.Api.Controllers;
using InternationalBusinessMen.Api.Dtos;
using InternationalBusinessMen.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Test
{
    [TestClass]
    public class ProductsControllerTest
    {
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly Mock<IProductTransactionService> _mockProductTransactionService;

        public ProductsControllerTest()
        {
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _mockProductTransactionService = new Mock<IProductTransactionService>();
    
        }


        [TestMethod]
        public async Task ProductsController_GetTransactionsCollectionByProduct_Ok()
        {
            //Arrange
            _mockProductTransactionService.Setup(m => m.CreateModel(It.IsAny<string>()))
                                          .Returns(Task.FromResult(TestRepository.GetProductTransactionsDto("T2006")));

            ProductsController controller = new ProductsController(_mockProductTransactionService.Object, _mockLogger.Object);

            //Act
            IActionResult result = await controller.GetCollectionByProduct("T2006");
            ObjectResult dtoAsObjectResult = result as ObjectResult;
            var dtoResult = dtoAsObjectResult.Value as ProducTransactionsDto;

            // Assert
            Assert.IsNotNull(dtoAsObjectResult);
            Assert.AreEqual(StatusCodes.Status200OK, dtoAsObjectResult.StatusCode);
            Assert.AreEqual(2, dtoResult.Transactions.Count());
        }

        [TestMethod]
        public async Task ProductsController_GetTransactionsCollectionByProduct_InternalServerError()
        {
            //Arrange
            _mockProductTransactionService.Setup(m => m.CreateModel(It.IsAny<string>()))
                                          .Throws(new Exception());

            ProductsController controller = new ProductsController(_mockProductTransactionService.Object, _mockLogger.Object);

            //Act
            IActionResult result = await controller.GetCollectionByProduct("T2006");
            var internalServerErrorCode = result as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), typeof(StatusCodeResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, internalServerErrorCode.StatusCode);
        }



    }
}
