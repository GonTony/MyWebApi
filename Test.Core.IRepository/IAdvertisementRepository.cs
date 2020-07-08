using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Model.Models;

namespace Test.Core.IRepository
{
    public interface IAdvertisementRepository:IBaseRepository<Advertisement>
    {
        Task<string> OwnTest();
        //long Add(Advertisement model);
        //bool Delete(Advertisement model);
        //bool Update(Advertisement model);
        //List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);
    }
}
