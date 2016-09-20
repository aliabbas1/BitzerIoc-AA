using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Domain.Entities;
using BitzerIoC.Domain.ViewModels;
using BitzerIoC.Infrastructure.Repositories;
using BitzerIoC.Infrastructure.Security;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace TestLib
{
    public class IdentityRepositoryTest
    {
        //Arrange
        IdentityRepository repository = new IdentityRepository(new IdentityContext(configuration: null));

        string userId = "ae74b434-5881-4b4d-bde8-577d9e4ff461";


        [Theory]
        [InlineData("ea7e56b0-750b-4296-a133-b9a147f87886", "633450AB-C5BE-497D-8421-96E2B2D5F729", "633450AB-C5BE-497D-8421-96E2B2D5F729")]
        [InlineData("ea7e56b0-750b-4296-a133-b9a147f87886", "633450AB-C5BE-497D-8421-96E2B2D5F729", "533450AB-C5BE-497D-8421-96E2B2D5F729")]
        public void UpdateUser(string userId, string oldRoleId, string newRoleId)
        {

            //Act
            var result = repository.UpdateUser(userId, "1223333", oldRoleId, newRoleId, "Khuram", 1, true);

            //Assert
            Assert.True(result);
        }


        [Theory]
        [InlineData("ea7e56b0-750b-4296-a133-b9a147f87886", "533450AB-C5BE-497D-8421-96E2B2D5F729", "633450AB-C5BE-497D-8421-96E2B2D5F729", true)]
        public void UpdateUser(string userId, string oldRoleId, string newRoleId, bool t)
        {

            //Act
            var result = repository.UpdateUser(userId, "1223333", oldRoleId, newRoleId, "Khuram", 1, true);

            //Assert
            Assert.True(result);
        }


        public void ValidateCredentials()
        {
            //ToDo: Pending Web API test method
            using (HttpClient client = new HttpClient())
            {
                var requestUri = "";
                client.BaseAddress = new Uri("http://localhost:5003/");
                requestUri = "/api/Identity/ValidateCredentials";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
                client.PostAsync(requestUri, new StringContent("username=Ali&Password=A"));
            }
        }



        [Fact]
        public void DeleteUserBoundary()
        {
            //Act 
            var result = repository.DeleteUserBoundary(userId, 1);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteUserDevice()
        {
            //Act 
            var result = repository.DeleteUserDevice(userId, 4);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("demouser1@gmail.com", "Demo user 1", "533450AB-C5BE-497D-8421-96E2B2D5F729", "12345", 1)]
        public void CreateUser(string username,string name,string roleId,string phone,int boundaryId)
        {           
            //Act
            bool status = repository.CreateUser(username, name, phone, roleId, boundaryId, true);
            //Assert
            Assert.True(status);
             
        }

      

        [Fact]
        public void DeleteUser()
        {
            //Arrange
            string username1 = "demouser1@softteams.com";
            //Act
            bool status = repository.DeleteUser(userId);
            //Assert
            Assert.True(status);

        }

        [Fact]
        public void GetUserWithChildEntities()
        {
           //Act
            AspNetUser user = repository.GetUserWithChildEntities(userId);
            //Assert
            Assert.NotNull(user);
        }

        [Fact]
        public void GetUserBoundaries()
        {
            //Act
            UserBoundary boundary = repository.GetUserBoundary("54dac99f-a6d3-44f5-9e1f-8530e56d4567", 1);
            //Assert
            Assert.NotNull(boundary);
        }

        [Fact]
        public void UpdatePassword()
        {
            //Arrange
            string username = "khuram.mails@gmail.com";
            string salt = HashSecurity.GetSalt();
            string password = "GVG%55pk";
            string hashedPassword = HashSecurity.GetHash(password, salt);
            //Act
            bool status = repository.UpdatePassword(username, hashedPassword, salt);
            AspNetUser user = repository.GetUser(username);
            
            //Assert
            Assert.True(status);
            Assert.NotNull(user);
            Assert.Equal(user.HashSalt, salt);
            Assert.Equal(user.Password, hashedPassword);
        }
    }
}
