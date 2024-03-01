using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using XSX.Models;

namespace XSX.Util
{
    /// <summary>
    /// 四参数转换
    /// </summary>
    public class PointTransformFour
    {
        /// <summary>
        /// 转换参数
        /// </summary>
        public TransformationParamFour Transformation { get; }
        /// <summary>
        /// 三个坐标平移量
        /// </summary>
        private Vector<double> Delta { get; }
        /// <summary>
        /// 旋转和缩放量
        /// </summary>
        private Matrix<double> ScaleAndRoate { get; }
        private VectorBuilder<double> V { get; }
        /// <summary>
        /// 初始化四参数转换
        /// </summary>
        /// <param name="transformation">转换参数</param>
        public PointTransformFour(TransformationParamFour transformation)
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
        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="p">要转换的点</param>
        /// <returns></returns>
        public Point Transform(Point p)
        {
            var target = Delta + ScaleAndRoate * V.Dense(new double[] { p.X, p.Y });
            return new Point(target[0], target[1]);
        }
        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="points">要转换的点列表</param>
        /// <returns></returns>
        public IEnumerable<Point> Transform(IEnumerable<Point> points)
        {
            return points.Select(d => Transform(d));
        }
    }
    /// <summary>
    /// 四参数转换模型
    /// </summary>
    public class TransformationParamFour
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
        public TransformationParamFour(double deltaX, double deltaY, double scale, double theta)
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
        public TransformationParamFour(Dictionary<Point, Point> standardPoints)
        {
            if (standardPoints.Count < 2)
            {
                throw new ArgumentException("标准点数量不能小于2对。");
            }
            var M = Matrix<double>.Build;
            var L = M.Dense(1, 2 * standardPoints.Count,
                standardPoints.SelectMany(d => new double[] { d.Value.X - d.Key.X, d.Value.Y - d.Key.Y }).ToArray()).Transpose();
            var B = M.Dense(4, 2 * standardPoints.Count, standardPoints.SelectMany(d => new[] { 1, 0, d.Key.X, -d.Key.Y, 0, 1, d.Key.Y, d.Key.X }).ToArray()).Transpose();
            var P = M.DenseIdentity(standardPoints.Count * 2);
            var X = (B.Transpose() * P * B).Inverse() * B.Transpose() * P * L;
            this.DeltaX = X[0, 0];
            this.DeltaY = X[1, 0];
            var a = X[2, 0];
            var b = X[3, 0];
            this.Scale = Math.Sqrt(Math.Pow(a + 1, 2) + Math.Pow(b, 2));
            this.Theta = Math.Atan(b / (a + 1));
        }
    }
    /// <summary>
    /// 布尔莎七参数坐标转换
    /// </summary>
    public class PointTransformSeven
    {
        /// <summary>
        /// 转换参数
        /// </summary>
        public TransformationParamSeven Transformation { get; }
        /// <summary>
        /// 转换参数
        /// </summary>
        private Matrix<double> ScaleAndRoate { get; }
        /// <summary>
        /// 矩阵初始化器
        /// </summary>
        private MatrixBuilder<double> M { get; }
        /// <summary>
        /// 初始化布尔莎七参数坐标转换
        /// </summary>
        /// <param name="transformation">转换参数</param>
        public PointTransformSeven(TransformationParamSeven transformation)
        {
            Transformation = transformation;
            M = Matrix<double>.Build;
            this.ScaleAndRoate = M.Dense(7, 1, new double[]
            {
                Transformation.DeltaX,
                Transformation.DeltaY,
                Transformation.DeltaZ,
                Transformation.ThetaX,
                Transformation.ThetaY,
                Transformation.ThetaZ,
                Transformation.Scale
            });
        }
        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="p">要转换的点</param>
        /// <returns></returns>
        public Point3D Transform(Point3D p)
        {
            var B = M.Dense(7, 3, new double[] {
                    1,0,0,0,-p.Z,p.Y,p.X,
                    0,1,0,p.Z, 0,-p.X, p.Y,
                    0,0,1,-p.Y, p.X,0, p.Z
                        }).Transpose();
            var target = B.Multiply(ScaleAndRoate);
            return new Point3D() { X = target[0, 0] + p.X, Y = target[1, 0] + p.Y, Z = target[2, 0] + p.Z };
        }
        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="points">要转换的点列表</param>
        /// <returns></returns>
        public IEnumerable<Point3D> Transform(IEnumerable<Point3D> points)
        {
            return points.Select(d => Transform(d));
        }
    }
    /// <summary>
    /// 布尔莎七参数模型
    /// </summary>
    public class TransformationParamSeven
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
        /// Z偏移量
        /// </summary>
        public double DeltaZ { get; private set; }
        /// <summary>
        /// 缩放比例
        /// </summary>
        public double Scale { get; private set; }
        /// <summary>
        /// 旋转角X
        /// </summary>
        public double ThetaX { get; private set; }
        /// <summary>
        /// 旋转角Y
        /// </summary>
        public double ThetaY { get; private set; }
        /// <summary>
        /// 旋转角Z
        /// </summary>
        public double ThetaZ { get; private set; }
        /// <summary>
        /// 精度评定，中误差
        /// </summary>
        public double Sigma { get; set; }
        public TransformationParamSeven(Dictionary<Point3D, Point3D> standardPoints)
        {
            if (standardPoints.Count < 3)
            {
                throw new ArgumentException("标准点数量不能小于3对。");
            }
            var M = Matrix<double>.Build;
            var L = M.Dense(1, 3 * standardPoints.Count,
                standardPoints.SelectMany(d => new double[] { d.Value.X - d.Key.X, d.Value.Y - d.Key.Y, d.Value.Z - d.Key.Z }).ToArray()).Transpose();
            var B = M.Dense(7, 3 * standardPoints.Count, standardPoints.SelectMany(d => new[]
            {
                1, 0, 0, 0, - d.Key.Z, d.Key.Y, d.Key.X,
                0, 1, 0, d.Key.Z, 0, -d.Key.X, d.Key.Y,
                0, 0, 1, -d.Key.Y, d.Key.X, 0, d.Key.Z
            }).ToArray()).Transpose();
            var X = (B.Transpose() * B).Inverse() * (B.Transpose() * L);
            this.DeltaX = X[0, 0];
            this.DeltaY = X[1, 0];
            this.DeltaZ = X[2, 0];
            var a1 = X[3, 0];
            var a2 = X[4, 0];
            var a3 = X[5, 0];
            var a4 = X[6, 0];
            this.ThetaX = a1;
            this.ThetaY = a2;
            this.ThetaZ = a3;
            this.Scale = a4;
            var V = B * X - L;
            var P = M.DenseIdentity(standardPoints.Count * 3);
            Sigma = Math.Sqrt((V.Transpose() * P * V)[0, 0] / (standardPoints.Count * 3 - 7));
        }
    }

    public class Point3D
    {
        public Point3D()
        {

        }
        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            Z = z;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
