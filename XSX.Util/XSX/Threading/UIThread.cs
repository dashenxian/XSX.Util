using System;
using System.Threading;

namespace XSX.Threading
{
    /// <summary>
    /// UI线程全局类,使用前应该在UI线程中调用Init初始化
    /// </summary>
    public class UIThread
    {
        private static SynchronizationContext Context { get; set; }

        /// <summary>
        /// 同步更新UI控件的属性及绑定数据源
        /// </summary>
        /// <param name="act"></param>
        /// <param name="state"></param>
        public static void Send(Action<object> act, object state)
        {
            Context.Send(obj => { act(obj); }, state);
        }

        /// <summary>
        /// 同步更新UI控件的属性及绑定数据源
        /// </summary>
        /// <param name="act"></param>
        public static void Send(Action act)
        {
            Context.Send(_ => { act(); }, null);
        }

        /// <summary>
        /// 异步更新UI控件的属性及绑定数据源,Post将不会等委托方法的执行完成
        /// </summary>
        /// <param name="act"></param>
        /// <param name="state"></param>
        public static void Post(Action<object> act, object state)
        {
            Context.Post(obj => { act(obj); }, state);
        }

        /// <summary>
        /// 异步更新UI控件的属性及绑定数据源,Post将不会等委托方法的执行完成
        /// </summary>
        /// <param name="act"></param>
        public static void Post(Action act)
        {
            Context.Post(_ => { act(); }, null);
        }

        /// <summary>
        /// 在UI线程中初始化，只取第一次初始化时的同步上下文
        /// </summary>
        public static void Init()
        {
            if (Context == null)
            {
                Context = SynchronizationContext.Current;
            }
        }
    }
}
