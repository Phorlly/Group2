using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Cafe_Manage_System
{
    public abstract class Class_Connection
    {
        private string server = "localhost";
        private string user = "root";
        private string pwd = "";
        private string db = "db_for_spi";
        private string cs;

        //create contructor
        public Class_Connection()
        {

            //connection string with mysql authentication
            cs = @"server=" + server + ";user id=" + user + ";persistsecurityinfo=true;database=" + db + ";password=" + pwd + "";

            //connection string with window authentication
            //cs = "server=localhost;user id=root;persistsecurityinfo=True;database=dbspi";
        }
        //create function with cs 
        protected MySqlConnection GetMySqlConnection()
        {
            return new MySqlConnection(cs);
        }
    }
}
