using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
using System.Data.SqlClient;

namespace SMS.Offline.Repository
{
    public class FeeDetail
    {
        public string Amount { get; set; }
        public string Type { get; set; }
    }



    public class StoreFee
    {
        Database oDatabase = DatabaseFactory.CreateDatabase();
        DataSet dsHolder = new DataSet();

        private StoreFee() { }

        private string _FeeType;
        private decimal _FeeAmount;
        private string _StudentID;
        private string _EventBlob;

        public StoreFee(string FeeType, decimal FeeAmount, string StudentID)
        {
            this._FeeType = FeeType;
            this._FeeAmount = FeeAmount;
            this._StudentID = StudentID;

            FeeDetail _oFeeDetailJSON = new FeeDetail();

            _oFeeDetailJSON.Amount = _FeeAmount.ToString();
            _oFeeDetailJSON.Type = _FeeType;

            string jsonString = JsonConvert.SerializeObject(_oFeeDetailJSON);

            this._EventBlob = "Type|" + FeeType + "|Amount" + "|" + FeeAmount; //jsonString;
        }




        public void SaveFee()
        {
            try
            {
                string str = "usp_SaveFee";
                DbCommand storedProcCommand = oDatabase.GetStoredProcCommand(str);
                long _EventID = DateTime.Now.Ticks;
                oDatabase.AddInParameter(storedProcCommand, "@EventID", DbType.Int64, _EventID);
                oDatabase.AddInParameter(storedProcCommand, "@StudentID", DbType.String, _StudentID);
                oDatabase.AddInParameter(storedProcCommand, "@EventData", DbType.String, _EventBlob);
                oDatabase.AddInParameter(storedProcCommand, "@EventType", DbType.Int32, 1);

                dsHolder = oDatabase.ExecuteDataSet(storedProcCommand); //Could return anything, but what at this point is undecided.
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.InnerException.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error while storing into DB.");
            }
        }

        public void UpdateFeeReceipt(string ReceiptNo, string LocalFeeReceiptNo)
        {
            try
            {
                string str = "usp_SaveFeeReceiptNo";
                DbCommand storedProcCommand = oDatabase.GetStoredProcCommand(str);
                oDatabase.AddInParameter(storedProcCommand, "@FeeReceipt", DbType.String, ReceiptNo);
                oDatabase.AddInParameter(storedProcCommand, "@LocalFeeReceipt", DbType.String, LocalFeeReceiptNo);
                dsHolder = oDatabase.ExecuteDataSet(storedProcCommand); //Could return anything, but what at this point is undecided.
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.InnerException.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error while storing into DB.");
            }
        }
    }
}
