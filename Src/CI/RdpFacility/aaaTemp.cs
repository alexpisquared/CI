using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAV.Sys.Ext
{
  public static class aaaTemp
  {
    public static string Log(this Exception ex) { return ex.Message; }
  }
}
