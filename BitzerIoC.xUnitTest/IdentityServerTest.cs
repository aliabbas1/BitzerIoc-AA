using Xunit;
using Host.Configuration;
using Microsoft.AspNetCore.Hosting;
using BitzerIoC.Infrastructure.Security;

namespace TestLib
{
    public static class IdentityServerTest
    {
       
         

        [Theory]
        [InlineData("1", "7da61e6725aa27ca4f5a5ae0e73ea7dd", "b56cdafc8baeafd5d4eb088c47c95a559c1ca0aac36d47f2f8583fc28851d484")]
        [InlineData("11", "7da61e6725aa27ca4f5a5ae0e73ea7dd", "b56cdafc8baeafd5d4eb088c47c95a559c1ca0aac36d47f2f8583fc28851d484")]
        [InlineData("111", "7da61e6725aa27ca4f5a5ae0e73ea7dd", "b56cdafc8baeafd5d4eb088c47c95a559c1ca0aac36d47f2f8583fc28851d484")]
        public static void VerifyPassword(string inputPassword,string salt,string storedPassword)
        {
          
            //Act
            bool result = HashSecurity.CompareHashText(inputPassword, storedPassword, salt);

            //Assert
            Assert.True(result);
        }

    }
}
