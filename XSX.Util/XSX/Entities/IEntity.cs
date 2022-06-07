using System;
using System.Collections.Generic;
using System.Text;

namespace XSX.Entities
{
    /// <summary>
    /// 默认主键类型为Guid实体类型接口。
    /// </summary>
    public interface IEntity : IEntity<Guid>
    {

    }
    /// <summary>
    /// 为基本实体类型定义接口。系统中的所有实体都必须实现此接口。
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// 检查此实体是否为瞬态的（不持久存储在数据库中，并且没有 <see cref="Id"/>).
        /// </summary>
        /// <returns>如果此实体是瞬态的返回true</returns>
        bool IsTransient();
    }
}
