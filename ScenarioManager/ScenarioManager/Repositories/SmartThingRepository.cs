using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class SmartThingRepository
    {
        private readonly MainDbContext _context;
        public SmartThingRepository(MainDbContext context)
        {
            _context = context;
        }

        public DbSet<SmartThing> Things => _context.SmartThings;


        public SmartThing CreateSmartThing(SmartThing input)
        {
            _context.Add(input);
            return input;
        }


        public void EditSmartThing(SmartThing input)
        {
            var sensor = Things.Where(x => x.Id == input.Id).FirstOrDefault();
            if (sensor == null)
                throw new Exception("Вещь с таким Id не обнаружен");
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
            var sensor = Things.Where(x => x.Id == id).FirstOrDefault();
            if (sensor == null)
                throw new Exception("вещь с таким Id не обнаружен");
            _context.Remove(sensor);
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
