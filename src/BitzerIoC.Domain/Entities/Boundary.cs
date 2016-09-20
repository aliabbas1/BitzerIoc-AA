using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitzerIoC.Domain.Entities
{
    [Table("Boundaries")]
    public class Boundary
    {
        [Key]
        public int BoundaryId { get; set; } 
        public string BoundaryName { get; set; }

    }
}
