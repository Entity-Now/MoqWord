using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Enum
{
    public enum RepeatType
    {
        [Display(Name = "一次")]
        One = 1,
        [Display(Name = "三次")]
        Three = 3,
        [Display(Name = "五次")]
        Five = 5,
        [Display(Name = "八次")]
        Eight = 8,
        [Display(Name = "无限次")]
        Many = 999,
    }
}
