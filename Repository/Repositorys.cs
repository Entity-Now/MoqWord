using Mapster;
using MoqWord.Model.EntityDTO;
using MoqWord.Repository.Interface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Repository
{
    public class BookRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Book>(db, _config), IBookRepository;
    public class PersonalRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Personal>(db, _config), IPersonalRepository;
    public class SettingRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Setting>(db, _config), ISettingRepository;
    public class WordRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Word>(db, _config), IWordRepository;
    public class WordLogRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<WordLog>(db, _config), IWordLogRepository;
    public class TagRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Tag>(db, _config), ITagRepository;
    public class PopupConfigRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<PopupConfig>(db, _config), IPopupConfigRepository;
    public class ShortcutKeysRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<ShortcutKeys>(db, _config), IShortcutKeysRepository;
}
