
using Shouldly;
using XSX.Extension;
using Xunit;
using Xunit.Abstractions;

namespace Tests.XSX.Extension
{
    public class IPAddressExtensionsTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public IPAddressExtensionsTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Fact]
        public void IpAddressInRangeTest()
        {
            var ip = "192.168.2.3";
            var start = "192.168.1.1";
            var end = "192.168.2.255";
            ip.IpAddressInRange(start, end).ShouldBe(true);
        }
        [Fact]
        public void IpAddressOutRangeTest()
        {
            var ip = "192.168.3.3";
            var start = "192.168.1.1";
            var end = "192.168.2.255";
            ip.IpAddressInRange(start, end).ShouldBe(false);
        }

    }
}
