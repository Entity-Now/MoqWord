using ColorHelper;
using MoqWord.Core.Interface;
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

            return words.Select(x => new Word
            {
                WordName = x.name,
                Reps = 0,
                Lapses = 0,
                Grasp = false,
                UpdateDT = DateTime.Now,
                CreateDT = DateTime.Now,
                AnnotationUs = x.usphone ?? "",
                AnnotationUk = x.ukphone ?? "",
                Definition = "",
                Interval = 0,
                PartOfSpeech = "",
                Translation = string.Join("\n", x.trans),
            });
        }
    }
}
