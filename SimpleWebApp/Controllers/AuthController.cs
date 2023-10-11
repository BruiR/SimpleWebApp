﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs.AuthorizedPerson;
using SimpleWebApp.Services;
using SimpleWebApp.Services.Interfaces;
using System.Data;

namespace SimpleWebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorizedPersonService _authorizedPersonService;
        private readonly IJWTManagerService _jWTManager;

        public AuthController(IJWTManagerService jWTManager, IAuthorizedPersonService authorizedPersonService)
        {
            _jWTManager = jWTManager;
            _authorizedPersonService = authorizedPersonService;
        }

        /// <summary>
        /// Authorization and authentication.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///        "Login": "admin",
        ///        "Password": "admin"
        ///     }
        /// Enter login = admin password = admin to have access to UserController
        /// </remarks>
        /// <response code="200">Returns JWT token</response>
        /// <response code="401">Incorrect data was sent.</response>
        /// <response code="500">Unhandled exception has been thrown over the request execution.</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public async Task<ActionResult<TokenResponse>> Authenticate(AuthorizedPersonDto authorizedPersonDto)
        {
            try
            {
                var authorizedPerson = await _authorizedPersonService.Get(authorizedPersonDto.Login, authorizedPersonDto.Password);
                if (authorizedPerson == null)
                {
                    return Unauthorized();
                }
                var token = _jWTManager.Authenticate(authorizedPerson);

                if (token == null)
                {
                    return Unauthorized();
                }
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error($"Authenticate: Login = {authorizedPersonDto.Login} Password = {authorizedPersonDto.Password}" +
                    $"\n Excetion : {ex.Message}");
                return Problem();
            }
        }

    }
}
