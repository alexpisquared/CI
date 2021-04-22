using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfStoredProcWpfApp.Views
{
  public partial class SpDemoTableView : UserControl
  {
    readonly InventoryContext context = new(@"Server=MTdevSQLDB;Database=Inventory;Trusted_Connection=True;");
    public SpDemoTableView() => InitializeComponent();

    async Task FromSql()           // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
      try
      {
        ctrlPanel.IsEnabled = false;

        // USE [Inventory] GO
        //DECLARE	@return_value int      EXEC	@return_value = [dbo].[usp_Report_AllCashByGroup]		@pGroup_ID = 2,		@pDateType = N'2',		@pStartDateInt = 2,		@pEndDateInt = 2,		@pGroupName = N'2'          SELECT	'Return Value' = @return_value
        //CREATE PROCEDURE [dbo].[usp_Report_AllCashByGroup]  @pGroup_ID int = -1,  @pDateType char(1) = 'T', /*T = TradeDate S = SettlmntDate* /  @pStartDateInt int = 0, @pEndDateInt int = 0, @pGroupName varchar(50)        AS      select * from BookReport_view where  Book_ID in (select Book_ID from GroupMember where Group_ID = @pGroup_ID)
        var param = new SqlParameter("@FirstName", "Bill"); // new SqlParameter() { ParameterName = "@FirstName", SqlDbType =  System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Size = 50, Value = "Bill" };
        var parms = new[] {
        new SqlParameter("@pGroup_ID",      /**/ t1.Text),
        new SqlParameter("@pDateType",      /**/ t2.Text),
        new SqlParameter("@pStartDateInt",  /**/ (int)DateTime.Parse(t3.Text).ToOADate()),
        new SqlParameter("@pEndDateInt",    /**/ (int)DateTime.Parse(t4.Text).ToOADate()),
        new SqlParameter("@pGroupName",     /**/ t5.Text)};

        var rows = await context.BookReportViews.FromSqlRaw("usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName", parms).ToListAsync();
        Debug.WriteLine($"** {rows.Count}  rows returned");

        dg1.ItemsSource = rows;
        tbkError.Text = $"{rows.Count} rows found";
      }
      catch (System.Exception ex)
      {
        tbkError.Text = ex.Message;
      }
      finally
      {
        ctrlPanel.IsEnabled = true;
      }
    }

    async void onGo(object sender, RoutedEventArgs e) { await FromSql(); ; }
  }
}
