using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Core.Model.Dtos;
using Test.Core.Model.Models;

namespace TestWebApi.AutoMapper
{
    public class AutoMapperConfig: Profile
    {
        /// <summary>
        /// automapper 基本原理为 会自动找到继承Profile类 进行配置
        /// </summary>
        public AutoMapperConfig()
        {
            CreateMap<Advertisement, ADView>()
                .ForMember(n=>n.Createdate,o=>o.MapFrom(m=>m.Createdate.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(n=>n.IDAndDatetime,o=>o.MapFrom(m=>m.Id.ToString()+ m.Createdate.ToString("yyyy-MM-dd HH:mm:ss")));

        }
    }
}
