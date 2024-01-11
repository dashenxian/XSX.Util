using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using XSX.Util;
using Xunit;

namespace Tests.XSX.Util
{
    public class EnuTransformTest
    {
        [Fact]
        public void TransToEnu_CenterShouldBeZero_Test()
        {
            double L = 106.56536974434377;
            double B = 29.644273358829857;
            double H = 0;
            var transform = new EnuTransform(L, B, H);
            var result = transform.TransToEnu(L, B, H);
            result.x.ShouldBe(0);
            result.y.ShouldBe(0);
            result.z.ShouldBe(0);
        }
        [Fact]
        public void TransToEnuTest()
        {
            double L = 116.9395751953;
            double B = 36.7399177551;
            double H = 0;
            var transform = new EnuTransform(L, B, H);
            double x = 117;
            double y = 37;
            double z = 10.3;
            var result = transform.TransToEnu(x, y, z);
            Math.Abs(5378.520558 - result.x).ShouldBeLessThan(0.000001);
            Math.Abs(28864.325181 - result.y).ShouldBeLessThan(0.000001);
            Math.Abs(-57.481289 - result.z).ShouldBeLessThan(0.000001);
        }
        [Fact]
        public void TransToBLH_CenterShouldBeZero_Test()
        {
            double L = 106.56536974434377;
            double B = 29.644273358829857;
            double H = 0;
            var transform = new EnuTransform(L, B, H);
            var result = transform.TransToBLH(0, 0, 0);
            result.L.ShouldBe(L);
            result.B.ShouldBe(B);
            Math.Abs(H - result.H).ShouldBeLessThan(0.000_000_01);
        }
        [Fact]
        public void TransToBLHTest()
        {
            double L = 116.9395751953;
            double B = 36.7399177551;
            double H = 0;
            var transform = new EnuTransform(L, B, H);
            double x = 117;
            double y = 37;
            double z = 10.3;
            var result = transform.TransToBLH(5378.520558, 28864.325181, -57.481289);
            Math.Abs(x - result.L).ShouldBeLessThan(0.000_000_000_1);
            Math.Abs(y - result.B).ShouldBeLessThan(0.000_000_000_1);
            Math.Abs(z - result.H).ShouldBeLessThan(0.000_000_1);
        }
    }
}
