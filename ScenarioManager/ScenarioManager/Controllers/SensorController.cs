using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.Sensor;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Sensor")]
    [Authorize]
    public class SensorController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly SensorRepository _sensorRepository;
        public SensorController(SensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }
      

        [HttpGet("{id}")]
        public Sensor GetSensor(long id)
        {
            return _sensorRepository.Sensors.Where(x=>x.Id==id).FirstOrDefault();
        }

        [HttpGet]
        public IEnumerable<Sensor> GetSensors()
        {
            return _sensorRepository.Sensors.Include(x => x.Controller).Where(x => x.Controller.UserGroupId == GetUserGroupId());
        }

        [HttpPost]
        public Sensor AddSensor([FromBody] SensorInput input)
        {
            var returning =  _sensorRepository.CreateSensor(new Sensor()
            {
                Name = input.Name,
                Description = input.Description,
                ControllerId = input.ControllerId,
                Type = input.Type,
                UserGroupId = input.UserGroupId
            });
            _sensorRepository.SaveChanges();
            return returning;
        }
        [HttpPut]
        public void EditSensor ([FromBody] Sensor input)
        {
            _sensorRepository.EditSensor(input);
            _sensorRepository.SaveChanges();
        }
        [HttpDelete("{id}")]
        public void DeleteSensor(long id)
        {
            _sensorRepository.Delete(id);
            _sensorRepository.SaveChanges();
        }

        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.Where(x => x.Type == Constants.ClaimTypeNames.UserGroupId).FirstOrDefault().Value);
        }
    }
}
