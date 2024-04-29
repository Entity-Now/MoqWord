using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    public class BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public DateTime CreateDT { get; set; }
        public DateTime UpdateDT { get; set; }
    }
}
