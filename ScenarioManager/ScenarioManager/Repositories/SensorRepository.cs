using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class SensorRepository
    {
        private readonly MainDbContext _context;
        public SensorRepository(MainDbContext context)
        {
            _context = context;
        }

        public DbSet<Sensor> Sensors => _context.Sensors;
        public Sensor CreateSensor(Sensor input)
        {
            _context.Add(input);
            return input;
        }


        public void EditSensor(Sensor input)
        {
            var sensor = Sensors.Where(x => x.Id == input.Id).FirstOrDefault();
            if (sensor == null)
                throw new Exception("Sensor с таким Id не обнаружен");
            if (input.Name != null)
                sensor.Name = input.Name;
            if (input.ControllerId != -1)
                sensor.ControllerId = input.ControllerId;
            if (input.Description != null)
                sensor.Description = input.Description;
            if (input.Type != -1)
                sensor.Type = input.Type;
            
        }


        public void Delete(long id)
        {
            var sensor = Sensors.Where(x => x.Id == id).FirstOrDefault();
            if (sensor == null)
                throw new Exception("Сенсор с таким Id не обнаружен");
            _context.Remove(sensor);
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
