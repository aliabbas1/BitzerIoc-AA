using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BitzerIoC.Infrastructure.Utilities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BitzerIoC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
     
       // GET api/Email
        [HttpGet]
        public async Task<bool> SendEmail(string to, string from, string subject, string plainTextMesage, string htmlMessage, string replyTo = null)
        {
            EmailHelper emailHelper = new EmailHelper(to, from, subject, plainTextMesage, htmlMessage, null);
            return await emailHelper.SendEmailAsync();          
        }
    
    }
}
