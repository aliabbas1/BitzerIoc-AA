using BitzerIoC.Domain.DatabaseContext;
using System;
using Xunit;
using System.Linq;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Domain.Entities;

namespace TestLib
{
    public class GatewayRespositoryTest
    {

        //Act
        static IdentityContext db = new IdentityContext(configuration: null);
        GatewayRepository repositry = new GatewayRepository(db);
        



        /// <summary>
        /// Not null Test case
        /// </summary>
        [Fact]
        public void GetGateways()
        {
            //Act
            var result = repositry.GetGateways();

            //Assert
            Assert.NotNull(result);

            //Count Records
            int expected = db.Gateways.Count();
            int count = result.Count();
            Assert.Equal(expected, count);

        }

        /// <summary>
        /// Not null Test case
        /// </summary>
        [Theory]
        [InlineData("Gateway 1")]
        [InlineData("gateway 1")]
        public void GetGateway(string name)
        {
            //Act
            var result = repositry.GetGateway(name);

            //Assert
            Assert.NotNull(result);
            Assert.Contains("0090E845BB83", result.GatewayMAC);
        }

 

        [Theory]
        [InlineData(1, "0090E845BB83")]
        public void GetGateway(int gatewayId,string expectedGatewayMAC)
        {
            //Act
            var result = repositry.GetGateway(gatewayId);

            //Assert
            Assert.NotNull(result);
            Assert.Contains(expectedGatewayMAC, result.GatewayMAC);
        }



        /// <summary>
        /// Get single gateway by Name, Name should be unique otherwise ,
        /// if multple record found then get exception
        /// </summary>
        /// <param name="gatewayName"></param>
        /// <param name="boundaryId"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("Gateway 1",1, "0090E845BB83")]  //Test Case 1
        [InlineData("Gateway 2", 1, "000000000029")] //Test Case 2
        public void GetGateway(string gatewayName, int boundaryId,string expectedGatewayMAC)
        {
            //Act
            var result = repositry.GetGateway(gatewayName);

            //Assert
            Assert.NotNull(result);
            Assert.Contains(expectedGatewayMAC, result.GatewayMAC);
        }


        /// <summary>
        /// ToDo:Get single gateway in particular boundary
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="boundayId"></param>
        /// <returns></returns>
        ///   [Theory]
        [InlineData(1, 1, "0090E845BB83")]
        [InlineData(2, 1, "000000000029")]
        [InlineData(3, 2, "000000000030")]
        public void GetGateway(int gatewayId, int boundayId, string expectedGatewayMAC)
        {
            //Act
            var result = repositry.GetGateway(gatewayId);

            //Assert
            Assert.NotNull(result);
            Assert.Contains(expectedGatewayMAC, result.GatewayMAC);
        }

        /// <summary>
        /// Validate Gateway Mac, if Gateway Mac is already exist then return true else return false 
        /// </summary>
        /// <param name="gatewayMAC">GatewayMAC</param>
        /// <returns>bool result</returns>
        /// Author = Ali Abbas, version 1.0 

        [Fact]
        public void ValidateGateway()
        {
            //Arrange
            string GatewayMac = "0090E845BB83";
            //Act
            bool result = repositry.ValidateGateway(GatewayMac);

            //Assert
            Assert.True(result);
        }

        /// <summary>
        /// Create gateway in particular boundary,
        /// </summary>
        /// <param name="gatewayName">GatewayName</param>
        /// <param name="gatewayMAC">GatewayMAC</param>
        /// <param name="createdBy">CreatedBy</param>
        /// <param name="boundaryId">BoundaryId</param>
        /// <param name="isEnable">IsEnable</param>
        /// <returns>bool status</returns>
        /// Author = Ali Abbas, version 1.0 

        [Fact]
        public void CreateGateway()
        {
            //Arrange
            string gatewayName = "Gateway 3";
            string gatewayMAC = "000000000003";
            string createdBy = "Admin";
            int boundaryId = 1;
            bool isEnable = true;

            //Act
            bool status = repositry.CreateGateway(gatewayName, gatewayMAC, createdBy, boundaryId, isEnable);
            //Assert
            Assert.True(status);
        }

        /// <summary>
        /// Get single gateway in particular boundary
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="boundayId"></param>
        /// <returns></returns>
        /// Author = Ali Abbas, version 1.0 

        [Fact]
        public void GetGatewayById()
        {
            //Arrange
            int gatewayId = 1;
            int boundaryId = 1;

            //Act
            var result = repositry.GetGateway(gatewayId, boundaryId);

            //Assert
            Assert.NotNull(result);
        }

        /// <summary>
        /// update gateway in particular boundary,
        /// </summary>
        /// <param name="gatewayName">GatewayName</param>
        /// <param name="gatewayId">GatewayId</param>
        /// <param name="updatedBy">UpdatedBy</param>
        /// <param name="isEnable">IsEnable</param>
        /// <returns>return true if update successfully.</returns>
        /// Author = Ali Abbas, version 1.0 

        [Fact]
        public void UpdateGateway()
        {
            //Arrange
            int gatewayId = 1;
            string gatewayName = "Gateway 1";
            bool isEnable = false;
            string updatedBy = "Admin";
            
            //Act
            bool status = repositry.UpdateGateway(gatewayId, gatewayName, isEnable, updatedBy);
            //Assert
            Assert.True(status);
        }
        /// <summary>
        /// get devices in particular gateway,
        /// </summary>
        /// <param name="gatewayId">GatewayId</param>
        /// Author = Ali Abbas, version 1.0 

        [Fact]
        public void GetDevices()
        {
            //Arrange
            int gatewayId = 1;

            //Act
            var device = repositry.GetDevices(gatewayId);

            //Assert
            Assert.NotEmpty(device);
        }

        /// <summary>
        /// delete gateway, return true if deleted successfully.
        /// </summary>
        /// <param name="gatewayId">GatewayId</param>
        /// Author = Ali Abbas, version 1.0 

        [Fact]
        public void DeleteGateway()
        {
            //Arrange
            int gatewayId = 1;

            //Act
            var gateway = repositry.DeleteGateway(gatewayId);

            //Assert
            Assert.True(gateway);
        }



    }
}
