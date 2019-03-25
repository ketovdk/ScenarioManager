using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.Controller;
using ScenarioManager.Repositories;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/ForControllers")]
    public class ForControllersController: Controller
    {
        private readonly ControllerRepository _repository;
        private readonly ControllerScenariosRepository _connections;

        public ForControllersController(ControllerRepository repository, ControllerScenariosRepository connections)
        {
            _repository = repository;
            _connections = connections;
        }

        [HttpPost]
        public SmartController CreateController([FromBody]CreateControllerInput input)
        {
            var returning = _repository.CreateController(new SmartController()
            {
                Name = input.Name,
                Password = input.Password,
                Adress = input.Adress,
                UserGroupId = input.UserGroupId,
                Type = input.Type,
                Description = input.Description
            });
            _repository.SaveChanges();
            return returning;
        }

        [HttpPost("Scenario")]
        public Scenario GetScenario(ScenarioQuery input)
        {
            return _connections.All.Include(x => x.Scenario)
                .Include(x => x.Controller)
                .FirstOrDefault(x =>
                x.Controller.Id == input.Info.Id && 
                x.Controller.Password == input.Info.Password &&
                x.ScenarioId == input.ScenarioId)?.Scenario;
        }
        [HttpPost("Scenarios")]
        public IEnumerable<ScenarioOutput> GetScenarios(ControllerLoginInfo input)
        {
            return _connections.All.Include(x => x.Scenario).Include(x=>x.Controller).Where(x => x.Controller.Id == input.Id&&x.Controller.Password==input.Password).Select(x => new ScenarioOutput()
            {
                Scenario = x.Scenario,
                TurnedOn = x.TurnedOn
            });
        }
    }
}
