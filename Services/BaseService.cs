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

        public virtual T this[int id] => GetById(id);
        public virtual List<T> this[Expression<Func<T, bool>> where] => GetQuery(where).ToList();

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

        public virtual T Get(Expression<Func<T, bool>> where = null)
        {
            return repository.Get(where);
        }

        public virtual ISugarQueryable<T> GetAll()
        {
            return repository.GetAll();
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> where = null)
        {
            return repository.GetAsync(where);
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, bool isAsc = true)
        {
            return repository.GetAsync(where, orderby, isAsc);
        }

        public virtual T GetById(int id)
        {
            return repository.GetById(id);
        }

        public virtual Task<T> GetByIdAsync(int id)
        {
            return repository.GetByIdAsync(id);
        }

        public virtual ISugarQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return repository.GetQuery(predicate);
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
