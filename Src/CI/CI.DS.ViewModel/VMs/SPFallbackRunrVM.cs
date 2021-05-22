﻿using CI.DS.ViewModel.Commands;
using DB.Inventory.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace CI.DS.ViewModel.VMs
{
  public class SPFallbackRunrVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly SpdFbk _spdetl;
    readonly InventoryContext _context;
    string _uFName = "", _sqlConStr = "sql con str";

    public SPFallbackRunrVM(ILogger logger, IConfigurationRoot config, MainVM mainVM, SpdFbk spdetl)
    {
      _logger = logger;
      _config = config;
      _spdetl = spdetl;
      _context = new(SqlConStr = _config["SqlConStr"]); // string.Format(_config["SqlConStr"], cbxSrvr.SelectedValue)); // @"Server=mtUATsqldb;Database=Inventory;Trusted_Connection=True;"); // MTdevSQLDB

      UpdateViewCommand = new UpdateViewCommand(mainVM);

      UFName = spdetl.UFName;

      ValidateAllProperties();
    }

    public string UFName { get => _uFName; set => SetProperty(ref _uFName, value); }
    public string SqlConStr { get => _sqlConStr; set { SetProperty(ref _sqlConStr, value); } }

    public SpdFbk SpdFbk => _spdetl;

    public ICommand UpdateViewCommand { get; set; }
  }
}