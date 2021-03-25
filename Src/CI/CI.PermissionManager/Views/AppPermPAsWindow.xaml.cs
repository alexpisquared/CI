using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace CI.PermissionManager.Views
{
  public partial class AppPermPAsWindow : Window
  {
    readonly InventoryContext _context = new();
    readonly CollectionViewSource _applicationViewSource;

    public AppPermPAsWindow()
    {
      InitializeComponent();

      _applicationViewSource = (CollectionViewSource)FindResource(nameof(_applicationViewSource));

      DataContext = this;

      Loaded += onLoaded;
    }
    void onLoaded(object sender, RoutedEventArgs e)
    {
      _context.Applications.Load();           //await _context.Permissions.LoadAsync();
      _context.Permissions.Load();            //await _context.Permissions.LoadAsync();
      _context.PermissionAssignments.Load();  //await _context.Permissions.LoadAsync();
      Title = $"A:{_context.Applications.Local.Count} ◄ P:{_context.Permissions.Local.Count} ◄ pa:{_context.PermissionAssignments.Local.Count}";

      _applicationViewSource.Source = _context.Applications.Local.ToObservableCollection();
    }
    void onSave(object s, RoutedEventArgs e)
    {
      _context.SaveChanges();

      _applicationDataGrid.Items.Refresh();      // this forces the grid to refresh to latest values
      _applicationPermissionsDataGrid.Items.Refresh();
      _applicationPermissionPermissionAssignmentsDataGrid.Items.Refresh();
    }
    void onExit(object s, RoutedEventArgs e) => Close();

    protected override void OnClosing(CancelEventArgs e)
    {
      _context.Dispose();
      base.OnClosing(e);
    }
  }
}
