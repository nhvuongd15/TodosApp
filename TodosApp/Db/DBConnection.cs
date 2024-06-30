using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodosApp.Db
{
    public class DBConnection
    {
        private static SqlConnection? _instance = null;

        private DBConnection()
        {
            if (_instance == null)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";
                builder.UserID = "todoapp";
                builder.Password = "Password@123";
                builder.InitialCatalog = "todos";
                builder.TrustServerCertificate = true;

                _instance = new SqlConnection(builder.ConnectionString);
                _instance.Open();
            }
        }

        public static SqlConnection GetInstance()
        {
            if (_instance == null)
            {
                new DBConnection();
            }

            return _instance!;
        }


        public static void Close()
        {
            if (_instance != null)
            {
                _instance.Close();
            }
        }
    }
}
