using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScenarioManager.Model.DBModel;

namespace ScenarioManager.Model.DTO.Controller
{
    public class ScenarioOutput
    {
        public bool TurnedOn { get; set; }
        public Scenario Scenario { get; set; }
    }
}
