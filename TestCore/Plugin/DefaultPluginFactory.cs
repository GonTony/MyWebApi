using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestCore
{
    public class DefaultPluginFactory : IPluginFactory
    {
        public List<Assembly> LoaderAssembly { get; }

        protected class PluginInfo : PluginItem
        {
            public Type Type { get; set; }

            public IPlugin Instance { get; set; }


            public void SetInitFail(bool fail)
            {
                this.InitFail = fail;
            }
            public void SetStartFail(bool fail)
            {
                this.StartFail = fail;
            }
            public void SetRunning(bool run)
            {
                this.IsRunning = run;
            }
            public void SetMessage(string msg)
            {
                this.Message = msg;
            }
        }

        protected List<PluginInfo> PluginList = new List<PluginInfo>();

        public DefaultPluginFactory()
        {
            LoaderAssembly = new List<Assembly>();
        }

        public async Task<bool> Init(AppContextBase context)
        {
            foreach (PluginInfo item in PluginList)
            {
                try
                {
                    var result =await item.Instance.Init(context);
                    item.SetInitFail(result);
                }
                catch (Exception ex)
                {
                    item.SetInitFail(false);
                }
                
            }
            return true;
        }

       public  void Load(string pluginPath)
        {
            PluginList.Clear();
            if (string.IsNullOrEmpty(pluginPath)) {
                return;
            }
            if (!Directory.Exists(pluginPath))
            {
                return;
            }
            DirectoryLoader loader = new DirectoryLoader();
            var assembly = loader.LoadFromDirectory(pluginPath);//所有插件Assembly
            if (assembly != null && assembly.Count > 0) {
                Func<Type, bool> func = (t) => {
                    if (t.GetTypeInfo().IsAbstract) {
                        return false;
                    }
                    if (t.GetTypeInfo().IsPublic) {
                        return true;
                    }
                    return false;
                };
                foreach (Assembly item in assembly)
                {
                    LoaderAssembly.Add(item);
                    List<Type> types = loader.GetTypes(item, typeof(IPlugin), func);//获取该dll下的所有符合条件的Type  条件：继承IPlugin 非抽象类 public
                    if (types != null && types.Count > 0) {
                        foreach (Type type in types)
                        {
                            try
                            {
                                IPlugin model = Activator.CreateInstance(type) as IPlugin;
                                PluginInfo pi = new PluginInfo()
                                {
                                    Description = model.Description,
                                    Order = model.Order,
                                    ID = model.PluginID,
                                    Type = type,
                                    Instance = model,
                                    Name = model.PluginName
                                };

                                PluginList.Add(pi);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                }
            }


        }

        public Task<bool> Start(AppContextBase context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Stop(AppContextBase context)
        {
            throw new NotImplementedException();
        }
    }
}
