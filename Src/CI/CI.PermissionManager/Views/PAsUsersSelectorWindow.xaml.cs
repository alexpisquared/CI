﻿using CI.Standard.Lib.Extensions;
using CI.Standard.Lib.Helpers;
using DB.Inventory.Dbo.Models;
using EF.DbHelper.Lib;
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
  public partial class PAsUsersSelectorWindow : Visual.Lib.Base.WindowBase
  {
    readonly CollectionViewSource _userViewSource, _permViewSource;
    InventoryContext _context;
    bool _loaded, _isDbg, _isDirty;
    string? _last = null;
    int _userid, _permid;
    readonly ILogger<PAsUsersSelectorWindow> _logger;
    readonly Microsoft.Extensions.Configuration.IConfigurationRoot _config;
    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(PAsUsersSelectorWindow), new PropertyMetadata(.0)); public double Blur { get => (double)GetValue(BlurProperty); set => SetValue(BlurProperty, value); }
    public PAsUsersSelectorWindow(ILogger<PAsUsersSelectorWindow> logger, Microsoft.Extensions.Configuration.IConfigurationRoot config)
    {
      InitializeComponent();

      _userViewSource = (CollectionViewSource)FindResource(nameof(_userViewSource));
      _permViewSource = (CollectionViewSource)FindResource(nameof(_permViewSource));

      DataContext = this;

      Loaded += onLoaded;
      themeSelector.ThemeApplier = ApplyTheme;
      _logger = logger;
      _config = config;

#if DEBUG
      _isDbg = true;
#else
      _isDbg = false;
#endif

      var svrs = _config["ServerList"].Split(" ").ToList();
      cbxSrvr.ItemsSource = svrs;
      cbxSrvr.SelectedIndex = 0;
      _context = new(string.Format(_config["SqlConStr"], svrs.First()));
    }
    async void onLoaded(object s, RoutedEventArgs e)
    {
      await loadEF();

      ufp.Text = pfu.Text = "";

      _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {Environment.UserDomainName}\\{Environment.UserName}");

      _loaded = true;
    }
    async void onFlush(object s, RoutedEventArgs e)
    {
      ufp.Text = pfu.Text = "";
      dgPermReset(s, e);
      dgUserReset(s, e);
      await Task.Yield();
      SystemSounds.Asterisk.Play();
    }
    async void onSave(object s, RoutedEventArgs e) => await saveIfDirty();
    void dgPermReset(object s, RoutedEventArgs e) { ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null); dgPerm.Items.Refresh(); }
    void dgUserReset(object s, RoutedEventArgs e) { ((ObservableCollection<User/*  */>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null); dgUser.Items.Refresh(); }
    void onSettings(object s, RoutedEventArgs e) { }
    async void onAudio(object s, RoutedEventArgs e) { _isDbg = false; SystemSounds.Hand.Play(); await Task.Delay(300000); _isDbg = true; }
    void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = WindowState.Normal; }
    void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = WindowState.Maximized; }
    async void cbxSrvr_SelectionChanged(object s, SelectionChangedEventArgs e) { if (_loaded) await loadEF(); }
    void dgPerm_SelectionChanged(object s, SelectionChangedEventArgs e)   /**/{; Debug.Write($"** {((FrameworkElement)s).Name} \t SelectionChanged"); }
    void dgUser_SelectionChanged(object s, SelectionChangedEventArgs e)   /**/
    {
      Debug.Write($"** {((FrameworkElement)s).Name} \t SelectionChanged"); ;
      //if (_loaded && e.RemovedItems.Count > 0 && (e.RemovedItems[0] as User) != null && (e.RemovedItems[0] as User)?.Selectd == true)        (e.RemovedItems[0] as User).Selectd = false;
    }
    void dgUser_GotFocus(object s, RoutedEventArgs e)  /**/{ if (_last != "U") { Debug.Write(" p->U-▒▒▒▒ "); } }
    void dgPerm_GotFocus(object s, RoutedEventArgs e)  /**/{ if (_last != "P") { Debug.Write(" u->P-░░░░ "); } }
    void dgUser_LostFocus(object s, RoutedEventArgs e) /**/{; Debug.Write($"■▀▄ {((FrameworkElement)s).Name}  LostFocus  "); _last = "U"; }
    void dgPerm_LostFocus(object s, RoutedEventArgs e) /**/{; Debug.Write($"■▄▀ {((FrameworkElement)s).Name}  LostFocus  "); _last = "P"; }
    async void dgPerm_SelectedCellsChanged(object s, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded || e.AddedCells.Count < 1 || !(e.AddedCells[0].Column is DataGridTextColumn)) return;

      //if (e.RemovedCells.Count > 0 && ((Permission)e.RemovedCells[0].Item).Selectd == true) ((Permission)e.RemovedCells[0].Item).Selectd = false;

      await saveIfDirty();
      colPG.Visibility = Visibility.Collapsed;
      colUG.Visibility = Visibility.Visible;
      Debug.WriteLine($"■  {((FrameworkElement)s).Name} \t SelectedCellsChanged  {((ObservableCollection<Permission>)_permViewSource.Source).Count(r => r.Granted == true)} selects here");

      var prm = ((Permission)e.AddedCells[0].Item);
      _permid = prm.PermissionId;
      _userid = -1;

      ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => { r.Granted = null; r.Selectd = false; });
      prm.Selectd = true;

      var us = (ObservableCollection<User>)_userViewSource.Source;
      us.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in prm.PermissionAssignments) { var u = us.FirstOrDefault(r => r.UserIntId == pa.UserId); if (u != null) u.Granted = true; }

      await resetPermUnselectUser(); // dgUser.Items.Refresh();

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
      _userid = usr.UserIntId;
      _permid = -1;

      ((ObservableCollection<User>)_userViewSource.Source).ToList().ForEach(r => { r.Granted = null; r.Selectd = false; });      //CollectionViewSource.GetDefaultView(dgUser.ItemsSource).Refresh(); //tu: refresh bound datagrid
      usr.Selectd = true;

      var ps = (ObservableCollection<Permission>)_permViewSource.Source;
      ps.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in usr.PermissionAssignments) { var p = ps.FirstOrDefault(r => r.PermissionId == pa.PermissionId); if (p != null) p.Granted = true; }

      await resetUserUnselectPerm(); // dgPerm.Items.Refresh();      //dgUser.Items.Refresh();

      ufp.Text = $"---"; pfu.Text = $"{usr.UserId}  has  {usr.PermissionAssignments.Count}  permissions:";
    }

    async Task resetUserUnselectPerm()
    {
      await Task.Delay(100);
      ((ObservableCollection<User/*  */>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null);
      ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Selectd = false);
      dgUser.Items.Refresh(); dgPerm.Items.Refresh();
    }
    async Task resetPermUnselectUser()
    {
      await Task.Delay(100);
      ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null);
      ((ObservableCollection<User/*  */>)_userViewSource.Source).ToList().ForEach(r => r.Selectd = false);
      dgPerm.Items.Refresh(); dgUser.Items.Refresh();
    }
    async Task<int> saveIfDirty(bool skipUdate = false)
    {
      var rowsSaved = -1;
      
      if (!_isDirty)
        return rowsSaved;

      try
      {
        if (!skipUdate)
        {
          Blur = 5; pnlBusy.Visibility = Visibility.Visible;
          await Task.Delay(33);
          updateCrosRefTable();
        }

#if SaveForDevOnly //         if (true)// Environment.MachineName == "RAZER1" || new[] { ".", @"mtUATsqldb" }.Contains(cbxSrvr.SelectedValue))
        MessageBox.Show(this, "Press any key to continue...\n\n\t...or any other key to quit", "Changes Saved ...NOT!!! (SaveForDevOnly is ON) :(", MessageBoxButton.OK, MessageBoxImage.Information);
#else
        rowsSaved = await _context.SaveChangesAsync();
#endif

        if (rowsSaved > 0)
          _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {(_isDbg ? $"{rowsSaved,3} rows saved to DB" : "")}   ");
        else
          Title += $"-";

        _isDirty = false;
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
      finally { Blur = 0; pnlBusy.Visibility = Visibility.Hidden; }

      return rowsSaved;
    }

    async Task loadEF()
    {
      try
      {
        await Task.Delay(60);
        _context = new(string.Format(_config["SqlConStr"], cbxSrvr.SelectedValue));
        Title = $"{VersionHelper.CurVerStr} - {_context.ServerDatabase()}";

        await _context.Users.LoadAsync();
        await _context.Permissions.LoadAsync();
        await _context.PermissionAssignments.LoadAsync();
        Title += _isDbg ? $"  A:{_context.Applications.Local.Count} ◄ P:{_context.Permissions.Local.Count} ◄ pa:{_context.PermissionAssignments.Local.Count} ◄ u:{_context.Users.Local.Count}" : "";

        dgPerm.SelectedIndex = -1;
        dgUser.SelectedIndex = -1;

        _userViewSource.Source = _context.Users.Local.ToObservableCollection();
        _userViewSource.SortDescriptions.Add(new SortDescription(nameof(User.UserId), ListSortDirection.Ascending));

        _permViewSource.Source = _context.Permissions.Local.ToObservableCollection();
        _permViewSource.SortDescriptions.Add(new SortDescription(nameof(Permission.Name), ListSortDirection.Ascending)); //tu: instead of  .OrderBy(r => r.UserId); lest forfeit CanUserAddRows.

        btnAddMe.Visibility = _context.Users.Local.Any(r => r.UserId == $@"{Environment.UserDomainName}\{Environment.UserName}") ? Visibility.Collapsed : Visibility.Visible;
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
    }
    async void onAddMe(object s, RoutedEventArgs e)
    {
      _context.Users.Local.Add(new User { UserId = $@"{Environment.UserDomainName}\{Environment.UserName}", AdminAccess = 0, Type = "U", Status = "A" });
      _isDirty = true;
      await saveIfDirty(true);
      btnAddMe.Visibility = Visibility.Collapsed;
    }
    void updateCrosRefTable()
    {
      Debug.Write("    Upd xRef  " +
        $"G:{_context.Permissions.Local.Where(r => r.Granted == true).Count()}  +  " +
        $"f:{_context.Permissions.Local.Where(r => r.Granted == false).Count()}  +  " +
        $"n:{_context.Permissions.Local.Where(r => r.Granted is null).Count()}  =  " +
        $"n:{_context.Permissions.Local.Count}" +
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
