using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.Repository.Repositories.DBConnection
{
    public interface IConnection
    {
        public Task<object> GetExecuteScalarSP(String SPName, SqlParameter[] para);
        public Task<object> GetExecuteScalarQry(String Qry, SqlParameter[] para);
        public Task<DataTable> GetDataTable(String SPName, SqlParameter[] para);
        public Task<DataSet> GetDataSet(String SPName, SqlParameter[] para);
        public Task<int> ExecuteNonQuery(string spName, SqlParameter[] para);
    }
}
