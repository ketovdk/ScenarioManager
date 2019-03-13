using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.Controller
{
    public class Scenarios
    {
        public IEnumerable<long> ScenarioIds { get; set; }
        public long ControllerId { get; set; }
    }
}
