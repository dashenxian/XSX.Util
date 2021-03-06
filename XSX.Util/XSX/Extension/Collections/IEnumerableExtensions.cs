﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace XSX.Extension.Collections
{
    public static class IEnumerableExtensions
    {

        /// <summary>
        /// 集合转换成DataTable
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="tableName">表名称</param>
        /// <returns>转换完成的DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string tableName = "TempTable")
        {
            //如果需要剔除某些列可以修改这段代码
            var propertyList = typeof(T).GetProperties().Where(w => w.CanRead).ToArray();
            var columns = new ReadOnlyCollection<DataColumn>(propertyList
                .Select(pr => new DataColumn(pr.Name, GetDataType(pr.PropertyType))).ToArray());
            //生成对象转数据行委托
            var toRowData = BuildToRowDataDelegation<T>(typeof(T), propertyList);

            //创建表对象
            var table = new DataTable(tableName);
            //设置列
            foreach (var dataColumn in columns)
            {
                table.Columns.Add(new DataColumn(dataColumn.ColumnName, dataColumn.DataType));
            }

            //循环转换每一行数据
            foreach (var item in source)
            {
                table.Rows.Add(toRowData.Invoke(item));
            }

            //返回表对象
            return table;
        }
        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <param name="type">属性类型</param>
        /// <returns>数据类型</returns>
        private static Type GetDataType(Type type)
        {
            //枚举默认转换成对应的值类型
            if (type.IsEnum)
                return type.GetEnumUnderlyingType();
            //可空类型
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return GetDataType(type.GetGenericArguments().First());
            return type;
        }
        /// <summary>
        /// 构建转换成数据行委托
        /// </summary>
        /// <param name="type">传入类型</param>
        /// <param name="propertyList">转换的属性</param>
        /// <returns>转换数据行委托</returns>
        private static Func<T, object[]> BuildToRowDataDelegation<T>(Type type, PropertyInfo[] propertyList)
        {
            var source = Expression.Parameter(type);
            var items = propertyList.Select(property => ConvertBindPropertyToData(source, property));
            var array = Expression.NewArrayInit(typeof(object), items);
            var lambda = Expression.Lambda<Func<T, object[]>>(array, source);
            return lambda.Compile();
        }

        /// <summary>
        /// 将属性转换成数据
        /// </summary>
        /// <param name="source">源变量</param>
        /// <param name="property">属性信息</param>
        /// <returns>获取属性数据表达式</returns>
        private static Expression ConvertBindPropertyToData(ParameterExpression source, PropertyInfo property)
        {
            var propertyType = property.PropertyType;
            var expression = (Expression)Expression.Property(source, property);
            if (propertyType.IsEnum)
                expression = Expression.Convert(expression, propertyType.GetEnumUnderlyingType());
            return Expression.Convert(expression, typeof(object));
        }
    }
}
