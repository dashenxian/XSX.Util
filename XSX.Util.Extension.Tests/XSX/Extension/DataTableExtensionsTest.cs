using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Shouldly;
using XSX.Extension;
using Xunit;
using Xunit.Abstractions;

namespace Tests.XSX.Extension
{
    public class DataTableExtensionsTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DataTableExtensionsTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Fact]
        public void JoinTest()
        {
            DataTable table1 = new DataTable();
            table1.Columns.Add("ca1");
            table1.Columns.Add("ca2");
            table1.Rows.Add("ca2", "ca3");
            table1.Rows.Add("ca4", "ca35");
            var table2 = new DataTable();
            table2.Columns.Add("cb1");
            table2.Columns.Add("cb2");
            table2.Rows.Add("c22", "c23");
            table2.Rows.Add(null, "c43");
            var result = table1.Join(table2);
            result.Rows.Count.ShouldBe(2);
            result.Columns.Count.ShouldBe(4);
            result.Rows[0]["ca1"].ShouldBe("ca2");
            result.Rows[0]["ca2"].ShouldBe("ca3");
            result.Rows[0]["cb1"].ShouldBe("c22");
            result.Rows[0]["cb2"].ShouldBe("c23");
            result.Rows[1]["ca1"].ShouldBe("ca4");
            result.Rows[1]["ca2"].ShouldBe("ca35");
            result.Rows[1]["cb1"].ShouldBeOfType<DBNull>();
            result.Rows[1]["cb2"].ShouldBe("c43");
        }
        [Fact]
        public void JoinLeftIsNullTest()
        {
            DataTable table1 = null;
            var table2 = new DataTable();
            table2.Columns.Add("c2");
            table2.Rows.Add("c22");
            var result = table1.Join(table2);
            result.Rows.Count.ShouldBe(1);
            result.Columns.Count.ShouldBe(1);
        }
        [Fact]
        public void JoinRightIsNullTest()
        {
            DataTable table1 = null;
            var table2 = new DataTable();
            table2.Columns.Add("c2");
            table2.Columns.Add("c3");
            table2.Rows.Add("c22", "c23");
            table2.Rows.Add("c32", "c33");
            var result = table2.Join(table1);
            result.Rows.Count.ShouldBe(2);
            result.Columns.Count.ShouldBe(2);
            result.Rows[0]["c2"].ShouldBe("c22");
            result.Rows[0]["c3"].ShouldBe("c23");
            result.Rows[1]["c2"].ShouldBe("c32");
            result.Rows[1]["c3"].ShouldBe("c33");
        }
        [Fact]
        public void JoinLeftNoRowsTest()
        {
            var table1 = new DataTable();
            table1.Columns.Add("c1");
            var table2 = new DataTable();
            table2.Columns.Add("c2");
            table2.Columns.Add("c3");
            table2.Rows.Add("c22", "c23");
            table2.Rows.Add("c32", "c33");
            var result = table1.Join(table2);
            result.Rows.Count.ShouldBe(2);
            result.Columns.Count.ShouldBe(3);
            result.Rows[0]["c2"].ShouldBe("c22");
            result.Rows[0]["c3"].ShouldBe("c23");
            result.Rows[1]["c2"].ShouldBe("c32");
            result.Rows[1]["c3"].ShouldBe("c33");
        }

        [Fact]
        public void JoinRightNoRowsTest()
        {
            var table1 = new DataTable();
            table1.Columns.Add("c1");
            var table2 = new DataTable();
            table2.Columns.Add("c2");
            table2.Rows.Add("c22");
            var result = table2.Join(table1);
            result.Rows.Count.ShouldBe(1);
            result.Columns.Count.ShouldBe(2);
        }
        [Fact]
        public void JoinLeftNoColumnsTest()
        {
            var table1 = new DataTable();
            //table1.Columns.Add("c1");
            var table2 = new DataTable();
            table2.Columns.Add("c2");
            table2.Rows.Add("c22");
            var result = table1.Join(table2);
            result.Rows.Count.ShouldBe(1);
            result.Columns.Count.ShouldBe(1);
        }
        [Fact]
        public void JoinRightNoColumnsTest()
        {
            var table1 = new DataTable();
            //table1.Columns.Add("c1");
            var table2 = new DataTable();
            table2.Columns.Add("c2");
            table2.Rows.Add("c22");
            var result = table2.Join(table1);
            result.Rows.Count.ShouldBe(1);
            result.Columns.Count.ShouldBe(1);
        }
    }
}
