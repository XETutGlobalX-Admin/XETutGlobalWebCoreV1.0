using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XETutGlobalX.DAL.App_Code;

namespace XETutGlobalX.DAL.XETutGlobalX_DB
{
    public class clsDesignation
    {
        public DataSet GetDesignation(string parameter, DBConnectMode.DBConnect_Mode mode)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = new();
            string dbconnectionString = string.Empty;
            XETutGlobalX_DB.DBConnect dBConnect = new();
            switch (mode)
            {
                case DBConnectMode.DBConnect_Mode.Simple:
                    dbconnectionString = dBConnect.GetConnectionString(parameter);
                    break;
                case DBConnectMode.DBConnect_Mode.Complex:
                    string[] connectionSet = parameter.Split(';');
                    string DataSource = string.Empty; string InitialCatelog = string.Empty;
                    bool Encrypt = true;
                    bool IntegratedSecurity = true; 
                    bool TrustServerCertificate = true;
                    //DBConnect = DataSource + ";" + InitialCatalog + ";" + IntegratedSecurity + ";" + Encrypt +";" +  TrustServerCertificate + ";";
                    DataSource = connectionSet[0];
                    InitialCatelog = connectionSet[1];
                    IntegratedSecurity = Convert.ToBoolean(connectionSet[2]);
                    Encrypt = Convert.ToBoolean(connectionSet[3]);
                    TrustServerCertificate = Convert.ToBoolean(connectionSet[4]);


                    dbconnectionString = dBConnect.GetConnectionString(DataSource,InitialCatelog, Encrypt, IntegratedSecurity, TrustServerCertificate);
                    break;

            }
            if(dbconnectionString!=null && dbconnectionString != string.Empty)
            {
                conn.ConnectionString = dbconnectionString;
                conn = dBConnect.DBConnectOpen(conn);
                SqlCommand cmd = new("SP_GetDesignationDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }
    }
}
