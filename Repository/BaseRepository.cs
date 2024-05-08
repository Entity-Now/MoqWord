using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Repository
{
    public class BaseRepository<T> : SimpleClient<T>, IBaseRepository<T> where T : class , new()
    {
        public TypeAdapterConfig Config { get; set; }
        public BaseRepository(ISqlSugarClient db, TypeAdapterConfig _config) {
            Context = db;
            Config = _config;
        }

        T this[int id] => GetById(id);
        List<T> this[Expression<Func<T, bool>> where] => GetQuery(where).ToList();
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public virtual ISugarQueryable<T> GetAll()
        {
            return Context.Queryable<T>();
        }
        /// <summary>
        /// 查询符合条件的数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return Context.Queryable<T>().Where(predicate);
        }
        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>实体</returns>
        public T Get(Expression<Func<T, bool>> @where = null)
        {
            return @where == null ? Context.Queryable<T>().First() : Context.Queryable<T>().First(@where);
        }
        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>实体</returns>
        public virtual Task<T> GetAsync(Expression<Func<T, bool>> @where = null)
        {
            return @where == null ? Context.Queryable<T>().FirstAsync() : Context.Queryable<T>().FirstAsync(@where);
        }

        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <typeparam name="TS">排序</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>实体</returns>
        public virtual Task<T> GetAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, bool isAsc = true)
        {
            var data = Context.Queryable<T>();
            return isAsc ? data.OrderBy(orderby, OrderByType.Asc).FirstAsync() : Context.Queryable<T>().OrderBy(orderby, OrderByType.Desc).FirstAsync();
        }

        /// <summary>
        /// 获取第一条被AutoMapper映射后的数据
        /// </summary>
        /// <typeparam name="TDto">映射实体</typeparam>
        /// <typeparam name="TS">排序</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>实体</returns>
        public virtual TDto Get<TDto>(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, bool isAsc = true) where TDto : class
        {
            var data = Context.Queryable<T>();
            return isAsc ? data.OrderBy(orderby, OrderByType.Asc).First().Adapt<TDto>(Config) : Context.Queryable<T>().OrderBy(orderby, OrderByType.Desc).First().Adapt<TDto>(Config);
        }

        /// <summary>
        /// 根据ID找实体
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        public virtual T GetById(int id)
        {
            return Context.Queryable<T>().InSingle(id);
        }

        /// <summary>
        /// 根据ID找实体(异步)
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        public virtual Task<T> GetByIdAsync(int id)
        {
            return Context.Queryable<T>().InSingleAsync(id);
        }

        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int DeleteById(int id)
        {
            return Context.Deleteable<T>().In(id).ExecuteCommand();
        }
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteByIdAsync(int id)
        {
            return Context.Deleteable<T>().In(id).ExecuteCommandAsync();
        }
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int DeleteById(List<int> id)
        {
            return Context.Deleteable<T>().In(id).ExecuteCommand();
        }
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteByIdAsync(List<int> id)
        {
            return Context.Deleteable<T>().In(id).ExecuteCommandAsync();
        }
        /// <summary>
        /// 根据实体删除数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int DeleteByEntity(T t)
        {
            return Context.Deleteable<T>(t).ExecuteCommand();
        }
        /// <summary>
        /// 根据实体删除数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteByEntityAsync(T t)
        {
            return Context.Deleteable<T>(t).ExecuteCommandAsync();
        }
        /// <summary>
        /// 更新指定ID的实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int UpdateById(T t)
        {
            return Context.Updateable<T>(t).ExecuteCommand();
        }
        /// <summary>
        /// 更新指定ID的实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateByIdAsync(T t)
        {
            return Context.Updateable<T>(t).ExecuteCommandAsync();
        }
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int InsertOrUpdate(T t)
        {
            return Context.Storageable<T>(t).DefaultAddElseUpdate().ExecuteCommand();
        }
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual Task<int> InsertOrUpdateAsync(T t)
        {
            return Context.Storageable<T>(t).DefaultAddElseUpdate().ExecuteCommandAsync();
        }
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual int InsertOrUpdate(List<T> t)
        {
            return Context.Storageable<T>(t).DefaultAddElseUpdate().ExecuteCommand();
        }
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual Task<int> InsertOrUpdateAsync(List<T> t)
        {
            return Context.Storageable<T>(t).DefaultAddElseUpdate().ExecuteCommandAsync();
        }
    }
}
