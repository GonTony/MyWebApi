using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.CustomVersionApi
{
    public static class CustomApiVersonHelper
    {
        public enum ApiVersion {
            /// <summary>
            /// v1版本
            /// </summary>
            v1 =1,
            /// <summary>
            /// v2版本 
            /// </summary>
            v2=2
        }
    }
}
