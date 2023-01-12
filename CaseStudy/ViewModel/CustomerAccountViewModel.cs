using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.Models;
namespace CaseStudy.ViewModel
{
    public class CustomerAccountViewModel
    {
        public Account Account {get;set;}
        public Customer Customer { get; set; }
        public List<Customer> ViewID { get; set; }
        public List<Account> ViewAccounts { get; set; }
    }
    public enum AccountType
    {
        Savings,
        Checkings
    }
}
