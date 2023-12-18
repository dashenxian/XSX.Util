using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace XSX.Util
{
    public class EnuTransform
    {
        /// <summary>
        /// 圆周率
        /// </summary>
        const double Pi = Math.PI;
        /// <summary>
        /// 角度转弧度常量
        /// </summary>
        const double D2R = Pi / 180.0;
        /// <summary>
        /// 弧度转角度常量
        /// </summary>
        const double R2D = 180.0 / Pi;
        /// <summary>
        /// 地球长半轴
        /// </summary>
        const double a = 6378137.0;
        //const double f_inverse = 298.257223563;         //WGS84扁率倒数
        /// <summary>
        /// CGCS2000扁率倒数
        /// </summary>
        const double f_inverse = 298.257222101;         //CGCS2000扁率倒数
        const double b = a - a / f_inverse;
        static readonly double e = Math.Sqrt(a * a - b * b) / a;
        /// <summary>
        /// 向量对象构造器
        /// </summary>
        static readonly VectorBuilder<double> V = Vector<double>.Build;
        /// <summary>
        /// ECEF转ENU转换矩阵
        /// </summary>
        public Matrix<double> World2LocalMatrix { get; private set; }
        /// <summary>
        /// 初始化转换类
        /// </summary>
        /// <param name="enuCenterL">经度</param>
        /// <param name="enuCenterB">纬度</param>
        /// <param name="enuCenterH">高程</param>
        public EnuTransform(double enuCenterL, double enuCenterB, double enuCenterH)
        {

            World2LocalMatrix = CalEcef2Enu(enuCenterL, enuCenterB, enuCenterH);
        }
        /// <summary>
        /// 经纬度转为enu坐标
        /// </summary>
        /// <param name="l">经度</param>
        /// <param name="b">纬度</param>
        /// <param name="h">高程</param>
        /// <returns></returns>
        public (double x, double y, double z) TransToEnu(double l, double b, double h)
        {
            return TransToEnu(l, b, h, World2LocalMatrix);
        }
        /// <summary>
        /// 经纬度转为enu坐标
        /// </summary>
        /// <param name="l">经度</param>
        /// <param name="b">纬度</param>
        /// <param name="h">高程</param>
        /// <param name="world2LocalMatrix">ECEF转ENU转换矩阵</param>
        /// <returns></returns>
        private static (double x, double y, double z) TransToEnu(double l, double b, double h, Matrix<double> world2LocalMatrix)
        {
            (var x1, var y1, var z1) = Blh2Xyz(l, b, h);
            var xyz = V.DenseOfArray(new double[] { x1, y1, z1, 1 });
            var enu = world2LocalMatrix * xyz;
            return (enu[0], enu[1], enu[2]);
        }
        /// <summary>
        /// 经纬度转ECEF坐标
        /// </summary>
        /// <param name="l">经度</param>
        /// <param name="b">纬度</param>
        /// <param name="h">高程</param>
        /// <returns></returns>
        private static (double x, double y, double z) Blh2Xyz(double l, double b, double h)
        {
            var l1 = l * D2R;
            var b1 = b * D2R;
            var h1 = h;

            var n = a / Math.Sqrt(1 - e * e * Math.Sin(b1) * Math.Sin(b1));
            var x1 = (n + h1) * Math.Cos(b1) * Math.Cos(l1);
            var y1 = (n + h1) * Math.Cos(b1) * Math.Sin(l1);
            var z1 = (n * (1 - e * e) + h1) * Math.Sin(b1);
            return (x1, y1, z1);
        }
        /// <summary>
        /// 计算ECEF转ENU坐标矩阵
        /// </summary>
        /// <param name="enuCenterL">经度</param>
        /// <param name="enuCenterB">纬度</param>
        /// <param name="enuCenterH">高程</param>
        /// <returns></returns>
        private static Matrix<double> CalEcef2Enu(double enuCenterL, double enuCenterB, double enuCenterH)
        {
            var topocentricOrigin = V.DenseOfArray(new double[] { enuCenterL, enuCenterB, enuCenterH });
            double rzAngle = -(topocentricOrigin[0] * D2R + Pi / 2);
            Matrix<double> rZ = CreateRotationMatrix(rzAngle, V.DenseOfArray(new double[] { 0, 0, 1 }));

            double rxAngle = -(Pi / 2 - topocentricOrigin[1] * D2R);
            Matrix<double> rX = CreateRotationMatrix(rxAngle, V.DenseOfArray(new double[] { 1, 0, 0 }));

            Matrix<double> rotation = DenseMatrix.CreateIdentity(4);
            rotation.SetSubMatrix(0, 0, rX * rZ);

            double tx = topocentricOrigin[0];
            double ty = topocentricOrigin[1];
            double tz = topocentricOrigin[2];
            (tx, ty, tz) = Blh2Xyz(tx, ty, tz);

            Matrix<double> translation = DenseMatrix.CreateIdentity(4);
            translation.SetColumn(3, V.DenseOfArray(new double[] { -tx, -ty, -tz, 1 }));

            var resultMat = rotation * translation;
            return resultMat;
        }
        /// <summary>
        /// 旋转矩阵
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private static Matrix<double> CreateRotationMatrix(double angle, Vector<double> axis)
        {
            var rotation = Matrix<double>.Build.DenseIdentity(3);

            double cosTheta = Math.Cos(angle);
            double sinTheta = Math.Sin(angle);
            double oneMinusCosTheta = 1 - cosTheta;

            var rotationAxis = axis;
            rotationAxis = rotationAxis.Normalize(2);

            rotation[0, 0] = cosTheta + (1 - cosTheta) * rotationAxis[0] * rotationAxis[0];
            rotation[0, 1] = rotationAxis[0] * rotationAxis[1] * oneMinusCosTheta - rotationAxis[2] * sinTheta;
            rotation[0, 2] = rotationAxis[0] * rotationAxis[2] * oneMinusCosTheta + rotationAxis[1] * sinTheta;

            rotation[1, 0] = rotationAxis[1] * rotationAxis[0] * oneMinusCosTheta + rotationAxis[2] * sinTheta;
            rotation[1, 1] = cosTheta + (1 - cosTheta) * rotationAxis[1] * rotationAxis[1];
            rotation[1, 2] = rotationAxis[1] * rotationAxis[2] * oneMinusCosTheta - rotationAxis[0] * sinTheta;

            rotation[2, 0] = rotationAxis[2] * rotationAxis[0] * oneMinusCosTheta - rotationAxis[1] * sinTheta;
            rotation[2, 1] = rotationAxis[2] * rotationAxis[1] * oneMinusCosTheta + rotationAxis[0] * sinTheta;
            rotation[2, 2] = cosTheta + (1 - cosTheta) * rotationAxis[2] * rotationAxis[2];

            return rotation;
        }
    }
}
