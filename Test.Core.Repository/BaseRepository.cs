using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.IRepository;
using Test.Core.Repository.SqlSugar;

namespace Test.Core.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity:class,new()
    {
        private DbContext _dbContext;

        protected SqlSugarClient db;

        private SimpleClient<TEntity> entityDB;

        public BaseRepository()
        {
            _dbContext = DbContext.GetDbContext();
            this.db = _dbContext.db_;
            this.entityDB = _dbContext.GetEntityDB<TEntity>();
        }

        /// <summary>
        /// 添加信息 返回返回所添加model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TEntity> AddEntityReturnEntity(TEntity model)
        {
            return await db.Insertable(model).ExecuteReturnEntityAsync();
        }
        /// <summary>
        /// 添加信息返回int
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> AddEntityReturnInt(TEntity model)
        {
            return await db.Insertable(model).ExecuteReturnIdentityAsync();
        }
        /// <summary>
        /// 添加信息 返回long
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> AddEntityReturnLong(TEntity model)
        {
            return await db.Insertable(model).ExecuteReturnBigIdentityAsync();
        }

        public async Task<bool> AddMany(List<TEntity> entities)
        {
            return await db.Insertable(entities.ToArray()).ExecuteCommandAsync()>0;
        }

        /// <summary>
        /// 通过model 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity model)
        {
            return await db.Deleteable(model).ExecuteCommandAsync()>0;
        }

        /// <summary>
        /// 通过主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            return await db.Deleteable<TEntity>(id).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 通过主键集合 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await db.Deleteable<TEntity>().In(ids).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 查询列表（所有）
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> GetAll()
        {
            return await db.Queryable<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetEntityByID(object id)
        {
            return await db.Queryable<TEntity>().In(id).FirstAsync();
        }

        /// <summary>
        /// 查询单条信息 通过主键
        /// </summary>
        /// <param name="pkValue">主键</param>
        /// <returns></returns>
        public async Task<TEntity> GetEntityByPK(object pkValue)
        {
            return await db.Queryable<TEntity>().InSingleAsync(pkValue);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="expressionWhere">where条件 若为空则查询所有</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetEntityByWhere(Expression<Func<TEntity, bool>> expressionWhere)
        {
            return await db.Queryable<TEntity>().WhereIF(expressionWhere!=null, expressionWhere).ToListAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="expressionGroupBy">groupby 条件</param>
        /// <param name="expressionWhere">where 条件 若为空则查询所有</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetEntityGroupBy(Expression<Func<TEntity, object>> expressionGroupBy, Expression<Func<TEntity, bool>> expressionWhere)
        {
            return await db.Queryable<TEntity>().WhereIF(expressionWhere != null, expressionWhere).GroupBy(expressionGroupBy).ToListAsync();
        }

        /// <summary>
        /// 单表分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetEntityPageList(int pageIndex, int pageSize, int totalCount)
        {
            RefAsync<int> refAsync = new RefAsync<int>(totalCount);
            return await db.Queryable<TEntity>().ToPageListAsync(pageIndex, pageSize, refAsync);
        }
        /// <summary>
        /// 暂未实现
        /// </summary>
        /// <typeparam name="TEntityTwo"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public Task<List<object>> GetEntityPageList<TEntityTwo>(int pageIndex, int pageSize, int totalCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询数据 
        /// </summary>
        /// <param name="strWhere">sql语句 where条件</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(string strWhere)
        {
            return await db.Queryable<TEntity>().WhereIF(!string.IsNullOrWhiteSpace(strWhere),strWhere).ToListAsync();
        }

        /// <summary>
        /// 查询数据 
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <param name="strOrderByFileds">order条件 eg:通过 id name排序</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds)
        {
            return await db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(!string.IsNullOrWhiteSpace(strOrderByFileds), strOrderByFileds).ToListAsync();
        }
        /// <summary>
        /// 查询数据 
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <param name="orderByExpression">order条件 eg:通过 id name排序</param>
        /// <param name="isAsc">是否升序 true->升序 false->降序</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByExpression!=null, orderByExpression,isAsc?OrderByType.Asc:OrderByType.Desc).ToListAsync();
        }

        /// <summary>
        /// 查询数据 
        /// </summary>
        /// <param name="whereExpression">where条件 sql语句</param>
        /// <param name="strOrderByFileds">order条件 eg:通过 id name排序 sql 语句</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(string strWhere, string strOrderByFileds)
        {
            return await db.Queryable<TEntity>().WhereIF(!string.IsNullOrWhiteSpace(strWhere), strWhere).OrderByIF(!string.IsNullOrWhiteSpace(strOrderByFileds), strOrderByFileds).ToListAsync();
        }

        /// <summary>
        /// 查询前xx条数据
        /// </summary>
        /// <param name="strWhere">sql where条件</param>
        /// <param name="intTop">前多少条数据</param>
        /// <param name="strOrderByFileds">排序依据</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds)
        {
            return await db.Queryable<TEntity>().WhereIF(!string.IsNullOrWhiteSpace(strWhere), strWhere).OrderByIF(!string.IsNullOrWhiteSpace(strOrderByFileds), strOrderByFileds).Take(intTop).ToListAsync();
        }


        /// <summary>
        /// 条件 排序 分页查询
        /// </summary>
        /// <param name="strWhere">sql where条件</param>
        /// <param name="intPageIndex">页数</param>
        /// <param name="intPageSize">页大小</param>
        /// <param name="strOrderByFileds">排序依据 sql</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds)
        {
            return await db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).WhereIF(!string.IsNullOrWhiteSpace(strWhere), strWhere).ToPageListAsync(intPageIndex, intPageSize);
        }
        /// <summary>
        /// 依据主键集合批量查询
        /// </summary>
        /// <param name="lstIds">主键集合</param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            return await db.Queryable<TEntity>().In(lstIds).ToListAsync();
        }

        /// <summary>
        /// 通过条件及排序分页查询
        /// </summary>
        /// <param name="whereExpression">条件 linq</param>
        /// <param name="intPageIndex">页数</param>
        /// <param name="intPageSize">每页大小</param>
        /// <param name="strOrderByFileds">排序依据</param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 0, int intPageSize = 20, string strOrderByFileds = null)
        {
            return await db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(!string.IsNullOrWhiteSpace(strOrderByFileds), strOrderByFileds).ToPageListAsync(intPageIndex, intPageSize);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity model)
        {
           
            var i=  await db.Updateable(model).ExecuteCommandAsync();
            return i > 0;
        }

        /// <summary>
        /// 更新 更新所有符合条件的数据 处主键外 其余全更新为entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity, string strWhere)
        {
            var i = await db.Updateable(entity).Where(strWhere).ExecuteCommandAsync();
            return i > 0;
        }

        /// <summary>
        /// 更新 忽略更新某些列 及更新某些列 通过条件判断
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "")
        {
            IUpdateable<TEntity> up = await Task.Run(() => db.Updateable(entity));
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = await Task.Run(() => up.IgnoreColumns(lstIgnoreColumns.ToArray()));
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = await Task.Run(() => up.UpdateColumns(lstColumns.ToArray()));
            }
            if (!string.IsNullOrEmpty(strWhere))
            {
                up = await Task.Run(() => up.Where(strWhere));
            }
            return await Task.Run(() => up.ExecuteCommand()) > 0;
        }
    }
}
