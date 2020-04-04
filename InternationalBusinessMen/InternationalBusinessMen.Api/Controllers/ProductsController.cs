using System;
using System.Threading.Tasks;
using InternationalBusinessMen.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternationalBusinessMen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductTransactionService _productTransactionService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductTransactionService productTransactionService,
            ILogger<ProductsController> logger)
        {
            _productTransactionService = productTransactionService;
            _logger = logger;
        }

        [HttpGet("{productId}/transactions")]
        public async Task<IActionResult> GetCollectionByProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest();
            }

            try
            {
                return Ok(await _productTransactionService.CreateModel(productId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed - retrieve products", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            
        }
    }
}