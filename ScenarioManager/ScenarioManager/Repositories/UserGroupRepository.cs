﻿using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class UserGroupRepository
    {
        private readonly MainDbContext _context;
        public UserGroupRepository(MainDbContext context)
        {
            _context = context;
        }

        public DbSet<UserGroup> UserGroups { get => _context.UserGroups; }

        public HashSet<long> GetParentGroups(long id)
        {
            var input = UserGroups.ToDictionary(x => x.Id, x => x);
            var returningAnswer = new HashSet<long>();
            if(input.ContainsKey(id))
            {
                var cur = input[id];
                while (cur.ParentGroupId.HasValue)
                {
                    returningAnswer.Add(cur.Id);
                    cur = input[cur.ParentGroupId.Value];
                }
                returningAnswer.Add(cur.Id);
                return returningAnswer;
            }
            else
            {
                throw new Exception("Группа с таким Id не найдена");
            }
        }
        public HashSet<long> GetChildrenGroups(long id)
        {
            var children = new Dictionary<long, List<long>>();
            foreach(var userGroup in UserGroups)
            {
                if(userGroup.ParentGroupId.HasValue)
                {
                    if (children.ContainsKey(userGroup.ParentGroupId.Value))
                        children[userGroup.ParentGroupId.Value].Add(userGroup.Id);
                    else
                        children.Add(userGroup.ParentGroupId.Value, new List<long> { userGroup.Id });
                }
            }

            var returningAnswer = new HashSet<long>();

            returningAnswer.Add(id);
            var queue = new Queue<long>();
            queue.Enqueue(id);

            while(queue.Count>0)
            {
                var current = queue.Dequeue();
                if (children.ContainsKey(current))
                    foreach (var a in children[current])
                    {
                        returningAnswer.Add(a);
                        queue.Enqueue(a);
                    }
            }


            return returningAnswer;
        }
        public UserGroup this[long id]
        {
            get { return UserGroups.FirstOrDefault(x => x.Id == id); }
        }

        public UserGroup Create(UserGroup input)
        {
            _context.Add(input);
            return input;
        }
        public void Edit(UserGroup input)
        {
            var userGroup = UserGroups.FirstOrDefault(x => x.Id == input.Id);
            if (userGroup == null)
                throw new Exception("Группа с таким Id не найдена");
            userGroup.Name = input.Name;
            if(input.Description!=null)
                userGroup.Description = input.Description;
            userGroup.ParentGroupId = input.ParentGroupId;
        }
        public void Delete(long id)
        {
            var userGroup = UserGroups.Where(x => x.Id == id).FirstOrDefault();
            if (userGroup == null)
                throw new Exception("Группа с таким Id не найдена");
            _context.Remove(userGroup);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
