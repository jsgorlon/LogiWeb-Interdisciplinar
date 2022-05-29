using System.Data.SqlClient;

namespace logiWeb.Repositories
{
    public abstract class DBContext
    {
         private readonly string strConn = @"Server=localhost;
                                            Database=master;
                                            Initial Catalog=LogiTech;
                                          Trusted_Connection=True;";
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