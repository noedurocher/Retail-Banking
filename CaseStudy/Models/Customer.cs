using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [Required(ErrorMessage ="SSNID is required")]
      
        public int SSNID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Age must be equal or greater than 1")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Address 1 is required")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        public string Status { get; set; }
    }
}
