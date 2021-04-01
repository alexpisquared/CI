using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace CI.PermissionManager.Views
{
  public partial class UserPAsWindow : GUI.Support.WpfLibrary.Base.WindowBase
  {
    readonly InventoryContext _context = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;");
    readonly CollectionViewSource _userViewSource;

    public UserPAsWindow()
    {
      InitializeComponent();

      _userViewSource = (CollectionViewSource)FindResource(nameof(_userViewSource));

      DataContext = this;

      Loaded += onLoaded;
    }
    async void onLoaded(object s, RoutedEventArgs e)
    {
      await _context.Users.LoadAsync();
      await _context.PermissionAssignments.LoadAsync();
      await _context.Permissions.LoadAsync();
      await _context.Applications.LoadAsync();
      Title = $"A:{_context.Applications.Local.Count} ◄ P:{_context.Permissions.Local.Count} ◄ pa:{_context.PermissionAssignments.Local.Count} ◄ u:{_context.Users.Local.Count}";

      _userViewSource.Source = _context.Users.Local.ToObservableCollection();//.OrderBy(r => r.UserId);
      _userViewSource.SortDescriptions.Add(new SortDescription(nameof(User.UserId), ListSortDirection.Ascending));
      dg1.SelectedIndex = -1;
    }
    void onSave(object s, RoutedEventArgs e)
    {
      _context.SaveChanges();

      dg1.Items.Refresh();      // this forces the grid to refresh to latest values
      dg2.Items.Refresh();
    }
    void onExit(object s, RoutedEventArgs e) => App.Current.Shutdown();

    protected override void OnClosing(CancelEventArgs e)
    {
      _context.Dispose();
      base.OnClosing(e);
    }
  }
}
