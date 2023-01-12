using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CaseStudy.Models;
using Microsoft.AspNetCore.Authorization;
using CaseStudy.ViewModel;
using System.Web;



namespace CaseStudy.Controllers
{
    [Authorize(Roles = "CustomerExecutive")]
    public class CustomerController : Controller
    {
        ICustomerRepo _crepo;
        public CustomerController(ICustomerRepo crepo)
        {
            _crepo = crepo;
        }
        public IActionResult Index()
        {           
            return View(_crepo.ViewCustomers());
        }

        public IActionResult ViewAllCustomers(int page = 0)
        {
            const int PageSize = 3;
            var count = _crepo.ViewCustomers().Count;
            var data = _crepo.ViewCustomers().Skip(page * PageSize).ToList();
            this.ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0);
            this.ViewBag.Page = page;
            return this.View(data);
        }

       

        public IActionResult DeleteCustomer(int id)
        {
            return View(_crepo.GetCustomer(id));
        }

        [HttpPost]
        public IActionResult DeleteCustomer(Customer c)
        {
            if(ModelState.IsValid)
            {
                var result = _crepo.DeleteCustomer(c);
                if(result == 0)
                {
                    ViewBag.ErrorMessage = "Could not delete customer. Customer has an account";
                    return View(c);
                }
                else
                {
                    ViewBag.Message = "Deleted customer successfully";
                    TempData["Message"] = "Deleted customer successfully";
                    return View(c);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Could not delete customer.";
                return View(c);
            }
        }

       
        public IActionResult CreateCustomer()
        {
            return View();
        }



        [HttpPost]
        public IActionResult CreateCustomer(Customer c)
        {
            ViewData["result"] = "Valid";
            if (!ModelState.IsValid)
            {
                ViewData["result"] = "Not Valid";
                return View();
            }
            else
            {
                /*List<Customer> custs = _crepo.ViewCustomers();
                foreach (Customer c1 in custs)
                {
                    if (c1.SSNID.Equals(c.SSNID))
                    {
                        ViewData["result"] = "Not Valid";
                        ViewBag.ErrorMessage = "SSN already existed";
                        return View();
                    }
                }*/
                int value = _crepo.CreateCustomer(c);
                if (value != 1)
                {
                    ViewData["result"] = "Not Valid";
                    ViewBag.ErrorMessage = "SSN already existed";
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }

        
        public IActionResult GetCustomer()
        {
            return View();
        }


        [HttpPost]
        public IActionResult DisplayCustomer(Customer c)
        {
          
            Customer value1 = _crepo.ViewCustomer(c.CustomerID);
            Customer value2 = _crepo.GetCustomerUsingSSNID(c.SSNID);
         
            if (c.SSNID == 0)
            {
                
                if(value1 != null)
                {
                    return View(_crepo.ViewCustomer(c.CustomerID));
                }
                else
                {
                    TempData["Result"] = "Invalid Customer ID";
                    return View("GetCustomer");
                }
            }
            if(c.CustomerID == 0)
            {
                
                if (value2 != null)
                {
                    return View(_crepo.GetCustomerUsingSSNID(c.SSNID));
                }
                else
                {
                    TempData["Result"] = "Invalid SSN ID";
                    return View("GetCustomer");
                }
            }
            TempData["Result"] = "Enter Customer ID or SSN ID only not both";
            return View("GetCustomer");
          
        }
        public IActionResult UpdateCustomer(int id)
        {
            return View(_crepo.GetCustomer(id));
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer c)
        {
            //try
            //{
            if (ModelState.IsValid)
            {
                var result = _crepo.UpdateCustomer(c);
                if (result == 0)
                {
                    ViewBag.ErrorMessage = "Could not update customer. Customer was not found.";
                    return View(c);
                }
                else
                {
                    ViewBag.Message = "Updated customer successfully";
                    return View(c);
                }
            }
            //}
            //catch
            //{
            ViewBag.ErrorMessage = "Could not update customer. Make sure to enter correct values.";
            return View(c);
            //}
        }
       
    }
}
