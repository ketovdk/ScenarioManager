using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.UserGroupDTO
{
    public class UserGroupWithoutParent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentGroupId { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserGroupWithoutParent> Children { get; set; }
    }
}
