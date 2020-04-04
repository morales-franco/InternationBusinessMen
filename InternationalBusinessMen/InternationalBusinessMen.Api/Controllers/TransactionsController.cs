using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using InternationalBusinessMen.Api.Dtos;
using InternationalBusinessMen.Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternationalBusinessMen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService,
            IMapper mapper,
            ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var transactionEntities = await _transactionService.GetAll();
                return Ok(_mapper.Map<IEnumerable<TransactionDto>>(transactionEntities));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed - retrieve transactions", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

    }
}