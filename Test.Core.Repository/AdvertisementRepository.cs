using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.IRepository;
using Test.Core.Model.Models;
using Test.Core.Repository.SqlSugar;

namespace Test.Core.Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {     
      public async Task<string> OwnTest()
        {
         Advertisement model=  await base.db.Queryable<Advertisement>().Where("Id=34282").FirstAsync();
            return model.Remark;
        }
    }
}
