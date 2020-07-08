using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestCore;
using TestCore.Plugin;
using Microsoft.Extensions.DependencyInjection;
using TPlugin.model;
using TPlugin.Interface;

namespace TPlugin
{
    public class Plugin : PluginBase
    {
        public override string Description {
            get {
                return "1111";
            }
        }

        public override string PluginID => "2222";

        public override string PluginName => "333333";     
        
        public override Task<bool> Init(AppContextBase context)
        {          
            context.Service.AddScoped<ITClass, TOneClass>();
            return base.Init(context);
        }
    }
}
