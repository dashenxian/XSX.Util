using System;
using System.Collections.Generic;
using System.Data;
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
        public static Func<DataRow, T> ToExpression<T>(DataRow dataRow) where T : class, new()
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
    }

}
