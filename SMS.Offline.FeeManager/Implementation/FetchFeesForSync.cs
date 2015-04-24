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

namespace SMS.Offline.Repository
{
    public class FetchFeesForSync
    {
        Database oDatabase = DatabaseFactory.CreateDatabase();
        DataSet dsHolder = new DataSet();

        public struct FeeSync
        {
            public string LocalReceiptNo;
            public decimal Amount;
            public string FeeType;
            public DateTime PaidDate;
            internal string StudenID;
        }

        public IList<FeeSync> SourceFee()
        {
            try
            {
                IList<FeeSync> oFeeSync;
                string str = "usp_SourceSyncAllFee";
                DbCommand storedProcCommand = oDatabase.GetStoredProcCommand(str); //Should this be just a string such as SELECT * from ut_PaymentReceivedEvent WHERE ReceiptNo IS NULL using  --GetSqlStringCommand?
                dsHolder = oDatabase.ExecuteDataSet(storedProcCommand); //Could return anything, but what at this point is undecided.
                oFeeSync = dsHolder.Tables[0].AsEnumerable()
                          .Select(row => new FeeSync
                          {
                              LocalReceiptNo = row["LocalReceiptNo"].ToString(),
                              Amount = (decimal.Parse(row["EventData"].ToString())),
                              FeeType = row["EventType"].ToString(),
                              PaidDate = (DateTime)row["DateRaised"],
                              StudenID = row["StudentID"].ToString()
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
