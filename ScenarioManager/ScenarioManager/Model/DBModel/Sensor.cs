using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class Sensor
    {
        [Required]
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Type { get; set; }

        [ForeignKey("ControllerId")]
        public SmartController Controller { get; set; }
    
        public long? ControllerId { get; set; }

        [ForeignKey("UserGroupId")]
        public UserGroup UserGroup { get; set; }
        public long UserGroupId { get; set; }
    }
}
