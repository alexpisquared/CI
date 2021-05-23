﻿using DB.Inventory.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Media;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using CI.Standard.Lib.Extensions;

namespace CI.DS.Visual.Views
{
  public partial class UserSpSelectorView : UserControl
  {
    readonly CollectionViewSource _userViewSource, _permViewSource;
    InventoryContext _context;
    bool _loaded, _isDbg, _isDirty;
    string? _last = null;
    int _userid, _permid;
    ILogger _logger;
    Microsoft.Extensions.Configuration.IConfigurationRoot _config;
    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(UserSpSelectorView), new PropertyMetadata(.0)); public double Blur { get => (double)GetValue(BlurProperty); set => SetValue(BlurProperty, value); }
    public UserSpSelectorView()
    {
      InitializeComponent();

      _userViewSource = (CollectionViewSource)FindResource(nameof(_userViewSource));
      _permViewSource = (CollectionViewSource)FindResource(nameof(_permViewSource));

      //todo: themeSelector.ThemeApplier = ApplyTheme;
    } //todo:
    public UserSpSelectorView(ILogger logger, Microsoft.Extensions.Configuration.IConfigurationRoot config)
    {
      _userViewSource = (CollectionViewSource)FindResource(nameof(_userViewSource));
      _permViewSource = (CollectionViewSource)FindResource(nameof(_permViewSource));

      init(logger, config);
    } //todo:
    void init(ILogger logger, Microsoft.Extensions.Configuration.IConfigurationRoot config)
    {
#if DEBUG
      _isDbg = true;
#else
      _isDbg = false;
#endif

      //todo: themeSelector.ThemeApplier = ApplyTheme;
      _logger = logger;
      _config = config;

      var svrs = _config["ServerList"].Split(" ").ToList();
      cbxSrvr.ItemsSource = svrs;
      cbxSrvr.SelectedIndex = 0;
      _context = new(string.Format(_config["SqlConStr"], svrs.First()));
    }
    async void onLoaded(object s, RoutedEventArgs e)
    {
      init((ILogger?)((CI.DS.ViewModel.VMs.UserSpSelectorVM)DataContext)._logger, ((CI.DS.ViewModel.VMs.UserSpSelectorVM)DataContext)._config);

      await loadEF();

      ufp.Text = pfu.Text = "";

      _logger.LogInformation($"  {Environment.UserDomainName}\\{Environment.UserName}");

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
    void dgPermReset(object s, RoutedEventArgs e) { ((ObservableCollection<Dbprocess>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null); dgPerm.Items.Refresh(); }
    void dgUserReset(object s, RoutedEventArgs e) { ((ObservableCollection<User/*  */>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null); dgUser.Items.Refresh(); }
    void onSettings(object s, RoutedEventArgs e) { }
    async void onAudio(object s, RoutedEventArgs e) { _isDbg = false; SystemSounds.Hand.Play(); await Task.Delay(300000); _isDbg = true; }
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

      //if (e.RemovedCells.Count > 0 && ((Dbprocess)e.RemovedCells[0].Item).Selectd == true) ((Dbprocess)e.RemovedCells[0].Item).Selectd = false;

      await saveIfDirty();
      colPG.Visibility = Visibility.Collapsed;
      colUG.Visibility = Visibility.Visible;
      Debug.WriteLine($"■  {((FrameworkElement)s).Name} \t SelectedCellsChanged  {((ObservableCollection<Dbprocess>)_permViewSource.Source).Count(r => r.Granted == true)} selects here");

      var prm = ((Dbprocess)e.AddedCells[0].Item);
      _permid = prm.Id;
      _userid = -1;

      ((ObservableCollection<Dbprocess>)_permViewSource.Source).ToList().ForEach(r => { r.Granted = null; r.Selectd = false; });
      prm.Selectd = true;

      var us = (ObservableCollection<User>)_userViewSource.Source;
      us.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in prm.ProcessUserAccesses) { var u = us.FirstOrDefault(r => r.UserIntId == pa.UserId); if (u != null) u.Granted = true; }

      await resetPermUnselectUser(); // dgUser.Items.Refresh();

      pfu.Text = $"---"; ufp.Text = $"{prm.StoredProcName}  assigned to  {prm.ProcessUserAccesses.Count}  users:";
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

      var ps = (ObservableCollection<Dbprocess>)_permViewSource.Source;
      ps.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in usr.ProcessUserAccesses) { var p = ps.FirstOrDefault(r => r.Id == pa.DbprocessId); if (p != null) p.Granted = true; }

      await resetUserUnselectPerm(); // dgPerm.Items.Refresh();      //dgUser.Items.Refresh();

      ufp.Text = $"---"; pfu.Text = $"{usr.UserId}  has  {usr.ProcessUserAccesses.Count}  Dbprocesses:";
    }

