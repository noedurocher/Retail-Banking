using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CaseStudy.Models;
using Microsoft.AspNetCore.Authorization;
using CaseStudy.ViewModel;

namespace CaseStudy.Controllers
{
    [Authorize(Roles ="Cashier,Teller")]
    public class TransactionController : Controller
    {
        static int id;
        ITransactionRepo _repo;
        public TransactionController(ITransactionRepo repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TransferScreen(TransferViewModel transfer)
        {
            TransferViewModel nullTransfer = new TransferViewModel();
            if (transfer.Amount < 0)
            {
                ViewBag.ErrorMessage = "Amount can't be negative";
                nullTransfer.SourceAccountID = transfer.SourceAccountID;
                return View(nullTransfer);
            }
            if (transfer.SourceAccountID == transfer.TargetAccountID)
            {
                ViewBag.ErrorMessage = "Target Account cannot be the same as Source Account.";
                nullTransfer.SourceAccountID = transfer.SourceAccountID;
                return View(nullTransfer);
            }
            TransferViewModel transfer1 = _repo.TransferScreen(transfer.Amount, transfer.SourceAccountID, transfer.TargetAccountID);
            if (transfer1.SourceAccountID == 0)
            {
                ViewBag.ErrorMessage = "Source account has no funds.";
                nullTransfer.SourceAccountID = transfer.SourceAccountID;
                return View(nullTransfer);
            }
            else if (transfer1.SourceAccountID == -1)
            {

                ViewBag.ErrorMessage = "Source account has insufficient funds.";
                nullTransfer.SourceAccountID = transfer.SourceAccountID;
                return View(nullTransfer);
            }
            else if (transfer1.SourceAccountID == -2)
            {

                ViewBag.ErrorMessage = "Target account could not be found.";
                nullTransfer.SourceAccountID = transfer.SourceAccountID;
                return View(nullTransfer);
            }
            else if (transfer1.SourceAccountID == -3)
            {

                ViewBag.ErrorMessage = "Source account could not be found.";
                nullTransfer.SourceAccountID = transfer.SourceAccountID;
                return View(nullTransfer);
            }
            else if (transfer1.SourceAccountID == -4)
            {

                ViewBag.ErrorMessage = "Source account and target account can not be the same.";
                nullTransfer.SourceAccountID = transfer.SourceAccountID;
                return View(nullTransfer);
            }
            else
            {
                ViewBag.Message = "Transfer successful";
                return View(transfer1);
            }

        }

        public IActionResult TransferAccount(int SourceAccountID)
        {
            TransferViewModel transfer = new TransferViewModel();
            transfer.SourceAccountID = SourceAccountID;           
            return View(transfer);
        }
    }
}
