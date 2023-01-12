using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.ViewModel;

namespace CaseStudy.Models
{
    public interface ITransactionRepo
    {
        public int add(Transaction transaction);
        TransferViewModel TransferScreen(int amount,int id1, int id2);
        public List<Transaction> ViewTransactionsDate(int id, DateTime d1, DateTime d2);
        public List<Transaction> ViewTransactions(int id,int count);
        public int delete(int id);
    }
}
