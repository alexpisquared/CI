using CI.GUI.Support.WpfLibrary.Extensions;
using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
    readonly InventoryContext _context;
    readonly CollectionViewSource _userViewSource, _permViewSource;
    bool _loaded, _audible;
    bool? _isUser = null;
    int _userid, _permid;
    private bool _isDirty;
    readonly ILogger<PAsUsersSelectorWindow> _logger;
    readonly Microsoft.Extensions.Configuration.IConfigurationRoot _config;
    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(PAsUsersSelectorWindow), new PropertyMetadata(.0)); public double Blur { get => (double)GetValue(BlurProperty); set => SetValue(BlurProperty, value); }
    public PAsUsersSelectorWindow(Microsoft.Extensions.Logging.ILogger<PAsUsersSelectorWindow> logger, Microsoft.Extensions.Configuration.IConfigurationRoot config)
    {
      InitializeComponent();

      _userViewSource = (CollectionViewSource)FindResource(nameof(_userViewSource));
      _permViewSource = (CollectionViewSource)FindResource(nameof(_permViewSource));

      DataContext = this;

      Loaded += onLoaded;
      themeSelector.ApplyTheme = ApplyTheme;
      _logger = logger;
      _config = config;

      cbxServers.ItemsSource = _config["ServerList"].Split(" ").ToList(); ;
      cbxServers.SelectedIndex = 0;
      _context = new(string.Format(_config["SqlConStr"], cbxServers.SelectedValue));
    }
    async void onLoaded(object s, RoutedEventArgs e)
    {
      try
      {
        await Task.Delay(60);
        await _context.Users.LoadAsync();
        await _context.Permissions.LoadAsync();
        await _context.PermissionAssignments.LoadAsync();
        Title = $"A:{_context.Applications.Local.Count} ◄ P:{_context.Permissions.Local.Count} ◄ pa:{_context.PermissionAssignments.Local.Count} ◄ u:{_context.Users.Local.Count}";

        dgPerm.SelectedIndex = -1;
        dgUser.SelectedIndex = -1;

        _userViewSource.Source = _context.Users.Local.ToObservableCollection();
        _userViewSource.SortDescriptions.Add(new SortDescription(nameof(User.UserId), ListSortDirection.Ascending));

        _permViewSource.Source = _context.Permissions.Local.ToObservableCollection();
        _permViewSource.SortDescriptions.Add(new SortDescription(nameof(Permission.Name), ListSortDirection.Ascending)); //tu: instead of  .OrderBy(r => r.UserId); lest forfeit CanUserAddRows.

        ufp.Text = pfu.Text = "■ ■ ■";

        _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {Environment.UserName}   ");

        _loaded = true;
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
    }
    async void onFlush(object s, RoutedEventArgs e)
    {
      ufp.Text = pfu.Text = "■ ■ ■";
      dgPermReset(s, e);
      dgUserReset(s, e);
      await Task.Yield();
      SystemSounds.Asterisk.Play();
    }
    async void onSave(object s, RoutedEventArgs e) => await saveIfDirty();
    void dgPermReset(object s, RoutedEventArgs e) { ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null); dgPerm.Items.Refresh(); }
    void dgUserReset(object s, RoutedEventArgs e) { ((ObservableCollection<User/*  */>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null); dgUser.Items.Refresh(); }
    async void dgPerm_SelectedCellsChanged(object s, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded || e.AddedCells.Count < 1 || !(e.AddedCells[0].Column is DataGridTextColumn)) return;

      //if (e.RemovedCells.Count > 0 && ((Permission)e.RemovedCells[0].Item).Selectd == true) ((Permission)e.RemovedCells[0].Item).Selectd = false;

      await saveIfDirty();
      colPG.Visibility = Visibility.Collapsed;
      colUG.Visibility = Visibility.Visible;
      Debug.WriteLine($"■  {((FrameworkElement)s).Name} \t SelectedCellsChanged  {((ObservableCollection<Permission>)_permViewSource.Source).Count(r => r.Granted == true)} selects here");

      var prm = ((Permission)e.AddedCells[0].Item);
      prm.Selectd = true;
      _permid = prm.PermissionId;
      _userid = -1;

      ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null);
      var us = (ObservableCollection<User>)_userViewSource.Source;
      us.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in prm.PermissionAssignments) { var u = us.FirstOrDefault(r => r.UserIntId == pa.UserId); if (u != null) u.Granted = true; }

      dgUser.Items.Refresh();

      pfu.Text = $"---"; ufp.Text = $"{prm.Name}  assigned to  {prm.PermissionAssignments.Count}  users:";
    }
    async void dgUser_SelectedCellsChanged(object s, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded || e.AddedCells.Count < 1 || !(e.AddedCells[0].Column is DataGridTextColumn)) return;

      //if (e.RemovedCells.Count > 0 && ((User)e.RemovedCells[0].Item).Selectd == true) ((User)e.RemovedCells[0].Item).Selectd = false;      

      await saveIfDirty();
      colPG.Visibility = Visibility.Visible;
      colUG.Visibility = Visibility.Collapsed;
      Debug.WriteLine($"■  {((FrameworkElement)s).Name} \t SelectedCellsChanged  {((ObservableCollection<User>)_userViewSource.Source).Count(r => r.Granted == true)} selects here");

      var usr = ((User)e.AddedCells[0].Item);
      usr.Selectd = true;
      _userid = usr.UserIntId;
      _permid = -1;

      ((ObservableCollection<User>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null);      //CollectionViewSource.GetDefaultView(dgUser.ItemsSource).Refresh(); //tu: refresh bound datagrid

      var ps = (ObservableCollection<Permission>)_permViewSource.Source;
      ps.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in usr.PermissionAssignments) { var p = ps.FirstOrDefault(r => r.PermissionId == pa.PermissionId); if (p != null) p.Granted = true; }

      dgPerm.Items.Refresh();      //dgUser.Items.Refresh();

      ufp.Text = $"---"; pfu.Text = $"{usr.UserId}  has  {usr.PermissionAssignments.Count}  permissions:";
    }
    void onSettings(object s, RoutedEventArgs e) { }
    async void onAudio(object s, RoutedEventArgs e) { _audible = false; SystemSounds.Hand.Play(); await Task.Delay(300000); _audible = true; }
    void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = WindowState.Normal; }
    void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = WindowState.Maximized; }
    void cbxServers_SelectionChanged(object s, SelectionChangedEventArgs e) { }
    void dgPerm_SelectionChanged(object s, SelectionChangedEventArgs e)   /**/{; Debug.Write($"** {((FrameworkElement)s).Name} \t SelectionChanged"); }
    void dgUser_SelectionChanged(object s, SelectionChangedEventArgs e)   /**/
    {
      Debug.Write($"** {((FrameworkElement)s).Name} \t SelectionChanged");
      //if (_loaded && e.RemovedItems.Count > 0 && (e.RemovedItems[0] as User) != null && (e.RemovedItems[0] as User)?.Selectd == true)        (e.RemovedItems[0] as User).Selectd = false;
    }
    void dgUser_GotFocus(object s, RoutedEventArgs e)  /**/{; Debug.Write($"** {((FrameworkElement)s).Name} \t GotFocus"); if (_isUser != true) { Debug.Write("░▒░▒░▒░▒"); ((ObservableCollection<User/*  */>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null); } }
    void dgPerm_GotFocus(object s, RoutedEventArgs e)  /**/{; Debug.Write($"** {((FrameworkElement)s).Name} \t GotFocus"); if (_isUser == true) { Debug.Write("░▒░▒░▒░▒"); ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null); } }
    void dgUser_LostFocus(object s, RoutedEventArgs e) /**/{; Debug.Write($"■▀▄ {((FrameworkElement)s).Name} \t LostFocus"); _isUser = true; }
    void dgPerm_LostFocus(object s, RoutedEventArgs e) /**/{; Debug.Write($"■▄▀ {((FrameworkElement)s).Name} \t LostFocus"); _isUser = false; }

    async Task<int> saveIfDirty(bool skipUdate = false)
    {
      var rs = -1;
      if (!_isDirty)
        return rs;

      try
      {
        if (Environment.MachineName == "RAZER1" || new[] { ".", @".\SqlExpress" }.Contains(cbxServers.SelectedValue))
        {
          if (!skipUdate)
          {
            Blur = 5; pnlBusy.Visibility = Visibility.Visible;
            await Task.Delay(33);
            updateCrosRefTable();
          }

          rs = await _context.SaveChangesAsync();
          if (rs > 0)
          {
            Title = $"{rs,3} rows saved to DB";
            Debug.WriteLine(Title);
            _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {Title}   ");
          }
          else
            Title += $"-";

          _isDirty = false;
        }
        else
          MessageBox.Show(this, "Press any key to continue...\n\n\t...or any other key to quit", "Changes Saved ...NOT!!!", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
      finally
      {
        Blur = 0; pnlBusy.Visibility = Visibility.Hidden;
      }
      return rs;
    }
    void updateCrosRefTable()
    {
      Debug.Write("    Upd xRef  " +
        $"G:{_context.Permissions.Local.Where(r => r.Granted == true).Count()}  +  " +
        $"f:{_context.Permissions.Local.Where(r => r.Granted == false).Count()}  +  " +
        $"n:{_context.Permissions.Local.Where(r => r.Granted is null).Count()}  =  " +
        $"n:{_context.Permissions.Local.Count()}" +
        $"   ");

      if (_userid > 0 && _permid < 0)
      {
#if false // no difference
        _context.Permissions.Local.ToList().ForEach(p =>
        {
          var dbpa = _context.PermissionAssignments.Local.FirstOrDefault(r => r.UserId == _userid && r.PermissionId == p.PermissionId);
          if (dbpa != null)
          {
            if (p.Granted == true)
              dbpa.Status = "G";
            else
              _context.PermissionAssignments.Local.Remove(dbpa);
          }
          else if (p.Granted == true)
            _context.PermissionAssignments.Local.Add(new PermissionAssignment { UserId = _userid, PermissionId = p.PermissionId, Status = "G" });
        });
#else
        _context.Permissions.Local.Where(r => r.Granted == true).ToList().ForEach(p =>
        {
          var dbpa = _context.PermissionAssignments.Local.FirstOrDefault(r => r.UserId == _userid && r.PermissionId == p.PermissionId);
          if (dbpa != null)
          {
            if (dbpa.Status != "G")
              dbpa.Status = "G";
          }
          else
            _context.PermissionAssignments.Local.Add(new PermissionAssignment { UserId = _userid, PermissionId = p.PermissionId, Status = "G" });
        });

        _context.Permissions.Local.Where(r => r.Granted == false).ToList().ForEach(p =>
        {
          var dbpa = _context.PermissionAssignments.Local.FirstOrDefault(r => r.UserId == _userid && r.PermissionId == p.PermissionId);
          if (dbpa != null)
            _context.PermissionAssignments.Local.Remove(dbpa);
        });
#endif
      }
      else if (_userid < 0 && _permid > 0)
      {
        _context.Users.Local.ToList().ForEach(u =>
        {
          var dbpa = _context.PermissionAssignments.Local.FirstOrDefault(r => r.UserId == u.UserIntId && r.PermissionId == _permid);
          if (dbpa != null)
          {
            if (u.Granted == true)
              dbpa.Status = "G";
            else
              _context.PermissionAssignments.Local.Remove(dbpa);
          }
          else if (u.Granted == true)
            _context.PermissionAssignments.Local.Add(new PermissionAssignment { UserId = u.UserIntId, PermissionId = _permid, Status = "G" });
        });
      }

    }
    internal async Task Recalc(FrameworkElement s)
    {
      var dc = ((FrameworkElement)s.TemplatedParent).DataContext;
      if (dc is Permission perm && _userid > 0)
      {
        var dbpa = _context.PermissionAssignments.Local.FirstOrDefault(r => r.UserId == _userid && r.PermissionId == perm.PermissionId);
        if (dbpa == null && perm.Granted == true)
        {
          _context.PermissionAssignments.Local.Add(new PermissionAssignment { UserId = _userid, PermissionId = perm.PermissionId, Status = "G" });
          _isDirty = true;
        }
        if (dbpa != null && perm.Granted == false)
        {
          _context.PermissionAssignments.Local.Remove(dbpa);
          _isDirty = true;
        }
      }
      else if (dc is User user && _permid > 0)
      {
        var dbpa = _context.PermissionAssignments.Local.FirstOrDefault(r => r.UserId == user.UserIntId && r.PermissionId == _permid);
        if (dbpa == null && user.Granted == true)
        {
          _context.PermissionAssignments.Local.Add(new PermissionAssignment { UserId = user.UserIntId, PermissionId = _permid, Status = "G" });
          _isDirty = true;
        }
        if (dbpa != null && user.Granted == false)
        {
          _context.PermissionAssignments.Local.Remove(dbpa);
          _isDirty = true;
        }
      }

      await saveIfDirty(true);

      if (_userid > 0 && _permid < 0)
      {
        ufp.Text = $"---";
        pfu.Text = $"{_context.Users.Local.FirstOrDefault(r => r.UserIntId == _userid)?.UserId}  has  {_context.Permissions.Local.Where(r => r.Granted == true).Count()}  permissions:";
      }
      else if (_userid < 0 && _permid > 0)
      {
        pfu.Text = $"---";
        ufp.Text = $"{_context.Permissions.Local.FirstOrDefault(r => r.PermissionId == _permid)?.Name}  assigned to  {_context.Users.Local.Where(r => r.Granted == true).Count()}  users:";
      }
    }
    protected override void OnClosing(CancelEventArgs e)
    {
      _context.Dispose();
      base.OnClosing(e);
    }
  }
}
