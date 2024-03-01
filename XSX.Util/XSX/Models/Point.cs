using System;

namespace XSX.Models
{

    public struct Point
    {
        public Point(double x, double y) => (X, Y) = (x, y);
        public double X { get; }
        public double Y { get; }
        public static Vector2d operator -(Point a, Point b)
        {
            return new Vector2d(b.X - a.X, b.Y - a.Y);
        }
        public static Vector2d operator +(Point a, Point b)
        {
            return new Vector2d(a.X + b.X, a.Y + b.Y);
        }
        public static Point operator *(Point a, double value)
        {
            return new Point(a.X * value, a.Y * value);
        }
        public static Point operator *(double value, Point a)
        {
            return new Point(a.X * value, a.Y * value);
        }
        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }
        public override string ToString()
        {
            return $"{this.X},{this.Y}";
        }
        public readonly override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public readonly bool Equals(Point other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// 比较两个点位置小于指定误差
        /// </summary>
        /// <param name="other"></param>
        /// <param name="Tolerance"></param>
        /// <returns></returns>
        public readonly bool EqualsWithTolerance(Point other, double Tolerance)
        {
            var b = (Point)other;
            return Math.Abs(b.X - X) <= Tolerance && Math.Abs(b.Y - Y) <= Tolerance;
        }
    }
}
