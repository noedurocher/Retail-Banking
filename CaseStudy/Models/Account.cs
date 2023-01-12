using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class Account
    {
        [Key]
        [Display(Name = "Customer ID")]
        [Required(ErrorMessage = "Customer ID is required")]
        public int CustomerID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Account ID")]
        public int AccountID { get; set; }

        [Display(Name = "Account Type")]
        [Required(ErrorMessage = "Account Type is required")]
        public string AccountType { get; set; }

        [Display(Name = "Account Balance")]
        [Required(ErrorMessage = "Account Balance is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int AccountBalance { get; set; }

        public string Status { get; set; }
    }

    
}
