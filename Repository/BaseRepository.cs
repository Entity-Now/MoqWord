using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Repository
{
    internal class BaseRepository<T> : SimpleClient<T>, IBaseRepository<T> where T : class , new()
    {
        public BaseRepository(ISqlSugarClient db) {
            Context = db;
        }

        public List<T> getAll()
        {
            return Context.Queryable<T>().ToList();
        }
    }
}
