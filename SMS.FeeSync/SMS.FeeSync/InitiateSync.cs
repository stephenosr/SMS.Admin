using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


using SMS.FeeSync.Repository;

namespace SMS.FeeSync
{
    class InitiateSync
    {
        public struct FeeSync
        {
            public string EventBlob;
            public int EventType;
            public DateTime PaidDate;
            internal string StudentID;

        }

        static void Main(string[] args)
        {
            SyncFee();

            Console.WriteLine("Would you like to pull the data from the server? - Y/N");
            bool _response = (Console.ReadLine().ToString().ToUpper()=="Y") ? true : false;

            if(_response)
            {
                PullFeeFromServer();
            }

            Console.Write("App will terminate......");
            Console.ReadKey();
        }

        private static void PullFeeFromServer()
        {
            FetchFeesForSync oSyncDetails = new FetchFeesForSync();
            SMS.FeeSync.FeeSync oSync = new SMS.FeeSync.FeeSync();
            oSync.FeeID = Guid.Parse(""); 
            var Mssg =  oSync.PullFee().Content.ReadAsStringAsync().Result.ToString();
            //[TODO]: Define the payment type - structure same as the one in Web API - SORD
            //FeePayment Payment = JsonConvert.DeserializeObject<FeePayment>(Mssg);
        }

        
        static bool SyncFee()
        {
            Console.WriteLine("System about to sync, would you like to proceed? - Y/N");

            if (Console.ReadLine().ToUpper() == "N")
            {
                Console.WriteLine("Exiting App");
                return false;
            }

            FetchFeesForSync oSyncDetails = new FetchFeesForSync();
            SMS.FeeSync.FeeSync oSync = new SMS.FeeSync.FeeSync();

            var oListFeesToSync = oSyncDetails.SourceFee();
            var oListFeesToSyncTemp = oListFeesToSync.Where(k=>k.EventType==1)
                .Select(l =>
                    new
                    {
                        EventBlob = l.EventBlob,
                        EventDate = l.PaidDate,
                        StudentID = l.StudentID,
                        EventType = l.EventType,
                        EventID = l.EventID
                    }
                );

            foreach (var oFeeDetail in oListFeesToSyncTemp)
            {

                oSync.EventBlob = oFeeDetail.EventBlob;
                oSync.PaidDate = oFeeDetail.EventDate;

                oSync.StudenID = oFeeDetail.StudentID;
                oSync.EventType = oFeeDetail.EventType;
                oSync.EventID = oFeeDetail.EventID;

                HttpResponseMessage oSyncReturn = oSync.SyncRegisterFee();
                if (oSyncReturn.IsSuccessStatusCode)
                {
                    Console.WriteLine(string.Format("Student - {0} fee paid {1} ", oFeeDetail.StudentID, oFeeDetail.EventBlob));

                    CleanFeeDetails oFeeCleanup = new CleanFeeDetails();
                    if(oFeeCleanup.ClearPendingSync(oFeeDetail.EventID))
                    {
                        //Log error here if delete is not done..
                        Console.WriteLine("Unable to delete {0}", oFeeDetail.EventID);

                    }

                }
                else
                {
                    //This is not needed as only the log on the server side will indicate the successfull registry of the event.
                    Console.WriteLine(string.Format("Failure sync for receipt {0} - Event {1}", oSync.FeeReceipt, oFeeDetail.EventID));
                    //
                    //Log error here.
                }
            }
            return true;
        }
    }
}
