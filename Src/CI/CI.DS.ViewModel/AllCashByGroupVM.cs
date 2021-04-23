using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CI.DS.ViewModel
{
  public class AllCashByGroupVM : ObservableValidator
  {
    public AllCashByGroupVM() => ValidateAllProperties();

    int _Group_ID = 2, _StartDat = 20200101, _EndDateI = 20210505;
    string _DateType = "2", _GroupNam = "2", _DetailedReport = "";
    ObservableCollection<BookReportView> _BookReports = new();

    public ObservableCollection<BookReportView> BookReports { get => _BookReports; set => SetProperty(ref _BookReports, value, true); }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int Group_ID { get => _Group_ID; set => SetProperty(ref _Group_ID, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string DateType { get => _DateType; set => SetProperty(ref _DateType, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int StartDat { get => _StartDat; set => SetProperty(ref _StartDat, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int EndDateI { get => _EndDateI; set => SetProperty(ref _EndDateI, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string GroupNam { get => _GroupNam; set => SetProperty(ref _GroupNam, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }

    public string DetailedReport { get => _DetailedReport; set => SetProperty(ref _DetailedReport, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }

    ICommand? _searchCommand;
    public ICommand? SearchCommand => _searchCommand ?? (_searchCommand = new RelayCommand(async () =>
    {
      DetailedReport = await FromSql();       //BookReports.Clear(); await foreach (BookReports result in searchClient.SearchAsync(SearchText)) { BookReports.Add(result); }      //RaisePropertyChanged(() => DetailedReport);
    }
    , () => (!string.IsNullOrEmpty(DateType) && !string.IsNullOrEmpty(GroupNam))));


    async Task<string> FromSql()           // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
      InventoryContext context = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;"); // MTdevSQLDB
      try
      {
        // USE [Inventory] GO        //DECLARE	@return_value int      EXEC	@return_value = [dbo].[usp_Report_AllCashByGroup]		@pGroup_ID = 2,		@pDateType = N'2',		@pStartDateInt = 2,		@pEndDateInt = 2,		@pGroupName = N'2'          SELECT	'Return Value' = @return_value        //CREATE PROCEDURE [dbo].[usp_Report_AllCashByGroup]  @pGroup_ID int = -1,  @pDateType char(1) = 'T', /*T = TradeDate S = SettlmntDate* /  @pStartDateInt int = 0, @pEndDateInt int = 0, @pGroupName varchar(50)        AS      select * from BookReport_view where  Book_ID in (select Book_ID from GroupMember where Group_ID = @pGroup_ID)
        // new SqlParameter() { ParameterName = "@FirstName", SqlDbType =  System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Size = 50, Value = "Bill" };
        var parms = new[] {
        new SqlParameter("@pGroup_ID",      /**/ Group_ID ),
        new SqlParameter("@pDateType",      /**/ DateType ),
        new SqlParameter("@pStartDateInt",  /**/ StartDat ),
        new SqlParameter("@pEndDateInt",    /**/ EndDateI ),
        new SqlParameter("@pGroupName",     /**/ GroupNam )};

        var rows = await context.BookReportViews.FromSqlRaw("usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName", parms).ToListAsync();

        BookReports.Clear();
        rows.ForEach(row => BookReports.Add(row));

        Debug.WriteLine($"** {rows.Count}  rows returned");

        return $"{rows.Count} rows found";
      }
      catch (System.Exception ex) { return ex.Message; }
      finally { }
    }
  }
}
