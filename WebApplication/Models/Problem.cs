using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Problem
    {
        public int ID { get; set; }

        [Required (ErrorMessage = "Please specify the name")]
        public string Name { get; set; }

        [Required (ErrorMessage = "Please create a description")]
        public string Description { get; set; }

        [Required (ErrorMessage = "Please desribe the input")]
        public string InputDescription { get; set; }

        [Required(ErrorMessage = "Please desribe the output")]
        public string OutputDescription { get; set; }

        [MinimumCount(1, ErrorMessage = "At least 1 test must exist")]
        public List<Test> Tests { get; set; } = new List<Test>();
    }

    public class MinimumCount : ValidationAttribute
    {
        private readonly int minimumElements;

        public MinimumCount(int min)
        {
            minimumElements = min;
        }

        public override bool IsValid(object value)
        {
            var collection = value as ICollection;
            if (collection != null)
            {
                return collection.Count >= minimumElements;
            }

            return false;
        }
    }
}
