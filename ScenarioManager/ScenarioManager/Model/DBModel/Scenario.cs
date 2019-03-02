using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DBModel
{
    public class Scenario
    {
        [Required]
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }


        public string Description { get; set; }
        [Required]
        public string Script { get; set; }

        [Required]
        public string Author { get; set; }
        [Required]
        public bool Publicity { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public long UserGroupId { get; set; }

        [ForeignKey("UserGroupId")]
        public UserGroup UserGroup { get; set;}
    }
}
