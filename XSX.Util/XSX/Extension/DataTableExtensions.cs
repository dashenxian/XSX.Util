using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace XSX.Extension
{
    public static class DataTableExtensions
    {

        /// <summary>
        /// DataTable生成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToList<T>(this DataTable dataTable) where T : class, new()
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            List<T> collection = new List<T>(dataTable.Rows.Count);
            if (dataTable.Rows.Count == 0)
            {
                return collection;
            }
            Func<DataRow, T> func = ToExpression<T>(dataTable.Rows[0]);

            foreach (DataRow dr in dataTable.Rows)
            {
                collection.Add(func(dr));
            }
            return collection;
        }

        /// <summary>
        /// 生成表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private static Func<DataRow, T> ToExpression<T>(DataRow dataRow) where T : class, new()
        {
            if (dataRow == null) throw new ArgumentNullException("dataRow", "当前对象为null 无法转换成实体");
            ParameterExpression paramter = Expression.Parameter(typeof(DataRow), "dr");
            List<MemberBinding> binds = new List<MemberBinding>();
            for (int i = 0; i < dataRow.ItemArray.Length; i++)
            {
                String colName = dataRow.Table.Columns[i].ColumnName;
                PropertyInfo pInfo = typeof(T).GetProperty(colName);
                if (pInfo == null) continue;
                MethodInfo mInfo = typeof(DataRowExtensions).GetMethod("Field", new Type[] { typeof(DataRow), typeof(String) }).MakeGenericMethod(pInfo.PropertyType);
                MethodCallExpression call = Expression.Call(mInfo, paramter, Expression.Constant(colName, typeof(String)));
                MemberAssignment bind = Expression.Bind(pInfo, call);
                binds.Add(bind);
            }
            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(T)), binds.ToArray());
            return Expression.Lambda<Func<DataRow, T>>(init, paramter).Compile();
        }
        /// <summary>
        /// 合并两个表格列，行数按顺序对应，列顺序不保证与原来相同，请使用列名代替列序号访问合并后的数据
        /// </summary>
        /// <param name="table1">连接左表</param>
        /// <param name="table2">连接右表</param>
        /// <returns>连接后的表</returns>
        public static DataTable Join(this DataTable table1, DataTable table2)
        {
            if (table1 == null && table2 == null)
            {
                return new DataTable();
            }
            if (table1 == null || table1.Columns.Count == 0)
            {
                return table2.Copy();
            }
            if (table2 == null || table2.Columns.Count == 0)
            {
                return table1.Copy();
            }

            var tableLeft = table1;
            var tableRight = table2;
            if (tableLeft.Rows.Count == 0)
            {
                tableLeft = table2;
                tableRight = table1;
            }
            var leftJoin =
                (from t1 in tableLeft.AsEnumerable().Select((row, index) => new { Row = row, Index = index })
                 join t2 in tableRight.AsEnumerable().Select((row, index) => new { Row = row, Index = index })
                     on t1.Index equals t2.Index into t2Group
                 from t2 in t2Group.DefaultIfEmpty()
                 select t1.Row.ItemArray.Concat(t2 == null ? Enumerable.Repeat(DBNull.Value, tableRight.Columns.Count) : t2.Row.ItemArray).ToArray()).ToList();

            var result = new DataTable();
            foreach (DataColumn col in tableLeft.Columns)
            {
                result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataColumn col in tableRight.Columns)
            {
                result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (var row in leftJoin)
            {
                result.Rows.Add(row);
            }

            return result;
        }
    }

}
