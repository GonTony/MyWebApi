using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.IRepository;
using Test.Core.IServer;
using Test.Core.Repository;

namespace Test.Core.Server
{
   public  class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
       protected IBaseRepository<TEntity> dalBase;
       public async Task<TEntity> AddEntityReturnEntity(TEntity model)
        {
           return await dalBase.AddEntityReturnEntity(model);
        }

       public async Task<int> AddEntityReturnInt(TEntity model)
        {
           return await dalBase.AddEntityReturnInt(model);
        }

       public async Task<long> AddEntityReturnLong(TEntity model)
        {
           return await dalBase.AddEntityReturnLong(model);
        }

        public async Task<bool> AddMany(List<TEntity> entities)
        {
            return await dalBase.AddMany(entities);
        }

        public async Task<bool> Delete(TEntity model)
        {
           return await dalBase.Delete(model);
        }

       public async Task<bool> DeleteById(object id)
        {
           return await dalBase.DeleteById(id);
        }

       public async Task<bool> DeleteByIds(object[] ids)
        {
            return await dalBase.DeleteByIds(ids);
        }

       public async Task<List<TEntity>> GetAll()
        {
            return await dalBase.GetAll();
        }

       public async Task<TEntity> GetEntityByID(object id)
        {
            return await dalBase.GetEntityByID(id);
        }

       public async Task<TEntity> GetEntityByPK(object pkValue)
        {
            return await dalBase.GetEntityByPK(pkValue);
        }

       public async Task<List<TEntity>> GetEntityByWhere(Expression<Func<TEntity, bool>> expressionWhere)
        {
            return await dalBase.GetEntityByWhere(expressionWhere);
        }

       public async Task<List<TEntity>> GetEntityGroupBy(Expression<Func<TEntity, object>> expressionGroupBy, Expression<Func<TEntity, bool>> expressionWhere)
        {
            return await dalBase.GetEntityGroupBy(expressionGroupBy, expressionWhere);
        }

       public async Task<List<TEntity>> GetEntityPageList(int pageIndex, int pageSize, int totalCount)
        {
            return await dalBase.GetEntityPageList(pageIndex, pageSize, totalCount);
        }

       public async Task<List<object>> GetEntityPageList<TEntityTwo>(int pageIndex, int pageSize, int totalCount)
        {
            return await dalBase.GetEntityPageList<TEntityTwo>(pageIndex, pageSize, totalCount);
        }

       public async Task<List<TEntity>> Query(string strWhere)
        {
            return await dalBase.Query(strWhere);
        }

       public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await dalBase.Query(whereExpression, strOrderByFileds);
        }

       public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await dalBase.Query(whereExpression, orderByExpression, isAsc);
        }

       public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await dalBase.Query(strWhere, strOrderByFileds);
        }

       public async Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds)
        {
            return await dalBase.Query(strWhere, intTop, strOrderByFileds);
        }

       public async Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds)
        {
            return await dalBase.Query(strWhere, intPageIndex, intPageSize, strOrderByFileds);
        }

       public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            return await dalBase.QueryByIDs(lstIds);
        }

       public async Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 0, int intPageSize = 20, string strOrderByFileds = null)
        {
            return await dalBase.QueryPage(whereExpression, intPageIndex, intPageSize, strOrderByFileds);
        }

       public async Task<bool> Update(TEntity model)
        {
            return await dalBase.Update(model);
        }

       public async Task<bool> Update(TEntity entity, string strWhere)
        {
            return await dalBase.Update(entity, strWhere);
        }

       public async Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
        {
            return await dalBase.Update(entity, lstColumns, lstIgnoreColumns, strWhere);
        }
    }
}
