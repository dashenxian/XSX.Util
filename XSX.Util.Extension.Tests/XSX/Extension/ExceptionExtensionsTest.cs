using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using XSX.Extension;
using Xunit;

namespace Tests.XSX.Extension
{

    public class ExceptionExtensionsTest
    {
        [Fact]
        public void ReThrow_Exception_Test()
        {
            Exception reThrowEx = null;
            try
            {
                ThrowExceptionReThrow();
            }
            catch (Exception ex)
            {
                reThrowEx = ex;
            }
            reThrowEx.StackTrace.ShouldContain("g__ThrowException|");

            void ThrowExceptionReThrow()
            {
                try
                {
                    ThrowException();
                }
                catch (Exception e)
                {
                    e.ReThrow();
                }
            }
            void ThrowException()
            {
                throw new Exception("test");
            }
        }
        [Fact]
        public void ReThrow_Exceptions_Test()
        {
            var exCount = 5;
            AggregateException reThrowEx = null;
            try
            {
                ThrowExceptionReThrow();
            }
            catch (Exception ex)
            {
                reThrowEx = ex as AggregateException;
            }
            reThrowEx.InnerExceptions.Count.ShouldBe(exCount);

            void ThrowExceptionReThrow()
            {
                var exs = new List<Exception>();
                foreach (var VARIABLE in Enumerable.Range(0, exCount))
                {
                    try
                    {
                        ThrowException();
                    }
                    catch (Exception e)
                    {
                        exs.Add(e);
                    }
                }
                exs.ReThrow();
            }
            void ThrowException()
            {
                throw new Exception("test");
            }
        }
    }
}
