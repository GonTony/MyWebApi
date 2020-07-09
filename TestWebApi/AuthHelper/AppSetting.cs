using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.AuthHelper
{
    public class AppSetting
    {
       static IConfiguration _configuration { get; set; }
        static AppSetting()
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                //.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                // .Add(new JsonConfigurationSource { Path = "", Optional = false, ReloadOnChange = true })
                .Build();
            }
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {          
            try
            {
                var val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }

                return _configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }

        }

    }
}
