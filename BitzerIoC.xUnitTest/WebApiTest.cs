using Xunit;
using BitzerIoC.WebAPI;
using BitzerIoC.WebAPI.Controllers;
using BitzerIoC.Domain.DatabaseContext;
using BitzerIoC.Infrastructure.Repositories;
using System.Net.Http;

namespace TestLib
{
    public class WebApiTest
    {

     
        [Fact]
        public void GetRoles()
        {
            using (var client = new HttpClient())
            {

                var model = client
                            .GetAsync("http://local.api.bitzerioc.com/api/identity")
                            .Result.Content.ReadAsStringAsync();

                Assert.NotNull(model);
            }


        }
    }
}
