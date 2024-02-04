using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace XSX.Util
{
    /// <summary>
    /// ENU坐标转换

    /// </summary>
    public class EnuTransform
    {
        /*
         * Cesium坐标转换
         * 1.转地心地固坐标（ECEF）
           var center = Cesium.Cartesian3.fromDegrees(longitude, latitude, height)；//其中，高度默认值为0，可以不用填写；longitude和latitude为角度           
           var positions = Cesium.Cartesian3.fromDegreesArray(coordinates);//其中，coordinates格式为不带高度的数组。例如：[-115.0, 37.0, -107.0, 33.0]           
           var positions = Cesium.Cartesian3.fromDegreesArrayHeights(coordinates);//coordinates格式为带有高度的数组。例如：[-115.0, 37.0, 100000.0, -107.0, 33.0, 150000.0]
         * 2.地心地固坐标（ECEF）转东北天坐标（ENU）
           //center为东北天坐标系的原点，center的坐标系统为ECEF
           var transform = Cesium.Transforms.eastNorthUpToFixedFrame(center); //transform为转换矩阵，与EnuTransform.Local2WorldMatrix一致
           var inv = Cesium.Matrix4.inverseTransformation(transform, new Cesium.Matrix4());
           //local为 ENU坐标, point为 ECEF坐标
           var local = Cesium.Matrix4.multiplyByPoint(inv, point, new Cesium.Cartesian3());
         */
        /// 
        /// <summary>
        /// 误差精度
        /// </summary>
        const double epsilon = 0.000000000000001;
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
        /// ENU转ECEF转换矩阵
        /// </summary>
        public Matrix<double> Local2WorldMatrix { get; private set; }
        /// <summary>
        /// 初始化转换类
        /// </summary>
        /// <param name="enuCenterL">站心经度</param>
        /// <param name="enuCenterB">站心纬度</param>
        /// <param name="enuCenterH">站心高程</param>
        public EnuTransform(double enuCenterL, double enuCenterB, double enuCenterH)
        {
            World2LocalMatrix = CalEcef2Enu(enuCenterL, enuCenterB, enuCenterH);
            Local2WorldMatrix = World2LocalMatrix.Inverse();
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
        /// ENU转为经纬度
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns></returns>
        public (double L, double B, double H) TransToBLH(double x, double y, double z)
        {
            return TransToBLH(x, y, z, Local2WorldMatrix);
        }
        /// <summary>
        /// ENU转为经纬度
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <param name="local2WorldMatrix">ECEF转ENU转换矩阵</param>
        /// <returns></returns>
        private static (double L, double B, double H) TransToBLH(double x, double y, double z, Matrix<double> local2WorldMatrix)
        {
            //(var x1, var y1, var z1) = Xyz2Blh(x, y, z);
            //var xyz = V.DenseOfArray(new double[] { -x1, -y1, -z1, 1 });
            //var enu = xyz * local2WorldMatrix;
            //return (enu[0], enu[1], enu[2]);

            var xyz = V.DenseOfArray(new double[] { x, y, z, 1 });
            var ecef = local2WorldMatrix * xyz;
            (var x1, var y1, var z1) = Xyz2Blh(ecef[0], ecef[1], ecef[2]);

            return (x1, y1, z1);
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
        /// <param name="enuCenterL">站心经度</param>
        /// <param name="enuCenterB">站心纬度</param>
        /// <param name="enuCenterH">站心高程</param>
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
        /// <summary>
        /// 经ENU转ECEF坐标
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <returns></returns>
        private static (double l, double b, double h) Xyz2Blh(double x, double y, double z)
        {
            double tmpX = x;
            double temY = y;
            double temZ = z;

            double curB = 0;
            double N = 0;
            double calB = Math.Atan2(temZ, Math.Sqrt(tmpX * tmpX + temY * temY));

            int counter = 0;
            while (Math.Abs(curB - calB) * R2D > epsilon && counter < 25)
            {
                curB = calB;
                N = a / Math.Sqrt(1 - e * e * Math.Sin(curB) * Math.Sin(curB));
                calB = Math.Atan2(temZ + N * e * e * Math.Sin(curB), Math.Sqrt(tmpX * tmpX + temY * temY));
                counter++;
            }

            x = Math.Atan2(temY, tmpX) * R2D;
            y = curB * R2D;
            z = temZ / Math.Sin(curB) - N * (1 - e * e);
            return (x, y, z);
        }
        /// <summary>
        /// 计算ENU转ECEF坐标矩阵
        /// </summary>
        /// <param name="enuCenterL">站心经度</param>
        /// <param name="enuCenterB">站心纬度</param>
        /// <param name="enuCenterH">站心高程</param>
        /// <returns></returns>
        private static Matrix<double> CalEnu2Ecef(double enuCenterL, double enuCenterB, double enuCenterH)
        {
            var topocentricOrigin = V.DenseOfArray(new double[] { enuCenterL, enuCenterB, enuCenterH });
            double rzAngle = topocentricOrigin[0] * D2R + Pi / 2;
            Matrix<double> rZ = CreateRotationMatrix(rzAngle, V.DenseOfArray(new double[] { 0, 0, 1 }));


            double rxAngle = (Pi / 2 - topocentricOrigin[1] * D2R);
            Matrix<double> rX = CreateRotationMatrix(rxAngle, V.DenseOfArray(new double[] { 1, 0, 0 }));

            Matrix<double> rotation = DenseMatrix.CreateIdentity(4);
            rotation.SetSubMatrix(0, 0, rX * rZ);
            //cout << rotation << endl;

            double tx = topocentricOrigin[0];
            double ty = topocentricOrigin[1];
            double tz = topocentricOrigin[2];
            (tx, ty, tz) = Blh2Xyz(tx, ty, tz);

            Matrix<double> translation = DenseMatrix.CreateIdentity(4);
            translation.SetColumn(3, V.DenseOfArray(new double[] { tx, ty, tz, 1 }));

            var resultMat = rotation * translation;
            return resultMat;
        }

    }
}
