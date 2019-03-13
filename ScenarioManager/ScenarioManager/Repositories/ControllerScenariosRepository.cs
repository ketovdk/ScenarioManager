using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class ControllerScenariosRepository
    {
        private readonly MainDbContext _context;
        public ControllerScenariosRepository(MainDbContext context)
        {
            _context = context;
        }
        public DbSet<ControllerScenarios> All => _context.ControllerScnarios;
        public void Remove(long ControllerId)
        {
            _context.RemoveRange(All.Where(x => x.ControllerId == ControllerId));
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Set(IEnumerable<long> scenarioIds, long controllerId)
        {
            Remove(controllerId);
            _context.AddRange(scenarioIds.Select(x => new ControllerScenarios()
            {
                ScenarioId = x,
                ControllerId = controllerId
            }));
        }
    }
}
