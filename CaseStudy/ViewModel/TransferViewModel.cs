using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy.ViewModel
{
    public class TransferViewModel
    {
        public int SourceAccountID { get; set; }
        public int TargetAccountID { get; set; }
        public int SourceCurrentBalance { get; set; }
        public int TargetCurrentBalance { get; set; }
        public int SourceUpdatedBalance { get; set; }
        public int TargetUpdatedBalance { get; set; }
        public int Amount { get; set; }
    }
}
