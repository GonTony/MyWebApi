using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore;

namespace TestWebApi.AOP
{
    public class TestCacheAOP : IInterceptor
    {
        ICaching _cache;

        public TestCacheAOP(ICaching cache)
        {
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            string cacheKey = GetCustomKey(invocation);
            object data = _cache.Get(cacheKey);
            if (data != null)
            {
                invocation.ReturnValue = data;
                return;
            }
            else {
                invocation.Proceed();
                object rValue= invocation.ReturnValue;
                if (rValue!=null&&!string.IsNullOrWhiteSpace(cacheKey)) {
                    _cache.Set(cacheKey, rValue);
                }
            }
        }

        public string GetCustomKey(IInvocation invocation)
        {
            string methodName = invocation.Method.Name;
            string typeName = invocation.TargetType.Name;
            string parmstr =$"{methodName}_{typeName}_";
            var parms = invocation.Arguments;
            foreach (var item in parms)
            {
                if (item is int || item is long || item is string)
                    parmstr += ($"{item.GetType().Name}:{item.ToString()}_");
                else if (item is DateTime)
                    parmstr += ($"{item.GetType().Name}:{((DateTime)item).ToString("yyyyMMddHHmmssfff")}_");
            }
            return parmstr.TrimEnd('_');
        }
    }
}
