using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Repository
{
    public interface ICategoryRepository : IBaseRepository<Category>;
    public interface IPersonalRepository : IBaseRepository<Personal>;
    public interface ISettingRepository : IBaseRepository<Setting>;
    public interface IWordRepository : IBaseRepository<Word>;
    public interface IWordLogRepository : IBaseRepository<WordLog>;
}
