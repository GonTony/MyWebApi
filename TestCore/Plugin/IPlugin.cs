using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestCore
{
    public interface IPlugin
    {
        string PluginID { get; }

        string PluginName { get; }

        string Description { get; }

        int Order { get; }


        Task<bool> Init(AppContextBase context);

        Task<bool> Start(AppContextBase context);

        Task<bool> Stop(AppContextBase context);

    }
}
