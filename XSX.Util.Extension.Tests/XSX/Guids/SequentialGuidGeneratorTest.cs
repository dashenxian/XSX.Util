using Shouldly;
using System.Threading;
using XSX.Guids;
using Xunit;

namespace Tests.XSX.Guids
{
    public class SequentialGuidGeneratorTest
    {

        /// <summary>
        /// 测试SequentialGuidGenerator无参数的创建GUID方法
        /// </summary>
        [Fact]
        public void CreateTestNoParamter()
        {
            //测试SequentialGuidGenerator.Create()方法
            IGuidGenerator generator = new SequentialGuidGenerator(new SequentialGuidGeneratorOptions());
            var guid1 = generator.Create();
            Thread.Sleep(0);
            var guid2 = generator.Create();
            guid2.ShouldBeGreaterThan(guid1);
        }
    }
}
