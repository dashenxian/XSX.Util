using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using XSX.Extension.Collections;
using Xunit;

namespace Tests.XSX.Extension.Collections
{
    public class IEnumerableExtensionsTest
    {
        [Fact]
        public void ToDataTableTest()
        {
            var students = new List<Student>()
            {
                new Student(){Name = "小明",IsGrown = true,Age = 20,Photo = new byte[]{1,2}},
                new Student(){Name = "小画",IsGrown = null,Age = 10,Photo = new byte[]{1,2,4}},
            };
            var table = students.ToDataTable();
            table.Columns.Count.ShouldBe(4);
            table.Rows.Count.ShouldBe(2);
            table.Rows[0][0].ShouldBe("小明");
            table.Rows[0][1].ShouldBe(true);
            table.Rows[0][2].ShouldBe(20);
            table.Rows[1][1].ShouldBeOfType(typeof(DBNull));
        }

        [Fact]
        public void GetCombinationTest()
        {
            var students = new List<Student>()
            {
                new Student(){Name = "小明",IsGrown = true,Age = 20,Photo = new byte[]{1,2}},
                new Student(){Name = "小画",IsGrown = null,Age = 10,Photo = new byte[]{1,2,4}},
                new Student(){Name = "小红",IsGrown = null,Age = 10,Photo = new byte[]{1,2,4}},
            };
            var combinationList = students.GetCombination().ToList();
            combinationList.Count.ShouldBe(7);
            combinationList.Where(ls => ls.Count() == 1 && ls.Count(t => t == students[0]) == 1).Count().ShouldBe(1);
            combinationList.Where(ls => ls.Count() == 1 && ls.Count(t => t == students[1]) == 1).Count().ShouldBe(1);
            combinationList.Where(ls => ls.Count() == 1 && ls.Count(t => t == students[2]) == 1).Count().ShouldBe(1);

            combinationList.Where(ls => ls.Count() == 2
            && ls.Any(t => t == students[0])
            && ls.Any(t => t == students[1])).Count().ShouldBe(1);
            combinationList.Where(ls => ls.Count() == 2
            && ls.Any(t => t == students[0])
            && ls.Any(t => t == students[2])).Count().ShouldBe(1);
            combinationList.Where(ls => ls.Count() == 2
            && ls.Any(t => t == students[1])
            && ls.Any(t => t == students[2])).Count().ShouldBe(1);

            combinationList.Where(ls => ls.Count() == 3
            && ls.Any(t => t == students[0])
            && ls.Any(t => t == students[1])
            && ls.Any(t => t == students[2])).Count().ShouldBe(1);
        }

        [Fact]
        public void GetStrEndMaxNumberTest()
        {
            var list = new List<string> { "a1", "a2", "a123", "abc144", "ded2", "avc" };

            string maxNumericString = list.GetStrEndMaxNumber();
            maxNumericString.ShouldBe("abc144");
        }
        class Student
        {
            public string Name { get; set; }
            public bool? IsGrown { get; set; }
            public int Age { get; set; }
            public byte[] Photo { get; set; }
        }
    }
}
