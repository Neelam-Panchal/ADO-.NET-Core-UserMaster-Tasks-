using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace UserMaster.Core.DBContext
{
    public class SQLDBHelper
    {
        public string ConnectionString { get; set; }

        public SQLDBHelper(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("myconn");
        }


        public SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }






    }
}
