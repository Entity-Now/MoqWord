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
    public class CategoryRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Category>(db, _config), ICategoryRepository;
    public class PersonalRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Personal>(db, _config), IPersonalRepository;
    public class SettingRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Setting>(db, _config), ISettingRepository;
    public class WordRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Word>(db, _config), IWordRepository;
    public class WordLogRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<WordLog>(db, _config), IWordLogRepository;
    public class TagRepository(ISqlSugarClient db, TypeAdapterConfig _config) : BaseRepository<Tag>(db, _config), ITagRepository;
}
