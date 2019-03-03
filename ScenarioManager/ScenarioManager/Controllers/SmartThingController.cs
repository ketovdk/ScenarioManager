using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.SmartThing;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/SmartThing")]
    [Authorize]
    public class SmartThingController: Controller
    {
        private readonly SmartThingRepository _thingRepository;
        public SmartThingController(SmartThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }


        [HttpGet("{id}")]
        public SmartThing GetThings(long id)
        {
            return _thingRepository.Things.Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpGet]
        public IEnumerable<SmartThing> GetThings()
        {
            return _thingRepository.Things.Include(x => x.Controller).Where(x => x.Controller.UserGroupId == GetUserGroupId());
        }

        [HttpPost]
        public SmartThing AddThing([FromBody] SmartThingInput input)
        {
            var returning = _thingRepository.CreateSmartThing(new SmartThing()
            {
                Name = input.Name,
                Description = input.Description,
                ControllerId = input.ControllerId,
                Type = input.Type
            });
            _thingRepository.SaveChanges();
            return returning;
        }
        [HttpPut]
        public void EditThing([FromBody] SmartThing input)
        {
            _thingRepository.EditSmartThing(input);
            _thingRepository.SaveChanges();
        }
        [HttpDelete("{id}")]
        public void DeleteSmartThing(long id)
        {
            _thingRepository.Delete(id);
            _thingRepository.SaveChanges();
        }

        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.Where(x => x.Type == Constants.ClaimTypeNames.UserGroupId).FirstOrDefault().Value);
        }
    }
}
