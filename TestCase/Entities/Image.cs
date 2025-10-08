using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCase.Entities
{
    public class Image : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}