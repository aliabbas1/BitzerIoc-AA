
using BitzerIoC.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzerIoC.Domain.Entities
{
    [Table("AspNetRoleClaims")]
    public class AspNetRoleClaim
    {
        [Key]
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string RoleId { get; set; }
        public virtual AspNetRole Role { get; set; }
    }
}
