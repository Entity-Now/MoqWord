using Mapster;
using MoqWord.Model.EntityDTO;
using MoqWord.Repository.Interface;
using MoqWord.Services.Interface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services
{
    
    public class PersonalService(IPersonalRepository repository) : BaseService<Personal>(repository), IPersonalService;
    public class WordService(IWordRepository repository) : BaseService<Word>(repository), IWordService;
    public class WordLogService(IWordLogRepository repository) : BaseService<WordLog>(repository), IWordLogService;
    public class TagService(ITagRepository repository) : BaseService<Tag>(repository), ITagService;
    public class PopupConfigService(IPopupConfigRepository repository) : BaseService<PopupConfig>(repository), IPopupConfigService;
    public class ShortcutKeysService(IShortcutKeysRepository repository) : BaseService<ShortcutKeys>(repository), IShortcutKeysService;
}
