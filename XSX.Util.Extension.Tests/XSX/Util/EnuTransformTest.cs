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
        //[Fact]
        //public void EnuTransformTest()
        //{
        //    var standardPoints = new Dictionary<Point, Point>()
        //    {
        //        { new Point(653068.71271461656, 3281704.0923241354), new Point(651796.2895339021, 3281825.0074003497) },
        //        { new Point(653177.54815978638, 3281675.9374143807), new Point(651903.916431491, 3281857.4774478413) },
        //        { new Point(653068.7127, 3281704.0923), new Point(651796.2895, 3281825.0074) },
        //        { new Point(653178.5482, 3281675.9410), new Point(651904.7691, 3281857.9999) }
        //    };

        //    var transformaParam = new TransformationParamFour(standardPoints);
        //    var pointTransforma = new PointTransformFour(transformaParam);
        //    var targetPoint = pointTransforma.Transform(new Point(653122.0487, 3281691.3951));
        //    Math.Abs(targetPoint.X - 651848.4617).ShouldBeLessThan(0.001);
        //    Math.Abs(targetPoint.Y - 3281841.8601).ShouldBeLessThan(0.001);
        //}
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

    }
}
