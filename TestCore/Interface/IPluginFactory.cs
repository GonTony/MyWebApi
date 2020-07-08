using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestCore;

namespace TestCore
{
   public interface IPluginFactory
    {
        List<Assembly> LoaderAssembly { get; }

        //IPlugin GetPlugin(string pluginId);
        //PluginItem GetPluginInfo(string pluginId, bool secret = false);
        //List<PluginItem> GetPluginList(bool secret = false);
        void Load(string pluginPath);


        Task<bool> Init(AppContextBase context);

        Task<bool> Start(AppContextBase context);

        Task<bool> Stop(AppContextBase context);
    }
}
