using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Shouldly;
using XSX.Extension;
using Xunit;
using Xunit.Abstractions;

namespace Tests.XSX.Extension
{
    public class StringExtensionsTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public StringExtensionsTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Fact]
        public void IsNotNullAndEmptyTest()
        {
            string str = null;
            var result = str.IsNotNullAndEmpty();
            result.ShouldBe(false);
            str = "";
            result = str.IsNotNullAndEmpty();
            result.ShouldBe(false);
            str = " ";
            result = str.IsNotNullAndEmpty();
            result.ShouldBe(true);
        }

        [Fact]
        public void IsNullOrEmptyTest()
        {
            string str = null;
            var result = str.IsNullOrEmpty();
            result.ShouldBe(true);
            str = "";
            result = str.IsNullOrEmpty();
            result.ShouldBe(true);
            str = " ";
            result = str.IsNullOrEmpty();
            result.ShouldBe(false);
        }
        [Fact]
        public void ToSentenceCaseTest()
        {
            var str = "ToSentenceCaseTest";
            var result = str.ToSentenceCase();
            result.ShouldBe("To sentence case test");
        }
        [Fact]
        public void ToMd5Test()
        {
            var str = "ToMd5Test";
            var result1 = str.ToMd5();
            var result2 = str.EncryptMd5();
            result1.ShouldBe(result2);
        }
    }
}
