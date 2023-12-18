using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using XSX.Util;
using Xunit;

namespace Tests.XSX.Util
{
    public class GeometryUtilityTest
    {
        [Theory]
        [InlineData(1, 0, 0, 0, 10, 0, true)]
        [InlineData(0, 1, 0, 0, 0, 10, true)]
        [InlineData(5, 5, 0, 0, 10, 10, true)]
        [InlineData(5.2, 5, 0, 0, 10, 10, false)]
        //[InlineData(600, 010, 0, 0, 1000, 10, false)]
        public void CheckIsPointOnLineSegmentTest(
            double pointX, double pointY,
            double pointStartX, double pointStartY,
            double pointEndX, double pointEndY,
            bool expectResult
            )
        {
            var point = new Point(pointX, pointY);
            var pointStart = new Point(pointStartX, pointStartY);
            var pointEnd = new Point(pointEndX, pointEndY);
            var result = GeometryUtility.CheckIsPointOnLineSegment(point, pointStart, pointEnd);
            result.ShouldBe(expectResult);
        }

    }
}
