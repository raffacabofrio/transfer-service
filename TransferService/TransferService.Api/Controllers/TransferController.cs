using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using TransferService.Api.Filters;
using TransferService.Api.ViewModels;
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

        [Authorize("Bearer")]
        [HttpGet("Statement")]
        public Result<IList<EntryVM>> Statement()
        {
            var userId = new Guid(Thread.CurrentPrincipal?.Identity?.Name);
            var statement = _transferService.getStatement(userId);
            var statementVM = _mapper.Map<List<EntryVM>>(statement);

            return new Result<IList<EntryVM>>(statementVM);
        }

        [Authorize("Bearer")]
        [HttpPost("Transfer")]
        [Throttle(Name = "Transfer", Seconds = 10, VaryByIp = false)]
        public IActionResult Transfer([FromBody] TransferVM transferVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = new Guid(Thread.CurrentPrincipal?.Identity?.Name);
            var result = _transferService.Transfer(userId, transferVM.AccountNumberOrigin, transferVM.AccountNumberDestination, transferVM.Value);

            return Ok(new Result()
            {
                Value = result,
                SuccessMessage = "Transferência efetuada com sucesso."
            });
        }

    }

        
}