using System;
using System.Collections.Generic;
using System.Reflection;

namespace XSX.Entities
{
    /// <summary>
    /// 默认主键类型为Guid的实体类型。
    /// </summary>
    public abstract class Entity : Entity<Guid>, IEntity
    {

    }

    /// <summary>
    /// IEntity接口的基本实现。
    /// 实体可以继承直接实现到IEntity接口的此类。
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体主键的类型</typeparam>
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 实体唯一标识
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// 检查此实体是否为瞬态的（不持久存储在数据库中，并且没有 <see cref="Id"/>).
        /// </summary>
        /// <returns>如果此实体是瞬态的返回true</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey)))
            {
                return true;
            }

            //EF Core的解决方法，因为在附加到dbcontext时将int / long设置为最小值
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (Entity<TPrimaryKey>)obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType().GetTypeInfo();
            var typeOfOther = other.GetType().GetTypeInfo();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}
