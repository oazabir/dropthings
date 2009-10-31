using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmarALZabir.AspectF;
using Microsoft.Data.Caching;
using System.Configuration;

namespace Dropthings.Util
{
    public class VelocityCacheResolver : ICacheResolver
    {
        private static readonly DataCacheFactory _Factory;
        private static readonly DataCache _Cache;
        private const string REGION_NAME = "Dropthings";

        static VelocityCacheResolver()
        {
            _Factory = new DataCacheFactory();
            _Cache = _Factory.GetCache(ConfigurationManager.AppSettings["VelocityCacheName"]);

            try
            {
                _Cache.CreateRegion(REGION_NAME, false);
            }
            catch
            {
                // if the region already exists, this will throw an exception.
                // Velocity has no API to check if a region exists or not.
            }
        }
        
        #region ICacheResolver Members

        public void Add(string key, object value, TimeSpan timeout)
        {
            if (null == value)
                return;

            DataCacheItemVersion version = _Cache.Add(key, value, timeout, REGION_NAME);
            if (version == null)
                throw new ApplicationException("DataCache.Add failed");
        }

        public void Add(string key, object value)
        {
            if (null == value)
                return;

            DataCacheItemVersion version = _Cache.Add(key, value, REGION_NAME);
            if (version == null)
                throw new ApplicationException("DataCache.Add failed");
        }

        public bool Contains(string key)
        {
            return _Cache.Get(key, REGION_NAME) != null;
        }

        public void Flush()
        {
            _Cache.ClearRegion(REGION_NAME);
        }

        public void Remove(string key)
        {
            if (_Cache.Get(key, REGION_NAME) != null)
                _Cache.Remove(key, REGION_NAME);
        }
        
        public object Get(string key)
        {
            return _Cache.Get(key, REGION_NAME);
        }

        public void Set(string key, object value, TimeSpan timeout)
        {
            if (null == value)
            {
                Remove(key);
            }
            else
            {
                var version = _Cache.Put(key, value, timeout, REGION_NAME);
                if (null == version)
                    throw new ApplicationException("DataCache.Set failed");
            }
        }

        public void Set(string key, object value)
        {
            if (null == value)
            {
                Remove(key);
            }
            else
            {
                var version = _Cache.Put(key, value, REGION_NAME);
                if (null == version)
                    throw new ApplicationException("DataCache.Set failed");
            }
        }

        #endregion
    }
}
