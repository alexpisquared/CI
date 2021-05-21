using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DS.ViewModel.VMs
{
  public class SPParamManagerVM : ObservableValidator
  {
    readonly ILogger _logger;
    readonly IConfigurationRoot _config;
    readonly MainVM _mainVM;

    public SPParamManagerVM(ILogger logger, IConfigurationRoot config, MainVM mainVM)
    {
      _logger = logger;
      _config = config;
      _mainVM = mainVM;
    }

    public StoredProcDetail? StoredProcDetail { get; set; }
  }
}
