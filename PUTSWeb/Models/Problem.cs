using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PUTSWeb.Helpers;

namespace PUTSWeb.Models
{
    public class Problem
    {
        [Display (Name="ID")]
        public int ProblemID { get; set; }

        [Display (Name = "Name")]
        [Required (ErrorMessage = "Please specify the name")]
        public string Name { get; set; }

        [Display (Name = "Description")]
        [Required (ErrorMessage = "Please create a description")]
        public string Description { get; set; }

        [Display (Name = "Input")]
        [Required (ErrorMessage = "Please desribe the input")]
        public string InputDescription { get; set; }

        [Display(Name = "Output")]
        [Required(ErrorMessage = "Please desribe the output")]
        public string OutputDescription { get; set; }

        [Display(Name = "Examples")]
        public List<Example> Examples { get; set; } = new List<Example>();

        [Display(Name = "Tests")]
        [MinimumCount(1, ErrorMessage = "At least 1 test must exist")]
        public List<Test> Tests { get; set; } = new List<Test>();

        public int TimesSolved { get; set; } = 0;

        [Display(Name = "Show failed test cases")]
        public bool ShowFailedTestCases { get; set; } = true;

        [Display(Name = "Added")]
        public DateTime AddedDate { get; set; }
    }
}
