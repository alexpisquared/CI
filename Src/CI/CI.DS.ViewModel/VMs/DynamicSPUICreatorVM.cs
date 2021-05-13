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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DS.ViewModel.VMs
{
  public class DynamicSPUICreatorVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly StoredProcDetail _spdetl;
    readonly InventoryContext _context;
    int _group_ID = 2, _startDat = 20200101, _endDateI = 20210505;
    string _dateType = "T", _groupNam = "2", _detailedReport = "";
    ObservableCollection<BookReportView> _bookReports = new();
    ObservableCollection<BookGroup> _bookGroups = new();
    ICommand? _searchCommand;

    public DynamicSPUICreatorVM(ILogger logger, IConfigurationRoot config, MainVM mainVM, StoredProcDetail spdetl)
    {
      _logger = logger;
      _config = config;
      _spdetl = spdetl;
      _context = new(_config["SqlConStr"]); // string.Format(_config["SqlConStr"], cbxSrvr.SelectedValue)); // @"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;"); // MTdevSQLDB

      UpdateViewCommand = new UpdateViewCommand(mainVM);

      ValidateAllProperties();

      Task<List<BookGroup>>.Run(() => _context.BookGroups.OrderBy(r => r.Name).ToList()).ContinueWith(_ => { try { Debug.WriteLine($"** {_.Result.Count}  rows returned"); } catch (Exception ex) { _logger.LogError(ex, "Really?"); } }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public StoredProcDetail StoredProcDetail { get => _spdetl; }

    public ICommand UpdateViewCommand { get; set; }
  }
}