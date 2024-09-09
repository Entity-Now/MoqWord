using AntDesign;
using FreeSql.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Win32;
using MoqWord.Model.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using MyTag = MoqWord.Model.Entity.Tag;
using ColorHelper;
using MoqWord.Utlis;

namespace MoqWord.Components.Page
{
    public partial class WordList
    {
        // 表单是否显示
        bool modalIsVisible = false;
        bool wordsView = false;
        // 当前选中的导入平台
        string selectPlatform = "Qwerty";
        Dictionary<string, Func<IImportWords>> importPlatform = new()
        {
            ["Qwerty"] = () => new QwertyLearnerImport(),
            ["不背单词"] = () => new QwertyLearnerImport(),
            ["有道"] = () => new QwertyLearnerImport(),
        };
        Dictionary<string, string> langType = new()
        {
            ["en"] = "英语",
            ["ja"] = "日语",
            ["code"] = "代码",
        };
        Dictionary<string, MyTag> currentTags { get; set; } = new();
        List<MyTag>? tags { get; set; }
        List<string> allTag { get; set; } = new List<string>();
        // tag多选内容
        IEnumerable<string> selectTags { get; set; } = new List<string>();
        List<BookDTO>? categories { get; set; }
        List<string>? languageCategorys { get; set; }
        BookDTO? currSelectBook { get; set; }
        Book model { get; set;} =  new Book();
        string curBook { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            tags = _tagService.All().ToList();
            categories = _service.GetAllBook().ToList();
            // 读取本地字典
            var distConfs = ResourceHelper.GetFileText("Local/dist_conf.json").StringToAny<List<DistConf>>();

            categories.AddRange(distConfs.Where(d => !categories.Any(c => d.name == c.Name)).Select(d =>
            {
                var color = ColorGenerate.GetLightRandomColor<HEX>().ToString();
                return new BookDTO
                {
                    Category = d.category,
                    Color = color,
                    Count = d.length,
                    GraspCount = 0,
                    Name = d.name,
                    LanguageCategory = d.languageCategory,
                    Language = d.language,
                    IsExternal = true,
                    UpdateDT = DateTime.Now,
                    Description = "",
                    ElapsedDays = 0,
                    ScheduledDays = 0,
                    Path = d.url,
                    Tags = d.tags.Select(t => new MyTag
                    {
                        Name = t,
                        CreateDT = DateTime.Now
                    }).ToList()
                };
            }));
            languageCategorys = categories.GroupBy(c => c.LanguageCategory).Select(c => c.Key).Where(c => !string.IsNullOrEmpty(c)).ToList();
            curBook = getLanName(languageCategorys[0]);
        }
        void openModal()
        {
            modalIsVisible = true;
        }
        void openWordsView(BookDTO item)
        {
            if (item.Words is null or default(List<Word>))
            {
                item.Words = _wordService.Query(x => x.BookId == item.Id).ToList();
            }
            wordsView = true;
            currSelectBook = item;
        }
        void OkHandle(EditContext context)
        {
            Task.Run(async () =>
            {
                try
                {
                    var open = new OpenFileDialog();
                    var showDialog = open.ShowDialog();
                    if (showDialog == true)
                    {
                        var haveTags = tags.Where(x => selectTags.Any(i => i == x.Name));
                        var addTags = selectTags.Where(x => !haveTags.Any(i => i.Name == x)).Select(item => new MyTag
                        {
                            Name = item,
                            CreateDT = DateTime.Now,
                            UpdateDT = DateTime.Now
                        });
                        var wordSource = File.ReadAllText(open.FileName);
                        model.CreateDT = DateTime.Now;
                        model.UpdateDT = DateTime.Now;
                        model.Tags = haveTags.Concat(addTags).ToList();
                        // handle words
                        var importHandle = importPlatform[selectPlatform]();
                        model.Words = importHandle.ImportWords(wordSource).ToList();
                        // save words
                        var insertState = await _service.InsertNav(model)
                        .Include(x => x.Words)
                        .Include(x => x.Tags)
                        .ExecuteCommandAsync();
                        if (insertState)
                        {
                            _message.Success("导入成功~");
                            modalIsVisible = false;
                            InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            _message.Error("导入失败~");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _message.Error(ex.Message);
                }

            });
        }
        async void SelectBook(BookDTO c)
        {
            Book book = c.Adapt<Book>();
            var findBook = await _service.FirstAsync(b => b.Name == c.Name && b.Language == c.Language);
            if (findBook != null)
            {
                if (_service.SelectBook(book))
                {
                    await _message.Success("设置成功~");
                    return;
                }
                await _message.Warning("设置失败~");
                return;
            }
            // 导入json文件
            var readBook = ResourceHelper.GetFileText($"{Constants.SelfPath}/Local/{c.Path.Replace("/", @"\")}");
            var qwerty = importPlatform["Qwerty"]();
            book.IsCurrent = true;
            book.Words = qwerty.ToWords(JsonHelpaer.StringToAny<QwertyLearnerWord[]>(readBook)).ToList();
            var insertRes = await _service.InsertNav(book)
                .Include(b => b.Words)
                .Include(w => w.Tags)
                .ExecuteCommandAsync();
            if (insertRes)
            {
                await _message.Success("导入成功！");
                _service.SelectBook(_service.First(b => b.Name == book.Name));
            }
            else
            {
                await _message.Warning("导入失败！");
            }
        }

        async void PlayWord(Word word)
        {
            var sound = _settingService.getCurrentSound();
            await sound.PlayAsync(word.WordName);
        }

        string getLanName(string val)
        {
            return langType.TryGetValue(val, out var langName) ? langName : val;
        }
        string getLanKey(string val)
        {
            var findVal = langType.FirstOrDefault(x => x.Value == val);
            return !string.IsNullOrEmpty(findVal.Key)  ? findVal.Key : val;
        }

        void SetCurBook(string val)
        {
            curBook = val;
            currentTags.Clear();
        }

        void SetCurTag(string key, MyTag myTag)
        {
            if (currentTags.ContainsKey(key))
            {
                currentTags[key] = myTag;
            }
            else
            {
                currentTags.Add(key, myTag);
            }
        }
    }
}
