using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.UserInfoDTO
{
    public class PasswordChange
    {
        public string Login { get; set; }
        public string PreviousPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
