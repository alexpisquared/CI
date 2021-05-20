using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EfStoredProcWpfApp.Views
{
  public partial class SpDemoExecute : UserControl
  {
    readonly InventoryContext _context = new(@"Server=mtUATsqldb;Database=Inventory;Trusted_Connection=True;");
    
    public SpDemoExecute()    {      InitializeComponent();    }

    public void ExecuteSqlCommand() // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
      var rv = _context.Database.ExecuteSqlRaw("usp_AddAccountGroupSymbol @p0, @p1", new[] {
        new SqlParameter("@p0", 2),
        new SqlParameter("@p1", "AGF")        }); // ~ var rowsAffected = context.Database.ExecuteSqlCommand("Update Students set FirstName = 'Bill' where StudentId = 1;");
      Console.WriteLine($"** {rv}  -1 0 1 ");
    }

    public void FromSql()           // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
      // USE [Inventory] GO
      //DECLARE	@return_value int      EXEC	@return_value = [dbo].[usp_Report_AllCashByGroup]		@pGroup_ID = 2,		@pDateType = N'2',		@pStartDateInt = 2,		@pEndDateInt = 2,		@pGroupName = N'2'          SELECT	'Return Value' = @return_value
      //CREATE PROCEDURE [dbo].[usp_Report_AllCashByGroup]  @pGroup_ID int = -1,  @pDateType char(1) = 'T', /*T = TradeDate S = SettlmntDate* /  @pStartDateInt int = 0, @pEndDateInt int = 0, @pGroupName varchar(50)        AS      select * from BookReport_view where  Book_ID in (select Book_ID from GroupMember where Group_ID = @pGroup_ID)
      var param = new SqlParameter("@FirstName", "Bill"); // new SqlParameter() { ParameterName = "@FirstName", SqlDbType =  System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Size = 50, Value = "Bill" };
      var parms = new[] {
        new SqlParameter("@pGroup_ID",     2),
        new SqlParameter("@pDateType",     "2"),
        new SqlParameter("@pStartDateInt", "2"),
        new SqlParameter("@pEndDateInt",   2),
        new SqlParameter("@pGroupName",    "2")        };

      var rows = _context.BookReportViews.FromSqlRaw("usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName", parms).ToList();
      Console.WriteLine($"** {rows.Count}  rows returned");
    }

  }
}
