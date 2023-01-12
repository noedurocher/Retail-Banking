using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public interface IErrorRepo
    {
        public string GetErrorDetails(int id);
    }
}
