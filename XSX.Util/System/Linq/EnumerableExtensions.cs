using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector)
        {
            return list.Distinct(new GenericCompare<T>((a, b) => keySelector(a).Equals(keySelector(b))));
        }

        /// <summary>
        /// 动态排序
        /// </summary>
        /// <typeparam name="TEntity">排序列表类型</typeparam>
        /// <param name="source">列表</param>
        /// <param name="sortPropertyName">排序列名称</param>
        /// <param name="ascending">是否升序</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string sortPropertyName, bool ascending = true)
        {
            // 根据指定的字段名，获得TEntity类型的属性名称。不区分大小写	
            var sortPropertyInfo = typeof(TEntity).GetProperty(sortPropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (sortPropertyInfo == null)
            {
                throw new ArgumentException($"Property '{sortPropertyName}' not found in type '{typeof(TEntity).Name}'.", nameof(sortPropertyName));
            }

            // 构建ParameterExpression （生成表达式：sort）
            var parameterExp = Expression.Parameter(typeof(TEntity), "sort");

            // 构建PropertyExpression  （生成表达式：sort => sort.Property）
            var propertyExp = Expression.Property(parameterExp, sortPropertyName);

            // 判断属性类型是否是值类型，如果是则进行转换
            Expression convertedExp = sortPropertyInfo.PropertyType.IsValueType ? Expression.Convert(propertyExp, typeof(object)) : propertyExp;

            // 构建LambdaExpression    （生成表达式：sort => (object)sort.Property）
            var lambdaExp = Expression.Lambda<Func<TEntity, object>>(convertedExp, parameterExp);

            // 判断排序方向，如果是降序则使用OrderByDescending方法，否则使用OrderBy方法
            var orderByMethod = ascending ? "OrderBy" : "OrderByDescending";

            // 构建MethodCallExpression （生成表达式：source.OrderBy(sort => (object)sort.Property)）
            var methodCallExp = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { typeof(TEntity), typeof(object) }, source.Expression, lambdaExp);

            // 返回排序后的IQueryable<TEntity>
            return source.Provider.CreateQuery<TEntity>(methodCallExp);
        }
    }
}
