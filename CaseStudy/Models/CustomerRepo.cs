using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.Models
{
    public class CustomerRepo:ICustomerRepo
    {
        public ManagementContext _context;
        public CustomerRepo(ManagementContext context)
        {
            _context = context;
        }

        public int CreateCustomer(Customer c)
        {
            try
            {
               
                    DateTime date = DateTime.Now;
                    c.Status = $"New Customer Added on {date}";
                    _context.Customer.Add(c);
                    _context.SaveChanges();
                    return 1;
            }
            catch
            {
                return 0;
            }
        }

        public Customer ViewCustomer(int id)
        {
            try
            {
                return _context.Customer.Where(a => a.CustomerID == id).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

      

        public int DeleteCustomer(Customer c)
        {
              Account  account = _context.Account.Where(a => a.CustomerID == c.CustomerID).FirstOrDefault();
            if (account==null)
            {
                _context.Customer.Remove(c);
                _context.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customer.Where(a => a.CustomerID == id).FirstOrDefault();
        }

        public List<Customer> ViewCustomers()
        {
            return _context.Customer.ToList();
        }

        public int UpdateCustomer(Customer c)
        {
            try
            {
                DateTime date = DateTime.Now;
                c.Status = $"Updated on {date}";
                _context.Entry(c).State = EntityState.Modified;
                _context.SaveChanges();
                return 1;
            }
            catch 
            {

                return 0;
            }
        }

        public Customer GetCustomerUsingSSNID(int id)
        {
            try
            {
                return _context.Customer.Where(a => a.SSNID == id).FirstOrDefault();
            }
            catch
            {
                return  null;
            }

             
        }
    }
}
