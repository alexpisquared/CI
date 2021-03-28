﻿using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
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
    readonly InventoryContext _context = new();
    readonly CollectionViewSource _userViewSource;


    readonly CollectionViewSource _permViewSource;
    bool _loaded, _audible;
    public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(PAsUsersSelectorWindow), new PropertyMetadata(.0)); public double Blur { get => (double)GetValue(BlurProperty); set => SetValue(BlurProperty, value); }

    public PAsUsersSelectorWindow()
    {
      InitializeComponent();

      _userViewSource = (CollectionViewSource)FindResource(nameof(_userViewSource));
      _permViewSource = (CollectionViewSource)FindResource(nameof(_permViewSource));

      DataContext = this;

      Loaded += onLoaded;
      themeSelector.ApplyTheme = ApplyTheme;
    }
    async void onLoaded(object s, RoutedEventArgs e)
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

      _loaded = true;
    }
    async void onFlush(object s, RoutedEventArgs e)
    {
      ufp.Text = pfu.Text = "■ ■ ■";
      dgPermReset(s, e);
      dgUserReset(s, e);
      await Task.Yield();
      SystemSounds.Asterisk.Play();
    }
    void onSave(object s, RoutedEventArgs e)
    {
      if (Environment.MachineName == "RAZER1")
      {
        updateCrosRefTable();
        var rs = _context.SaveChanges();
        Title = $"{rs} saved to DB";
      }
      else
        MessageBox.Show(this, "Press any key to continue...\n\n\t...or any other key to quit", "Changes Saved", MessageBoxButton.OK, MessageBoxImage.Information);

      onFlush(s, e);
      dgUser.Items.Refresh();      // this forces the grid to refresh to latest values
      dgPerm.Items.Refresh();
    }

    int _userid, _permid;
    void updateCrosRefTable()
    {
      if (_userid > 0 && _permid < 0)
      {
        Debug.WriteLine(
          $"G:{_context.Permissions.Local.Where(r => r.Granted == true).Count()}  +  " +
          $"f:{_context.Permissions.Local.Where(r => r.Granted == false).Count()}  +  " +
          $"n:{_context.Permissions.Local.Where(r => r.Granted is null).Count()}  =  " +
          $"n:{_context.Permissions.Local.Count()}" +
          $"");

        _context.Permissions.Local.Where(r => r.Granted == true).ToList().ForEach(p =>
        {
          var dbpa = _context.PermissionAssignments.Local.FirstOrDefault(r => r.UserId == _userid && r.PermissionId == p.PermissionId);
          if (dbpa != null)
            dbpa.Status = p.Granted == true ? "G" : "-";
          else
            _context.PermissionAssignments.Local.Add(new PermissionAssignment { UserId = _userid, PermissionId = p.PermissionId, Status = "G" });
        });

        _context.PermissionAssignments.Local.Where(r => r.UserId == _userid).ToList().ForEach(up =>
                  {
                    var ps = _context.Permissions.Local.Where(p => p.PermissionId == up.PermissionId);
                    up.Status = ps.Count() > 0 ? "G" : "-";
                  });
      }
      else
      if (_userid < 0 && _permid > 0) { }

    }

    void onExit(object s, RoutedEventArgs e) => App.Current.Shutdown();

    protected override void OnClosing(CancelEventArgs e)
    {
      _context.Dispose();
      base.OnClosing(e);
    }

    void dgPermReset(object s, RoutedEventArgs e) { ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Granted = null); dgPerm.Items.Refresh(); }
    void dgUserReset(object s, RoutedEventArgs e) { ((ObservableCollection<User>)_userViewSource.Source).ToList().ForEach(r => r.Granted = null); dgUser.Items.Refresh(); }
    void dgPerm_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded || e.AddedCells.Count < 1 || !(e.AddedCells[0].Column is DataGridTextColumn)) return;

      var prm = ((Permission)e.AddedCells[0].Item);
      _permid = prm.PermissionId;
      _userid = -1;

      ((ObservableCollection<Permission>)_permViewSource.Source).ToList().ForEach(r => r.Granted = false);
      var us = (ObservableCollection<User>)_userViewSource.Source;
      us.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in prm.PermissionAssignments)
      {
        var u = us.FirstOrDefault(r => r.UserIntId == pa.UserId);
        if (u != null)
          u.Granted = true;
      }

      dgUser.Items.Refresh();

      pfu.Text = $"· · ·";
      ufp.Text = $"{prm.Name}  assigned to  {prm.PermissionAssignments.Count}  users:";
    }
    void dgUser_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded || e.AddedCells.Count < 1 || !(e.AddedCells[0].Column is DataGridTextColumn)) return;

      var usr = ((User)e.AddedCells[0].Item);
      _userid = usr.UserIntId;
      _permid = -1;

      ((ObservableCollection<User>)_userViewSource.Source).ToList().ForEach(r => r.Granted = false);
      var ps = (ObservableCollection<Permission>)_permViewSource.Source;
      ps.ToList().ForEach(r => r.Granted = false);

      foreach (var pa in usr.PermissionAssignments)
      {
        var p = ps.FirstOrDefault(r => r.PermissionId == pa.PermissionId);
        if (p != null)
          p.Granted = true;
      }

      dgPerm.Items.Refresh();

      pfu.Text = $"{usr.UserId}  has  {usr.PermissionAssignments.Count}  permissions:";
      ufp.Text = $"· · ·";
    }

    void onSettings(object sender, RoutedEventArgs e) { }
    async void onAudio(object s, RoutedEventArgs e) { _audible = false; SystemSounds.Hand.Play(); await Task.Delay(300000); _audible = true; }
    void onWindowMinimize(object s, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = WindowState.Normal; }
    void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = WindowState.Maximized; }

    internal void Recalc(object s)
    {
      if (_userid > 0 && _permid < 0)
      {
        ufp.Text = $"· · ·";
        pfu.Text = $"{_context.Users.Local.FirstOrDefault(r => r.UserIntId == _userid)?.UserId}  has  {_context.Permissions.Local.Where(r => r.Granted == true).Count()}  permissions:";
      }
      else if (_userid < 0 && _permid > 0)
      {
        pfu.Text = $"· · ·";
        ufp.Text = $"{_context.Permissions.Local.FirstOrDefault(r => r.PermissionId == _permid)?.Name}  assigned to  {_context.Users.Local.Where(r => r.Granted == true).Count()}  users:";
      }
    }
  }
}
