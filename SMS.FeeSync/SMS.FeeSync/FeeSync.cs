using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using SMS.SyncCommunicator;
using SMS.FeeSync.Def;

namespace SMS.FeeSync
{

    public class FeeSync
    {

        public string EventBlob { private get; set; }
        public int EventType { get; set; }
        public DateTime PaidDate{ get; set; }
        public string URL { private get; set; }
        public Int64 EventID { private get; set; }
        public string FeeReceipt { get; set; }
        public string StudenID { get; set; }

        public Guid FeeID { get; set; }

        public HttpResponseMessage SyncRegisterFee()
        {
            SyncFeeDetails oSyncFees = new SyncFeeDetails()
            {
                FeeBlob = EventBlob,
                StudenID = this.StudenID,
                PaidDate = PaidDate
            };

            Task<HttpResponseMessage> GetHotelImagesResponse = 
                Communicator.InvokeFeeRegister(oSyncFees, "Fees/RegisterFee");
             // These string constants will be stored in a different file as Constants.cs
            
            return GetHotelImagesResponse.Result;
        }


        //Ideally this should be part of another class doing only queries, will have to change it SORD.
        public HttpResponseMessage PullFee()
        {
            Guid FeeID = this.FeeID;

            Task<HttpResponseMessage> GetHotelImagesResponse =
                Communicator.InvokeFeePull(FeeID, "Fees/FetchFee");
            // These string constants will be stored in a different file as Constants.cs

            return GetHotelImagesResponse.Result;
        }
    }

}
