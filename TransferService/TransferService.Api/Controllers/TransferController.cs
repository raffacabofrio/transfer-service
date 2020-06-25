using System;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TransferService.Api.Filters;
using TransferService.Domain.Common;
using TransferService.Service;

namespace TransferService.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAllHeaders")]
    [GetClaimsFilter]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public TransferController(ITransferService transferService,
                                 IMapper mapper)
        {
            _transferService = transferService;
            _mapper = mapper;
        }

        [Authorize("Bearer")]
        [HttpGet("Balance")]
        public Result Balance()
        {
            var userId = new Guid(Thread.CurrentPrincipal?.Identity?.Name);

            return new Result()
            {
                Value = _transferService.getBalance(userId)
            };
        }

    }

        
}