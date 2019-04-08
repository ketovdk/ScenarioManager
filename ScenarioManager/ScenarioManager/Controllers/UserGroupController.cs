using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Mappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.UserGroupDTO;
using ScenarioManager.Repositories;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/UserGroup")]
    [Authorize]
    public class UserGroupController : Controller
    {
        private readonly IMapper<UserGroup, EditUserGroup> _editUserGroupMapper;
        private readonly UserGroupRepository _repository;
        private readonly IMapper<UserGroup, CreateUserGroup> _createUserGroupMapper;
        private readonly IMapper<UserGroupWithoutConnections, UserGroup> _userGroupWithoutConnectionsMapper;
        private readonly IMapper<UserGroupWithoutParent, UserGroup> _userGroupWithoutParentMapper;
        public UserGroupController(UserGroupRepository repository, IMapper<UserGroup, CreateUserGroup> createUserGroupMapper,
            IMapper<UserGroup, EditUserGroup> editUserGroupMapper,
            IMapper<UserGroupWithoutParent, UserGroup> userGroupWithoutParentMapper,
            IMapper<UserGroupWithoutConnections, UserGroup> userGroupWithoutConnectionsMapper)
        {
            _repository = repository;
            _createUserGroupMapper = createUserGroupMapper;
            _editUserGroupMapper = editUserGroupMapper;
            _userGroupWithoutConnectionsMapper = userGroupWithoutConnectionsMapper;
            _userGroupWithoutParentMapper = userGroupWithoutParentMapper;
        }


        private void AppendChildren(UserGroup ug, ref Dictionary<long, IEnumerable<UserGroup>> groups)
        {
            if (groups.ContainsKey(ug.Id))
                ug.ChildrenGroups = groups[ug.Id];
            else
                ug.ChildrenGroups = new List<UserGroup>();
            foreach (var ugChildrenGroup in ug.ChildrenGroups)
            {
                AppendChildren(ugChildrenGroup, ref groups);
            }
        }
        [HttpGet]
        public UserGroupWithoutParent GetUserGroup()
        {
            long id = GetUserGroupId();
            var userGroup = _repository.UserGroups.FirstOrDefault(x => x.Id == id);
            if (userGroup == null)
                throw new Exception("Элемент не найден");

            var groups = _repository.UserGroups.Where(x => x.ParentGroupId.HasValue).GroupBy(x => x.ParentGroupId.Value)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());
            AppendChildren(userGroup, ref groups);
            return _userGroupWithoutParentMapper.Map(userGroup);
        }



        [HttpGet("All")]
        [Authorize(Roles = Constants.RoleNames.Admin)]
        public IEnumerable<UserGroupWithoutConnections> GetUserGroups()
        {
            return _repository.UserGroups.Select(_userGroupWithoutConnectionsMapper.Map);
        }

        [HttpGet("ById/{id}")]
        public UserGroupWithoutConnections GetUserGroupById(long id)
        {
            var returning = _repository[id];

            if (returning == null)
                throw new Exception("Группа с таким Id не существует");

            return _userGroupWithoutConnectionsMapper.Map(returning);
        }

        [HttpGet("Children")]
        [Authorize(Roles = Constants.RoleNames.Integrator)]
        public IEnumerable<UserGroupWithoutConnections> GetChildUserGroups()
        {
            var childGroupsIds = GetChildrenUserGroups();
            return _repository.UserGroups.Where(x => childGroupsIds.Contains(x.Id)).Select(_userGroupWithoutConnectionsMapper.Map);
        }

        [HttpPost]
        public long CreateUserGroup([FromBody] CreateUserGroup input)
        {
            var userGroup = _createUserGroupMapper.Map(input);
            _repository.Create(userGroup);
            _repository.SaveChanges();
            return userGroup.Id;
        }

        [HttpPut]
        public void EditUserGroup([FromBody] EditUserGroup input)
        {
            var userGroup = _editUserGroupMapper.Map(input);
            _repository.Edit(userGroup);
            _repository.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void DeleteUserGroup(long id)
        {
            _repository.Delete(id);
            _repository.SaveChanges();
        }


        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == Constants.ClaimTypeNames.UserGroupId).Value);
        }
        private HashSet<long> GetParentUserGroups()
        {
            return _repository.GetParentGroups(GetUserGroupId());
        }
        private HashSet<long> GetChildrenUserGroups()
        {
            return _repository.GetChildrenGroups(GetUserGroupId());
        }
    }
}