using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmarALZabir.AspectF;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace Dropthings.Util
{
    class EntlibCacheResolver : ICache
    {
        private readonly static ICacheManager _CacheManager = CacheFactory.GetCacheManager("DropthingsCache");
        #region ICache Members

        public object Get(string key)
        {
            return _CacheManager.GetData(key);
        }

        public void Put(string key, object item)
        {
            _CacheManager.Add(key, item);
        }

        #endregion

        #region ICache Members

        public void Add(string key, object value, TimeSpan timeout)
        {
            _CacheManager.Add(key, value, CacheItemPriority.Normal, null,
                new AbsoluteTime(DateTime.Now.Add(timeout)));
        }

        public void Add(string key, object value)
        {
            _CacheManager.Add(key, value);
        }

        public bool Contains(string key)
        {
            return _CacheManager.Contains(key);
        }

        public void Flush()
        {
            _CacheManager.Flush();
        }

        public void Remove(string key)
        {
            _CacheManager.Remove(key);
        }

        public void Set(string key, object value, TimeSpan timeout)
        {
            if (_CacheManager.Contains(key))
                _CacheManager.Remove(key);
            this.Add(key, value, timeout);
        }

        public void Set(string key, object value)
        {
            if (_CacheManager.Contains(key))
                _CacheManager.Remove(key);
            this.Add(key, value);
        }

        #endregion
    }
}
