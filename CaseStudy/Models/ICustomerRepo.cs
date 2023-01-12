using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public interface ICustomerRepo
    {
        int DeleteCustomer(Customer c);
        Customer GetCustomer(int id);
        int CreateCustomer(Customer c);
        int UpdateCustomer(Customer c);
        Customer ViewCustomer(int id);
        List<Customer> ViewCustomers();

        Customer GetCustomerUsingSSNID(int id);
    }
}
