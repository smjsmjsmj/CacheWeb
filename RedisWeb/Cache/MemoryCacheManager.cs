using RedisWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace RedisWeb.Cache
{
    public class MemoryCacheManager : ICacheManager
    {


        private static volatile  MemoryCacheManager memoryCacheManager = null;
        private CacheItemPolicy policy = null;
        private CacheEntryRemovedCallback callback = null;

        public static MemoryCacheManager GetMemoryCacheManager()
        {
            string lockKey = "lockKey";
            lockKey = "hello" + lockKey;
            lock (lockKey)
            {
                if (memoryCacheManager == null)
                {
                    memoryCacheManager = new MemoryCacheManager();
                }
                return memoryCacheManager;
            }
           
        }

        public void Clear()
        {
            foreach (var item in MemoryCache.Default)
            {
                this.Remove(item.Key);
            }
        }

        public bool Contains(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

        public TEntity Get<TEntity>(string key)
        {
            return (TEntity)MemoryCache.Default.Get(key);
        }

        
        public void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        public void Set(string key, object value, double cacheMinutesTime)
        {
            policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheMinutesTime);
            policy.RemovedCallback = callback;
            MemoryCache.Default.Add(key, value, policy);
        }
        public void Set(string key, object value, TimeSpan cacheTime)
        {
            policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.SlidingExpiration = cacheTime;
            policy.RemovedCallback = callback;
            MemoryCache.Default.Add(key, value, policy);
        }

        public IEnumerable<TEntity> GetKeys<TEntity>(string value)
        {
            var keys = MemoryCache.Default.Where(m=>m.Key.Contains(value));
            return keys.Select(m => (TEntity)m.Value);
        }


    }

}