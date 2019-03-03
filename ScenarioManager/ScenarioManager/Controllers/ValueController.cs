using Microsoft.AspNetCore.Mvc;
using ScenarioManager.Repositories;
using ScenarioManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    public class ValueController: Controller
    {
        private readonly LoginService _loginService;
        private readonly UserLoginInfoRepository _userLoginInfoRepository;
        public ValueController(LoginService loginService, UserLoginInfoRepository userLoginInfoRepository)
        {
            _loginService = loginService;
            _userLoginInfoRepository = userLoginInfoRepository;
        }
        [HttpGet]
        public string Get()
        {
            return "HelloWorld";
        }

        [HttpGet("InitUser")]
        public string Init()
        {
            _loginService.Register(
                new Model.DTO.UserInfoDTO.LoginPassword()
                {
                    Login="Dima",
                    Password="Qwerty1"
                },
                Constants.RoleNames.Admin);
            _userLoginInfoRepository.SaveChanges();
            return "done";
        }
    }
}
