using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Repository.Interface
{
    public interface IBaseRepository<T> : ISimpleClient<T> where T : BaseEntity, new()
    {
        T this[int id] => GetById(id);
        List<T> this[Expression<Func<T, bool>> where] => GetQuery(where).ToList();

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        ISugarQueryable<T> GetAll();
        /// <summary>
        /// 查询符合条件的数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ISugarQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>实体</returns>
        T Get(Expression<Func<T, bool>> @where = null);
        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>实体</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> @where = null);

        /// <summary>
        /// 获取第一条数据
        /// </summary>
        /// <typeparam name="TS">排序</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns>实体</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> @where, Expression<Func<T, object>> @orderby, bool isAsc = true);


        /// <summary>
        /// 根据ID找实体
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        T GetById(int id);

        /// <summary>
        /// 根据ID找实体(异步)
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteById(int id);

        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteById(List<int> id);
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByIdAsync(int id);
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByIdAsync(List<int> id);
        /// <summary>
        /// 根据实体删除数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int DeleteByEntity(T t);
        /// <summary>
        /// 根据实体删除数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> DeleteByEntityAsync(T t);
        /// <summary>
        /// 更新指定ID的实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int UpdateById(T t);
        /// <summary>
        /// 更新指定ID的实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> UpdateByIdAsync(T t);
        /// <summary>
        /// 级联插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        InsertNavTaskInit<T, T> InsertNav(List<T> list);
        /// <summary>
        /// 级联插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        InsertNavTaskInit<T, T> InsertNav(T list);
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int InsertOrUpdate(T t);
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> InsertOrUpdateAsync(T t);
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int InsertOrUpdate(List<T> t);
        /// <summary>
        /// 插入或更新
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<int> InsertOrUpdateAsync(List<T> t);
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
    }
}
