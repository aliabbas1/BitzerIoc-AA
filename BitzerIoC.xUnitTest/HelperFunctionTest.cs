using BitzerIoC.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestLib
{
    public class HelperFunctionTest
    {
        [Fact]
        public void ParseQueryString()
        {
            //Arrange
            string queryString = "http://www.w3schools.com/html/demo_form_submit.asp?text=Hello+G%C3%BCnter";

            //Act
            Dictionary<string,string> dic = GenericHelper.ParseQueryString(queryString);

            //Asset
            Assert.Equal(1, dic.Count);

        }

        public  void GetReturnUri()
        {
            

            
        }
    }
}
