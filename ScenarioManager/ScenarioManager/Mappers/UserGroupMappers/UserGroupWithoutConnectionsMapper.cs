using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.UserGroupDTO;

namespace ScenarioManager.Mappers.UserGroupMappers
{
    public class UserGroupWithoutConnectionsMapper : IMapper<UserGroupWithoutConnections, UserGroup>
    {
        public UserGroupWithoutConnections Map(UserGroup input)
        {
            return new UserGroupWithoutConnections()
            {
                Description = input.Description,
                Id = input.Id,
                Name = input.Name,
                ParentGroupId = input.ParentGroupId
            };
        }
    }
}
