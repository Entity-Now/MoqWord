using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Notify
{
    public class BookNotify : INotification
    {
        public Book Book { get; set; }
        public BookNotify(Book _Book)
        {
            Book = _Book;
        }
    }
}
