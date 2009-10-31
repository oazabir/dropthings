using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmarALZabir.AspectF;

namespace Dropthings.Util
{
    class NoCacheResolver : ICache
    {

        #region ICache Members

        public void Add(string key, object value, TimeSpan timeout)
        {
            
        }

        public void Add(string key, object value)
        {
            
        }

        public bool Contains(string key)
        {
            return false;
        }

        public void Flush()
        {
            
        }

        public object Get(string key)
        {
            return null;
        }

        public void Remove(string key)
        {
            
        }

        public void Set(string key, object value, TimeSpan timeout)
        {
            
        }

        public void Set(string key, object value)
        {
            
        }

        #endregion
    }
}
