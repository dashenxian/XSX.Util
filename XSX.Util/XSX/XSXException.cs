using System;

namespace XSX
{
    public class XSXException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="XSXException"/> object.
        /// </summary>
        public XSXException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="XSXException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public XSXException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="XSXException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public XSXException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
