﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DS.ViewModel.VMs
{
  public class SPCureatedRunrVM : ObservableValidator
  {
    private ILogger _logger;
    private IConfigurationRoot _config;
    private MainVM _mainVM;
    private SpdUsr _spd;

    public SPCureatedRunrVM(ILogger logger, IConfigurationRoot config, MainVM mainVM, SpdUsr spd)
    {
      _logger = logger;
      _config = config;
      _mainVM = mainVM;
      _spd = spd;
    }
  }
}