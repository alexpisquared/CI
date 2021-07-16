using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DB.Inventory.Dbo.Models
{
  public partial class Dbprocess
  {
    [NotMapped] public bool? Granted { get; set; }
    [NotMapped] public bool Selectd { get; set; }
    //[NotMapped] public string DB_SP_Name => $"{Database?.Name ?? "[DB Unknown]"}.{Dbschema}.{StoredProcName}";
  }

  public partial class Permission
  {
    [NotMapped] public bool? Granted { get; set; }
    [NotMapped] public bool Selectd { get; set; }
  }

  public partial class User
  {
    [NotMapped] public bool? Granted { get; set; }
    [NotMapped] public bool Selectd { get; set; }
  }

}