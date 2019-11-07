namespace PUTSWeb.Models
{
    public class EvaluationViewModel
    {
        public int ProblemID { get; set; }
        public string ProblemName { get; set; }

        public PaginatedList<EvaluationRow> PaginatedList { get; set; }

        public class EvaluationRow
        {
            public string FullName { get; set; }
            public string Group { get; set; }
            public ProblemResult ProblemResult { get; set; }
        }       
    }
}
