using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Core.Interface
{
    public interface IImportWords
    {
        IEnumerable<Word> ImportWords(string wordList);

        IEnumerable<Word> ToWords<T>(IEnumerable<T> source);
    }
}
