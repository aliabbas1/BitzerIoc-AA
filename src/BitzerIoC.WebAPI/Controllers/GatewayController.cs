using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Infrastructure.AppConstants;
using Microsoft.Extensions.Logging;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BitzerIoC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly GatewayRepository _gatewayRepository;
        private readonly ILogger<GatewayController> _logger;

        public GatewayController(GatewayRepository gatewayRepository, ILogger<GatewayController> logger)
        {
            _gatewayRepository = gatewayRepository;
            _logger = logger;
        }

        // GET: api/gateway/1
        [HttpGet]
        [Route("{boundaryId:int}")]
        public IEnumerable<Gateway> GetGateways(int boundaryId)
        {
            return _gatewayRepository.GetGateways(boundaryId);
        }

        /// <summary>
        /// <param name="gatewayId">GatewayId</param>
        /// <param name="boundaryId">BoundaryId</param>
        /// <returns>return Gateway in particular boundary.</returns>
        /// <uri>api/Gateway/{gatewayId-int parameter}/{boundaryId-int parameter}</uri>
        /// Author = Ali Abbas, version 1.0 
        /// </summary>
        [HttpGet]
        [Route("{gatewayMac}")]
        public async Task<bool> ValidateGateway(string gatewayMac)
        {
            try
            {
                return await Task.Run(() => _gatewayRepository.ValidateGateway(gatewayMac));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error:" + ex.Message);
                return false;
            }
            
        }

        /// <summary>
        /// <param name="gatewayId">GatewayId</param>
        /// <param name="boundaryId">BoundaryId</param>
        /// <returns>return Gateway in particular boundary.</returns>
        /// <uri>api/Gateway/{gatewayId-int parameter}/{boundaryId-int parameter}</uri>
        /// Author = Ali Abbas, version 1.0 
        /// </summary>

        [HttpGet("{gatewayId:int}/{boundaryId:int}")]
        public async Task<Gateway> GetGateway(int gatewayId, int boundaryId)
        {
            try
            {
                return await Task.Run(() => _gatewayRepository.GetGateway(gatewayId, boundaryId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error" + ex.Message);
                throw ex;
            }

        }
  
		
		
        /// <summary>
        /// Update gateway, gatewayId and boundaryId is required for updation
        /// <param name="gatewayId">GatewayId</param>
        /// <param name="gatewayName">GatewayName</param>
        /// <param name="isEnable">IsEnable</param>
        /// <param name="updatedBy">UpdatedBy</param>
        /// <returns>true if gateway updated.</returns>
        /// <uri>api/Gateway/{gatewayId - int parameter}/{gatewayName - string parameter}/{isEnable - bool parameter}/{updatedBy - string parameter}</uri>
        /// Author = Ali Abbas, version 1.0 
        /// </summary>

        [HttpPost]
        [Route("{gatewayId}/{gatewayName}/{isEnable}/{updatedBy}")]
        public async Task<bool> UpdateGateway(int gatewayId, string gatewayName, bool isEnable, string updatedBy)
        {
            if (gatewayName != "" || gatewayName != null)
            {
                return await Task.Run(() => _gatewayRepository.UpdateGateway(gatewayId, gatewayName, isEnable, updatedBy));
            }
            else
                return false;
        }



         /// <summary>
        /// Create gateway, GatewayName and GatewayMac is required for creation
        /// <param name="gatewayName">GatewayName</param>
        /// <param name="gatewayMAC">GatewayMAC</param>
        /// <param name="createdBy">CreatedBy</param>
        /// <param name="boundaryId">BoundaryId</param>
        /// <param name="isEnable">IsEnable</param>
        /// <returns>bool value, return true if operation successfully perform. </returns>
        /// <uri>api/Gateway/ValidateGateway/{GatewayMac-parameter}</uri>
        /// Author = Ali Abbas, version 1.0 
        /// </summary>
        [HttpPost]
        [Route("{GatewayName}/{GatewayMAC}/{CreatedBy}/{BoundaryId}/{IsEnable}/")]
        public async Task<bool> CreateGateway(string gatewayName, string gatewayMAC, string createdBy, int boundaryId, bool isEnable)
        {
            try
            {
                return await Task.Run(() => _gatewayRepository.CreateGateway(gatewayName, gatewayMAC, createdBy, boundaryId, isEnable));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error" + ex.Message);
                return false;
            }
        }



        /// <summary>
        /// Delete gateway, gatewayId is required 
        /// <param name="gatewayId">GatewayId</param>
        /// <returns>retun true if no device found in particular gateway and delete gateway successfully.</returns>
        /// <uri>api/Gateway/{gatewayId - int parameter}</uri>
        /// Author = Ali Abbas, version 1.0 
        /// </summary>

        [HttpDelete]
        [Route("{gatewayId:int}")]
        public async Task<bool> DeleteGateway(int gatewayId)
        {
            if (gatewayId != 0)
            {
                IEnumerable<Device> device = await Task.Run(() => _gatewayRepository.GetDevices(gatewayId));

                if (device.Count() > 0)
                    return false;
                else
                    return await Task.Run(() => _gatewayRepository.DeleteGateway(gatewayId));
            }
            else
                return false;
        }
    }
	
	    
}