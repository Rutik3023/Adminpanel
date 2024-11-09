using System.Data;
using System.Data.SqlClient;

namespace  AdminPanel.Models
{
    public class DBHelper
    {
        public SqlConnection con = new SqlConnection();
        public SqlCommand cmd = new SqlCommand();



        public void OpenConnection(string ConnectionString)
        {
            string _connectionString = ConnectionString;

            //string ConDecrypt = MSEncrypto.Encryption.Decrypt(_connectionString);

            con.ConnectionString = _connectionString;
            con.Open();

            cmd.Connection = con;
        }

        public void CloseConnection()
        {
            con.Close();
        }


        public int FillData(string sql)
        {
            try
            {
                cmd.CommandText = sql;
                int no = cmd.ExecuteNonQuery();
                return no;
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public DataSet DaSet(string sql)
        {
            cmd.CommandText = sql;
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds;
        }

        public SqlDataReader DataReader(string sql)
        {
            cmd.CommandText = sql;

            return cmd.ExecuteReader();

        }
    }
}
