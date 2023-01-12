using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudy.Models;
using CaseStudy.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace CaseStudy.Controllers
{
    [Authorize(Roles = "AccountExecutive")]
    public class AccountController : Controller
    {
        IAccountRepo _repo;
        ICustomerRepo _crepo;
        IErrorRepo _erepo;
        ITransactionRepo _trepo;
        public AccountController(IAccountRepo repo,ICustomerRepo crepo,IErrorRepo erepo,ITransactionRepo trepo)
        {
            _repo = repo;
            _crepo = crepo;
            _erepo = erepo;
            _trepo = trepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewAllAccounts(int page = 0)
        {
            const int PageSize = 3;
            var count = _repo.GetAllAccounts().Count;
            var data = _repo.GetAllAccounts().Skip(page * PageSize).ToList();
            this.ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);
            this.ViewBag.Page = page;
            return this.View(data);
        }
        public IActionResult Add()
        {
            CustomerAccountViewModel customerAccountViewModel = new CustomerAccountViewModel();
            customerAccountViewModel.ViewID = _repo.GetData();
            return View(customerAccountViewModel);
        }
        [HttpPost]
        public IActionResult Create(CustomerAccountViewModel customerAccountViewModel)
        {
            if (ModelState.IsValid)
            {
                customerAccountViewModel.Account.Status = "Account created on: " + DateTime.Now.ToString();
                int value = _repo.AddAccount(customerAccountViewModel.Account);
                if (value == 200)
                {
                    string error = _erepo.GetErrorDetails(200).ToString();
                    TempData["UserMessage"] = error;
                }
                    //TempData["UserMessage"] = "Cannot create more than 2 accounts, Delete an account to proceed.";
                else if (value == 300)
                {
                    string error = _erepo.GetErrorDetails(300).ToString();
                    TempData["UserMessage"] = error;
                }
                    //TempData["UserMessage"] = "Account Type already exist.";
                else
                {
                    Customer customer = new Customer();
                    customer = _crepo.GetCustomer(customerAccountViewModel.Account.CustomerID);
                    DateTime today = DateTime.Now;
                    customer.Status = "Create Account " + Convert.ToString(today);
                    _crepo.UpdateCustomer(customer);
                    TempData["UserMessage"] = "Account creation initiated successfully.";
                    return RedirectToAction("Index");
                }
                //return RedirectToAction("Index");
                customerAccountViewModel.ViewID = _repo.GetData();
                return View("Add", customerAccountViewModel);
            }
            return View("Add");
        }
        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult DeleteAccountDisplay()
        {
            return View();
        }

        public IActionResult DeleteFrom(int AccountID)
        {
            return View(_repo.GetAccountDetails(AccountID));
        }

        [HttpPost]
        public IActionResult DeleteFroms(Account account)
        {
            int value = _repo.delete(account);
            if (value == 500)
            {
                return RedirectToAction("ConfirmDeleteWithBalance", account);
                //TempData["UserMessage"] = "Account deletion inititated successfully with balance.";
            }
            else
            {
                TempData["UserMessage"] = "Account deletion inititated successfully.";
            }
            Customer customer = new Customer();
            customer = _crepo.GetCustomer(account.CustomerID);
            _trepo.delete(account.AccountID);
            DateTime today = DateTime.Now;
            customer.Status = "Delete Account " + Convert.ToString(today);
            _crepo.UpdateCustomer(customer);

            return RedirectToAction("DeleteMessage");
        }

        public IActionResult DeleteAccounts(CustomerAccountViewModel customerAccountViewModel)
        {
            //if (customerAccountViewModel.Customer.SSNID == 0)
            //{
                List<Account> accounts = _repo.GetAccountUsingCustomerID(customerAccountViewModel.Account.CustomerID);
                if (accounts.Count == 0)
                {
                    List<Account> accounts1 = _repo.GetAccountUsingSSNID(customerAccountViewModel.Customer.SSNID);
                    if (accounts1.Count == 0)
                    {
                        string error1 = _erepo.GetErrorDetails(2).ToString();
                        TempData["UserMessage"] = error1;
                        //TempData["UserMessage"] = "No accounts found.";
                        return View("Delete");
                    }
                    return View(accounts1);
                    //string error = _erepo.GetErrorDetails(2).ToString();
                    //TempData["UserMessage"] = error;
                    ////TempData["UserMessage"] = "No accounts found.";
                    //return View("Delete");
                }
                return View(accounts);
            //}
            //else
            //{
            //    List<Account> accounts = _repo.GetAccountUsingSSNID(customerAccountViewModel.Customer.SSNID);
            //    if (accounts.Count == 0)
            //    {
            //        string error = _erepo.GetErrorDetails(2).ToString();
            //        TempData["UserMessage"] = error;
            //        //TempData["UserMessage"] = "No accounts found.";
            //        return View("Delete");
            //    }
            //    return View(accounts);
            //}
        }

        public IActionResult ConfirmDelete(int AccountID)
        {
            return View(_repo.GetAccountDetails(AccountID));
        }

        public IActionResult ConfirmDeleteWithBalance(Account account)
        {
            return View(account);
        }
        [HttpPost]
        public IActionResult ConfirmDeleteWithBalances(Account account)
        {
            _repo.confirmdelete(account);
            TempData["UserMessage"] = "Account deletion inititated successfully with balance.";
            Customer customer = new Customer();
            customer = _crepo.GetCustomer(account.CustomerID);
            _trepo.delete(account.AccountID);
            DateTime today = DateTime.Now;
            customer.Status = "Delete Account " + Convert.ToString(today);
            _crepo.UpdateCustomer(customer);
            return RedirectToAction("DeleteMessage");
        }

        [HttpPost]
        public IActionResult ConfirmDelete(Account account)
        {
            int value = _repo.delete(account);
            if(value == 500)
            {
                return RedirectToAction("ConfirmDeleteWithBalance", account);
                //TempData["UserMessage"] = "Account deletion inititated successfully with balance.";
            }
            else
            {
                TempData["UserMessage"] = "Account deletion inititated successfully.";
            }
            Customer customer = new Customer();
            customer = _crepo.GetCustomer(account.CustomerID);
            _trepo.delete(account.AccountID);
            DateTime today = DateTime.Now;
            customer.Status = "Delete Account " + Convert.ToString(today);
            _crepo.UpdateCustomer(customer);

            return RedirectToAction("DeleteMessage");
        }

        public IActionResult DeleteMessage()
        {
            return View();
        }

        public IActionResult DeleteBySSNID()
        {
            return View();
        }

        public IActionResult ViewAccountAndCustomer()
        {
            return View();
        }

        public IActionResult DisplayAccountAndCustomerDetails(Account a)
        {
            ViewData["result"] = "Not Valid";
            CustomerAccountViewModel cust = _repo.ViewAccountAndCustomerDetails(a.AccountID);
            if(cust!= null)
            {
                ViewData["result"] = "Valid";
                return View(cust);
            }
            else
            {
                return View("ViewAccountAndCustomer");
            }
         
        }

        public IActionResult AccountInfo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxMethod()
        {
            List<Account> accounts = _repo.GetAllAccounts();
            return Json(accounts);
        }
    }
}
