using System.Data.SqlClient;

namespace logiWeb.Repositories
{
    public abstract class DBContext
    {
        private readonly string strConn = @"Data Source=DESKTOP-BP1KIU0\SQLSERVE01;
         Initial Catalog=BDEcommerce;
         Integrated Security=true";

        protected SqlConnection connection;

        public DBContext()
        {
            connection = new SqlConnection(strConn);
            connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}