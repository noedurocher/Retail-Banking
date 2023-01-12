using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.Models;
namespace CaseStudy.ViewModel
{
    public class TransactionViewModel
    {
        public List<Transaction> ViewTransaction { get; set; }
        public Transaction Transaction { get; set; }
        //public int id { get; set; }
        public int count { get; set; }
        public DateTime d1 { get; set; }
        public DateTime d2 { get; set; }
    }
}
