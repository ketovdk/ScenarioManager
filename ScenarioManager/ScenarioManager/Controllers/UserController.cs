using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Mappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Repositories;
using ScenarioManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    [Authorize]
    public class UserController:Controller
    {
        private readonly UserRepository _userRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly IMapper<UserDTO, User> _mapper;
        private readonly AccountService _accountService;
        public UserController(UserRepository userRepository, UserGroupRepository userGroupRepository, IMapper<UserDTO, User> mapper, AccountService accountService)
        {
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpGet]
        public IEnumerable<UserDTO> AllUsers()
        {
            var children = GetChildrenUserGroups();
            return _userRepository.Users.Include(x => x.UserGroup).Where(x => children.Contains(x.UserGroupId)).Select(x => _mapper.Map(x));
        }
        [HttpGet("Single/{login}")]
        public UserDTO GetUser(string login)
        {
            var user = _userRepository.Users.Include(x => x.UserGroup).Where(x => x.Login == login).FirstOrDefault();
            if (user == null)
                throw new Exception("Пользователь не существует");
            var children = GetChildrenUserGroups();
            if (children.Contains(user.UserGroupId))
                return _mapper.Map(user);
            else
                throw new Exception("Пользователь не доступен");
        }

        [Authorize(Roles =Constants.RoleNames.Integrator)]
        [HttpDelete("{login}")]
        public void DeleteUser(string login)
        {
            var user = _userRepository.Users.Include(x => x.UserGroup).Where(x => x.Login == login).FirstOrDefault();
            if (user == null)
                throw new Exception("Пользователь не существует");

            var children = GetChildrenUserGroups();
            if (children.Contains(user.UserGroupId))
            {
                if(user.UserType==UserType.Integrator)
                {
                    if (user.UserGroupId != GetUserGroupId())
                    {
                        _userRepository.Delete(login);
                        _userRepository.SaveChanges();
                    }
                    else
                        throw new Exception("У вас нет доступа к этому пользователю");
                }
                else
                {
                    _userRepository.Delete(login);
                    _userRepository.SaveChanges();
                }
            }
            else
                throw new Exception("У вас нет доступа к этому пользователю");
        }

        [Authorize(Roles = Constants.RoleNames.Integrator)]
        [HttpPut]
        public void EditUser(User input)
        {
            if (
            User.Identity.Name==input.Login)
            {
                _userRepository.Edit(input);
                _userRepository.SaveChanges();
                return;
            }
            var user = _userRepository.Users.Include(x => x.UserGroup).Where(x => x.Login == input.Login).FirstOrDefault();
            if (user == null)
                throw new Exception("Пользователь не существует");

            var children = GetChildrenUserGroups();
            if (children.Contains(user.UserGroupId))
            {
                if (user.UserType == UserType.Integrator)
                {
                    if (user.UserGroupId != GetUserGroupId())
                    {
                        _userRepository.Edit(input);
                        _userRepository.SaveChanges();
                    }
                    else
                        throw new Exception("У вас нет доступа к этому пользователю");
                }
                else
                {
                    _userRepository.Edit(input);
                    _userRepository.SaveChanges();
                }
            }
            else
                throw new Exception("У вас нет доступа к этому пользователю");
        }
        [HttpGet("AllIntegrators")]
        [Authorize(Roles = Constants.RoleNames.Admin)]
        public IEnumerable<UserDTO> GetIntegrators()
        {
            return _userRepository.Users.Where(x => x.UserType == UserType.Integrator).Select(x=>_mapper.Map(x));
        }

        [HttpGet("Children/Integrators")]
        [Authorize(Roles = Constants.RoleNames.Integrator)]
        public IEnumerable<UserDTO> GetChildIntegrators()
        {
            var childrenGroups = GetChildrenUserGroups();
            return _userRepository.Users
                .Where(x => x.UserType == UserType.Integrator && childrenGroups.Contains(x.UserGroupId)).AsEnumerable()
                .Select(_mapper.Map);
        }

        [HttpGet("Children/Users")]
        [Authorize(Roles = Constants.RoleNames.Integrator)]
        public IEnumerable<UserDTO> GetChildUsers()
        {
            var childrenGroups = GetChildrenUserGroups();
            return _userRepository.Users
                .Where(x => x.UserType == UserType.SimpleUser && childrenGroups.Contains(x.UserGroupId)).AsEnumerable()
                .Select(_mapper.Map);
        }
        [HttpGet("AllSimpleUsers")]
        [Authorize(Roles = Constants.RoleNames.Admin)]
        public IEnumerable<UserDTO> GetSimpleUsers()
        {
            return _userRepository.Users.Where(x => x.UserType == UserType.SimpleUser).Select(x=>_mapper.Map(x));
        }
        [Authorize(Roles = Constants.RoleNames.Integrator)]
        [HttpPost]
        public void PostUser([FromBody]UserCreate input)
        {
            var children = GetChildrenUserGroups();
            if (!children.Contains(input.UserGroupId))
                throw new Exception("Эта группа пользователей вам не доступна");

            var loginPassword = new LoginPassword()
            {
                Login = input.Login,
                Password = input.Password
            };
            if(input.UserType==UserType.Integrator)
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
                FIO=input.FIO,
                Info=input.Info,
                UserGroupId = input.UserGroupId,
                UserType = input.UserType,
            });
            _userRepository.SaveChanges();
        }



        private HashSet<long> GetParentUserGroups()
        {
          return _userGroupRepository.GetParentGroups(GetUserGroupId());
        }
        private HashSet<long> GetChildrenUserGroups()
        {
            return _userGroupRepository.GetChildrenGroups(GetUserGroupId());
        }
        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == Constants.ClaimTypeNames.UserGroupId).Value);
        }
    }
}
