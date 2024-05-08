using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CellTypeAttribute : Attribute
    {
        public object Value { get; set; }
        public string Title { get; set; }
        public Type? ValueType { get; set; }
        public CellType CellType { get; set; }
        public string Description { get; set; }
        public List<SelectValue>? Options { get; set; }

        public CellTypeAttribute() { }
        public CellTypeAttribute(string title, object value) : this()
        {
            Title = title;
            Value = value;
        }
        public CellTypeAttribute(string title, object value, CellType cellType) : this(title, value)
        {
            CellType = cellType;
        }
        public CellTypeAttribute(string title, object value,  List<SelectValue> options, CellType cellType) : this(title, value)
        {
            Options = options;
        }
        public CellTypeAttribute(string title, object value, List<SelectValue> options, CellType cellType, Type? type) : this(title, value, options, cellType)
        {
            ValueType = type;
        }
        public CellTypeAttribute(string title, object value, CellType cellType, Type? type) : this(title, value, cellType)
        {
            ValueType = type;
        }
    }
}
