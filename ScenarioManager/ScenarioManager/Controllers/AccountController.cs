using Microsoft.AspNetCore.Mvc;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController:Controller
    {
        private readonly TokenService _tokenService;
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService, TokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }
        [HttpPost]
        public Token Login([FromBody]LoginPassword input)
        {
            return _accountService.LogIn(input);
        }

        [HttpPost("Refresh")]
        public Token Refresh([FromBody] string token)
        {
            return _tokenService.UpdateFullTokenAsync(token).Result;
        }
    }
}
