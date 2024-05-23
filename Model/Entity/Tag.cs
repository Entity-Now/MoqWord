using MoqWord.Model.EntityDTO;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class Tag : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Navigate(typeof(CategoryTag), nameof(CategoryTag.TagId), nameof(CategoryTag.CategoryId))]
        public IEnumerable<Category> Categories { get; set; }
    }
}
