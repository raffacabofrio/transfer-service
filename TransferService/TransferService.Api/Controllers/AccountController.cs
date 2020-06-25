using System;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TransferService.Api.Filters;
using TransferService.Api.ViewModels;
using TransferService.Domain;
using TransferService.Domain.Common;
using TransferService.Domain.Exceptions;
using TransferService.Infra.CrossCutting.Identity;
using TransferService.Infra.CrossCutting.Identity.Interfaces;
using TransferService.Service;

namespace TransferService.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAllHeaders")]
    [GetClaimsFilter]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IApplicationSignInManager _signManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService,
                                 IApplicationSignInManager signManager,
                                 IMapper mapper,
                                 IConfiguration configuration)
        {
            _userService = userService;
            _signManager = signManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        #region GET

        [HttpGet]
        [Authorize("Bearer")]
        public UserVM Get()
        {
            var id = new Guid(Thread.CurrentPrincipal?.Identity?.Name);
            var user = _userService.Find(id);

            var userVM = _mapper.Map<UserVM>(user);
            return userVM;
        }

        [Authorize("Bearer")]
        [HttpGet("Profile")]
        public object Profile()
        {
            var id = new Guid(Thread.CurrentPrincipal?.Identity?.Name);
            return new { profile = _userService.Find(id).Profile.ToString() };
        }

        #endregion GET

        #region POST

        [HttpPost("Register")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(409)]
        public IActionResult Post([FromBody] RegisterUserVM registerUserVM, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<User>(registerUserVM);

            var result = _userService.Insert(user);

            if (result.Success)
                return Ok(_signManager.GenerateTokenAndSetIdentity(result.Value, signingConfigurations, tokenConfigurations));

            return Conflict(result);
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public IActionResult Login(
            [FromBody] LoginUserVM loginUserVM,
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfigurations tokenConfigurations,
            [FromHeader(Name = "x-requested-with")] string client,
            [FromHeader(Name = "client-version")] string clientVersion)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(loginUserVM);
            var result = _userService.AuthenticationByEmailAndPassword(user);

            if (result.Success)
            {
                var response = new Result
                {
                    Value = _signManager.GenerateTokenAndSetIdentity(result.Value, signingConfigurations, tokenConfigurations)
                };

                return Ok(response);
            }

            return NotFound(result);
        }

        #endregion POST

        #region PUT

        [HttpPut]
        [Authorize("Bearer")]
        [ProducesResponseType(typeof(Result<User>), 200)]
        [ProducesResponseType(409)]
        public IActionResult Update([FromBody] UpdateUserVM updateUserVM, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(updateUserVM);

            user.Id = new Guid(Thread.CurrentPrincipal?.Identity?.Name);

            var result = _userService.Update(user);

            if (!result.Success)
                return Conflict(result);

            return Ok(_signManager.GenerateTokenAndSetIdentity(result.Value, signingConfigurations, tokenConfigurations));
        }

        [Authorize("Bearer")]
        [HttpPut("ChangePassword")]
        public Result<User> ChangePassword([FromBody] ChangePasswordUserVM changePasswordUserVM)
        {
            var user = new User() { Password = changePasswordUserVM.OldPassword };
            user.Id = new Guid(Thread.CurrentPrincipal?.Identity?.Name);
            return _userService.ValidOldPasswordAndChangeUserPassword(user, changePasswordUserVM.NewPassword);
        }

        [Authorize("Bearer")]
        [HttpGet("Balance")]
        public Result Balance()
        {
            var userId = new Guid(Thread.CurrentPrincipal?.Identity?.Name);
            var balance = _userService.getBalance(userId);
            return new Result()
            {
                Value = balance
            };
        }



        #endregion PUT


    }
}