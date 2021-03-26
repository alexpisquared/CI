using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CI.PermissionManager.Views
{
  public partial class PAsUsersSelectorWindow : GUI.Support.WpfLibrary.Base.WindowBase
  {
    readonly InventoryContext _context = new();
    readonly CollectionViewSource _userViewSource;
    readonly CollectionViewSource _permViewSource;
    bool _loaded;
    private bool _audible;
    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(PAsUsersSelectorWindow), new PropertyMetadata(.0)); public double Blur { get { return (double)GetValue(BlurProperty); } set { SetValue(BlurProperty, value); } }

    public PAsUsersSelectorWindow()
    {
      InitializeComponent();

      _userViewSource = (CollectionViewSource)FindResource(nameof(_userViewSource));
      _permViewSource = (CollectionViewSource)FindResource(nameof(_permViewSource));

      DataContext = this;

      Loaded += onLoaded;
    }
    async void onLoaded(object s, RoutedEventArgs e)
    {
      await _context.Users.LoadAsync();
      await _context.Permissions.LoadAsync();
      await _context.PermissionAssignments.LoadAsync();
      await _context.Applications.LoadAsync();
      Title = $"A:{_context.Applications.Local.Count} ◄ P:{_context.Permissions.Local.Count} ◄ pa:{_context.PermissionAssignments.Local.Count} ◄ u:{_context.Users.Local.Count}";

      dgPerm.SelectedIndex = -1;

      _userViewSource.Source = _context.Users.Local.ToObservableCollection();//.OrderBy(r => r.UserId);
      _userViewSource.SortDescriptions.Add(new SortDescription(nameof(User.UserId), ListSortDirection.Ascending));

      _permViewSource.Source = _context.Permissions.Local.ToObservableCollection();//.OrderBy(r => r.UserId);
      _permViewSource.SortDescriptions.Add(new SortDescription(nameof(Permission.Name), ListSortDirection.Ascending));

      dgPerm.SelectedIndex = -1;
      dgUser.SelectedIndex = -1;
      _loaded = true;
    }
    void onSave(object s, RoutedEventArgs e)
    {
      _context.SaveChanges();

      dgUser.Items.Refresh();      // this forces the grid to refresh to latest values
      dgPerm.Items.Refresh();
    }
    void onExit(object s, RoutedEventArgs e) => App.Current.Shutdown();

    protected override void OnClosing(CancelEventArgs e)
    {
      _context.Dispose();
      base.OnClosing(e);
    }

    void dgPerm_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
    {
      if (!_loaded || e.AddedCells.Count < 1 || !(e.AddedCells[0].Column is DataGridTextColumn)) return;

      var prm = ((DB.Inventory.Models.Permission)e.AddedCells[0].Item);
      var pas = prm.PermissionAssignments;

      Debug.WriteLine($" {prm.Name,-32} has {pas.Count,4} asignments.");

      var ps = (ObservableCollection<Permission>)_permViewSource.Source;
      ps.ToList().ForEach(r => r.Granted = false);
      var us = (ObservableCollection<User>)_userViewSource.Source;
      us.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in pas)
      {
        var u = us.FirstOrDefault(r => r.UserIntId == pa.UserId);
        if (u != null)
          u.Granted = true;
      }

      dgUser.Items.Refresh();

      //dgPerm.Items.Refresh();
      //dgPerm.CurrentCell = new DataGridCellInfo(dgPerm.Items[0], dgPerm.Columns[0]);
      //dgPerm.SelectedCells.Add(dgPerm.CurrentCell);
    }

    void dgUser_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded || e.AddedCells.Count < 1 || !(e.AddedCells[0].Column is DataGridTextColumn)) return;

      var usr = ((DB.Inventory.Models.User)e.AddedCells[0].Item);
      var pas = usr.PermissionAssignments;

      Debug.WriteLine($" {usr.UserId,-32} has {pas.Count,4} asignments.");

      var us = (ObservableCollection<User>)_userViewSource.Source;
      us.ToList().ForEach(r => r.Granted = false);
      var ps = (ObservableCollection<Permission>)_permViewSource.Source;
      ps.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in pas)
      {
        var p = ps.FirstOrDefault(r => r.PermissionId == pa.PermissionId);
        if (p != null)
          p.Granted = true;
      }

      dgPerm.Items.Refresh();
      //dgUser.Items.Refresh();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
    async void onAudio(object s, RoutedEventArgs e) { _audible = false; SystemSounds.Hand.Play(); await Task.Delay(300000); _audible = true; }
    void onWindowMinimize(object s, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = WindowState.Normal; }
    void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = WindowState.Maximized; }

  }
}
