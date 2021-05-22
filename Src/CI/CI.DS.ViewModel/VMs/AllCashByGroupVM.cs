using CI.Standard.Lib.Extensions;
using CI.DS.ViewModel.Commands;
using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CI.DS.ViewModel.VMs
{
  public class AllCashByGroupVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly InventoryContext _context;
    int _group_ID = 2, _startDat = 20200101, _endDateI = 20210505;
    string _dateType = "T", _groupNam = "2", _detailedReport = "";
    ObservableCollection<BookReportView> _bookReports = new();
    ObservableCollection<BookGroup> _bookGroups = new();
    ICommand? _searchCommand;

    public AllCashByGroupVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;
      _context = new(_config["SqlConStr"]); // string.Format(_config["SqlConStr"], cbxSrvr.SelectedValue)); // @"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;"); // MTdevSQLDB

      UpdateViewCommand = new UpdateViewCommand(mainVM);

      ValidateAllProperties();

      Task<List<BookGroup>>.Run(() => _context.BookGroups.OrderBy(r => r.Name).ToList()).ContinueWith(_ =>
      {
        try
        {
          Debug.WriteLine($"** {_.Result.Count}  rows returned");
          BookGroups.Clear();
          _.Result.ForEach(row => BookGroups.Add(row));
        }
        catch (Exception ex) { ex.Log(); _logger.LogError(ex, "Really?"); }
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public ObservableCollection<BookReportView> BookReports { get => _bookReports; set => SetProperty(ref _bookReports, value, true); }
    public ObservableCollection<BookGroup> BookGroups { get => _bookGroups; set => SetProperty(ref _bookGroups, value, true); }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int Group_ID { get => _group_ID; set => SetProperty(ref _group_ID, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string DateType { get => _dateType; set => SetProperty(ref _dateType, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int StartDat { get => _startDat; set => SetProperty(ref _startDat, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int EndDateI { get => _endDateI; set => SetProperty(ref _endDateI, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string GroupNam { get => _groupNam; set => SetProperty(ref _groupNam, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }

    public string DetailedReport { get => _detailedReport; set => SetProperty(ref _detailedReport, value, true); }

    public ICommand? SearchCommand => _searchCommand ??= new RelayCommand(async () =>
    {
      DetailedReport = await runRawSqlQuery();       //BookReports.Clear(); await foreach (BookReports result in searchClient.SearchAsync(SearchText)) { BookReports.Add(result); }      //RaisePropertyChanged(() => DetailedReport);
    }
    , () => (!string.IsNullOrEmpty(DateType) && !string.IsNullOrEmpty(GroupNam)));
    public ICommand UpdateViewCommand { get; set; }

    async Task<string> runRawSqlQuery() // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
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

        var rows = await _context.BookReportViews.FromSqlRaw("usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName", parms).ToListAsync();

        BookReports.Clear();
        rows.ForEach(row => BookReports.Add(row));

        Debug.WriteLine($"** {rows.Count}  rows returned");

        return $"{rows.Count} rows found";
      }
      catch (Exception ex) { ex.Log(); _logger.LogError(ex.ToString()); return ex.Message; }
      finally { }
    }
  }
}
