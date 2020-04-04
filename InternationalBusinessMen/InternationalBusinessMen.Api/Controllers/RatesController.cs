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
    public class RatesController : ControllerBase
    {
        private readonly IRateService _rateService;
        private readonly IMapper _mapper;
        readonly ILogger<RatesController> _logger;

        public RatesController(IRateService rateService,
            IMapper mapper,
            ILogger<RatesController> logger)
        {
            _rateService = rateService;
            _mapper = mapper;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var ratesEntities = await _rateService.GetAll();
                return Ok(_mapper.Map<IEnumerable<RateDto>>(ratesEntities));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed - retrieve rates",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}