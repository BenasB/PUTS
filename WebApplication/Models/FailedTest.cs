﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class FailedTest
    {
        [Display (Name = "Expected")]
        public string Expected { get; set; }

        [Display(Name = "Returned")]
        public string Returned { get; set; }
    }
}
