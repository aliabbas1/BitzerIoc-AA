using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzerIoC.Domain.Entities
{
    [Table("AspNetUsers")]
    public class AspNetUser
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HashSalt { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool IsEnable { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int StatusId { get; set; }
       
        public string TokenKey { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public bool? IsComplete { get; set; }
        public virtual ICollection<AspNetUserClaim> UserClaims { get; set; }
        public virtual List<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<UserBoundary> UserBoundaries { get; set; }
        public virtual ICollection<UserDevice> UserDevices { get; set; }
     
    }
}
