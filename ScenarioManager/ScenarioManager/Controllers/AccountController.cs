using Microsoft.AspNetCore.Mvc;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ScenarioManager.Model.DTO;
using ScenarioManager.Repositories;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController:Controller
    {
        private readonly TokenService _tokenService;
        private readonly AccountService _accountService;
        private readonly AdminRepository _adminRepository;
        private readonly UserRepository _userRepository;
        public AccountController(AccountService accountService, TokenService tokenService, AdminRepository adminRepository, UserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
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

        [HttpGet("Info")]
        public AccountInfo GetInfo()
        {
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            if (role == null)
                throw new Exception("Что-то странное, нет роли");
            if (role == Constants.RoleNames.Admin)
            {
                var user = _adminRepository.Admins.FirstOrDefault(x => x.Login == User.Identity.Name);
                if (user == null)
                    throw new Exception("пользователь удален");
                return new AccountInfo()
                {
                    Login = User.Identity.Name,
                    Role = role,
                    FIO = user.FIO
                };
            }
            else
            {
                var user = _userRepository.Users.FirstOrDefault(x => x.Login == User.Identity.Name);
                if (user == null)
                    throw new Exception("Пользователь удален");
                return new AccountInfo()
                {
                    Login = User.Identity.Name,
                    Role = role,
                    FIO = user.FIO
                };
            }
        }
    }
}
