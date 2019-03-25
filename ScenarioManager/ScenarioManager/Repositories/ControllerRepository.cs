using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class ControllerRepository
    {
        private readonly MainDbContext _dbContext;
        public ControllerRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public DbSet<SmartController> Controllers => _dbContext.Controllers;
        public SmartController CreateController(SmartController input)
        {
            _dbContext.Add(input);
            return input;
        }


        public SmartController EditController(SmartController input)
        {
            var controller = Controllers.Where(x => x.Id == input.Id).FirstOrDefault();
            if (controller == null)
                throw new Exception("Сценарий с таким Id не обнаружен");
            if (input.Name != null)
                controller.Name = input.Name;
            if (input.Description != null)
                controller.Description = input.Description;
            if (input.Type != -1)
                controller.Type = input.Type;
            if (input.UserGroupId != -1)
                controller.UserGroupId = input.UserGroupId;
            if (input.Password != null)
                controller.Password = input.Password;
            if (input.Adress != null)
                controller.Adress = input.Adress;
            return controller;
        }


        public void Delete(long id)
        {
            var controller = Controllers.Where(x => x.Id == id).FirstOrDefault();
            if (controller == null)
                throw new Exception("Сценарий с таким Id не обнаружен");
            _dbContext.Remove(controller);
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
