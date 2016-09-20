using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Entities
{
    [Table("UserBoundaries")]
    public class UserBoundary
    {
        [Key]
      	public int UserBoundaryId {get; set;}
	    public string UserId { get;set; }                
	    public int  BoundaryId { get; set; }
        public virtual List<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual Boundary Boundary { get; set; }
        
    }
}
