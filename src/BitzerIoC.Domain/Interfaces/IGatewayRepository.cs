using BitzerIoC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitzerIoC.Domain.Interfaces
{
    public interface IGatewayRepository
    {
        IEnumerable<Gateway> GetGateways();
        IEnumerable<Gateway> GetGateways(int boundaryId);
        Gateway GetGateway(int gatewayId);
        Gateway GetGateway(int gatewayId, int boundayId);
        Gateway GetGateway(string gatewayName);
        Gateway GetGateway(string gatewayName,int boundaryId);
        bool ValidateGateway(string gatewayMac);
        bool CreateGateway(string gatewayName, string gatewayMAC, string createdBy, int boundaryId, bool isEnable);
        IEnumerable<Device> GetDevices(int gatewayId);
        bool DeleteGateway(int gatewayId);


    }
}
