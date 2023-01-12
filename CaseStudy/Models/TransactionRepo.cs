using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.Models
{
    public class TransactionRepo : ITransactionRepo
    {
        public ManagementContext _context;
        public TransactionRepo(ManagementContext context)
        {
            _context = context;
        }

        public List<Transaction> ViewTransactions(int id,int count)
        {
            return _context.Transactions.Where(a => a.AccountID == id).OrderBy(a => a.TransactionID).Take(count).ToList();
        }

        public List<Transaction> ViewTransactionsDate(int id, DateTime d1, DateTime d2)
        {
            return _context.Transactions.Where(a => a.AccountID == id).Where(x => x.TransactionDate >= d1).Where(x => x.TransactionDate <= d2).ToList();
        }
        public int add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return 1;
        }
        public TransferViewModel TransferScreen(int amount, int id1, int id2)
        {
            TransferViewModel transfer = new TransferViewModel();
            if (id1 == id2)
            {
                transfer.SourceAccountID = -4;
                return transfer;
            }
            Account account1 = _context.Account.Where(a => a.AccountID == id1).FirstOrDefault();
            Account account2 = _context.Account.Where(a => a.AccountID == id2).FirstOrDefault();           
            if (account1.AccountBalance == 0)
            {
                transfer.SourceAccountID = 0;
                return transfer;
            }           
           else if(account1.AccountBalance < amount)
            {
                transfer.SourceAccountID = -1;
                return transfer;
            }
            else if (account2 == null)
            {
                transfer.SourceAccountID = -2;
                return transfer;
            }
            else if (account1 == null)
            {
                transfer.SourceAccountID = -3;
                return transfer;
            }
            else
            {
                DateTime date = Convert.ToDateTime(DateTime.Now);
                transfer.SourceAccountID = account1.AccountID;
                transfer.TargetAccountID = account2.AccountID;
                transfer.SourceCurrentBalance = account1.AccountBalance;
                transfer.TargetCurrentBalance = account2.AccountBalance;
                account1.AccountBalance = account1.AccountBalance - amount;
                account1.Status = $"Withdraw on {date} ";
                account2.AccountBalance = account2.AccountBalance + amount;
                account2.Status= $"Deposit on {date} ";
                _context.Entry(account1).State = EntityState.Modified;
                _context.SaveChanges();
                _context.Entry(account2).State = EntityState.Modified;
                _context.SaveChanges();
                transfer.SourceUpdatedBalance = account1.AccountBalance;
                transfer.TargetUpdatedBalance = account2.AccountBalance;
                Transaction transaction1 = new Transaction();
                transaction1.AccountID = account1.AccountID;
                transaction1.TransactionAmount = amount;
                transaction1.TransactionDate = Convert.ToDateTime(DateTime.Now);
                transaction1.TransactionType = "Withdraw";
                _context.Transactions.Add(transaction1);
                _context.SaveChanges();
                Transaction transaction2 = new Transaction();
                transaction2.AccountID = account2.AccountID;
                transaction2.TransactionAmount = amount;
                transaction2.TransactionDate = Convert.ToDateTime(DateTime.Now);
                transaction2.TransactionType = "Deposit";
                _context.Transactions.Add(transaction2);
                _context.SaveChanges();
                return transfer;
            }

        }

        public int delete(int id)
        {
            _context.Transactions.RemoveRange(_context.Transactions.Where(a => a.AccountID == id));
            _context.SaveChanges();
            return 1;
        }
    }
}
