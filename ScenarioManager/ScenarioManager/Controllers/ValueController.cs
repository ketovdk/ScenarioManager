using Microsoft.AspNetCore.Mvc;
using ScenarioManager.Repositories;
using ScenarioManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScenarioManager.Model.DBModel;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    public class ValueController: Controller
    {
        private readonly LoginService _loginService;
        private readonly UserLoginInfoRepository _userLoginInfoRepository;
        private readonly UserRepository _userRepository;
        private readonly ScenarioRepository _scenarioRepository;
        private readonly ControllerRepository _controllerRepository;
        private readonly ControllerScenariosRepository _connections;
        private readonly UserGroupRepository _userGroupRepository;
        public ValueController(LoginService loginService,
            UserRepository userRepository,
            ScenarioRepository scenarioRepository,
            ControllerRepository controllerRepository,
            ControllerScenariosRepository connections, 
            UserGroupRepository userGroupRepository,
                UserLoginInfoRepository userLoginInfoRepository)
        {
            _userRepository = userRepository;
            _scenarioRepository = scenarioRepository;
            _connections = connections;
            _userGroupRepository = userGroupRepository;
            _controllerRepository = controllerRepository;
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
            /*_loginService.Register(
                new Model.DTO.UserInfoDTO.LoginPassword()
                {
                    Login="Dima",
                    Password="Qwerty1"
                },
                Constants.RoleNames.Admin);
            _userLoginInfoRepository.SaveChanges();*/
            _loginService.Register(
                new Model.DTO.UserInfoDTO.LoginPassword()
                {
                    Login = "DimaIntegr",
                    Password = "Qwerty1"
                },
                Constants.RoleNames.Integrator);
            var ug = _userGroupRepository.Create(new UserGroup()
            {
                Name = "test1",

            });
            _userRepository.Create(new User()
            {
                Login = "DimaIntegr",
                UserGroupId = ug.Id,
                UserType = UserType.Integrator
            });
            var sc=_scenarioRepository.CreateScenario(new Scenario()
            {
                Author = "DimaIntegr",
                Description = "test",
                Name = "test",
                Publicity = false,
                Script = "TestScript",
                Type = 1,
                UserGroupId = ug.Id
            });
            var ct = _controllerRepository.CreateController(new SmartController()
            {
                Adress = "http://localhost:5000/api",
                Description = "test",
                Name = "test",
                Password = "Qwerty1",
                Type = 1,
                UserGroupId = ug.Id
            });
            var list = new List<KeyValuePair<long, bool>>();
            list.Add(new KeyValuePair<long, bool>(sc.Id, true)
            );
            _connections.Set(list, ct.Id);
            _userGroupRepository.SaveChanges();
            return ct.Id.ToString()+" "+sc.Id.ToString();
        }
    }
}
