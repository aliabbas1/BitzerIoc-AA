using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzerIoC.Domain.Entities
{
    [Table("AspNetRoles")]
    public class AspNetRole
    {
        [Key]
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public virtual List<AspNetUserRole> AspNetUserRoles { get; set; }

    }
}
