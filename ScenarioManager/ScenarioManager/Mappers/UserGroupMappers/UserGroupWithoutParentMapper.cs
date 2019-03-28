using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.UserGroupDTO;

namespace ScenarioManager.Mappers.UserGroupMappers
{
    public class UserGroupWithoutParentMapper : IMapper<UserGroupWithoutParent, UserGroup>
    {
        public UserGroupWithoutParent Map(UserGroup input)
        {
            return new UserGroupWithoutParent()
            {
                Id = input.Id,
                ParentGroupId = input.ParentGroupId,
                Name = input.Name,
                Description = input.Description,
                Children = input.ChildrenGroups?.Select(Map)
            };
        }
    }
}
