using Mapster;
using MoqWord.Repository.Interface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Repository
{
    public class BaseRepository<T> : SimpleClient<T>, IBaseRepository<T> where T : BaseEntity , new()
    {
        public TypeAdapterConfig Config { get; set; }
        public BaseRepository(ISqlSugarClient db, TypeAdapterConfig _config) {
            Context = db;
            Config = _config;
        }

        T this[int id] => GetById(id);
        List<T> this[Expression<Func<T, bool>> where] => Query(where).ToList();
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public virtual ISugarQueryable<T> All()
        {
            return Context.Queryable<T>();
        }
        /// <summary>
        /// 查询符合条件的数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return Context.Queryable<T>().Where(predicate);
        }
        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>实体</returns>
        public T First(Expression<Func<T, bool>> @where = null)
        {
            return @where == null ? Context.Queryable<T>().First() : Context.Queryable<T>().First(@where);
        }
        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>实体</returns>
        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> @where = null)
        {
            return @where == null ? await Context.Queryable<T>().FirstAsync() : await Context.Queryable<T>().FirstAsync(@where);
        }

        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <typeparam name="TS">排序</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>实体</returns>
        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, bool isAsc = true)
        {
            var data = Context.Queryable<T>();
            return isAsc ? await data.OrderBy(orderby, OrderByType.Asc).FirstAsync() : await Context.Queryable<T>().OrderBy(orderby, OrderByType.Desc).FirstAsync();
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
        public virtual TDto First<TDto>(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, bool isAsc = true) where TDto : class
        {
            var data = Context.Queryable<T>();
            return isAsc ? data.OrderBy(orderby, OrderByType.Asc).First().Adapt<TDto>(Config) : Context.Queryable<T>().OrderBy(orderby, OrderByType.Desc).First().Adapt<TDto>(Config);
        }

        /// <summary>
        /// 根据ID找实体
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        public virtual T FindById(int id)
        {
            return Context.Queryable<T>().InSingle(id);
        }

        /// <summary>
        /// 根据ID找实体(异步)
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        public virtual async Task<T> FindByIdAsync(int id)
        {
            return await Context.Queryable<T>().InSingleAsync(id);
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
        public virtual async Task<int> DeleteByIdAsync(int id)
        {
            return await Context.Deleteable<T>().In(id).ExecuteCommandAsync();
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
        public virtual async Task<int> DeleteByIdAsync(List<int> id)
        {
            return await Context.Deleteable<T>().In(id).ExecuteCommandAsync();
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
        public virtual async Task<int> DeleteByEntityAsync(T t)
        {
            return await Context.Deleteable<T>(t).ExecuteCommandAsync();
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
        public virtual async Task<int> UpdateByIdAsync(T t)
        {
            return await Context.Updateable<T>(t).ExecuteCommandAsync();
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual int Update(T t, Expression<Func<T, bool>> expression)
        {
            return Context.Updateable(t).Where(expression).ExecuteCommand();
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T t, Expression<Func<T, bool>> expression)
        {
            return await Context.Updateable(t).Where(expression).ExecuteCommandAsync();
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual int Update(List<T> t, Expression<Func<T, bool>> expression) 
        {
            return Context.Updateable(t).ExecuteCommand();
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(List<T> t, Expression<Func<T, bool>> expression)
        {
            return await Context.Updateable(t).ExecuteCommandAsync();
        }
        /// <summary>
        /// 更新指定的列
        /// </summary>
        /// <param name="t"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual int SetColumns(Expression<Func<T, T>> expression, Expression<Func<T, bool>> wh)
        {
            return Context.Updateable<T>().SetColumns(expression).Where(wh).ExecuteCommand();
        }
        /// <summary>
        /// 更新指定的列
        /// </summary>
        /// <param name="t"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual async Task<int> SetColumnsAsync(Expression<Func<T, T>> expression, Expression<Func<T, bool>> wh)
        {
            return await Context.Updateable<T>().SetColumns(expression).Where(wh).ExecuteCommandAsync();
        }
        /// <summary>
        /// 级联插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual InsertNavTaskInit<T,T> InsertNav(List<T> list)
        {
            return Context.InsertNav(list);
        }
        /// <summary>
        /// 级联插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual InsertNavTaskInit<T, T> InsertNav(T list)
        {
            return Context.InsertNav(list);
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
        public virtual async Task<int> InsertOrUpdateAsync(T t)
        {
            return await Context.Storageable<T>(t).DefaultAddElseUpdate().ExecuteCommandAsync();
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
        public virtual async Task<int> InsertOrUpdateAsync(List<T> t)
        {
            return await Context.Storageable<T>(t).DefaultAddElseUpdate().ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual int InsertList(List<T> list)
        {
            return Context.Insertable<T>(list).ExecuteCommand();
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual async Task<int> InsertListAsync(List<T> list)
        {
            return await Context.Insertable<T>(list).ExecuteCommandAsync();
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            return Context.Queryable<T>().Count();
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> CountAsync()
        {
            return await Context.Queryable<T>().CountAsync();
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> expression)
        {
            return Context.Queryable<T>().Where(expression).Count();
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await Context.Queryable<T>().Where(expression).CountAsync();
        }
    }
}
