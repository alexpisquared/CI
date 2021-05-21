using CI.DS.ViewModel;
using CI.DS.ViewModel.Helpers;
using CI.DS.ViewModel.VMs;
using CI.Standard.Lib.Extensions;
using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CI.DS.Visual.Views
{
  public partial class SPParamManagerView : UserControl
  {
    bool _isdbg = false;

    public SPParamManagerView() => InitializeComponent();

    void onLoaded(object s, RoutedEventArgs e)
    {
#if DEBUG
      _isdbg = true;
#endif
    }
  }
}