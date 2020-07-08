using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Redis
{
   public interface IRedisManager
    {
        TEntity GetTEntity<TEntity>(string key);

        void Set(string key, object value, TimeSpan timeSpan);

        void Clear();

        bool IsExits(string key);

        void Remove(string key);
    }
}
