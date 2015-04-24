using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
using System.Data.SqlClient;

namespace SMS.FeeSync.Repository
{
    public struct FeeSync
    {
        public string EventBlob;
        public int EventType;
        public DateTime PaidDate;
        public string StudentID;
        public Int64 EventID;
    }

    public class FetchFeesForSync
    {
        Database oDatabase = DatabaseFactory.CreateDatabase();
        DataSet dsHolder = new DataSet();

        public IList<FeeSync> SourceFee()
        {
            try
            {
                IList<FeeSync> oFeeSync;
                string str = "usp_SourceSyncAllFee";
                DbCommand storedProcCommand = oDatabase.GetStoredProcCommand(str); //Should this be just a string such as SELECT * from ut_PaymentReceivedEvent WHERE ReceiptNo IS NULL using  --GetSqlStringCommand?
                
                dsHolder = oDatabase.ExecuteDataSet(storedProcCommand); 

                oFeeSync = dsHolder.Tables[0].AsEnumerable()
                          .Select(row => new FeeSync
                          {
                              //LocalReceiptNo = row["LocalReceiptNo"].ToString(),
                              EventBlob = row["EventData"].ToString(),
                              EventType = int.Parse(row["EventType"].ToString()),
                              PaidDate = (DateTime)row["DateRaised"],
                              StudentID = row["StudentID"].ToString(),
                              EventID = Int64.Parse(row["EventID"].ToString())
                          }).ToList();
                return oFeeSync;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.InnerException.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while storing into DB -{0}", ex.Message));
            }
        }
    }
}
