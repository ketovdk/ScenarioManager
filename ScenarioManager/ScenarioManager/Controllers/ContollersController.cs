using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.Controller;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScenarioManager.Constants;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Controller")]
    [Authorize]
    public class ControllersController : Controller
    {
        private readonly ControllerRepository _controllerRepository;
        private readonly ControllerScenariosRepository _scenarios;
        public ControllersController(ControllerRepository controllerRepository, ControllerScenariosRepository scenarios)
        {
            _controllerRepository = controllerRepository;
            _scenarios = scenarios;
        }
        [HttpGet]
        public IEnumerable<SmartController> Controllers()
        {
            return _controllerRepository.Controllers.Where(x => x.UserGroupId == GetUserGroupId());
        }
        [HttpGet("{id}")]
        public SmartController Controller(long id)
        {
            return _controllerRepository.Controllers.FirstOrDefault(x => x.Id == id && x.UserGroupId == GetUserGroupId());
        }

        [HttpPost("TurnScenario")]
        public void TurnScenario([FromBody] ControllerScenarios input)
        {
            _scenarios.Set(input.ScenarioId, input.ControllerId, input.TurnedOn);
            _scenarios.SaveChanges();
        }

        [HttpGet("Scenarios/{controllerId}")]
        public IEnumerable<ScenarioOutput> GetScenarios(long controllerId)
        {
            return _scenarios.All.Include(x => x.Scenario).Where(x => x.ControllerId == controllerId).Select(x => new ScenarioOutput()
                {
                    Scenario = x.Scenario,
                    TurnedOn = x.TurnedOn
                });
        }
        [HttpPost("Scenarios")]
        public void SetScenarios([FromBody] Scenarios input)
        {
            _scenarios.Set(input.ScenarioIds, input.ControllerId);
            _scenarios.SaveChanges();
        }
        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == ClaimTypeNames.UserGroupId).Value);
        }
        [HttpPost]
        public SmartController CreateController([FromBody]CreateControllerInput input)
        {
            var returning= _controllerRepository.CreateController(new SmartController()
            {
                Name = input.Name,
                Password = input.Password,
                Adress=input.Adress,
                UserGroupId = input.UserGroupId,
                Type = input.Type,
                Description = input.Description
            });
            _controllerRepository.SaveChanges();
            return returning;
        }
        [HttpPut]
        public void EditController([FromBody] CreateControllerInput input)
        {
            _controllerRepository.EditController(new SmartController()
            {
                Name = input.Name,
                Password = input.Password,
                UserGroupId = input.UserGroupId,
                Type = input.Type,
                Description = input.Description
            });
            _controllerRepository.SaveChanges();            
        }
        [HttpDelete("{id}")]
        public void DeleteController(long id)
        {
            _controllerRepository.Delete(id);
            _controllerRepository.SaveChanges();
        }

    }
}
