using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Interface
{
    /// <summary>
    /// 自建容器
    /// </summary>
   public interface IServiceContainer
    {
        void AddInstance(object instance);
        void AddInstance(Type type, object instance);
        void AddInstance<TInstance>(TInstance instance) where TInstance : class;
        List<object> GetInstance();
        object GetInstance(Type type);
        TInstance GetInstance<TInstance>() where TInstance : class;
        void RemoveInstance(Type type);
        void RemoveInstance(object instance);
    }
}
