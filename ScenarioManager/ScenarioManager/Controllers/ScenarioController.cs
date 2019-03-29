using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Mappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ScenarioManager.Services;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Scenario")]
    [Authorize]
    public class ScenarioController: Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ScenarioRepository _scenarioRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly ControllerScenariosRepository _connections;
        private readonly IMapper<ScenarioDTO, Scenario> _mapper;
        public ScenarioController(ScenarioRepository scenarioRepository,
            UserGroupRepository userGroupRepository,
            IMapper<ScenarioDTO, Scenario> mapper,
            ControllerScenariosRepository connections)
        {
            _scenarioRepository = scenarioRepository;
            _userGroupRepository = userGroupRepository;
            _mapper = mapper;
            _connections = connections;
        }
        [HttpGet("Public")]
        public IEnumerable<ScenarioDTO> PublicScenarios()
        {
            return _scenarioRepository.Scenarios.Where(x => x.Publicity).Select(x=>_mapper.Map(x));
        }
        [HttpGet("Available")]
        public IEnumerable<ScenarioDTO> ParentScenarios()
        {
            var parents = _userGroupRepository.GetParentGroups(GetUserGroupId());
            return _scenarioRepository.Scenarios.Include(x => x.UserGroup).Where(x => parents.Contains(x.UserGroupId)).Select(x => _mapper.Map(x));
        }
        [HttpGet("Children")]
        public IEnumerable<ScenarioDTO> ChildrenScenarios()
        {
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            return _scenarioRepository.Scenarios.Include(x => x.UserGroup).Where(x => children.Contains(x.UserGroupId)).Select(x => _mapper.Map(x));

        }

        [HttpGet("{id}")]
        public ScenarioDTO GetScenario(long id)
        {
            var parents = _userGroupRepository.GetParentGroups(GetUserGroupId());
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            var scenario = _scenarioRepository.Scenarios.Include(x=>x.UserGroup).Where(x => x.Id == id).FirstOrDefault();
            if (scenario == null)
                throw new Exception("Такого сценария не существует");
            if (parents.Contains(scenario.Id) || children.Contains(scenario.Id))
                return _mapper.Map(scenario);
            else
                throw new Exception("Этот сценарий вам не доступен");
        }

        [HttpPut]
        public async Task EditScenario([FromBody]Scenario input)
        {
            if (User.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value ==
                Constants.RoleNames.SimpleUser)
                throw new Exception("Не доступно простому пользователю");
            var currentScenario = _scenarioRepository.Scenarios.FirstOrDefault(x => x.Id == input.Id);
            if (currentScenario == null)
                throw new Exception("Такого сценария не существует");
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            if (children.Contains(currentScenario.UserGroupId))
            {
                _scenarioRepository.EditScenario(input);
                foreach (var controller in _connections.All.Include(x => x.Controller)
                    .Where(x => x.ScenarioId == input.Id).Select(x => x.Controller))
                {
                    await ControllerInfoSender.UpdateAsync(controller.Adress, input.Id);
                }
                _scenarioRepository.SaveChanges();
            }
            else
                throw new Exception("Этот сценарий вам не доступен");
        }

        [HttpDelete("{id}")]
        public async Task DeleteScenario(long id)
        {
            if (User.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value ==
                Constants.RoleNames.SimpleUser)
                throw new Exception("Не доступно простому пользователю");
            var currentScenario = _scenarioRepository.Scenarios.FirstOrDefault(x => x.Id == id);
            if (currentScenario == null)
                throw new Exception("Такого сценария не существует");
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            if (children.Contains(currentScenario.UserGroupId))
            {
                _scenarioRepository.Delete(id);;
                foreach (var controller in _connections.All.Include(x => x.Controller)
                    .Where(x => x.ScenarioId == id).Select(x => x.Controller))
                {
                    await ControllerInfoSender.DeleteAsync(controller.Adress, id);
                }
                _scenarioRepository.SaveChanges();
            }
            else
                throw new Exception("Этот сценарий вам не доступен");
        }

        
        [HttpPost]
        public Scenario CreateScenario([FromBody]Scenario input)
        {
            if (User.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value ==
                Constants.RoleNames.SimpleUser)
                throw new Exception("Не доступно простому пользователю");
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            if (children.Contains(input.UserGroupId))
            {
                var returningScenario =_scenarioRepository.CreateScenario(input);
                _scenarioRepository.SaveChanges();
                return returningScenario;
            }
            else
                throw new Exception("эта группа пользователей вам недоступна");
        }


        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.Where(x => x.Type == Constants.ClaimTypeNames.UserGroupId).FirstOrDefault().Value);
        }
    }
}
