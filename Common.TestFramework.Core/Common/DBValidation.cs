using System;
using System.Data;
using System.Data.SqlClient;



namespace  Common.TestFramework.Core
{
    public class DBValidation
    {  
        public void ExecuteSQLScript(string script, string connectionString)
        {   

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(script, conn);
                cmd.CommandType = CommandType.Text;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        public DataSet GetTestDataDataSet(string script, string connectionString)
        {
            DataSet result = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(script, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.Text;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(result);
                }
            }
            return result;
        }

        public static int ExecuteScalar(string script, string connectionString)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(script, conn);
                cmd.CommandType = CommandType.Text;

                cmd.Connection.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
            }
            return result;
        }
        
    }


}
