using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class SmartController
    {
        [Key]
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Type { get; set; }

        [ForeignKey("UserGroupId")]
        public UserGroup UserGroup { get; set; }
        [Required]
        public long UserGroupId { get; set; }
    }
}
