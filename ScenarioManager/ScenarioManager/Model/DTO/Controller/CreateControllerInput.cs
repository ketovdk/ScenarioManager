﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.Controller
{
    public class CreateControllerInput
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public long UserGroupId { get; set; }
        public string Adress { get; set; }
    }
}
