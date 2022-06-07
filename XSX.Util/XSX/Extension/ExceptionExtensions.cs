using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;

namespace XSX.Extension
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Uses <see cref="Capture"/> method to re-throws exception
        /// while preserving stack trace.
        /// </summary>
        /// <param name="exception">Exception to be re-thrown</param>
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
        /// <summary>
        /// Uses <see cref="AggregateException"/> method to re-throws exception
        /// while preserving stack trace.
        /// </summary>
        /// <param name="exceptions">Exception to be re-thrown</param>
        public static void ReThrow(this IEnumerable<Exception> exceptions)
        {
            throw new AggregateException(exceptions);
        }
    }
}
