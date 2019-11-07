using System.ComponentModel.DataAnnotations;

namespace PUTSWeb.Models.InputModels
{
    public class DeletePersonalDataInputModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
