using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord
{
    public class TypeMappingConfig
    {
        public static TypeAdapterConfig getConfig()
        {
            var config = new TypeAdapterConfig();
            config.ForType<Book, BookDTO>()
                .Map(dest => dest.Count, src => src.Words.Count())
                .Map(dest => dest.GraspCount, src => src.Words.Count(x => x.Grasp));

            return config;
        }
    }
}
