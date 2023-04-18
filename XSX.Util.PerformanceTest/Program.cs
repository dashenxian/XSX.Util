using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Security.Cryptography;
using System.Text;

namespace XSX.Util.PerformanceTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<StringReplace>();

        }


    }
    //[SimpleJob(RuntimeMoniker.Net60)]
    [RPlotExporter]
    public class StringReplace
    {
        public string Text { get; set; } = @"123*df\#dfdfds";


        [Benchmark]
        [Arguments(@"123*df\#dfdfds", "_")]
        public string MakeValidFileName(string fileName, string replacement = "_")
        {
            var str = new StringBuilder();
            var invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (var c in fileName)
            {
                if (invalidFileNameChars.Contains(c))
                {
                    str.Append(replacement ?? "");
                }
                else
                {
                    str.Append(c);
                }
            }

            return str.ToString();
        }
        [Benchmark]
        [Arguments(@"123*df\#dfdfds", "_")]
        public string MakeValidFileName2(string fileName, string replacement = "_")
        {
            var invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char c in invalidFileNameChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }
        [Benchmark]
        [Arguments(@"123*df\#dfdfds", '_')]
        public string MakeValidFileName3(string fileName, char replacement = '_')
        {
            var invalidChars = new HashSet<char>(Path.GetInvalidFileNameChars());
            return new string(fileName.Select(c => invalidChars.Contains(c) ? replacement : c).ToArray());
        }
    }

}