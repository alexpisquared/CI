﻿using CI.GUI.Support.WpfLibrary.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RMSClient.Comm;
using RMSClient.Models;
using RMSClient.Models.RMS;
using RMSClient.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace RMSClient
{
  public partial class RmsClientMainWindow : WindowBase
  {
    readonly ILogger<RmsClientMainWindow> _logger;
    readonly IConfigurationRoot _config;
    readonly RMSContext _dbRMS;
    readonly CollectionViewSource _accountRequestViewSource;

    readonly AppSettings _appSettings;
    readonly string _constr;
    bool _loaded = false;

    public RmsClientMainWindow(ILogger<RmsClientMainWindow> logger, IConfigurationRoot config)
    {
      InitializeComponent();//DataContext = this;

      dt1.SelectedDate = DateTimeOffset.Now.Date.AddDays(-7);
      dt2.SelectedDate = DateTimeOffset.Now.Date;

      _accountRequestViewSource = (CollectionViewSource)FindResource(nameof(_accountRequestViewSource));

      //MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZVa += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZVa:{ZVa}"); }; //tu:
      //MouseLeftButtonDown += (s, e) => { try { DragMove(); } catch { logger.LogWarning("Ignore mouse complaints for now.- interfering with sockets?"); } };
      _logger = logger;
      _config = config;
      _appSettings = config.Get<AppSettings>();
#if DEBUG
      if (Environment.MachineName == "RAZER1") { Top = 1650; Left = 10; }
      else { Top = 1600; Left = 2500; }
      _constr = _appSettings.RmsDebug;
#else
      _constr = _appSettings.RmsRelease;
#endif
      _dbRMS = new RMSContext(_constr);


      themeSelector.ApplyTheme = ApplyTheme;
    }

    async Task find()
    {
      if (!_loaded) return;

      try
      {
        btnFind.Focus();
        vb1.Visibility = Visibility.Visible;
        const int top = 100;
        var sw = Stopwatch.StartNew();
        var acnt = string.IsNullOrEmpty(tbxAccount.Text) || tbxAccount.Text == "xxxxxxxxx" ? null : tbxAccount.Text;

#if DIRECT_RO_BINDING
        var sti = cbxOAF.SelectedIndex == 0 ? 1 : 7;
        var lst = _dbRMS.RmsDboRequestInvDboAccountViews.
          Where(r =>
            dt1.SelectedDate <= r.SendingTimeGmt && r.SendingTimeGmt <= dt2.SelectedDate.Value.AddDays(1) && (acnt == null || r.AdpaccountCode.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv")))
            && (cbxOAF.SelectedIndex == 1 || r.StatusId == sti)
            ).OrderBy(r => r.SendingTimeGmt);

        dg1.ItemsSource = await lst.Take(top).ToListAsync();
#elif RawSql
        var fullrv = _dbRM.RmsDboRequestInvDboAccountViews.Where(r => dt1.SelectedDate <= r.SendingTimeGmt && r.SendingTimeGmt <= dt2.SelectedDate);
        var report = $"Top {Math.Min(top, fullrv.Count())} rows out of {fullrv.Count()} matches found in ";
        DataContext = await fullrv.Take(top).ToListAsync();
#else
        await _dbRMS.RmsDboRequestInvDboAccountViews                             /**/.Where(r => /*r.TypeID!=5 &&*/ dt1.SelectedDate <= r.SendingTimeGmt && r.SendingTimeGmt <= dt2.SelectedDate && (acnt == null || r.AdpaccountCode.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv"))) && (cbxOAF.SelectedValue.ToString() == "All" || (r.Status == cbxOAF.SelectedValue.ToString()))).          LoadAsync();
        var l = _dbRMS.RmsDboRequestInvDboAccountViews.Local.ToObservableCollection().Where(r => /*r.TypeID!=5 &&*/ dt1.SelectedDate <= r.SendingTimeGmt && r.SendingTimeGmt <= dt2.SelectedDate && (acnt == null || r.AdpaccountCode.Contains(acnt)) && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv"))) && (cbxOAF.SelectedValue.ToString() == "All" || (r.Status == cbxOAF.SelectedValue.ToString())));
        _accountRequestViewSource.Source = l.Take(top);
#endif
        var cnt = await lst.CountAsync();
        var report = cnt <= top ? $"Total {cnt} matches found in " : $"Top {Math.Min(top, cnt),3}  rows out of {cnt,5}  matches found in ";

        Title = $"RMS Client ({Environment.UserName}) - {_dbRMS.Server()} - {report} {sw.Elapsed.TotalSeconds,5:N2} sec.";

        _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {Title}   params: {dt1.SelectedDate} - {dt2.SelectedDate}   {acnt}   {(cnkDirein.IsChecked == true ? "Direct Reinvest" : "")}");
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
      finally
      {
        vb1.Visibility = Visibility.Collapsed;
        tbxAccount.Focus();
      }
    }

    async void onLoaded(object s, RoutedEventArgs e)
    {
      _loaded = true;
      await find();
      //_db.Database.EnsureCreated();

      themeSelector.SetCurThemeToMenu(Thm);

#if DEBUG_UNIT_TEST
      onPopupPOC();
#endif
    }
    async void onDiRein(object s, RoutedEventArgs e) => await find();
    async void onDateCh(object s, SelectionChangedEventArgs e) => await find();
    async void onFind(object s, RoutedEventArgs e) => await find();
    async void onAccountChanged(object s, TextChangedEventArgs e)
    {
      if (!_loaded) return;

      var prev = tbxAccount.Text;
      await Task.Delay(555);
      if (prev == tbxAccount.Text)
        await find();
    }
    async void onSelect(object s, SelectedCellsChangedEventArgs e)
    {
      if (!_loaded) return;

      var requestID = ((RmsDboRequestInvDboAccountView)((Selector)s).SelectedValue).OrderId;
      await Task.Delay(555);
      if (requestID != ((RmsDboRequestInvDboAccountView)((Selector)s).SelectedValue).OrderId)
        return;

      var report = "Nothing yet ... ";
      Title = $"RMS Client ({Environment.UserName}) - looking for history ...";
      var sw = Stopwatch.StartNew();

      try
      {
        var l = _dbRMS.RequestHistories.Where(r => r.RequestId == requestID);
        dg2.ItemsSource = await l.ToListAsync();
        report = $"Total  {l.Count()}  historical entires found in ";
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
      finally
      {
        Title = $"RMS Client ({Environment.UserName}) - {report} {sw.Elapsed.TotalSeconds,5:N2} sec.";
        _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {Title}   ");
      }
    }
    async void onPopup(object s, MouseButtonEventArgs e)
    {
      try
      {
        var request = (RmsDboRequestInvDboAccountView)((Selector)s).SelectedValue;
        var dialogue = new ProcessOrderPopup
        {
          Owner = this,
          OrderId = request.OrderId,
          OrderStatus = request.OrderStatus,
          Symbol = request.Symbol,
          AdpaccountCode = request.AdpaccountCode,
          Quantity = request.Quantity,
          AvgPx = request.AvgPx
        };

        if (dialogue.ShowDialog() == true) //&& MessageBox.Show($"Sending new order status  {dialogue.NewOrderStatus}  \n\nwith note \n\n{dialogue.Note}\n\nto ...upstairs", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
#if DEBUG
          dialogue.Note += $"Test @ {DateTime.Now} - {dialogue.NewOrderStatus} - {dialogue.NewOrderAction} - {dialogue.Quantity} 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 ".Substring(0, 100);
#endif
          LoadStatuses();
          var _serverSession = new ServerSession(_logger);
          _serverSession.SetMainForm(this);
          _serverSession.Connect(_appSettings.IpAddress, _appSettings.Port);
          await Task.Delay(250);
          _serverSession.SendChangeRequest(request.OrderId, dialogue.NewOrderStatus.ToString(), (uint)(dialogue.Quantity ?? 0), dialogue.Note ?? "");
          SystemSounds.Beep.Play();
        }
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception in onPopup()", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    async void onPopupPOC()
    {
      try
      {
        var request = _dbRMS.RmsDboRequestInvDboAccountViews.First();
        var dialogue = new ProcessOrderPopup
        {
          Owner = this,
          OrderId = request.OrderId,
          OrderStatus = request.OrderStatus,
          Symbol = request.Symbol,
          AdpaccountCode = request.AdpaccountCode,
          Quantity = request.Quantity,
          AvgPx = request.AvgPx
        };

        LoadStatuses();
        var _serverSession = new ServerSession(_logger);
        _serverSession.SetMainForm(this);
        _serverSession.Connect(_appSettings.IpAddress, _appSettings.Port);
        await Task.Delay(250);
        _serverSession.SendChangeRequest(request.OrderId, OrderStatusEnum.Rejected.ToString(), (uint)request.Quantity, $"Test @ {DateTime.Now} - {dialogue.NewOrderStatus} - {dialogue.NewOrderAction} - {dialogue.Quantity} 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 ".Substring(0, 100));
        SystemSounds.Beep.Play();
        await Task.Delay(500);
        SystemSounds.Hand.Play();
        await Task.Delay(5000);
        Application.Current.Shutdown();
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    void onClip(object s, RoutedEventArgs e)
    {
      try
      {
        Clipboard.SetData(DataFormats.StringFormat, DataContext);
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    void onExit(object s, RoutedEventArgs e) => Close();
    void onWindowMinimize(object sender, RoutedEventArgs e) { WindowState = System.Windows.WindowState.Minimized; }
    void onWindowRestoree(object sender, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = System.Windows.WindowState.Normal; }
    void onWindowMaximize(object sender, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = System.Windows.WindowState.Maximized; }

    readonly Dictionary<string, ServerSession.RequestStatus> m_statusDict = new Dictionary<string, ServerSession.RequestStatus>();
    delegate void NewRequestDelegate(int n);
    void RefreshDataSelectRow(int requestID)
    {
      try
      {
        Task.Run(async () => await find());
        preSelectRequestRow(requestID);
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    void preSelectRequestRow(int requestID) { }
    void LoadStatuses() => _dbRMS.Statuses.ToList().ForEach(r => m_statusDict[r.Name] = (ServerSession.RequestStatus)r.StatusId);
    void OnNewRequestHandler(int n) => RefreshDataSelectRow(n);
    internal void OnNewRequest(int requestID)
    {
      var deleg = new NewRequestDelegate(OnNewRequestHandler);
      Dispatcher.Invoke(deleg, requestID);
    }
    internal ServerSession.RequestStatus GetStatusID(string statusName) => m_statusDict[statusName];

    protected override void OnClosing(CancelEventArgs e)
    {
      _dbRMS.Dispose();
      _dbRMS.Dispose();
      base.OnClosing(e);
    }
  }

  public enum OrderStatusEnum { Unknown, Done, PartialyDone, Rejected }; //from $\\trunk\Server\RMS\RMSClient\ChangeRequest.Designer.cs: "Received", "Rejected", "Cancelled", "PartialyDone", "Done"
  public enum OrderActionEnum { Unknown, SendUpdate, Acknowledge, UnlockOrder, Cancel };
}