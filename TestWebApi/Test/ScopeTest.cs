using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Redis;

namespace TestWebApi.Test
{
    public class ScopeTest
    {
        IRedisManager RedisManager_;
        public ScopeTest(IRedisManager redisManager)
        {
            RedisManager_ = redisManager;
        }

        public string TestScope() {
            return "ssss";
        }
    }
}
