using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.Controller
{
    public class ScenarioQuery
    {
        public long ScenarioId { get; set; }
        public ControllerLoginInfo Info { get; set; }
    }
}
