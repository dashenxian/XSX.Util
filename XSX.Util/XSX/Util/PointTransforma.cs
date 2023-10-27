using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace XSX.Util
{
    public class PointTransforma
    {
        public TransformationParam Transformation { get; }
        private Vector<double> Delta { get; }
        private Matrix<double> ScaleAndRoate { get; }
        private VectorBuilder<double> V { get; }
        public PointTransforma(TransformationParam transformation)
        {
            Transformation = transformation;
            V = Vector<double>.Build;
            Delta = V.Dense(new double[] { Transformation.DeltaX, Transformation.DeltaY });
            this.ScaleAndRoate = Transformation.Scale * Matrix<double>.Build.Dense(2, 2, new double[]
            {
                Math.Cos(Transformation.Theta),
                Math.Sin(Transformation.Theta),
                -Math.Sin(Transformation.Theta),
                Math.Cos(Transformation.Theta),
            });
        }

        public Point Transforma(Point p)
        {
            var target = Delta + ScaleAndRoate * V.Dense(new double[] { p.X, p.Y });
            return new Point() { X = target[0], Y = target[1] };
        }
    }
    public class TransformationParam
    {
        /// <summary>
        /// x偏移量
        /// </summary>
        public double DeltaX { get; private set; }
        /// <summary>
        /// y偏移量
        /// </summary>
        public double DeltaY { get; private set; }
        /// <summary>
        /// 缩放比例
        /// </summary>
        public double Scale { get; private set; }
        /// <summary>
        /// 旋转角
        /// </summary>
        public double Theta { get; private set; }
        public TransformationParam(double deltaX, double deltaY, double scale, double theta)
        {
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.Scale = scale;
            this.Theta = theta;
        }
        /// <summary>
        /// 计算转换参数
        /// </summary>
        /// <param name="standardPoints">标准点<![CDATA[<source,targe>]]></param>
        /// <returns></returns>
        public TransformationParam(Dictionary<Point, Point> standardPoints)
        {
            if (standardPoints.Count<2)
            {
                throw new ArgumentException("标准点数量不能小于2对。");
            }
            var M = Matrix<double>.Build;
            var L = M.Dense(1, 2 * standardPoints.Count,
                standardPoints.SelectMany(d => new double[] { d.Value.X - d.Key.X, d.Value.Y - d.Key.Y }).ToArray()).Transpose();
            var B = M.Dense(4, 2 * standardPoints.Count, standardPoints.SelectMany(d => new[] { 1, 0, d.Key.X, -d.Key.Y, 0, 1, d.Key.Y, d.Key.X }).ToArray()).Transpose();
            var P = M.DenseIdentity(standardPoints.Count*2);
            var X = (B.Transpose() * P * B).Inverse() * B.Transpose() * P * L;
            this.DeltaX = X[0, 0];
            this.DeltaY = X[1, 0];
            var a = X[2, 0];
            var b = X[3, 0];
            this.Scale = Math.Sqrt(Math.Pow(a + 1, 2) + Math.Pow(b, 2));
            this.Theta = Math.Atan(b / (a + 1));
        }
    }
    public class Point
    {
        public Point()
        {

        }
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
