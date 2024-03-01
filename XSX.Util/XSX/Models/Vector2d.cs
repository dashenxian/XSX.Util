namespace XSX.Models
{
    public struct Vector2d
    {
        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public static Vector2d XAxis => new Vector2d(1.0, 0.0);
        public static Vector2d YAxis => new Vector2d(0.0, 1.0);
        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            return new Vector2d(b.X - a.X, b.Y - a.Y);
        }
        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.X + b.X, a.Y + b.Y);
        }
        public static double operator *(Vector2d a, Vector2d b)
        {
            return a.X * b.Y - b.X * a.Y;
        }
    }
}
