using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.Controller
{
    public class Scenarios
    {
        //Ключ сценария, включен/выключен
        public IEnumerable<KeyValuePair<long, bool>> ScenarioIds { get; set; }
        public long ControllerId { get; set; }
    }
}
