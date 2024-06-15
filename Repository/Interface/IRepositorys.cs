using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoqWord.Model.EntityDTO;

namespace MoqWord.Repository.Interface
{
    public interface ICategoryRepository : IBaseRepository<Category>;
    public interface IPersonalRepository : IBaseRepository<Personal>;
    public interface ISettingRepository : IBaseRepository<Setting>;
    public interface IWordRepository : IBaseRepository<Word>;
    public interface IWordLogRepository : IBaseRepository<WordLog>;
    public interface ITagRepository : IBaseRepository<Tag>;
    public interface IPopupConfigRepository : IBaseRepository<PopupConfig>;
}
