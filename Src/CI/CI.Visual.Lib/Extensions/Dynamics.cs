using CI.Standard.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CI.Standard.Lib.Extensions
{
  public static class Dynamics
  {
    public static DataTable ToDataTable(this IEnumerable<dynamic> items)
    {
      var dataTable = new DataTable();

      var data = items.ToArray();

      foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
        dataTable.Columns.Add(key);

      foreach (var d in data)
        dataTable.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());

      return dataTable;
    }
  }
}