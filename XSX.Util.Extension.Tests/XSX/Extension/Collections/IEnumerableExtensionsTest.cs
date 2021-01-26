using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
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

        class Student
        {
            public string Name { get; set; }
            public bool? IsGrown { get; set; }
            public int Age { get; set; }
            public byte[] Photo { get; set; }
        }
    }
}
