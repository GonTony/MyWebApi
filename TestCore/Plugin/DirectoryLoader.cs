using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.Loader;
using System.Linq;

namespace TestCore
{
   public class DirectoryLoader
    {

        /// <summary>
        /// 扫描文件夹内所有的dll
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public virtual List<Assembly> LoadFromDirectory(string dir) {
            if (!Directory.Exists(dir)) {
                return new List<Assembly>();           
            }
            List<Assembly> assemblies = new List<Assembly>();
            string[] fileDir= Directory.GetFiles(dir);
            foreach (string item in fileDir)
            {
                if (!item.EndsWith(".dll"))
                {
                    continue;
                }
                else
                {
                    Assembly assembly = Assembly.LoadFrom(item);
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }

        public virtual List<Type> GetTypes(Assembly assembly,Type typeBase,Func<Type,bool> checker) {
            Func<Type, bool> func = (t) =>
            {
                bool isExc = false;
                if (typeBase.GetTypeInfo().IsAssignableFrom(t))
                {//t是否继承实现typeBase
                    isExc = true;
                }
                else {
                    isExc = false;
                }

                if (isExc&&checker!=null) {
                    return checker(t);
                }
                return false;
            };

            return assembly.GetTypes().Where(func).ToList();
        }
    }
}
