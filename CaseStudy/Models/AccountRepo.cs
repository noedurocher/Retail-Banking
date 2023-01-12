using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.ViewModel;

namespace CaseStudy.Models
{
    public class AccountRepo:IAccountRepo
    {
        public ManagementContext _context;
        public AccountRepo(ManagementContext context)
        {
            _context = context;
        }

        public int AddAccount(Account account)
        {
            List<Account> accounts = CustomerAccountData(account);
            if(accounts.Count == 0)
            {
                _context.Account.Add(account);
                _context.SaveChanges();
                return 1;
            }
            if (accounts.Count == 2)
                return 200;
            if (accounts.First().AccountType == account.AccountType)
                return 300;
            _context.Account.Add(account);
            _context.SaveChanges();
            return 1;
        }

        public List<Account> CustomerAccountData(Account account)
        {
            return _context.Account.Where(a => a.CustomerID == account.CustomerID).ToList();
        }

        public List<Account> GetAccountUsingCustomerID(int id)
        {
            return _context.Account.Where(a => a.CustomerID == id).ToList();

        }

        public List<Account> GetAccountUsingSSNID(int id)
        {
            Customer customer = GetCustomerID(id);
            if (customer == null)
                return new List<Account>();
            int customerid = customer.CustomerID;
            return _context.Account.Where(a => a.CustomerID == customerid).ToList();
        }

        public List<Customer> GetData()
        {
            return _context.Customer.ToList();
        }

        public List<Account> GetAllAccounts()
        {
            return _context.Account.ToList();
        }
        public Customer GetCustomerID(int id)
        {
            return _context.Customer.Where(a => a.SSNID == id).FirstOrDefault();
        }

        public Account GetAccountDetails(int id)
        {
            return _context.Account.Where(a => a.AccountID == id).FirstOrDefault();
        }

        public int confirmdelete(Account account)
        {
            _context.Account.Remove(account);
            _context.SaveChanges();
            return 1;
        }

        public int delete(Account account)
        {
            if (account.AccountBalance > 0)
            {
                //_context.Account.Remove(account);
                //_context.SaveChanges();
                return 500;
            }
            _context.Account.Remove(account);
            _context.SaveChanges();
            return 1;
        }

        public CustomerAccountViewModel ViewAccountAndCustomerDetails(int accountNum)
        {
            try{
                var res = (from a in _context.Account
                           join c in _context.Customer on a.CustomerID equals c.CustomerID
                           where a.AccountID == accountNum
                           select new CustomerAccountViewModel()
                           {
                               Customer = c,
                               Account = a
                           }).FirstOrDefault();
                return res;
            }
            catch
            {
                return null;
            }
           
        }

        public int deposit(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();
            return account.AccountBalance;
        }

        public int Withdraw(Account account, int amount)
        {
            try
            {
                if (account.AccountBalance < 1 || account.AccountBalance<amount)
                {
                    return 0;
                }
                else
                {
                    _context.Entry(account).State = EntityState.Modified;
                    _context.SaveChanges();
                    return account.AccountBalance;
                }
                
                
               
            }catch
            {
                return 0;
            }
        }
    }
}
