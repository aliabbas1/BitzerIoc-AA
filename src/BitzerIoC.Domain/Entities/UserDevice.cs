using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Entities
{
    /// <summary>
    /// ToDo: Many to many relationship managment
    /// </summary>
    [Table("UserDevices")]
    public class UserDevice
    {
        [Key]
        public  int UserDeviceId { get; set; }
        public string UserId { get; set; }
        public int DeviceId { get; set; }
        public int UserClaimId { get; set; }

    }
}
