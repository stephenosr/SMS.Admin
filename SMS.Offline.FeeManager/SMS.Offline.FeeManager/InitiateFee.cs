using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using SMS.Offline.Repository;
//using SMS.SyncCommunicator;
//using SMS.Offline.Implementation;

namespace SMS.Offline.FeeManager
{
    public struct FeeSyncDetails
    {
        public string LocalReceiptNo;
        public decimal Amount;
        public string FeeType;
        public DateTime PaidDate;
        public string StudenID;
    }

    class InitiateFee
    {

        static void Main(string[] args)
        {
            //This interface will take in fee entries - amount, type
            SaveFee();

        }

        static bool SaveFee()
        {
            bool _acceptPayment = true;

            while (_acceptPayment)
            {
                Console.Write("Enter fee in the Feecode - : ");
                string FeeCode = Console.ReadLine();

                Console.Write("Enter fee Amount  - : ");
                decimal Feeamount = (decimal.Parse(Console.ReadLine()));

                StoreFee oFee = new StoreFee(FeeCode, Feeamount, "qwew1101010");
                oFee.SaveFee();
                Console.WriteLine("Save Completed  - would you like to enter another fee? - Y/N :");
                _acceptPayment = (Console.ReadLine().ToString().ToUpper() == "Y") ? true : false;
            }
            return true;
        }

        
    }
}