    async Task resetUserUnselectPerm()
    {
      await Task.Delay(100);
      ((ObservableCollection<User/*  */>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null);
      ((ObservableCollection<Dbprocess>)_permViewSource.Source).ToList().ForEach(r => r.Selectd = false);
      dgUser.Items.Refresh(); dgPerm.Items.Refresh();
    }
    async Task resetPermUnselectUser()
    {
      await Task.Delay(100);
      ((ObservableCollection<Dbprocess>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null);
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
          _logger.LogInformation($"  {(_isDbg ? $"{rowsSaved,3} rows saved to DB" : "")}   ");
        else
          tbkTitle.Text += $"-";

        _isDirty = false;
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(null); }
      finally { Blur = 0; pnlBusy.Visibility = Visibility.Hidden; }

      return rowsSaved;
    }

    async Task loadEF()
    {
      try
      {
        await Task.Delay(60);
        _context = new(string.Format(_config["SqlConStr"], cbxSrvr.SelectedValue));
        await _context.Users.LoadAsync();
        await _context.Dbprocesses.LoadAsync();
        await _context.ProcessUserAccesses.LoadAsync();
        tbkTitle.Text = _isDbg ? $"A:{_context.Applications.Local.Count} ◄ P:{_context.Dbprocesses.Local.Count} ◄ pa:{_context.ProcessUserAccesses.Local.Count} ◄ u:{_context.Users.Local.Count}" : "";

        dgPerm.SelectedIndex = -1;
        dgUser.SelectedIndex = -1;

        _userViewSource.Source = _context.Users.Local.ToObservableCollection();
        _userViewSource.SortDescriptions.Add(new SortDescription(nameof(User.UserId), ListSortDirection.Ascending));

        _permViewSource.Source = _context.Dbprocesses.Local.ToObservableCollection();
        _permViewSource.SortDescriptions.Add(new SortDescription(nameof(Dbprocess.DbprocessName), ListSortDirection.Ascending)); //tu: instead of  .OrderBy(r => r.UserId); lest forfeit CanUserAddRows.

        btnAddMe.Visibility = _context.Users.Local.Any(r => r.UserId == $@"{Environment.UserDomainName}\{Environment.UserName}") ? Visibility.Collapsed : Visibility.Visible;
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(null); }
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
        $"G:{_context.Dbprocesses.Local.Where(r => r.Granted == true).Count()}  +  " +
        $"f:{_context.Dbprocesses.Local.Where(r => r.Granted == false).Count()}  +  " +
        $"n:{_context.Dbprocesses.Local.Where(r => r.Granted is null).Count()}  =  " +
        $"n:{_context.Dbprocesses.Local.Count}" +
        $"   ");

      if (_userid > 0 && _permid < 0)
      {
#if false // no difference
        _context.Dbprocesses.Local.ToList().ForEach(p =>
        {
          var dbpa = _context.ProcessUserAccesses.Local.FirstOrDefault(r => r.UserId == _userid && r.DbprocessId == p.DbprocessId);
          if (dbpa != null)
          {
            if (p.Granted == true)
              dbpa.Status = "G";
            else
              _context.ProcessUserAccesses.Local.Remove(dbpa);
          }
          else if (p.Granted == true)
            _context.ProcessUserAccesses.Local.Add(new ProcessUserAccess { UserId = _userid, DbprocessId = p.DbprocessId , RoleId = "U"});
        });
#else
        _context.Dbprocesses.Local.Where(r => r.Granted == true).ToList().ForEach(p =>
        {
          var dbpa = _context.ProcessUserAccesses.Local.FirstOrDefault(r => r.UserId == _userid && r.DbprocessId == p.Id);
          if (dbpa != null)
          {
            Debugger.Break(); //             if (dbpa.Status != "G")              dbpa.Status = "G";
          }
          else
            _context.ProcessUserAccesses.Local.Add(new ProcessUserAccess { UserId = _userid, DbprocessId = p.Id, RoleId = "U" });
        });

        _context.Dbprocesses.Local.Where(r => r.Granted == false).ToList().ForEach(p =>
        {
          var dbpa = _context.ProcessUserAccesses.Local.FirstOrDefault(r => r.UserId == _userid && r.DbprocessId == p.Id);
          if (dbpa != null)
            _context.ProcessUserAccesses.Local.Remove(dbpa);
        });
#endif
      }
      else if (_userid < 0 && _permid > 0)
      {
        _context.Users.Local.ToList().ForEach(u =>
        {
          var dbpa = _context.ProcessUserAccesses.Local.FirstOrDefault(r => r.UserId == u.UserIntId && r.DbprocessId == _permid);
          if (dbpa != null)
          {
            if (u.Granted == true)
              Debugger.Break(); //             dbpa.Status = "G";
            else
              _context.ProcessUserAccesses.Local.Remove(dbpa);
          }
          else if (u.Granted == true)
            _context.ProcessUserAccesses.Local.Add(new ProcessUserAccess { UserId = u.UserIntId, DbprocessId = _permid, RoleId = "U" });
        });
      }
    }

