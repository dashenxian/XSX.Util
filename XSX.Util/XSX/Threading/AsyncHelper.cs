using JetBrains.Annotations;
using Nito.AsyncEx;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace XSX.Threading
{
    /// <summary>
    /// 提供一些与异步方法一起使用的辅助方法。
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// 检查给定方法是否为异步方法。
        /// </summary>
        /// <param name="method">要检查的方法</param>
        public static bool IsAsyncMethod([NotNull] this MethodInfo method)
        {
            return method.ReturnType == typeof(Task) ||
                   (method.ReturnType.GetTypeInfo().IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
        }
        /// <summary>
        /// 判断类型是否为Task或Task`T`
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTaskOrTaskOfT(this Type type)
        {
            if (type == typeof(Task))
            {
                return true;
            }
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }
        /// <summary>
        /// 判断类型是否为Task`T`
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTaskOfT(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }
        /// <summary>
        /// 获取Task包装里的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type UnwrapTask(Type type)
        {
            Check.NotNull<Type>(type, nameof(type));
            if (type == typeof(Task))
            {
                return typeof(void);
            }
            return type.IsTaskOfT() ? type.GenericTypeArguments[0] : type;
        }
        /// <summary>
        /// 同步运行异步方法。
        /// </summary>
        /// <param name="func">异步方法</param>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <returns>异步操作的结果</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        /// <summary>
        /// 同步运行异步方法。
        /// </summary>
        /// <param name="action">异步方法</param>
        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}
