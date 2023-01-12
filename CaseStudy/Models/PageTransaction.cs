using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class PageTransaction
    {
        public int AccountID { get; set; }
        public int count { get; set; }
        public List<Transaction> transactions { get; set; }
    }
}
