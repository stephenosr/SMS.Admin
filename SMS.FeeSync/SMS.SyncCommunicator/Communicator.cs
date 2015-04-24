using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web;
using System.Configuration;
using SMS.FeeSync.Def;

namespace SMS.SyncCommunicator
{
    public class Communicator
    {
        private static string sURL = ConfigurationManager.AppSettings["BaseURL"].ToString();

        public async static Task<HttpResponseMessage> InvokeRoundPrism(SyncFeeDetails Parameters, string ServiceName)
        {
            HttpResponseMessage ResponseResult = null;

            try
            {
                string sParams = string.Empty;


                //This will have to be of specific types
                //SyncFeeDetails oSyncFeeDetails = new SyncFeeDetails();
                //oSyncFeeDetails.FeeBlob = Parameters.;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(sURL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Response = await client.PostAsJsonAsync((sURL + ServiceName), Parameters).ContinueWith(l => l.Result.Content.ReadAsAsync<HttpResponseMessage>().Result);
                return Response;
            }
            catch (Exception ex)
            {
                ResponseResult = new HttpResponseMessage();
                ResponseResult.Content = new StringContent(ex.Message);
            }
            return ResponseResult;
        }
    }
}
