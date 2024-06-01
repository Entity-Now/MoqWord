using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Data
{
    public class WordNotify : INotification
    {
        public int CurrentIndex { get; set; }
        public WordNotify( int currentIndex)
        {
            CurrentIndex = currentIndex;
        }
    }
}
