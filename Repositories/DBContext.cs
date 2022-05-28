using System.Data.SqlClient;

namespace logiWeb.Repositories
{
    public abstract class DBContext
    {
        private readonly string strConn = @"Data Source=DESKTOP-973N7TE\SQLEXPRESS;
         Initial Catalog=Logitech;
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