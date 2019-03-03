﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.Sensor
{
    public class SensorInput
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public long ControllerId { get; set; }
    }
}
