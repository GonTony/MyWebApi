using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Model.Dtos;
using Test.Core.Model.Models;

namespace Test.Core.IServer
{
   public interface IAdvertisementServer:IBaseService<Advertisement>
    {
        Task<string> OwnTest();

        Task<ADView> GetAdModelToView(long id);
    }
}
