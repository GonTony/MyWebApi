using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestCore
{
   public class AppContextBase
    {
        public static AppContextBase Current;

        public AppContextBase(IServiceCollection services)
        {
            Current = this;
            Service = services;
        }
        public IPluginFactory PluginFactory { get; set; }

        public string DBConString = string.Empty;

        public IServiceCollection Service { get; protected set; }

        public async virtual Task<bool> Init()
        {
            return true;
        }

        public async virtual Task<bool> Start()
        {
            return true;
        }
    }
}
