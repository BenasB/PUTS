namespace WebApplication.Models
{
    public class ProgramResult
    {
        public int ProgramResultID { get; set; }

        public int ProblemID { get; set; }
        public int PassedTests { get; set; }
        public int TestCount { get; set; }
        public int PercentageResult { get; set; }
    }
}
