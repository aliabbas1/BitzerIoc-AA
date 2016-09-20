
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzerIoC.Domain.Entities
{
    [Table("AspNetUserClaims")]
    public class AspNetUserClaim
    {
        [Key]
        public int UserClaimId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AspNetUser User { get; set; }
    }
}
