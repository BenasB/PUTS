using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Example : Test
    {
        [Display(Name = "Explanation")]
        public string Explanation { get; set; }
    }
}
