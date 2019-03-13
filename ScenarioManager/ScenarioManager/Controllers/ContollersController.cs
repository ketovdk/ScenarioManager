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

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Controller")]
    [Authorize]
    public class ControllersController : Controller
    {
        private readonly ControllerRepository _controllerRepository;
        private readonly ControllerScenariosRepository _scenarios;
        public ControllersController(ControllerRepository controllerRepository)
        {
            _controllerRepository = controllerRepository;
        }
        [HttpGet]
        public IEnumerable<SmartController> Controllers()
        {
            return _controllerRepository.Controllers.Where(x => x.UserGroupId == GetUserGroupId());
        }
        [HttpGet("{id}")]
        public SmartController Controller(long id)
        {
            return _controllerRepository.Controllers.Where(x => x.Id == id && x.UserGroupId == GetUserGroupId()).FirstOrDefault();
        }

        [HttpGet("Scenarios/{controllerId}")]
        public IEnumerable<Scenario> GetScenarios(long controllerId)
        {
            return _scenarios.All.Include(x => x.Scenario).Where(x => x.ControllerId == controllerId).Select(x => x.Scenario);
        }
        [HttpPost("Scenarios")]
        public void SetScenarios([FromBody] Scenarios input)
        {
            _scenarios.Set(input.ScenarioIds, input.ControllerId);
            _scenarios.SaveChanges();
        }
        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.Where(x => x.Type == Constants.ClaimTypeNames.UserGroupId).FirstOrDefault().Value);
        }
        [HttpPost]
        public SmartController CreateController([FromBody]CreateControllerInput input)
        {
            var returning= _controllerRepository.CreateController(new SmartController()
            {
                Name = input.Name,
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
