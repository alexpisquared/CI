using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CI.Standard.Lib.Extensions
{
  public static class Dynamics
  {
    public static DataTable ToDataTable(this IEnumerable<dynamic> dynamicRows)
    {
      var dataTable = new DataTable();

      try
      {
        var drArray = dynamicRows.ToArray();

        foreach (var key in ((IDictionary<string, object>)drArray[0]).Keys)
          dataTable.Columns.Add(key);

        foreach (var d in drArray)
          dataTable.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
      }
      catch (Exception ex)
      {
        ex.Log("Ignore and return an empty DataTable. ");
      }

      return dataTable;
    }
  }
}