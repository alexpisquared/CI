using CI.GUI.Support.WpfLibrary.Base;
using RMSClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RMSClient.Views
{
  public partial class AppSettingsEditor : WindowBase
  {
    readonly AppSettings _appSettings;

    public AppSettingsEditor(AppSettings appSettings, Window owner)
    {
      IgnoreWindowPlacement = true;
      Owner = owner;
      WindowStartupLocation = owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;

      InitializeComponent();
      _appSettings = appSettings;
      DataContext = _appSettings;
    }

    void onSave(object sender, RoutedEventArgs e) { DialogResult = true; Close(); }
    void onQuit(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
  }
}
