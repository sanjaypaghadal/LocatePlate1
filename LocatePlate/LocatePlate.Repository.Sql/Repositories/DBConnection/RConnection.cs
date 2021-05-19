using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace LocatePlate.Repository.Repositories.DBConnection
{
    public class RConnection : IConnection
    {
        private readonly string connectionStr;
        public RConnection(IConfiguration configuration)
        {
            connectionStr = configuration.GetConnectionString("LocatePlateSqlContext").ToString();
        }
        public async Task<object> GetExecuteScalarSP(String SPName, SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.CommandText = SPName;
                foreach (SqlParameter par in para)
                {
                    cmd.Parameters.Add(par);
                }
                cmd.Connection = con;
                con.Open();
                return await Task.Run(() => cmd.ExecuteScalar());
            }
        }
        public async Task<object> GetExecuteScalarQry(String Qry, SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Qry;
                foreach (SqlParameter par in para)
                {
                    cmd.Parameters.Add(par);
                }
                cmd.Connection = con;
                con.Open();
                return await Task.Run(() => cmd.ExecuteScalar());
            }
        }
        public async Task<DataTable> GetDataTable(String SPName, SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;
                cmd.CommandTimeout = 1000;
                foreach (SqlParameter par in para)
                {
                    cmd.Parameters.Add(par);
                }
                cmd.Connection = con;
                con.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                await Task.Run(() => da.Fill(dt));
                return dt;
            }
        }
        public async Task<DataSet> GetDataSet(String SPName, SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;
                foreach (SqlParameter par in para)
                {
                    cmd.Parameters.Add(par);
                }
                cmd.Connection = con;
                cmd.CommandTimeout = 1000;
                con.Open();

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                await Task.Run(() => da.Fill(ds));
                return ds;
            }
        }
        public async Task<int> ExecuteNonQuery(string spName, SqlParameter[] para)
        {
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 1000;
                foreach (SqlParameter par in para)
                {
                    cmd.Parameters.Add(par);
                }
                cmd.Connection = con;
                con.Open();
                return await Task.Run(() => cmd.ExecuteNonQuery());
            }
        }




    }
}
