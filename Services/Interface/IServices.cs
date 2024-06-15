using MoqWord.Model.EntityDTO;
using MoqWord.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services.Interface
{
    
    public interface IPersonalService : IBaseService<Personal>;
    
    public interface IWordService : IBaseService<Word>;
    public interface IWordLogService : IBaseService<WordLog>;
    public interface ITagService : IBaseService<Tag>;
    public interface IPopupConfigService : IBaseService<PopupConfig>;
}
