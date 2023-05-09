using System;
using System.Collections.Generic;
using System.Text;

namespace XSX.Ids
{
    /// <summary>
    /// 雪花算法生成唯一ID的类
    /// </summary>
    public class Snowflake
    {
        // 常量
        private static long twepoch = 1288834974657L; // 起始时间戳（毫秒）
        private static long workerIdBits = 5L; // 机器ID所占位数
        private static long datacenterIdBits = 5L; // 数据中心ID所占位数
        private static long maxWorkerId = -1L ^ (-1L << (int)workerIdBits); // 最大机器ID
        private static long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits); // 最大数据中心ID
        private static long sequenceBits = 12L; // 序列号所占位数
        private static long workerIdShift = sequenceBits; // 机器ID左移位数
        private static long datacenterIdShift = sequenceBits + workerIdBits; // 数据中心ID左移位数
        private static long timestampLeftShift = sequenceBits + workerIdBits + datacenterIdBits; // 时间戳左移位数
        private static long sequenceMask = -1L ^ (-1L << (int)sequenceBits); // 序列号掩码

        // 变量
        private static long workerId; // 机器ID
        private static long datacenterId; // 数据中心ID
        private static long sequence = 0L; // 序列号
        private static long lastTimestamp = -1L; // 上一次生成ID的时间戳

        /// <summary>
        /// Snowflake构造函数，初始化机器ID和数据中心ID
        /// </summary>
        /// <param name="workerId">机器ID</param>
        /// <param name="datacenterId">数据中心ID</param>
        public Snowflake(long workerId, long datacenterId)
        {
            // 检查机器ID和数据中心ID是否合法
            if (workerId > maxWorkerId || workerId < 0)
            {
                throw new ArgumentException(string.Format("worker Id can't be greater than {0} or less than 0", maxWorkerId));
            }
            if (datacenterId > maxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(string.Format("datacenter Id can't be greater than {0} or less than 0", maxDatacenterId));
            }
            // 初始化机器ID和数据中心ID
            Snowflake.workerId = workerId;
            Snowflake.datacenterId = datacenterId;
        }

        /// <summary>
        /// 生成下一个唯一ID
        /// </summary>
        /// <returns>生成的唯一ID</returns>
        public static long NextId()
        {
            lock (typeof(Snowflake))
            {
                // 获取当前时间戳
                long timestamp = TimeGen();
                // 如果当前时间戳小于上次生成ID的时间戳，则抛出异常
                if (timestamp < lastTimestamp)
                {
                    throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", lastTimestamp - timestamp));
                }
                // 如果当前时间戳等于上次生成ID的时间戳，则递增序列号
                if (lastTimestamp == timestamp)
                {
                    sequence = (sequence + 1) & sequenceMask;
                    if (sequence == 0) // 序列号超出范围，需要等待下一毫秒
                    {
                        timestamp = TilNextMillis(lastTimestamp);
                    }
                }
                else // 如果当前时间戳大于上次生成ID的时间戳，则将序列号重置为0
                {
                    sequence = 0L;
                }
                lastTimestamp = timestamp; // 更新上次生成ID的时间戳
                                           // 将各部分组合成一个64位的唯一ID
                return ((timestamp - twepoch) << (int)timestampLeftShift) |
                    (datacenterId << (int)datacenterIdShift) |
                    (workerId << (int)workerIdShift) |
                    sequence;
            }
        }

        /// <summary>
        /// 等待下一毫秒
        /// </summary>
        /// <param name="lastTimestamp">上次生成ID的时间戳</param>
        /// <returns>下一毫秒的时间戳</returns>
        private static long TilNextMillis(long lastTimestamp)
        {
            long timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns>当前时间戳（毫秒）</returns>
        private static long TimeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        private static readonly Lazy<Snowflake> _snowflake = new Lazy<Snowflake>(() => new Snowflake(1, 1));
        public static Snowflake SnowflakeInstance
        {
            get { return _snowflake.Value; }
        }
    }
}
