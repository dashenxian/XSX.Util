using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using XSX.Util;
using Xunit;

namespace Tests.XSX.Util
{
    public class PointTransformaTest
    {
        [Fact]
        public void TransformaTest()
        {
            var standardPoints = new Dictionary<Point, Point>()
            {
                { new Point(653068.71271461656, 3281704.0923241354), new Point(651796.2895339021, 3281825.0074003497) },
                { new Point(653177.54815978638, 3281675.9374143807), new Point(651903.916431491, 3281857.4774478413) },
                { new Point(653068.7127, 3281704.0923), new Point(651796.2895, 3281825.0074) },
                { new Point(653178.5482, 3281675.9410), new Point(651904.7691, 3281857.9999) }
            };
            
            var transformaParam = new TransformationParam(standardPoints);
            var pointTransforma = new PointTransforma(transformaParam);
            var targetPoint = pointTransforma.Transforma(new Point(653122.0487, 3281691.3951));
            Math.Abs(targetPoint.X - 651848.4617).ShouldBeLessThan(0.001);
            Math.Abs(targetPoint.Y - 3281841.8601).ShouldBeLessThan(0.001);
        }
        [Fact]
        public void TransformaThrowExceptionTest()
        {
            var standardPoints = new Dictionary<Point, Point>()
            {
                { new Point(653068.71271461656, 3281704.0923241354), new Point(651796.2895339021, 3281825.0074003497) }
            };
            try
            {
                new TransformationParam(standardPoints);
            }
            catch (ArgumentException e)
            {
                e.ShouldNotBeNull();
            }
        }
    }
}
