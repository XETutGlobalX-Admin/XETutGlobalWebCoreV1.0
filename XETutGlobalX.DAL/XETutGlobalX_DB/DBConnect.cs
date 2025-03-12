using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace XETutGlobalX.DAL.XETutGlobalX_DB
{
    public class DBConnect
    {
        public string GetConnectionString(string DataSource, string InitialCatelog,bool Encrypt, bool IntegratedSecurity, bool TrustServerCertificate)
        {
            return "Data Source=" + DataSource + ";Initial Catalog=" + InitialCatelog + ";Integrated Security=" + IntegratedSecurity + "Encrypt=" + Encrypt + ";Trust Server Certificate=" + TrustServerCertificate;
        }
        public string GetConnectionString(string ConnectionString)
        {
            return ConnectionString;
        }
        public SqlConnection DBConnectOpen(SqlConnection conn)
        {
            conn.Open();
            return conn;
        }
        public SqlConnection DBConnectClose(SqlConnection conn)
        {
            conn.Close();
            return conn;
        }
    }
}
