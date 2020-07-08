using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore;

namespace TestWebApi
{
    public class AppContextWA:AppContextBase
    {      
        public AppContextWA(IServiceCollection services):base(services)
        {
            
        }
    }
}
