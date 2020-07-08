using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TestCore.Redis
{
    public class RedisManager : IRedisManager
    {
        private readonly string redisConnectionString;

        /// <summary>
        /// 一个变量经 volatile修饰后在所有线程中必须是同步的；任何线程中改变了它的值，所有其他线程立即获取到了相同的值。理所当然的，volatile修饰的变量 存取时比一般变量消耗的资源要多一点，因为线程有它自己的变量拷贝更为高效
        /// </summary>
        private volatile ConnectionMultiplexer redisConnection;

        private object LockRedis = new object();
        public RedisManager(ModelConfig config)
        {
            redisConnectionString = config.AppSettings.RedisCaching.ConnectionString;
            redisConnection = GetRedisConnection();
        }

        private ConnectionMultiplexer GetRedisConnection() {

            if (this.redisConnection != null && this.redisConnection.IsConnected) {
                return this.redisConnection;
            }
            if (!string.IsNullOrWhiteSpace(this.redisConnectionString)){
                lock (LockRedis) {
                    if (this.redisConnection != null) {
                        this.redisConnection.Dispose();
                    }
                    try
                    {
                        return ConnectionMultiplexer.Connect(redisConnectionString);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Redis 启动异常，请检查Redis服务是否启动");
                    }                    
                }
            }
            else {
                return null;
            }
        }
        public void Clear()
        {
            foreach (var endPoint in this.GetRedisConnection().GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    redisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }

        public TEntity GetTEntity<TEntity>(string key)
        {
            var value = redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<TEntity>(value);
            }
            else {
                return default(TEntity);
            }
        }

        public bool IsExits(string key)
        {
            return redisConnection.GetDatabase().KeyExists(key);
        }

        public void Remove(string key)
        {
            redisConnection.GetDatabase().KeyDelete(key);
        }

        public void Set(string key, object value, TimeSpan timeSpan)
        {
            if (value != null&&!string.IsNullOrWhiteSpace(key)) {
                if (value is byte[]) {
                    redisConnection.GetDatabase().StringSet(key, value as Byte[], timeSpan);
                }
                redisConnection.GetDatabase().StringSet(key, JsonConvert.SerializeObject(value), timeSpan);
            }
        }
    }
}
