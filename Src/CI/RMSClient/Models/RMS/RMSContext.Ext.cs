using Microsoft.EntityFrameworkCore;
using System.Linq;

#nullable disable

namespace RMSClient.Models.RMS
{
  public partial class RMSContext : DbContext
  {
    //public string Server => DbContext_Ext.Server(this);
  }

  public static class DbContext_Ext // replacing DbSaveLib and all others!!! (Aug 2018)
  {
    public static string Server(this DbContext db, string sd = "Server=") => db.Database.GetConnectionString().Split(';').First(r => r.StartsWith(sd)).Replace(sd, "");
    public static string Database(this DbContext db, string sd = "Database=") => db.Database.GetConnectionString().Split(';').First(r => r.StartsWith(sd)).Replace(sd, "");
  }
}
