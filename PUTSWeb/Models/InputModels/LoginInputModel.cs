using System.ComponentModel.DataAnnotations;

namespace PUTSWeb.Models.InputModels
{
    public class LoginInputModel
    {
        [Required (ErrorMessage = "Username is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
