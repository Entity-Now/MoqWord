using MoqWord.Repository.Interface;
using MoqWord.Services.Interface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity, new()
    {
        public virtual IBaseRepository<T> repository { get; set; }
        public BaseService(IBaseRepository<T> _repository) 
        {
            repository = _repository;
        }

        public virtual T this[int id] => FindById(id);
        public virtual List<T> this[Expression<Func<T, bool>> where] => Query(where).ToList();

        public virtual int DeleteByEntity(T t)
        {
            return repository.DeleteByEntity(t);
        }

        public virtual Task<int> DeleteByEntityAsync(T t)
        {
            return repository.DeleteByEntityAsync(t);
        }

        public virtual int DeleteById(int id)
        {
            return repository.DeleteById(id);
        }

        public virtual int DeleteById(List<int> id)
        {
            return repository.DeleteById(id);
        }

        public virtual Task<int> DeleteByIdAsync(int id)
        {
            return repository.DeleteByIdAsync(id);
        }

        public virtual Task<int> DeleteByIdAsync(List<int> id)
        {
            return repository.DeleteByIdAsync(id);
        }

        public virtual T First(Expression<Func<T, bool>> where = null)
        {
            return repository.First(where);
        }

        public virtual ISugarQueryable<T> All()
        {
            return repository.All();
        }

        public virtual Task<T> FirstAsync(Expression<Func<T, bool>> where = null)
        {
            return repository.FirstAsync(where);
        }

        public virtual Task<T> FirstAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, bool isAsc = true)
        {
            return repository.FirstAsync(where, orderby, isAsc);
        }

        public virtual T FindById(int id)
        {
            return repository.FindById(id);
        }

        public virtual Task<T> FindByIdAsync(int id)
        {
            return repository.FindByIdAsync(id);
        }

        public virtual ISugarQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return repository.Query(predicate);
        }

        public virtual InsertNavTaskInit<T, T> InsertNav(List<T> list)
        {
            return repository.InsertNav(list);
        }
        public virtual InsertNavTaskInit<T, T> InsertNav(T list)
        {
            return repository.InsertNav(list);
        }
        public virtual int InsertOrUpdate(T t)
        {
            return repository.InsertOrUpdate(t);
        }

        public virtual int InsertOrUpdate(List<T> t)
        {
            return repository.InsertOrUpdate(t);
        }

        public virtual Task<int> InsertOrUpdateAsync(T t)
        {
            return repository.InsertOrUpdateAsync(t);
        }

        public virtual Task<int> InsertOrUpdateAsync(List<T> t)
        {
            return repository.InsertOrUpdateAsync(t);
        }

        public virtual int UpdateById(T t)
        {
            return repository.UpdateById(t);
        }

        public virtual Task<int> UpdateByIdAsync(T t)
        {
            return repository.UpdateByIdAsync(t);
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual int Update(T t, Expression<Func<T, bool>> expression)
        {
            return repository.Update(t, expression);
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(T t, Expression<Func<T, bool>> expression)
        {
            return repository.UpdateAsync(t, expression);
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual int Update(List<T> t, Expression<Func<T, bool>> expression)
        {
            return repository.Update(t, expression);
        }
        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(List<T> t, Expression<Func<T, bool>> expression)
        {
            return repository.UpdateAsync(t, expression);
        }

        /// <summary>
        /// 更新指定的列
        /// </summary>
        /// <param name="t"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual int SetColumns(Expression<Func<T, T>> col, Expression<Func<T, bool>> wh)
        {
            return repository.SetColumns(col, wh);
        }
        /// <summary>
        /// 更新指定的列
        /// </summary>
        /// <param name="t"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual Task<int> SetColumnsAsync(Expression<Func<T, T>> col, Expression<Func<T, bool>> wh)
        {
            return repository.SetColumnsAsync(col, wh);
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count()
        {
            return repository.Count();
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> CountAsync()
        {
            return repository.CountAsync();
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> expression)
        {
            return repository.Count(expression);
        }
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <returns></returns>
        public virtual Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return repository.CountAsync(expression);
        }
    }
}
