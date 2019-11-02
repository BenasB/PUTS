using Processing;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class ResultViewModel
    {
        public int ProblemID { get; set; }

        public string Name { get; set; }

        public UserProgram.Result CompilationResult { get; set; }

        public List<TestResult> TestResults { get; set; } = new List<TestResult>();

        public int PassedTests { get; set; }

        public struct TestResult
        {
            public Test Test { get; set; }
            public UserProgram.Result ExecutionResult { get; set; }
            public UserProgram.Result EvaluationResult { get; set; }
        }
    }
}
