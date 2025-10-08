using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCase.Entities
{
    public class TestResultImage : BaseEntity
    {
        public int TestResultId { get; set; }
        public int ImageId { get; set; }
    }
}