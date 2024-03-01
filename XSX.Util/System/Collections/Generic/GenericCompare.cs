namespace System.Collections.Generic
{
    /// <summary>
    /// 比较器，自定义Distinct比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCompare<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> Expr { get; set; }
        public GenericCompare(Func<T, T, bool> expr)
        {
            this.Expr = expr;
        }
        public bool Equals(T x, T y)
        {
            if (Expr(x, y))
                return true;
            else
                return false;
        }
        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}
