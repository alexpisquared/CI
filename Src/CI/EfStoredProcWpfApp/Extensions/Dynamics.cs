﻿using CI.Standard.Lib.Helpers;
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
      var data = items.ToArray();
      //if (data.Count() == 0) return null;

      var dt = new DataTable();
      foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
      {
        dt.Columns.Add(key);
      }
      
      foreach (var d in data)
      {
        dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
      }

      return dt;
    }
  }
}