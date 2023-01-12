using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Transaction ID")]
        public int TransactionID { get; set; }

        [Key]
        [Display(Name = "Account ID")]
        public int AccountID { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        [Display(Name = "Transaction Amount")]
        public int TransactionAmount { get; set; }

        //public List<Transaction>? transactions { get; set; }

    }
}
