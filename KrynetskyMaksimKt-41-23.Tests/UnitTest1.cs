using System.Text.RegularExpressions;
using WebApplication1.Models;
namespace KrynetskyMaksimKt_41_23.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void IsValidGroupName_KT4123_true()
        {
            var testGroup = new WebApplication1.Models.Group
            { 
                Name = "КТ-41-23"
            };

            var result = testGroup.IsValidGroupName();

            Assert.True(result);


        }
    }
}
