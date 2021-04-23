﻿using DB.Inventory.Models;
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
    readonly Microsoft.Extensions.Configuration.IConfigurationRoot _config;

    public AllCashByGroupVM(Microsoft.Extensions.Configuration.IConfigurationRoot config)
    {
      _config = config;
    
      ValidateAllProperties();
    }


    int _group_ID = 2, _startDat = 20200101, _endDateI = 20210505;
    string _dateType = "2", _groupNam = "2", _detailedReport = "";
    ObservableCollection<BookReportView> _bookReports = new();
    ICommand? _searchCommand;

    public ObservableCollection<BookReportView> BookReports { get => _bookReports; set => SetProperty(ref _bookReports, value, true); }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int Group_ID { get => _group_ID; set => SetProperty(ref _group_ID, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string DateType { get => _dateType; set => SetProperty(ref _dateType, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int StartDat { get => _startDat; set => SetProperty(ref _startDat, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.   ")] public int EndDateI { get => _endDateI; set => SetProperty(ref _endDateI, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string GroupNam { get => _groupNam; set => SetProperty(ref _groupNam, value, true); /*(SearchCommand as Command).RaiseCanExecuteChanged(); */ }

    public string DetailedReport { get => _detailedReport; set => SetProperty(ref _detailedReport, value, true); }

    public ICommand? SearchCommand => _searchCommand ?? (_searchCommand = new RelayCommand(async () =>
    {
      DetailedReport = await FromSql();       //BookReports.Clear(); await foreach (BookReports result in searchClient.SearchAsync(SearchText)) { BookReports.Add(result); }      //RaisePropertyChanged(() => DetailedReport);
    }
    , () => (!string.IsNullOrEmpty(DateType) && !string.IsNullOrEmpty(GroupNam))));


    async Task<string> FromSql()           // https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
    {
      InventoryContext context = new(_config["SqlConStr"]); // string.Format(_config["SqlConStr"], cbxSrvr.SelectedValue)); // @"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;"); // MTdevSQLDB
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
