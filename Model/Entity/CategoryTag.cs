﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class CategoryTag : BaseEntity
    {
        public int TagId { get; set; }
        public int CategoryId { get; set; }
    }
}
