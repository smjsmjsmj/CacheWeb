using RedisWeb.Comment;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace RedisWeb.Cache
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly string redisConnenctionString;

        public volatile ConnectionMultiplexer redisConnection;

        private readonly object redisConnectionLock = new object();

        private static volatile RedisCacheManager redisCacheManager;

        public static RedisCacheManager GetRedisCacheManager()
        {
            object obj = new object();
            lock (obj)
            {
                if (redisCacheManager == null)
                {
                    redisCacheManager = new RedisCacheManager();
                }
                return redisCacheManager;
            }
        }


        public RedisCacheManager()
        {
            //string redisConfiguration = ConfigurationManager.AppSettings["redisCache"].ToString();
            string redisConfiguration = "10.172.37.109,abortConnect=false,connectRetry=3000,connectTimeout=600000,syncTimeout=600000";
            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }
            this.redisConnenctionString = redisConfiguration;
            this.redisConnection = GetRedisConnection();
        }

        private ConnectionMultiplexer GetRedisConnection()
        {
            if (this.redisConnection != null && this.redisConnection.IsConnected)
            {
                return this.redisConnection;
            }
            lock (redisConnectionLock)
            {
                if (this.redisConnection != null)
                {
                    this.redisConnection.Dispose();
                }
                this.redisConnection = ConnectionMultiplexer.Connect(redisConnenctionString);
            }
            return this.redisConnection;
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

        public bool Contains(string key)
        {
            return redisConnection.GetDatabase().KeyExists(key);
        }

        public TEntity Get<TEntity>(string key)
        {
            var value = redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                return SerializeHelper.Deserialize<TEntity>(value);
            }
            else
            {
                return default(TEntity);
            }
        }

        public void Remove(string key)
        {
            redisConnection.GetDatabase().KeyDelete(key);
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            try
            {
                if (value != null)
                {
                    redisConnection.GetDatabase().StringSet(key, SerializeHelper.Serialize(value), cacheTime);
                }
            }
            catch(Exception x)
            {
                Console.Write(x.Message);
            }
        }

   

    }
}