    internal async Task Recalc(FrameworkElement s)
    {
      var dc = ((FrameworkElement)s.TemplatedParent).DataContext;
      if (dc is Dbprocess perm && _userid > 0)
      {
        var dbpa = _context.ProcessUserAccesses.Local.FirstOrDefault(r => r.UserId == _userid && r.DbprocessId == perm.Id);
        if (dbpa == null && perm.Granted == true)
        {
          _context.ProcessUserAccesses.Local.Add(new ProcessUserAccess { UserId = _userid, DbprocessId = perm.Id, RoleId = "U" });
          _isDirty = true;
        }
        if (dbpa != null && perm.Granted == false)
        {
          _context.ProcessUserAccesses.Local.Remove(dbpa);
          _isDirty = true;
        }
      }
      else if (dc is User user && _permid > 0)
      {
        var dbpa = _context.ProcessUserAccesses.Local.FirstOrDefault(r => r.UserId == user.UserIntId && r.DbprocessId == _permid);
        if (dbpa == null && user.Granted == true)
        {
          _context.ProcessUserAccesses.Local.Add(new ProcessUserAccess { UserId = user.UserIntId, DbprocessId = _permid, RoleId = "U" });
          _isDirty = true;
        }
        if (dbpa != null && user.Granted == false)
        {
          _context.ProcessUserAccesses.Local.Remove(dbpa);
          _isDirty = true;
        }
      }

      await saveIfDirty(true);

      if (_userid > 0 && _permid < 0)
      {
        ufp.Text = $"---";
        pfu.Text = $"{_context.Users.Local.FirstOrDefault(r => r.UserIntId == _userid)?.UserId}  can run  {_context.Dbprocesses.Local.Where(r => r.Granted == true).Count()}  Dbprocesses:";
      }
      else if (_userid < 0 && _permid > 0)
      {
        pfu.Text = $"---";
        ufp.Text = $"{_context.Dbprocesses.Local.FirstOrDefault(r => r.Id == _permid)?.DbprocessName}  assigned to  {_context.Users.Local.Where(r => r.Granted == true).Count()}  users:";
      }
    }



    async void onTogglePermission(object s, RoutedEventArgs e)
    {
      await Recalc((FrameworkElement)s); //todo: 
    }
  }
}
