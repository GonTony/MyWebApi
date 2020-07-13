using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static TestWebApi.CustomVersionApi.CustomApiVersonHelper;

namespace TestWebApi.CustomVersionApi
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple =true,Inherited =true)]
    public class CustomVersionAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {
        public CustomVersionAttribute(string actionName = "[action]") : base("/api/{version}/[controller]/"+actionName)
        {
        }

        public CustomVersionAttribute(ApiVersion version, string actionName = "[action]") : base($"/api/{version.ToString()}/[controller]/{actionName}")
        {
            GroupName = version.ToString();
        }
        public string GroupName { get; set; }
    }
}
