using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs.AuthorizedPerson;
using SimpleWebApp.DTOs.User;
using SimpleWebApp.Repository;
using SimpleWebApp.Services.Interfaces;

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

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult<TokenResponse>> Authenticate(AuthorizedPersonDto authorizedPersonDto)
        {
            var authorizedPerson = await _authorizedPersonService.Get(authorizedPersonDto.Login, authorizedPersonDto.Password);
            if (authorizedPerson == null)
            {
                return Unauthorized();
            }
            var token =  _jWTManager.Authenticate(authorizedPerson);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

    }
}
