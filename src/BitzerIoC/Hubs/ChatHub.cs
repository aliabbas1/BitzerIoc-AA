using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BitzerIoC.Hubs
{
    public class ChatHub : Hub
    {
        public void send(string name, string message)
        {
            Clients.All.broadcastMessage(message);
        }
    }
}
