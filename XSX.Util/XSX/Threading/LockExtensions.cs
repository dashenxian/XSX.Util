using System;

namespace XSX.Threading
{

    /// <summary>
    /// 加锁的扩展方法
    /// </summary>
    public static class LockExtensions
    {
        /// <summary>
        /// 使用<paramref name="source"/>加锁执行 <paramref name="action"/>
        /// </summary>
        /// <param name="source">锁定的源对象</param>
        /// <param name="action">执行的方法</param>
        public static void Locking(this object source, Action action)
        {
            lock (source)
            {
                action();
            }
        }

        /// <summary>
        /// 使用<paramref name="source"/>加锁执行 <paramref name="action"/>
        /// </summary>
        /// <typeparam name="T">源对象类型</typeparam>
        /// <param name="source">锁定的源对象</param>
        /// <param name="action">执行的方法</param>
        public static void Locking<T>(this T source, Action<T> action) where T : class
        {
            lock (source)
            {
                action(source);
            }
        }

        /// <summary>
        /// 使用<paramref name="source"/>加锁执行 <paramref name="func"/>
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="source">锁定的源对象</param>
        /// <param name="func">执行的方法</param>
        /// <returns>执行方法<paramref name="func"/>的返回结果</returns>
        public static TResult Locking<TResult>(this object source, Func<TResult> func)
        {
            lock (source)
            {
                return func();
            }
        }

        /// <summary>
        /// 使用<paramref name="source"/>加锁执行 <paramref name="func"/>
        /// </summary>
        /// <typeparam name="T">源对象类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="source">锁定的源对象</param>
        /// <param name="func">执行的方法</param>
        /// <returns>执行方法<paramref name="func"/>的返回结果</returns>
        public static TResult Locking<T, TResult>(this T source, Func<T, TResult> func) where T : class
        {
            lock (source)
            {
                return func(source);
            }
        }
    }
}