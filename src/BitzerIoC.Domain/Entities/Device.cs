using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Entities
{
    [Table("Devices")]
    public class Device
    {
        [Key]
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string Communication { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnable { get; set; }
        public int GatewayId { get; set; }
        public virtual Gateway Gateway { get; set; }
    }
}
