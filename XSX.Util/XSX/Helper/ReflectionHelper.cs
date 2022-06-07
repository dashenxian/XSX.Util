using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XSX.Util.XSX.Helper
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
        /// 如：typeof(List<a/>)=>typeof(List< />)
        /// </summary>
        /// <param name="givenType">Type to check</param>
        /// <param name="genericType">Generic type</param>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var givenTypeInfo = givenType.GetTypeInfo();

            if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenTypeInfo.GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenTypeInfo.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
        }
        /// <summary>
        /// 获取静态属性或静态字段的值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">字段或属性名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        public static bool TryGetStaticFieldOrPropertyValue(Type type, string name, out object value)
        {
            Type temp = type;

            do
            {
                var field = temp.GetField(name, BindingFlags.Public | BindingFlags.Static);
                if (field != null)
                {
                    value = field.GetValue(null);
                    return true;
                }

                temp = temp.BaseType;
            } while (temp != null);


            temp = type;

            do
            {
                var prop = temp.GetProperty(name, BindingFlags.Public | BindingFlags.Static);
                if (prop != null)
                {
                    value = prop.GetValue(null, null);
                    return true;
                }

                temp = temp.BaseType;
            } while (temp != null);

            value = null;
            return false;
        }
        /// <summary>
        /// 获取静态属性或静态字段的值
        /// </summary>
        /// <typeparam name="T">字段或属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="name">字段或属性名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        public static bool TryGetStaticFieldOrPropertyValue<T>(Type type, string name, out T value)
        {
            var result = TryGetStaticFieldOrPropertyValue(type, name, out object obj);
            if (result && obj is T)
            {
                value = (T)obj;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
