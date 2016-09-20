using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Entities
{
    [Table("Gateways")]
    public class Gateway
    {
        [Key]
        public int GatewayId { get; set; }
        public string GatewayName { get; set; }
        public string GatewayMAC { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int BoundaryId { get; set; }
        /// <summary>
        /// Active or Inactive
        /// </summary>
        public bool ConnectionStatus { get; set; } = false;
        public bool IsEnable { get; set; }
        public virtual List<Device> Devices { get; set; }
    }
}
