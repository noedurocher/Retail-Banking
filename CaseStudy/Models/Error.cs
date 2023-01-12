using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.Models
{
    public class Error
    {
        [Key]
        public int ErrorID { get; set; }
        public string ErrorDescription { get; set; }
    }
}
