using System.Data.SqlClient;
using System.Data;

namespace Azure_BackUpper
{
    public class Connection
    {
        public SqlConnection Connect()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "(localdb)\\MSSQLLocalDB";
                builder.InitialCatalog = "TrainWay";

                SqlConnection connection = new SqlConnection(builder.ConnectionString);

                connection.Open();

                return connection;
            }
            catch
            {
                throw;
            }
        }

        public void Close_connection(SqlConnection con)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            catch
            {
                throw;
            }
        }
    }
}
