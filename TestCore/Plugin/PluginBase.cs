using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestCore.Plugin
{
    public abstract class PluginBase : IPlugin
    {
        public static readonly int DefaultOrder = 1000;

        public abstract string Description { get; }

        public virtual int Order
        {
            get
            {
                return DefaultOrder;
            }
        }
        public abstract string PluginID { get; }
        public abstract string PluginName { get; }

       public virtual Task<bool> Init(AppContextBase context)
        {
            return Task.FromResult(true);
        }

       public virtual Task<bool> Start(AppContextBase context)
        {
            return Task.FromResult(true);
        }

       public virtual Task<bool> Stop(AppContextBase context)
        {
            return Task.FromResult(true);
        }
    }
}
