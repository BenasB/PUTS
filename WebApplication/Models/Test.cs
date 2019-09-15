using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Test
    {
        public int ID { get; set; }

        [Display(Name = "Given Input")]
        public string GivenInput { get; set; }

        [Display(Name = "Expected Output")]
        [Required (ErrorMessage = "Expected output must exist")]
        public string ExpectedOutput { get; set; }
    }
}
