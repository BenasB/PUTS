namespace PUTSWeb.Models
{
    public class ProblemResult
    {
        public int ProblemResultID { get; set; }

        public int ProblemID { get; set; }
        public ProgramResult FirstResult { get; set; }
        public ProgramResult BestResult { get; set; }
    }
}
