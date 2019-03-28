﻿using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Mappers.User
{
    public class UserMapper : IMapper<UserDTO, Model.DBModel.User>
    {
        public UserDTO Map(Model.DBModel.User input)
        {
            return new UserDTO()
            {
                Login = input.Login,
                UserGroupId = input.UserGroupId,
                UserType = input.UserType,
                Info = input.Info,
                FIO = input.FIO
            };
        }
    }
}
