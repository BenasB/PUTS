using System.ComponentModel.DataAnnotations;

namespace PUTSWeb.Models
{
    public class Test
    {
        public int TestID { get; set; }

        [Display(Name = "Given Input")]
        public string GivenInput { get; set; }

        [Display(Name = "Expected Output")]
        [Required(ErrorMessage = "Expected output must exist")]
        public string ExpectedOutput { get; set; }

        public Problem Problem { get; set; }
    }
}
