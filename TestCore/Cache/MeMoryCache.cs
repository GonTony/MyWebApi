using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace TestCore
{
  public  class MeMoryCache : ICaching
    {
        ICaching icache = null;
        public MeMoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }
        private IMemoryCache _cache;    

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public void Set(string key, object value)
        {
            _cache.Set(key, value, TimeSpan.FromSeconds(60));
        }
    }
}
