using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Enum
{
    public enum Sound
    {
        [Display(Name = "默认")]
        Default,
        [Display(Name = "Edge音频")]
        Edge,
        [Display(Name = "有道音频")]
        Youdao
    }
}
