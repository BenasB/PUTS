using System;
using System.ComponentModel.DataAnnotations;

namespace PUTSWeb.Models
{
    public class Blog
    {
        public int BlogID { get; set; }

        public string Author { get; set; }

        [Display (Name = "Title")]
        [Required (ErrorMessage = "Please specify the title")]
        public string Title { get; set; }
        
        [Display (Name = "Text")]
        [Required(ErrorMessage = "Text can't be empty")]
        public string Text { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
