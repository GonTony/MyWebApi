using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TestCore;
using TPlugin.Interface;
using TPlugin.model;

namespace TPlugin
{
    /// <summary>
    /// 插件 Test控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "TestRole")]
    public class TestController: ControllerBase
    {
        private readonly ITClass tt;
        private ICaching cache_;
        public TestController(ITClass t,ICaching caching)
        {
            tt = t;
            cache_ = caching;
        }

        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]      
        public string policy() {
            string cacheString = cache_.Get("1")?.ToString();
            return tt.Name()+ cacheString;
        }
    }
}
