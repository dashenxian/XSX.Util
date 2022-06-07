using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.System.Linq
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void Distinct_Object_MultiPropt()
        {
            var students = new List<Student>{
                new Student{Name="小明",Age=8,IsMan=false},
                new Student{Name="小明",Age=8,IsMan=true},
                new Student{Name="小明1",Age=9,IsMan=true},
                new Student{Name="小明1",Age=9,IsMan=false},
                new Student{Name="小明2",Age=10,IsMan=false},
            };
            var ls = students.Distinct(s => new { s.Name, s.Age });
            ls.Count().ShouldBe(3);

            ls = students.Distinct(s => new { s.Name, s.IsMan });
            ls.Count().ShouldBe(5);
        }
        [Fact]
        public void Distinct_Object_SinglePropt()
        {
            var students = new List<Student>{
                new Student{Name="小明",Age=11,IsMan=false},
                new Student{Name="小明",Age=8,IsMan=true},
                new Student{Name="小明",Age=9,IsMan=true},
                new Student{Name="小明1",Age=9,IsMan=false},
                new Student{Name="小明2",Age=10,IsMan=false},
            };
            var ls = students.Distinct(s => s.Name);
            ls.Count().ShouldBe(3);
        }
        [Fact]
        public void Distinct_Int()
        {
            var students = new List<int> { 1, 2, 3, 4, 5, 1, 2 };
            var ls = students.Distinct(s => s);
            ls.Count().ShouldBe(5);
        }
        [Fact]
        public void Distinct_String()
        {
            var students = new List<string> { "1", "2", "3", "4", "5", "1", "2" };
            var ls = students.Distinct(s => s);
            ls.Count().ShouldBe(5);
        }
        class Student
        {
            public int Age { get; set; }
            public string Name { get; set; }
            public bool IsMan { get; set; }
        }
    }
}
