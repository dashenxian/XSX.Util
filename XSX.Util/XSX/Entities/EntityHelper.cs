﻿using JetBrains.Annotations;
using System;
using System.Reflection;
using XSX.Util.XSX.Helper;

namespace XSX.Entities
{
    /// <summary>
    /// 实体的一些辅助方法。
    /// </summary>
    public static class EntityHelper
    {
        public static bool IsEntity([NotNull] Type type)
        {
            return ReflectionHelper.IsAssignableToGenericType(type, typeof(IEntity<>));
        }

        public static Type GetPrimaryKeyType<TEntity>()
        {
            return GetPrimaryKeyType(typeof(TEntity));
        }

        /// <summary>
        /// 获取给定实体类型的主键类型
        /// </summary>
        public static Type GetPrimaryKeyType([NotNull] Type entityType)
        {
            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new XSXException("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements " + typeof(IEntity<>).AssemblyQualifiedName);
        }
    }
}
