using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace CI.DS.Visual.Views
{
  public class SqlSpRuner
  {
    public void AddSqlParam(List<SqlParameter> sqlParamList, string[] param, string svalue) => sqlParamList.Add(new SqlParameter
    {
      ParameterName = param.First(),
      Value = svalue,
      DbType = param[1] == "int" ? DbType.Int32 : DbType.String,
      SqlDbType = param[1] == "int" ? SqlDbType.Int : SqlDbType.NVarChar,
      Direction = param.Last() == "0" ? ParameterDirection.Input : ParameterDirection.Output // .InputOutput is problematic
    });
    public IEnumerable<dynamic> ExecuteReader(DbContext _db, string commandText, List<SqlParameter>? sqlParameters = null) //todo: async System.Collections.Generic.IAsyncEnumerable<dynamic> readTableDynamicly(string _sql)
    {
      var connection = _db.Database.GetDbConnection();
      var rv = new List<dynamic>();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
          connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = commandText;
        cmd.CommandType = sqlParameters is null ? CommandType.Text : CommandType.StoredProcedure;

        sqlParameters?.ForEach(p => cmd.Parameters.Add(p));

        using var reader = cmd.ExecuteReader();
        if (reader.HasRows)
          while (reader.Read())
          {
            rv.Add(getDynamicData(reader));
          }

        reader.Close();
      }
      finally
      {
        connection.Close();
      }

      return rv;
    }
    
    dynamic getDynamicData(System.Data.Common.DbDataReader reader)
    {
      var expandoObject = new ExpandoObject() as IDictionary<string, object>;
      for (var i = 0; i < reader.FieldCount; i++)
      {
        expandoObject.Add(reader.GetName(i), reader[i]);
      }
      return expandoObject;
    }
  }
}