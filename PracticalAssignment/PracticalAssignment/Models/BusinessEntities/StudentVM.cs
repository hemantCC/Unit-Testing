using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalAssignment.Models.BusinessEntities
{
    public class StudentVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required!")]
        public string Name { get; set; }
        [Range(5, 100, ErrorMessage = "Age Must be between 5 to 100")]
        public Nullable<int> Age { get; set; }
        [Required(ErrorMessage = "Standard is required!")]
        public string Standard { get; set; }
    }
}