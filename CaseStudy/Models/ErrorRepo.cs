using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class ErrorRepo:IErrorRepo
    {
        public ManagementContext _context;
        public ErrorRepo(ManagementContext context)
        {
            _context = context;
        }

        public string GetErrorDetails(int id)
        {
            Error error = new Error();
            error = _context.Error.Where(a => a.ErrorID == id).FirstOrDefault();
            return error.ErrorDescription;
        }
    }
}
