using BitzerIoC.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Domain.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using BitzerIoC.Infrastructure.Utilities;

namespace BitzerIoC.Infrastructure.Repositories
{
    public class GatewayRepository : IGatewayRepository
    {

        public static string TAG = typeof(GatewayRepository).Name;
        private IdentityContext db;

        public GatewayRepository(IdentityContext identityContext)
        {
            db = identityContext;
        }


        /// <summary>
        /// Get all gateways
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Gateway> GetGateways()
        {
            return db.Gateways.ToList();
        }


        /// <summary>
        /// Get single gateway by Name, Name should be unique otherwise ,
        /// if multple record found then get exception
        /// </summary>
        /// <param name="gatewayName"></param>
        /// <param name="boundaryId"></param>
        /// <returns></returns>
        public Gateway GetGateway(string gatewayName)
        {
            return (from gateway in db.Gateways
                    where gateway.GatewayName.Equals(gatewayName,StringComparison.OrdinalIgnoreCase)
                    select gateway).SingleOrDefault();
        }

        /// <summary>
        /// Get a particular gateway
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        public Gateway GetGateway(int gatewayId)
        {
            return (from gateway in db.Gateways
                    where gateway.GatewayId == gatewayId
                    select gateway).SingleOrDefault();
        }

        /// <summary>
        /// Get single gateway by Name, Name should be unique otherwise ,
        /// if multple record found then get exception
        /// </summary>
        /// <param name="gatewayName"></param>
        /// <param name="boundaryId"></param>
        /// <returns></returns>
        public Gateway GetGateway(string gatewayName, int boundaryId)
        {
            return (from gateway in db.Gateways
                    where gateway.GatewayName.Equals(gatewayName, StringComparison.OrdinalIgnoreCase)
                    select gateway)
                    .SingleOrDefault();
        }

        /// <summary>
        /// ToDo:Get single gateway in particular boundary
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="boundayId"></param>
        /// <returns></returns>
        public Gateway GetGateway(int gatewayId, int boundayId)
        {
            return (from  gateway in db.Gateways
                    where gateway.GatewayId == gatewayId
                          && gateway.BoundaryId == boundayId
                    select gateway)
                   .SingleOrDefault();
        }



        /// <summary>
        /// Get gateway in particular boundary, return Collection of Gateway
        /// </summary>
        /// <param name="boundaryId">BoundaryId</param>
        /// <returns>Collection of Gateways</returns>
        public IEnumerable<Gateway> GetGateways(int boundaryId)
        {
            return from gateway in db.Gateways
                   where gateway.BoundaryId == boundaryId
                   select gateway;

        }

        /// <summary>
        ///  Function take gatewayMAC as paramter then validate the gatewayMAC and return true if gatewayMAC exist.
        /// </summary>
        /// <param name="GatewayMAC">gatewayMAC</param>
        /// <returns>return true if gatewayMAC exist.</returns>
        /// Author = Ali Abbas, version 1.0 
        public bool ValidateGateway(string gatewayMac)
        {
            if (!string.IsNullOrEmpty(gatewayMac))
            {
                var gatewayMAC = db.Gateways.Where(g => g.GatewayMAC.Equals(gatewayMac, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (gatewayMAC != null)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Create gateway in particular boundary, return true if successfully created else return false
        /// </summary>
        /// <param name="gatewayName">GatewayName</param>
        /// <param name="gatewayMAC">GatewayMAC</param>
        /// <param name="createdBy">CreatedBy</param>
        /// <param name="boundaryId">BoundaryId</param>
        /// <param name="isEnable">IsEnable</param>
        /// <returns>True if gateway successfully created</returns>
        /// Author = Ali Abbas, version 1.0 

        public bool CreateGateway(string gatewayName, string gatewayMAC, string createdBy, int boundaryId, bool isEnable)
        {
            try
            {
                Gateway gateway = new Gateway()
                {
                    GatewayName = gatewayName,
                    GatewayMAC = gatewayMAC,
                    CreatedBy = createdBy,
                    BoundaryId = boundaryId,
                    IsEnable = isEnable,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                using (db.Database.BeginTransaction())
                {
                    db.Entry(gateway).State = EntityState.Added;
                    db.Gateways.Add(gateway);
                    db.SaveChanges();
                    db.Database.CommitTransaction();
                    return true;
                }
            }
            catch (Exception ex)
            {
                db.Database.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// update the gateway , return true if successfully updated else return false
        /// </summary>
        /// <param name="gatewayId">GatewayId</param>
        /// <param name="gatewayName">GatewayName</param>
        /// <param name="isEnable">IsEnable</param>
        /// <param name="updatedBy">UpdatedBy</param>
        /// <returns>True if gateway successfully updated</returns>
        /// Author = Ali Abbas, version 1.0 

        public bool UpdateGateway(int gatewayId, string gatewayName, bool isEnable, string upatedBy)
        {
            Gateway gateway = GetGateway(gatewayId);

            #region Validation
            if (gateway == null)
            {
                return false;
            }
            #endregion

            try
            {
                gateway.GatewayName = gatewayName;
                gateway.IsEnable = isEnable;
                gateway.ModifiedDate = DateTime.Now;
                gateway.ModifiedBy = upatedBy;

                db.Entry(gateway).State = EntityState.Modified;
                db.Gateways.Update(gateway);
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(TAG, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get devices
        /// </summary>
        /// <param name="gatewayId">GatewayId</param>
        /// <returns>return devices</returns>
        /// Author = Ali Abbas, version 1.0

        public IEnumerable<Device> GetDevices(int gatewayId)
        {
            return (from device in db.Devices
                    where device.GatewayId == gatewayId
                    select device);
        }

        /// <summary>
        /// Delete gateway , return true if successfully deleted else return false
        /// </summary>
        /// <param name="gatewayId">GatewayId</param>
        /// <returns>True if gateway successfully deleted</returns>
        /// Author = Ali Abbas, version 1.0 

        public bool DeleteGateway(int gatewayId)
        {
            Gateway gateway = GetGateway(gatewayId);

            #region Validation
            if (gateway == null)
            {
                return false;
            }
            #endregion

            try
            {
                db.Entry(gateway).State = EntityState.Deleted;
                db.Gateways.Remove(gateway);
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(TAG, ex);
                throw ex;
            }
        }
    }
}
