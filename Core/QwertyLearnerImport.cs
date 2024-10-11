using ColorHelper;
using MoqWord.Core.Interface;
using MoqWord.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Core
{
    public class QwertyLearnerImport : IImportWords
    {
        public IEnumerable<Word> ImportWords(string wordList)
        {
            var words = wordList.StringToAny<List<QwertyLearnerWord>>();

            return ToWords(words);
        }

        public IEnumerable<Word> ToWords<T>(IEnumerable<T> source)
        {
            if (source is IEnumerable<QwertyLearnerWord> word)
                return word.Select(x => new Word
                {
                    WordName = x.name,
                    Due = DateTime.Now,
                    EasinessFactor = 0,
                    ReciteTime = DateTime.Now,
                    Repetition = 0,
                    Reps = 0,
                    Lapses = 0,
                    Grasp = false,
                    LastReview = DateTime.Now,
                    UpdateDT = DateTime.Now,
                    CreateDT = DateTime.Now,
                    AnnotationUs = x.usphone ?? "",
                    AnnotationUk = x.ukphone ?? "",
                    Definition = "",
                    Interval = 0,
                    PartOfSpeech = "",
                    Translates = x.trans.Select(t => new Translate{
                        Trans = t,
                        UpdateDT= DateTime.Now,
                        CreateDT = DateTime.Now,
                    }).ToList(),
                });
            else
                return Enumerable.Empty<Word>();
        }
    }
}
