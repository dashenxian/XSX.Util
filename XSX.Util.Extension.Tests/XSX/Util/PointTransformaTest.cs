using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using XSX.Util;
using Xunit;

namespace Tests.XSX.Util
{
    public class PointTransformaTest
    {
        [Fact]
        public void TransformaFourTest()
        {
            var standardPoints = new Dictionary<Point, Point>()
            {
                { new Point(653068.71271461656, 3281704.0923241354), new Point(651796.2895339021, 3281825.0074003497) },
                { new Point(653177.54815978638, 3281675.9374143807), new Point(651903.916431491, 3281857.4774478413) },
                { new Point(653068.7127, 3281704.0923), new Point(651796.2895, 3281825.0074) },
                { new Point(653178.5482, 3281675.9410), new Point(651904.7691, 3281857.9999) }
            };
            
            var transformaParam = new TransformationParamFour(standardPoints);
            var pointTransforma = new PointTransformFour(transformaParam);
            var targetPoint = pointTransforma.Transform(new Point(653122.0487, 3281691.3951));
            Math.Abs(targetPoint.X - 651848.4617).ShouldBeLessThan(0.001);
            Math.Abs(targetPoint.Y - 3281841.8601).ShouldBeLessThan(0.001);
        }
        [Fact]
        public void TransformaFourThrowExceptionTest()
        {
            var standardPoints = new Dictionary<Point, Point>()
            {
                { new Point(653068.71271461656, 3281704.0923241354), new Point(651796.2895339021, 3281825.0074003497) }
            };
            try
            {
                new TransformationParamFour(standardPoints);
            }
            catch (ArgumentException e)
            {
                e.ShouldNotBeNull();
            }
        }
        [Fact]
        public void TransformaSevenTest()
        {
            var standardPoints = new Dictionary<Point3D, Point3D>()
            {
                {new Point3D(-2066241.5001,5360801.8835,2761896.3022),new Point3D(-2066134.4869,5360847.0595,2761895.5970)},
                {new Point3D(-1983936.0407,5430615.7282,2685375.7214),new Point3D(-1983828.7084,5430658.9827,2685374.6681)},
                {new Point3D(-1887112.7302,5468749.1944,2677688.9806),new Point3D(-1887005.1714,5468790.6487,2677687.2680)},
                {new Point3D(-1808505.4212,5512502.2716,2642356.5720),new Point3D(-1808397.7260,5512542.0921,2642354.4550)},
                {new Point3D(-1847017.0670,5573542.7934,2483802.9904),new Point3D(-1846909.0036,5573582.6511,2483801.6147)},
            };

            var transformaParam = new TransformationParamSeven(standardPoints);
            var pointTransforma = new PointTransformSeven(transformaParam);
            var sourcePoint = new Point3D(-2066241.5001, 5360801.8835, 2761896.3022);
            var expectTargetPoint = new Point3D(-2066134.4869, 5360847.0595, 2761895.5970);
            var targetPoint = pointTransforma.Transform(sourcePoint);
            Math.Abs(expectTargetPoint.X - targetPoint.X).ShouldBeLessThan(0.0349);
            Math.Abs(expectTargetPoint.Y - targetPoint.Y).ShouldBeLessThan(0.0349);
            Math.Abs(expectTargetPoint.Z - targetPoint.Z).ShouldBeLessThan(0.0349);
            Math.Abs(transformaParam.Sigma- 0.0349).ShouldBeLessThan(0.001);
        }
        [Fact]
        public void TransformaSevenSigmaTest()
        {
            var standardPoints = new Dictionary<Point3D, Point3D>()
            {
                {new Point3D(-1964642.836,4484908.586,4075486.898),new Point3D(-1964734.964,4484768.547,4075386.77 )},
                {new Point3D(-1967082.716,4490541.646,4068048.151),new Point3D(-1967174.802,4490401.508,4067948.166)},
                {new Point3D(-1958106.37 ,4482074.179,4082054.321),new Point3D(-1958198.61 ,4481934.193,4081954.089)},
                {new Point3D(-1958396.995,4485396.445,4077966.297),new Point3D(-1958489.229,4485256.399,4077866.134)},
                {new Point3D(-1953364.459,4481502.655,4084942.265),new Point3D(-1953456.789,4481362.679,4084841.963)},
                {new Point3D(-1957928.755,4492765.305,4070011.563),new Point3D(-1958020.995,4492625.151,4069911.537)},
            };

            var transformaParam = new TransformationParamSeven(standardPoints);
            var sourcePoints=new List<Point3D>()
            {
                new Point3D(-1.9545e+06,4.4903e+06,4.0742e+06),
                new Point3D(-1.9520e+06,4.4853e+06,4.0811e+06),
                new Point3D(-1.9569e+06,4.4974e+06,4.0652e+06),
                new Point3D(-1.9519e+06,4.4949e+06,4.0704e+06),
                new Point3D(-1.9535e+06,4.5010e+06,4.0629e+06),
            };

            var pointTransforma = new PointTransformSeven(transformaParam);
            var targetPoint = pointTransforma.Transform(sourcePoints).ToList();
            Math.Abs(transformaParam.Sigma - 0.007287575001997314).ShouldBeLessThan(0.001);
        }
        [Fact]
        public void TransformaSevenThrowExceptionTest()
        {
            var standardPoints = new Dictionary<Point3D, Point3D>()
            {
                { new Point3D(653068.71271461656, 3281704.0923241354,0), new Point3D(651796.2895339021, 3281825.0074003497,0) },
                { new Point3D(653068.71271461656, 3281704.0923241354,0), new Point3D(651796.2895339021, 3281825.0074003497,0) }
            };
            try
            {
                new TransformationParamSeven(standardPoints);
            }
            catch (ArgumentException e)
            {
                e.ShouldNotBeNull();
            }
        }
    }
}
