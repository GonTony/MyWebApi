using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.IRepository;
using Test.Core.IServer;
using Test.Core.Model.Dtos;
using Test.Core.Model.Models;

namespace Test.Core.Service
{
    public class AdvertisementServer : BaseService<Advertisement>, IAdvertisementServer
    {
        IAdvertisementRepository _dal;
        IMapper _mapper;
        public AdvertisementServer(IAdvertisementRepository advertisementRepository,IMapper mapper)
        {
           this._dal = advertisementRepository;
           base.dalBase = advertisementRepository;
            _mapper = mapper;
        }

        public async Task<ADView> GetAdModelToView(long id)
        {
            var model = await _dal.GetEntityByID(id);
            ADView viewModel = null;
            viewModel = _mapper.Map<ADView>(model);
            return viewModel;
        }

        public async Task<string> OwnTest()
        {
            return await _dal.OwnTest();
        }
    }
}
