using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PUTSWeb.Helpers;

namespace PUTSWeb.Models
{
    public class SolveViewModel
    {
        [Required (ErrorMessage = "Please select a file")]
        [SourceFileValidation (ErrorMessage = "Extension not supported")]
        public IFormFile SourceFile { get; set; }

        [Display(Name = "ID")]
        public int ProblemID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Input")]
        public string InputDescription { get; set; }

        [Display(Name = "Output")]
        public string OutputDescription { get; set; }

        [Display(Name = "Examples")]
        public List<Example> Examples { get; set; } = new List<Example>();
    }
}
