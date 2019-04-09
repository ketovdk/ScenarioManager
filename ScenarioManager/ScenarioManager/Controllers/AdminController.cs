using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScenarioManager.Mappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using ScenarioManager.Model.DTO.AdminDTO;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Repositories;
using ScenarioManager.Services;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    [Authorize(Roles = Constants.RoleNames.Admin)]
    public class AdminController : Controller
    {
        private readonly AdminRepository _repository;
        private readonly IMapper<Admin, AdminWithPassword> _mapper;
        private readonly AccountService _accountService;
        private readonly TokenRepository _tokenRepository;
        private readonly UserRepository _userRepository;
        public AdminController(AdminRepository repository, UserRepository userRepository, IMapper<Admin, AdminWithPassword> mapper, AccountService accountService, TokenRepository tokenRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _accountService = accountService;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<Admin> GetAdmins()
        {
            return _repository.Admins.OrderBy(x => x.Login);
        }

        [HttpGet("{login}")]
        public Admin GetAdmin(string login)
        {
            return _repository.Admins.Where(x => x.Login == login).FirstOrDefault();
        }

        [HttpPost("User")]
        public void CreateUser([FromBody] UserCreate input)
        {
            var loginPassword = new LoginPassword()
            {
                Login = input.Login,
                Password = input.Password
            };
            if (input.UserType == UserType.Integrator)
            {
                _accountService.RegisterIntegrator(loginPassword, input.UserGroupId);
            }
            else
            {
                _accountService.RegisterUser(loginPassword, input.UserGroupId);
            }
            _userRepository.Create(new User()
            {
                Login = input.Login,
                UserGroupId = input.UserGroupId,
                UserType = input.UserType,
                FIO=input.FIO,
                Info = input.Info
            });
            _userRepository.SaveChanges();
        }

        [HttpPost]
        public void CreateAdmin([FromBody] AdminWithPassword input)
        {
            _accountService.RegisterAdmin(new LoginPassword()
            {
                Login = input.Login,
                Password = input.Password,
                
            });
            _repository.Create(_mapper.Map(input));
            _repository.SaveChanges();
        }

        [HttpPut]
        public void EditAdmin([FromBody] AdminWithPassword input)
        {
            if (_accountService.LogIn(new LoginPassword()
            {
                Login = input.Login,
                Password = input.Password
            })==null)
                throw new Exception("не верный пароль");
            _repository.Edit(_mapper.Map(input));
            _repository.SaveChanges();
        }

        [HttpDelete("{login}")]
        public void DeleteAdmin(string login)
        {
            if (login != Constants.RoleNames.MainAdminLogin)
            {
                _repository.Delete(login);

                _accountService.Delete(login);
                _repository.SaveChanges();
            }
        }

    }
}