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
            var result2 = str.EncryptMD5();
            result1.ShouldBe(result2);
        }
    }
}
