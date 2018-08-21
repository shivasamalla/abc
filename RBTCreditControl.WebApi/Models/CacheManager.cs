using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RBTCreditControl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RBTCreditControl.WebApp.Models
{
    public interface ICacheManager<T>
    {
        T GetOrSetUserSession(string Key, T obj);
        bool RemoveCacheObject(string Key);
        bool CheckSessionExists(string Key);
    }
    public class CacheManager<T>: ICacheManager<T>
    {
        static readonly ReaderWriterLockSlim cacheLock_User = new ReaderWriterLockSlim();
        private IMemoryCache _cache;
        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }
        public T GetOrSetUserSession(string Key,T data)
        {
            T CachedData;
            try
            {
                cacheLock_User.EnterReadLock();
                //Returns null if the string does not exist, prevents a race condition where the cache invalidates between the contains check and the retreival.
                if (_cache.TryGetValue(Key, out CachedData))
                {
                    return CachedData;
                }
            }
            finally
            {
                cacheLock_User.ExitReadLock();
            }

            #region Key not in cache Add into Cache
            cacheLock_User.EnterWriteLock();
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                // Save data in cache.
                _cache.Set(Key, data);
                return data;
            }
            finally
            {
                cacheLock_User.ExitWriteLock();
            }
            #endregion
        }

        public bool RemoveCacheObject(string Key)
        {
            cacheLock_User.EnterWriteLock();
            try
            {
                _cache.Remove(Key);
                return true;
            }
            finally
            {
                cacheLock_User.ExitWriteLock();
            }
           
        }

        public bool CheckSessionExists(string Key)
        {
            cacheLock_User.EnterReadLock();
            try
            {
                T CachedData;
                return _cache.TryGetValue(Key, out CachedData);
                //Returns null if the string does not exist, prevents a race condition where the cache invalidates between the contains check and the retreival.
            }
            finally
            {
                cacheLock_User.ExitReadLock();
            }
        }
    }
}
