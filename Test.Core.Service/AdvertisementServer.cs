using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.IRepository;
using Test.Core.IServer;
using Test.Core.Model.Models;

namespace Test.Core.Service
{
    public class AdvertisementServer : BaseService<Advertisement>, IAdvertisementServer
    {
        IAdvertisementRepository _dal;
        public AdvertisementServer(IAdvertisementRepository advertisementRepository)
        {
           this._dal = advertisementRepository;
           base.dalBase = advertisementRepository;
        }

       public async Task<string> OwnTest()
        {
            return await _dal.OwnTest();
        }
    }
}
