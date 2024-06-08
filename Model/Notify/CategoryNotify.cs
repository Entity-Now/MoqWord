using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Notify
{
    public class CategoryNotify : INotification
    {
        public Category category { get; set; }
        public CategoryNotify(Category _category)
        {
            category = _category;
        }
    }
}
