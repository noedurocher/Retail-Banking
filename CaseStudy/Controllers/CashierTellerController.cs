using CaseStudy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.ViewModel;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace CaseStudy.Controllers
{
    [Authorize(Roles = "Cashier,Teller")]
    public class CashierTellerController : Controller
    {
        
        IAccountRepo _accountrepo;
        ICustomerRepo _customerrepo;
        ITransactionRepo _transactionRepo;
        IErrorRepo _errorRepo;
        public CashierTellerController(IAccountRepo arepo,ICustomerRepo crepo,ITransactionRepo trepo,IErrorRepo erepo)
        {
            _accountrepo = arepo;
            _customerrepo = crepo;
            _transactionRepo = trepo;
            _errorRepo = erepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewAccountBySSNID()
        {
            return View();
        }

        public IActionResult ViewAccountsUsingID()
        {
            return View();
        }

        public IActionResult ViewAccountUsingAccountID()
        {
            return View();
        }
        
        public IActionResult ViewAccounts(CustomerAccountViewModel customerAccountViewModel)
        {
            CustomerAccountViewModel c = new CustomerAccountViewModel();
            //if (customerAccountViewModel.Customer.SSNID == 0)
            //{
                c.ViewAccounts = _accountrepo.GetAccountUsingCustomerID(customerAccountViewModel.Account.CustomerID);
                if(c.ViewAccounts.Count == 0)
                {
                    c.ViewAccounts = _accountrepo.GetAccountUsingSSNID(customerAccountViewModel.Customer.SSNID);
                    if (c.ViewAccounts.Count == 0)
                    {
                        string error = _errorRepo.GetErrorDetails(2).ToString();
                        TempData["UserMessage"] = error;
                        //TempData["UserMessage"] = "No accounts found";
                        //return RedirectToAction("Index");
                        return RedirectToAction("ViewAccountBySSNID");
                    }
                    //string error = _errorRepo.GetErrorDetails(2).ToString();
                    //TempData["UserMessage"] = error;
                    ////TempData["UserMessage"] = "No accounts found";
                    ////return RedirectToAction("Index");
                    //return RedirectToAction("ViewAccountsUsingID");
                    //return View(c.ViewAccounts);
                }
                return View(c.ViewAccounts);
            //}
            //else
            //{
            //    c.ViewAccounts = _accountrepo.GetAccountUsingSSNID(customerAccountViewModel.Customer.SSNID);
            //    if (c.ViewAccounts.Count == 0)
            //    {
            //        string error = _errorRepo.GetErrorDetails(2).ToString();
            //        TempData["UserMessage"] = error;
            //        //TempData["UserMessage"] = "No accounts found";
            //        //return RedirectToAction("Index");
            //        return RedirectToAction("ViewAccountBySSNID");
            //    }
            //    return View(c.ViewAccounts);
            //}
        }

        public IActionResult ViewAccount(CustomerAccountViewModel customerAccountViewModel)
        {
            Account account = _accountrepo.GetAccountDetails(customerAccountViewModel.Account.AccountID);
            if (account == null)
            {
                string error = _errorRepo.GetErrorDetails(2).ToString();
                TempData["UserMessage"] = error;
                //TempData["UserMessage"] = "No accounts found";
                //return RedirectToAction("Index");
                return RedirectToAction("ViewAccountUsingAccountID");
            }
            return View(account);
        }
        public async Task<IActionResult> Deposit(Account account)
        {
            DepositViewModel depositViewModel = new DepositViewModel();
            depositViewModel.Account = _accountrepo.GetAccountDetails(account.AccountID);
            Customer customer = new Customer();
            customer = _customerrepo.GetCustomer(depositViewModel.Account.CustomerID);
            DateTime today = DateTime.Now;
            customer.Status = "Deposit " + Convert.ToString(today);
            _customerrepo.UpdateCustomer(customer);
            return View(depositViewModel); 
        }
        [HttpPost]
        public IActionResult Deposit(DepositViewModel depositViewModel)
        {
            depositViewModel.Account.Status = ("Deposit on:" + DateTime.Now).ToString();
            depositViewModel.Account.AccountBalance += depositViewModel.amount;
            int newaccountbalance = _accountrepo.deposit(depositViewModel.Account);
            Transaction transaction = new Transaction();
            transaction.AccountID = depositViewModel.Account.AccountID;
            transaction.TransactionDate = Convert.ToDateTime(DateTime.Now);
            transaction.TransactionType = "Deposit";
            transaction.TransactionAmount = depositViewModel.amount;
            _transactionRepo.add(transaction);
            TempData["UserMessage"] = $"New Account Balance for {depositViewModel.Account.AccountID}: ${newaccountbalance}";
            return RedirectToAction("Display");
        }
        public IActionResult Display()
        {
            return View();
        }

        public IActionResult GetStatement()
        {
            
            return View();
        }
        public static int id;
        public static int count;
        //PageTransaction pageTransaction = new PageTransaction();
        public IActionResult Get(TransactionViewModel transactionViewModel)
        {
            //int x = transactionViewModel.Transaction.AccountID;
            transactionViewModel.ViewTransaction = _transactionRepo.ViewTransactions(transactionViewModel.Transaction.AccountID, transactionViewModel.count);
            if (transactionViewModel.ViewTransaction.Count == 0)
            {
                string error = _errorRepo.GetErrorDetails(3).ToString();
                TempData["UserMessage"] = error;
                //TempData["UserMessage"] = "No avaliable statements or Account does not exist";
                return RedirectToAction("GetStatement");
            }
            //pageTransaction.transactions = transactionViewModel.ViewTransaction;
            id = transactionViewModel.Transaction.AccountID;
            count = transactionViewModel.count;
            return RedirectToAction("GetStatements");
        }
        public IActionResult GetStatements(int?page)
        {

            //Transaction transaction = new Transaction();
            PageTransaction pageTransaction = new PageTransaction();
            List<Transaction> objTransactionList = new List<Transaction>();
            pageTransaction.transactions = _transactionRepo.ViewTransactions(id, count);
            objTransactionList = pageTransaction.transactions;
            //transactionViewModel.ViewTransaction = _transactionRepo.ViewTransactions(transactionViewModel.Transaction.AccountID,transactionViewModel.count);
            //if(transactionViewModel.ViewTransaction.Count == 0)
            //{
            //    string error = _errorRepo.GetErrorDetails(3).ToString();
            //    TempData["UserMessage"] = error;
            //    //TempData["UserMessage"] = "No avaliable statements or Account does not exist";
            //    return RedirectToAction("GetStatement");
            //}
            int pageSize = 10;
            int pageIndex = 5;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Transaction> transactions = null;
            pageTransaction.transactions = objTransactionList;
            transactions = objTransactionList.ToPagedList(pageIndex, pageSize);
            //tansactions = transactions1.ToPagedList(pageIndex, pageSize);
            return View(transactions);
        }
        public static DateTime d1;
        public static DateTime d2;
        public IActionResult StatementDisplay()
        {
            return View();
        }

        public IActionResult GetStatementByDates(TransactionViewModel transactionViewModel)
        {
            transactionViewModel.d1 = DateTime.Now;
            transactionViewModel.d2 = DateTime.Now;
            return View(transactionViewModel);
        }

        public IActionResult GetDates(TransactionViewModel transactionViewModel)
        {
            if (transactionViewModel.d1 > transactionViewModel.d2)
            {
                TempData["UserMessage"] = "Start Date should be less than End Date.";
                return RedirectToAction("GetStatementByDates");
            }
            transactionViewModel.ViewTransaction = _transactionRepo.ViewTransactionsDate(transactionViewModel.Transaction.AccountID, transactionViewModel.d1, transactionViewModel.d2);
            if (transactionViewModel.ViewTransaction.Count == 0)
            {
                string error = _errorRepo.GetErrorDetails(3).ToString();
                TempData["UserMessage"] = error;
                //TempData["UserMessage"] = "No avaliable statements or Account does not exist";
                return RedirectToAction("GetStatementByDates");
            }
            //return View(transactionViewModel);
            //pageTransaction.transactions = transactionViewModel.ViewTransaction;
            id = transactionViewModel.Transaction.AccountID;
            d1 = transactionViewModel.d1;
            d2 = transactionViewModel.d2;
            return RedirectToAction("GetStatementsByDate");
        }

        public IActionResult GetStatementsByDate(int?page)
        {
            //Transaction transaction = new Transaction();
            PageTransaction pageTransaction = new PageTransaction();
            List<Transaction> objTransactionList = new List<Transaction>();
            pageTransaction.transactions = _transactionRepo.ViewTransactionsDate(id, d1,d2);
            objTransactionList = pageTransaction.transactions;
            //transactionViewModel.ViewTransaction = _transactionRepo.ViewTransactions(transactionViewModel.Transaction.AccountID,transactionViewModel.count);
            //if(transactionViewModel.ViewTransaction.Count == 0)
            //{
            //    string error = _errorRepo.GetErrorDetails(3).ToString();
            //    TempData["UserMessage"] = error;
            //    //TempData["UserMessage"] = "No avaliable statements or Account does not exist";
            //    return RedirectToAction("GetStatement");
            //}
            int pageSize = 10;
            int pageIndex = 5;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Transaction> transactions = null;
            pageTransaction.transactions = objTransactionList;
            transactions = objTransactionList.ToPagedList(pageIndex, pageSize);
            //tansactions = transactions1.ToPagedList(pageIndex, pageSize);
            return View(transactions);
            //transactionViewModel.ViewTransaction = _transactionRepo.ViewTransactionsDate(transactionViewModel.Transaction.AccountID, transactionViewModel.d1,transactionViewModel.d2);
            //if (transactionViewModel.ViewTransaction.Count == 0)
            //{
            //    string error = _errorRepo.GetErrorDetails(3).ToString();
            //    TempData["UserMessage"] = error;
            //    //TempData["UserMessage"] = "No avaliable statements or Account does not exist";
            //    return RedirectToAction("GetStatementByDates");
            //}
            //return View(transactionViewModel);
        }

        //Withdraw
        public IActionResult Withdraw(Account account)
        {
            WithdrawViewModel w = new WithdrawViewModel();
            w.Account = _accountrepo.GetAccountDetails(account.AccountID);
            return View(w);
        }
        [HttpPost]
        public IActionResult Withdraw(WithdrawViewModel w)
        {
            TempData["UserMessage"] = "";
            w.Account.Status = ("Withdraw on:" + DateTime.Now).ToString();
            int newaccountbalance = _accountrepo.Withdraw(w.Account, w.amount);
            if (newaccountbalance == 0)
            {
                TempData["UserMessage"] = "Withdraw not allowed, please choose smaller amount";
                return View(w);
            }
            else
            {
                w.Account.AccountBalance -= w.amount;

                Transaction transaction = new Transaction();
                transaction.AccountID = w.Account.AccountID;
                transaction.TransactionDate = Convert.ToDateTime(DateTime.Now);
                transaction.TransactionType = "Withdraw";
                transaction.TransactionAmount = w.amount;
                _transactionRepo.add(transaction);
                TempData["UserMessage"] = $"${w.amount} withdrawn successfully";
                return View(w);
            }
           
        }
    }
}
