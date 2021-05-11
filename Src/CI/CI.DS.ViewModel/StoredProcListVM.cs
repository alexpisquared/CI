using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace CI.DS.ViewModel
{
  public class StoredProcListVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly InventoryContext _context;
    string _selectedDBP, _pKey;
    ObservableCollection<StoredProcDetail> _dbProcesses = new();

    public StoredProcListVM(ILogger logger, IConfigurationRoot config)
    {
      _logger = logger;
      _config = config;
      _context = new(_config["SqlConStr"]); // string.Format(_config["SqlConStr"], cbxSrvr.SelectedValue)); // @"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;"); // MTdevSQLDB


      using var connectionDb = _context.Database.GetDbConnection();
      if (connectionDb.State.Equals(ConnectionState.Closed))
        connectionDb.Open();

      using (var command = connectionDb.CreateCommand())
      {
        command.CommandText = _sql;

        using var reader = command.ExecuteReader();
        if (!reader.HasRows)
          Debug.WriteLine("No rows found.");
        else
          while (reader.Read())
          {
            _dbProcesses.Add(new StoredProcDetail(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
          }

        reader.Close();
      }

      connectionDb.Close();

    }

    static void HasRows(SqlConnection connection)
    {
      using (connection)
      {
        SqlCommand command = new("SELECT CategoryID, CategoryName FROM Categories;", connection);
        connection.Open();

        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            Debug.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
          }
        }
        else
        {
          Debug.WriteLine("No rows found.");
        }
        reader.Close();
      }
    }

    public ObservableCollection<StoredProcDetail> StoredProcDetails { get => _dbProcesses; set => SetProperty(ref _dbProcesses, value, true); }

    public string PKey { get => _pKey; set => SetProperty(ref _pKey, value); }

    public IConfigurationRoot Config => _config;
    public ILogger Logger => _logger;

    const string _sql = @"
select schema_name(obj.schema_id) AS [Schema],       obj.name AS SPName,   substring(par.parameters, 0, len(par.parameters)) as Parameters,        mod.Definition
from sys.objects obj
join sys.sql_modules mod
     on mod.object_id = obj.object_id
cross apply (select p.name + ' ' + TYPE_NAME(p.user_type_id) + ', ' 
             from sys.parameters p
             where p.object_id = obj.object_id 
                   and p.parameter_id != 0 
             for xml path ('') ) par (parameters)
where obj.type in ('P', 'X')
order by 2";
  }

  public class StoredProcDetail
  {
    public StoredProcDetail(string pKey, string name, string desc) => (SPName, Parameters, Definition) = (pKey, name, desc);

    public string SPName { get; set; }
    public string Parameters { get; set; }
    public string Definition { get; set; }

    public override string ToString() { return Parameters; }
  }
}