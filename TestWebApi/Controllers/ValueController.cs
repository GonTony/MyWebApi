using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Core.IServer;
using Test.Core.Model.Models;
using TestCore;
using TestCore.Redis;
using TestWebApi.AuthHelper.JWT;
using TestWebApi.Test;
using static TestWebApi.AuthHelper.JWT.JWTHelper;

namespace TestWebApi.Controllers
{
    /// <summary>
    /// firlst 测试控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class ValueController:ControllerBase
    {
        ICaching _cach = null;
        IRedisManager redisManager_;
        IAdvertisementServer _server;
        public ValueController(ICaching caching,IRedisManager redisManager,IAdvertisementServer iadservice)
        {
            _cach = caching;
            redisManager_ = redisManager;
            _server = iadservice;
        }
        /// <summary>
        /// 测试1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HH() {
            string returnStr = "HAHA";
            if (_cach.Get("1") != null)
            {
                returnStr = _cach.Get("1").ToString();
            }
            else {
                _cach.Set("1", "Set 1!!!!");
            }
            return returnStr;
        }

        /// <summary>
        /// 获取jwt Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> GetJwtToken(TokenModelJwt model) {
            var jwtStr = JWTHelper.IssueJwt(model);
            return Ok(new { code = "200", jwt = jwtStr });
        }

        /// <summary>
        /// Test 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]//权限验证
        public async Task<string> Test()
        {
            
            return await _server.OwnTest();
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="model">实体信息</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi =true)]//隐藏接口，仅在swagger上 postman可以请求
        [HttpPost]
        public async Task<long> AddAD([FromBody] Advertisement model) {
            if (model != null)
                return await _server.AddEntityReturnLong(model);
            else {
                return await Task.FromResult<long>(0);
            }

        }

        /// <summary>
        /// 添加10000条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddADREntitys([FromBody] Advertisement model)
        {
            for (int i = 0; i< 10000; i++) {
                if (model != null)
                    model.Remark = "批量插入" + i.ToString();
                    await _server.AddEntityReturnEntity(model);
            }

            return await Task.FromResult(true);

        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AddMany([FromBody] Advertisement model,long number)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Advertisement> lists = new List<Advertisement>();
            for (int i = 0; i < number; i++)
            {
                Advertisement ad = new Advertisement { Remark = "批量插入 Many" + i.ToString(),ImgUrl=model.ImgUrl,Title=model.Title,Url=model.Url };                         
                lists.Add(ad);
            }
            sw.Stop();
            await _server.AddMany(lists);
            return await Task.FromResult($"OK-time:{sw.Elapsed}---Tid:{Thread.CurrentThread.ManagedThreadId}");

        }

        [HttpPost]
        public async Task<Advertisement> AddADREntity([FromBody] Advertisement model)
        {
            if (model != null)
                return await _server.AddEntityReturnEntity(model);
            else
            {
                return null;
            }

        }

        /// <summary>
        /// SQL语句获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Advertisement>> GetADSQL(int id)
        {
            if (id == 0)
            {
                return await _server.Query(o => o.Id > 0, it => it.Id, false);
            }
            else
            {
                return await _server.Query("id=" + id);
            }
        }
        /// <summary>
        /// 通过id来获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Advertisement> GetEntityByID(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else {
                return await _server.GetEntityByID(id);
            }
        }

        /// <summary>
        /// 通过linq信息来获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Advertisement>> GetADLINQ(int id)
        {
            if (id == 0)
            {
                return await _server.GetAll();
            }
            else
            {
                return await _server.GetEntityByWhere(O=>O.Id==id);
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> Update(Advertisement model)
        {
          return await _server.Update(model);  
        }

        [HttpPut]
        public async Task<bool> UpdateIg(Advertisement model)
        {
            return await _server.Update(model,new List<string> { "Title"},new List<string> { "Url"}, "remark='5'");
        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            return await _server.DeleteById(id);
        }

        [HttpPost]
        public async Task<bool> Delete(Advertisement model)
        {
            return await _server.Delete(model);
        }

        [HttpGet]      
        public string TestRedis(string key) {
            if (!string.IsNullOrWhiteSpace(key))
            {
                if (redisManager_.IsExits(key))
                {
                    string value = redisManager_.GetTEntity<string>(key);
                    return value;
                }
                else {
                    redisManager_.Set(key, "set_" + key, TimeSpan.FromSeconds(90));
                    return "push " + key;
                }               
            }
            else
                return "key is null";
        }
    }
}
