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
        One,
        [Display(Name = "三次")]
        Three,
        [Display(Name = "五次")]
        Five,
        [Display(Name = "八次")]
        Eight,
        [Display(Name = "无限次")]
        Many,
    }
}
