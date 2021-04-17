using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

/// MS Docs
/// https://docs.microsoft.com/en-us/ef/core/querying/raw-sql 
/// https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Querying/RawSQL
/// 
/// Cool sites to check:
/// https://www.michalbialecki.com/2020/09/14/executing-raw-sql-with-entity-framework-core-5/
/// https://www.learnentityframeworkcore.com/raw-sql
/// 
/// https://www.youtube.com/watch?v=qCATqU6Ayno - 3 min video
///  
namespace EfStoredProcConsoleApp
{
  public class Class1
  {
    readonly InventoryContext context = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;");

    public void FromSql()           // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
      var param = new SqlParameter("@FirstName", "Bill"); // new SqlParameter() { ParameterName = "@FirstName", SqlDbType =  System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Size = 50, Value = "Bill" };

      var parms = new[] {
        new SqlParameter("@pGroup_ID", 2),   
        new SqlParameter("@pDateType", "2"),
        new SqlParameter("@pStartDateInt", "2"),
        new SqlParameter("@pEndDateInt", 2),
        new SqlParameter("@pGroupName", "2")        };

      var rows = context.BookReportViews.FromSqlRaw("usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName", parms).ToList();
      Console.WriteLine($"** {rows.Count}");
    }
    public void ExecuteSqlCommand() // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
      /*CREATE PROCEDURE CreateStudent
          @FirstName Varchar(50),
          @LastName Varchar(50)
      AS
      BEGIN
          SET NOCOUNT ON;
          Insert into Students( [FirstName] ,[LastName] ) Values (@FirstName, @LastName)
      END*/
      context.Database.ExecuteSqlRaw("CreateStudents @p0, @p1", parameters: new[] { "Bill", "Gates" }); // ~ var rowsAffected = context.Database.ExecuteSqlCommand("Update Students set FirstName = 'Bill' where StudentId = 1;");
    }
    public void Projection()        // https://www.learnentityframeworkcore.com/raw-sql
    {
      var rows = context.Users
          .FromSqlRaw("SELECT * FROM Users")
          .Select(b => new
          {
            UserId___ = b.UserId,
            Type_____ = b.Type,
            AdminAccs = b.AdminAccess,
            Status___ = b.Status
          }).ToList();

      Console.WriteLine($"** {rows.Count}");
    }
  }
}
/*
USE [Inventory26]
GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[usp_Report_AllCashByGroup]		@pGroup_ID = 2,		@pDateType = N'2',		@pStartDateInt = 2,		@pEndDateInt = 2,		@pGroupName = N'2'
SELECT	'Return Value' = @return_value
GO

CREATE PROCEDURE [dbo].[usp_Report_AllCashByGroup]
  @pGroup_ID int = -1,
  @pDateType char(1) = 'T', -- T = TradeDate S = SettlmntDate
  @pStartDateInt int = 0,
  @pEndDateInt int = 0,
  @pGroupName varchar(50)
AS
  select * from BookReport_view where  Book_ID in (select Book_ID from GroupMember where Group_ID = @pGroup_ID)
*/