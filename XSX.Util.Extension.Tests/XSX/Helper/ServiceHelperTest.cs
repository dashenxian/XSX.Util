using Shouldly;
using XSX.Helper;
using Xunit;

namespace Tests.XSX.Helper
{
    public class ServiceHelperTest
    {
        [Fact]
        public void IsServiceExistedTest()
        {
            var serviceName = "Themes";
            ServiceHelper.IsServiceExisted(serviceName).ShouldBe(true);
        }
    }
}
