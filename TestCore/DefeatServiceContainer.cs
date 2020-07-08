using System;
using System.Collections.Generic;
using System.Text;
using TestCore.Interface;
using System.Linq;

namespace TestCore
{
    public class DefeatServiceContainer : IServiceContainer
    {
        Dictionary<Type, object> serviceCache;
        public DefeatServiceContainer()
        {
            serviceCache = new Dictionary<Type, object>();
        }

        public void AddInstance(object instance)
        {
            if (instance == null) return;
           
            lock (serviceCache)
            {
                Type tKey = instance.GetType();
                if (serviceCache.ContainsKey(tKey))
                {
                    serviceCache[tKey] = instance;
                }
                else
                {
                    serviceCache.Add(tKey, instance);
                }
            }
        }

        public void AddInstance(Type type, object instance)
        {
            if (instance == null) return;
            lock (serviceCache) {
                if (serviceCache.ContainsKey(type))
                {
                    serviceCache[type] = instance;
                }
                else
                {
                    serviceCache.Add(type, instance);
                }
            }
           
        }

        public virtual void AddInstance<TInstance>(TInstance instance) where TInstance : class
        {
            Type tkey = typeof(TInstance);
            AddInstance(tkey, instance);
        }

        public List<object> GetInstance()
        {
            lock (serviceCache) {
                return serviceCache.Values.ToList();
            }
        }

        public object GetInstance(Type type)
        {
            lock (serviceCache) {
                if (serviceCache.ContainsKey(type))
                {
                    return serviceCache[type];
                }
                else {
                    return null;
                }
            }
        }

        public TInstance GetInstance<TInstance>() where TInstance : class
        {
            Type type= typeof(TInstance);
            return GetInstance(type) as TInstance;
        }

        public void RemoveInstance(Type type)
        {
            lock (serviceCache) {
                if (serviceCache.ContainsKey(type)) {
                    serviceCache.Remove(type);
                }
            }
        }

        public void RemoveInstance(object instance)
        {
            if (instance == null) return;
            Type type = instance.GetType();
            RemoveInstance(type);
        }
    }
}
