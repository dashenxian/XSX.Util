using System;
using System.Collections.Generic;
using System.Text;

namespace XSX.Util
{
    public class GeometryUtility
    {
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="lineStart">线段起点</param>
        /// <param name="linEnd">线段终点</param>
        /// <param name="epsilon">误差</param>
        /// <returns></returns>
        public static bool CheckIsPointOnLineSegment(Point point, Point lineStart,Point linEnd, double epsilon = 0.1)
        {
            // 以下是另一个方法，以下方法性能比上面一个好

            // 根据点和任意线段端点连接的线段和当前线段斜率相同，同时点在两个端点中间
            // (x - x1) / (x2 - x1) = (y - y1) / (y2 - y1)
            // x1 < x < x2, assuming x1 < x2
            // y1 < y < y2, assuming y1 < y2
            // 但是需要额外处理 X1 == X2 和 Y1 == Y2 的计算

            var minX = Math.Min(lineStart.X, linEnd.X);
            var maxX = Math.Max(lineStart.X, linEnd.X);

            var minY = Math.Min(lineStart.Y, linEnd.Y);
            var maxY = Math.Max(lineStart.Y, linEnd.Y);

            if (!(minX <= point.X) || !(point.X <= maxX) || !(minY <= point.Y) || !(point.Y <= maxY))
            {
                return false;
            }

            // 以下处理水平和垂直线段
            if (Math.Abs(lineStart.X - linEnd.X) < epsilon)
            {
                // 如果 X 坐标是相同，那么只需要判断点的 X 坐标是否相同
                // 因为在上面代码已经判断了 点的 Y 坐标是在线段两个点之内
                return Math.Abs(lineStart.X - point.X) < epsilon || Math.Abs(linEnd.X - point.X) < epsilon;
            }

            if (Math.Abs(lineStart.Y - linEnd.Y) < epsilon)
            {
                return Math.Abs(lineStart.Y - point.Y) < epsilon || Math.Abs(linEnd.Y - point.Y) < epsilon;
            }

            return Math.Abs((point.X - lineStart.X) / (linEnd.X - lineStart.X) - (point.Y - lineStart.Y) / (linEnd.Y - lineStart.Y)) < epsilon;
        }
    }
}
