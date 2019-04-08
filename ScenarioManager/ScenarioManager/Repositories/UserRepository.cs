using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class UserRepository
    {
        private readonly MainDbContext _context;
        public UserRepository(MainDbContext context)
        {
            _context = context;
        }
        public DbSet<User> Users { get => _context.Users; }
        public User Create(User input)
        {
            _context.Add(input);
            return input;
        }

        public User Edit(User input)
        {
            var user = Users.Where(x => x.Login == input.Login).FirstOrDefault();
            if (user == null)
                throw new Exception("Сценарий с таким Id не обнаружен");
            if (input.UserGroupId != -1)
                user.UserGroupId = input.UserGroupId;
            if (input.UserType != user.UserType)
                user.UserType = input.UserType;
            if (input.FIO != null)
                user.FIO = input.FIO;
            if (input.Info != null)
                user.Info = input.Info;
            return input;
        }

        public void Delete(string login)
        {
            var user = Users.Where(x => x.Login == login).FirstOrDefault();
            if (user == null)
                throw new Exception("Сценарий с таким Id не обнаружен");
            _context.Remove(user);
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
