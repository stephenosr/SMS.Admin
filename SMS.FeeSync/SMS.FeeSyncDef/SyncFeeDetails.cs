using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.FeeSync.Def
{
    public class SyncFeeDetails
    {
        public string FeeBlob { get; set; }
        public DateTime PaidDate { get; set; }
        public string StudenID { get; set; }
    }
}
