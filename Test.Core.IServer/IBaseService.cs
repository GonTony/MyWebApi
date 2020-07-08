using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.IServer
{
   public interface IBaseService<TEntity> where TEntity :class
    {
        #region 查询
        Task<List<TEntity>> GetAll();

        Task<TEntity> GetEntityByID(object id);

        Task<TEntity> GetEntityByPK(object pkValue);

        /// <summary>
        /// 查询信息通过主键集合
        /// </summary>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryByIDs(object[] lstIds);

        Task<List<TEntity>> Query(string strWhere);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);

        Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);

        Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);
        Task<List<TEntity>> GetEntityByWhere(Expression<Func<TEntity, bool>> expressionWhere);

        Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 0, int intPageSize = 20, string strOrderByFileds = null);
        Task<List<TEntity>> GetEntityPageList(int pageIndex, int pageSize, int totalCount);

        Task<List<object>> GetEntityPageList<TEntityTwo>(int pageIndex, int pageSize, int totalCount);

        Task<List<TEntity>> GetEntityGroupBy(Expression<Func<TEntity, object>> expressionGroupBy, Expression<Func<TEntity, bool>> expressionWhere);

        #endregion
        #region 添加
        Task<int> AddEntityReturnInt(TEntity model);

        Task<long> AddEntityReturnLong(TEntity model);

        Task<TEntity> AddEntityReturnEntity(TEntity model);

        Task<bool> AddMany(List<TEntity> entities);
        #endregion

        #region 更新
        Task<bool> Update(TEntity model);
        Task<bool> Update(TEntity entity, string strWhere);
        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");

        #endregion

        #region 删除
        Task<bool> DeleteById(object id);

        Task<bool> Delete(TEntity model);

        Task<bool> DeleteByIds(object[] ids);
        #endregion
    }
}
