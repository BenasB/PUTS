using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.InputModels
{
    public class DeletePersonalDataInputModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
