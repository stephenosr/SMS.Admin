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


    public class CleanFeeDetails
    {
        Database oDatabase = DatabaseFactory.CreateDatabase();
        DataSet dsHolder = new DataSet();

        public bool ClearPendingSync(Int64 EventID)
        {
            try
            {
                
                string str = "usp_DeleteFeePaymentEvent";
                DbCommand storedProcCommand = oDatabase.GetStoredProcCommand(str); //Should this be just a string such as SELECT * from ut_PaymentReceivedEvent WHERE ReceiptNo IS NULL using  --GetSqlStringCommand?
                oDatabase.AddInParameter(storedProcCommand, "@EventID", DbType.Int64, EventID);
                dsHolder = oDatabase.ExecuteDataSet(storedProcCommand);

                return true;
            }
            catch (SqlException sqlEx)
            {
                return false;
                //Log error
            }
            catch (Exception ex)
            {
                return false;
                //Log error
            }
        }
    }
}
