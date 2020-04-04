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
    public class RatesControllerTest
    {
        private readonly Mock<ILogger<RatesController>> _mockLogger;
        private readonly Mock<IRateService> _mockRateService;
        private readonly IMapper _mockMapper;

        public RatesControllerTest()
        {
            _mockLogger = new Mock<ILogger<RatesController>>();
            _mockRateService = new Mock<IRateService>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProfileMapper());
            });
            _mockMapper = mockMapper.CreateMapper();
        }

        [TestMethod]
        public async Task RatesController_GetAll_Ok()
        {
            //Arrange
            _mockRateService.Setup(m => m.GetAll())
                                          .Returns(Task.FromResult(TestRepository.GetAllRates()));

            RatesController controller = new RatesController(_mockRateService.Object, _mockMapper, _mockLogger.Object);

            //Act
            IActionResult result = await controller.GetAll();
            ObjectResult dtoAsObjectResult = result as ObjectResult;
            var dtoResult = dtoAsObjectResult.Value as IEnumerable<RateDto>;

            // Assert
            Assert.IsNotNull(dtoAsObjectResult);
            Assert.AreEqual(StatusCodes.Status200OK, dtoAsObjectResult.StatusCode);
            Assert.AreEqual(5, dtoResult.Count());
        }

        [TestMethod]
        public async Task RatesController_GetAll_Error()
        {
            //Arrange
            _mockRateService.Setup(m => m.GetAll())
                                         .Throws(new Exception());

            RatesController controller = new RatesController(_mockRateService.Object, _mockMapper, _mockLogger.Object);

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
