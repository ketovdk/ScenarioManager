using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class ControllerScenarios
    {
        [Key]
        [Required]
        public long ScenarioId { get; set; }
        [Key]
        [Required]
        public long ControllerId { get; set; }
        [ForeignKey("ScenarioId")]
        public Scenario Scenario { get; set; }
        [ForeignKey("ControllerId")]
        public SmartController Controller { get; set; }
        public bool TurnedOn { get; set; }
    }
}
