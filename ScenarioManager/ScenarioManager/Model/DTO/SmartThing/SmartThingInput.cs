using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.SmartThing
{
    public class SmartThingInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }      
        public long? ControllerId { get; set; }
        public long UserGroupId { get; set; }
    }
}
