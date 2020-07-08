using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TestCore;
using TPlugin.Interface;
using TPlugin.model;

namespace TPlugin
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController: ControllerBase
    {
        private readonly ITClass tt;
        private ICaching cache_;
        public TestController(ITClass t,ICaching caching)
        {
            tt = t;
            cache_ = caching;
        }

        [HttpGet]
        public string policy() {
            string cacheString = cache_.Get("1")?.ToString();
            return tt.Name()+ cacheString;
        }
    }
}
