using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.ViewModel;

namespace CaseStudy.Models
{
    public interface IAccountRepo
    {
        public int confirmdelete(Account account);
        public int AddAccount(Account account);
        public List<Account> CustomerAccountData(Account account);
        public List<Customer> GetData();
        public List<Account> GetAccountUsingCustomerID(int id);
        public List<Account> GetAccountUsingSSNID(int id);
        public Customer GetCustomerID(int id);
        public Account GetAccountDetails(int id);
        public int delete(Account account);
        public int deposit(Account account);
        public int Withdraw(Account account, int amount);
        CustomerAccountViewModel ViewAccountAndCustomerDetails(int accountNum);
        List<Account> GetAllAccounts();
    }
}
