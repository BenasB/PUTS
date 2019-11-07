using System.ComponentModel.DataAnnotations;

namespace PUTSWeb.Models
{
    public class Example
    {
        public int ExampleID { get; set; }

        [Display(Name = "Given Input")]
        public string GivenInput { get; set; }

        [Display(Name = "Expected Output")]
        [Required(ErrorMessage = "Expected output must exist")]
        public string ExpectedOutput { get; set; }

        [Display(Name = "Explanation")]
        public string Explanation { get; set; }

        public Problem Problem { get; set; }
    }
}
