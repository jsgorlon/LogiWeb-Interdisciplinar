using System.Data.SqlClient;

namespace logiWeb.Repositories
{
    public abstract class DBContext
    {
        /*private readonly string strConn = @"Data Source=DESKTOP-973N7TE\SQLEXPRESS;
         Initial Catalog=Logitech;
         Integrated Security=true"; */
        private readonly string strConn = @"Server=localhost;
                                            Database=master;
                                            Initial Catalog=LogiWeb;
                                          Trusted_Connection=True;
                                          MultipleActiveResultSets=True";
        protected SqlConnection connection;

        public DBContext()
        {
            connection = new SqlConnection(strConn);
            connection.Open();
        }

        public void Dispose()
        {
          if(connection != null)
            connection.Close();
            SqlConnection.ClearPool(connection);
        }

        /* ~DBContext(){
            Dispose(); 
        } */
    }
}