using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.ViewModels
{
    public class GatewayViewModel
    {
        [Range(0, 5, ErrorMessage = "This isn't right")]
        public int GatewayId { get; set; }

        [Required(ErrorMessage ="Error")]
        [MinLength(10,ErrorMessage ="Min Length Error")]
        public string GatewayName { get; set; }
        public bool IsEnable { get; set; }
        public string UpdatedBy { get; set; }
    }
}
