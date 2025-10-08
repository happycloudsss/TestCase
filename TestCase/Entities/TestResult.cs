using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCase.Entities
{
    public class TestResult : BaseEntity
    {
        public int TestCaseId { get; set; }
        public TestResultStatus Status { get; set; }
        public string PsrNo { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int TestRound { get; set; } = 1;
        public int TestCount { get; set; } = 1;
    }

    public enum TestResultStatus
    {
        OK,
        NG
    }
}