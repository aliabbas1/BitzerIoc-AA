using BitzerIoC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.DTO
{
    public class UserDetailDTO:AspNetUser
    {
        public List<string> Roles { get; set; }
        public List<string> RoleId { get; set; }
    }
}
