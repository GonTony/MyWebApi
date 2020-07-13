using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Core.IServer;
using Test.Core.Model.Dtos;
using TestWebApi.CustomVersionApi;

namespace TestWebApi.Controllers.v1
{
   // [Route("api/[controller]")]
    [CustomVersionAttribute(CustomApiVersonHelper.ApiVersion.v1)]
    [ApiController]
    public class TestVersionController : ControllerBase
    {
        IAdvertisementServer _adServer;
        public TestVersionController(IAdvertisementServer server)
        {
            _adServer = server;
        }

        [HttpGet]
        public async Task<ADView> GetAdInfo(long id=30000) {
            return await _adServer.GetAdModelToView(id);
        }
    }
}