using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Entities
{
    [Table("AspNetUserRoles")]
    public class AspNetUserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public int UserBoundaryId { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual AspNetRole Role { get; set; }
        public virtual UserBoundary UserBoundary { get; set; }
    }
}
