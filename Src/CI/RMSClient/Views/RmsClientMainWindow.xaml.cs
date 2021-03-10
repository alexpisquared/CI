﻿using AsyncSocketLib;
using AsyncSocketLib.CI.Model;
using CI.GUI.Support.WpfLibrary.Base;
using CI.GUI.Support.WpfLibrary.Extensions;
using CI.GUI.Support.WpfLibrary.Helpers;
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
        bool _loaded = false;
        string _isoFilenameONLY = $"{nameof(AppSettings)}.xml";
        readonly Dictionary<string, ServerSession.RequestStatus> m_statusDict = new Dictionary<string, ServerSession.RequestStatus>();
        delegate void NewRequestDelegate(int n);
        const int _delay = 50;
        public RmsClientMainWindow(ILogger<RmsClientMainWindow> logger, IConfigurationRoot config)
        {
            InitializeComponent(); //DataContext = this;

            dt1.SelectedDate = DateTimeOffset.Now.Date.AddDays(-7);
            dt2.SelectedDate = DateTimeOffset.Now.Date;

            _accountRequestViewSource = (CollectionViewSource)FindResource(nameof(_accountRequestViewSource));

            _logger = logger;
            _config = config;

            _appSettings = XmlIsoFileSerializer.Load<AppSettings>(_isoFilenameONLY); // = new AppSettings { Port = ushort.Parse(aps.FirstOrDefault(r => r.Key == nameof(AppSettings.Port))?.Value ?? "6756"), IpAddress = aps.FirstOrDefault(r => r.Key == nameof(AppSettings.IpAddress))?.Value ?? "10.10.19.152", RmsDbConStr = aps.FirstOrDefault(r => r.Key == "RmsDbConStr")?.Value, RmsDbConStr = aps.FirstOrDefault(r => r.Key == "RmsDbConStr")?.Value };      //var aps = config.GetSection("AppSettings").GetChildren();

#if DEBUG
            _dbRMS = new RMSContext(_appSettings.RmsDbConStr);
#else
      _dbRMS = new RMSContext(_appSettings.RmsDbConStr);
      Topmost = false;
#endif

            Title = _config["WhereAmI"];

            themeSelector.ApplyTheme = ApplyTheme;
        }
        public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(double), typeof(RmsClientMainWindow), new PropertyMetadata(.0)); public double Blur { get { return (double)GetValue(BlurProperty); } set { SetValue(BlurProperty, value); } }
        async Task find()
        {
            if (!_loaded) return;

            try
            {
                btnFind.Focus();
                Blur = 5;
                vb1.Visibility = Visibility.Visible;
                await Task.Delay(999);
                const int top = 100;
                var sw = Stopwatch.StartNew();
                var acnt = string.IsNullOrEmpty(tbxAccount.Text) || tbxAccount.Text == "xxxxxxxxx" ? null : tbxAccount.Text;

#if DIRECT_RO_BINDING
                const int outstg = 0, allord = 1, filled = 2;
                var lst = _dbRMS.RmsDboRequestInvDboAccountViews.
                  Where(r =>
                    dt1.SelectedDate <= r.SendingTimeGmt && r.SendingTimeGmt <= (dt2.SelectedDate ?? DateTime.Now).AddDays(1)
                    && (acnt == null || r.AdpaccountCode.Contains(acnt))
                    && (cnkDirein.IsChecked != true || (r.OtherInfo != null && r.OtherInfo.Contains("einv")))
                    && (
                        (cbxOAF.SelectedIndex == allord) ||
                        (cbxOAF.SelectedIndex == outstg && r.StatusId != (int)OrderStatus.rsDone && r.StatusId != (int)OrderStatus.rsRejected && r.StatusId != (int)OrderStatus.rsCancelled) ||
                        (cbxOAF.SelectedIndex == filled && r.StatusId == (int)OrderStatus.rsDone)
                    )
                  ).OrderBy(r => r.SendingTimeGmt);

                Debug.WriteLine(lst.ToQueryString());

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

                Title = $"({Environment.UserName}) - {_dbRMS.Server()} - {report} {sw.Elapsed.TotalSeconds,5:N2} sec.";

                _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {Title}   params: {dt1.SelectedDate} - {dt2.SelectedDate}   {acnt}   {(cnkDirein.IsChecked == true ? "Direct Reinvest" : "")}");
            }
            catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
            finally
            {
                vb1.Visibility = Visibility.Collapsed;
                Blur = 0;
                tbxAccount.Focus();
            }
        }
        async void onLoaded(object s, RoutedEventArgs e)
        {
            _loaded = true;
            await find();       //todo: _db.Database.EnsureCreated();

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
            if (!_loaded || ((RmsDboRequestInvDboAccountView)((Selector)s)?.SelectedValue)?.OrderId == null) return;

            var requestID = ((RmsDboRequestInvDboAccountView)((Selector)s)?.SelectedValue)?.OrderId;
            await Task.Delay(555);
            if (requestID != ((RmsDboRequestInvDboAccountView)((Selector)s)?.SelectedValue)?.OrderId)
                return;

            var report = "Nothing yet ... ";
            Title = $"({Environment.UserName}) - looking for history ...";
            var sw = Stopwatch.StartNew();

            try
            {
                var l = _dbRMS.RequestHistories.Where(r => r.RequestId == requestID);
                dg2.ItemsSource = await l.ToListAsync();
                report = $"Total  {l.Count()}  historical entires found in ";
            }
            catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
            finally
            {
                Title = $"({Environment.UserName}) - {report} {sw.Elapsed.TotalSeconds,5:N2} sec.";
                _logger.LogInformation($" +{(DateTime.Now - App.Started):mm\\:ss\\.ff}  {Title}   ");
            }
        }
        async void onPopup(object s, MouseButtonEventArgs e)
        {
            try
            {
                Blur = 5;
                var request = (RmsDboRequestInvDboAccountView)((Selector)s).SelectedValue;
                var dialogue = new ProcessOrderPopup
                {
                    Owner = this,
                    OrderId = request.OrderId,
                    OrderStatus = request.OrderStatus,
                    Symbol = request.Symbol,
                    AdpaccountCode = request.AdpaccountCode,
                    Quantity = request.Quantity ?? 0,
                    AvgPx = request.AvgPx ?? 0d
                };

                if (dialogue.ShowDialog() == true)
                {
#if DEBUG
                    dialogue.Note += $"Test @ {DateTime.Now:yMMdd.HHmm} - {dialogue.NewOrderStatus} - {dialogue.NewOrderAction} - {dialogue.Quantity} 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789 123456789■";
                    if (dialogue.Note.Length > 100)
                        dialogue.Note = dialogue.Note.Substring(0, 100);
#endif
                    var client = new AsynchronousClient();
                    var rv = await client.SendChangeRequest(_appSettings.IpAddress, _appSettings.Port, Environment.UserName, request.OrderId, dialogue.NewOrderStatus, dialogue.Quantity, dialogue.AvgPx, dialogue.Note ?? "");
                    SystemSounds.Beep.Play();
                    MessageBox.Show($"Result:\n\n\t{client.ChangeResponse.m_code}", "Under Construction", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    if (client.ChangeResponse.m_code == ResponseCode.rcOK)
                        await find();
                }
            }
            catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
            finally { Blur = 0; }
        }

        void onEditAppSettings(object s, RoutedEventArgs e)
        {
            try
            {
                Blur = 5;
                var w = new AppSettingsEditor(_appSettings, this);
                if (w.ShowDialog() == true)
                {
                    XmlIsoFileSerializer.Save(_appSettings, _isoFilenameONLY);
                }
            }
            finally
            {
                Blur = 0;
            }
        }
        void onClip(object s, RoutedEventArgs e)
        {
            try
            {
                var tabDelimetedValues = string.Join("\r\n", ((List<RmsDboRequestInvDboAccountView>)dg1.ItemsSource));
                Clipboard.SetText(tabDelimetedValues);
                _logger.LogInformation(tabDelimetedValues);
                SystemSounds.Beep.Play();
            }
            catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
        }
        void onExit(object s, RoutedEventArgs e) => Close();
        void onWindowMinimize(object s, RoutedEventArgs e) => WindowState = System.Windows.WindowState.Minimized;
        void onWindowRestoree(object s, RoutedEventArgs e) { wr.Visibility = Visibility.Collapsed; wm.Visibility = Visibility.Visible; WindowState = System.Windows.WindowState.Normal; }
        void onWindowMaximize(object s, RoutedEventArgs e) { wm.Visibility = Visibility.Collapsed; wr.Visibility = Visibility.Visible; WindowState = System.Windows.WindowState.Maximized; }
        void RefreshDataSelectRow(int requestID)
        {
            try
            {
                Task.Run(async () => await find());
                preSelectRequestRow(requestID);
            }
            catch (Exception ex) { _logger.LogError($"{ex}"); ex.Pop(this); }
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

    public enum OrderStatusEnum { Unknown, Done, PartialyDone, Rejected }; // $\\trunk\Server\RMS\RMSClient\ChangeRequest.Designer.cs: "Received", "Rejected", "Cancelled", "PartialyDone", "Done"
    public enum OrderActionEnum { Unknown, SendUpdate, Acknowledge, UnlockOrder, Cancel };
    public enum OrderStatus : int // $\\trunk\Server\RMS\Common\RMSMessage.h
    {
        rsNotSet = 0,
        rsSent = 1,
        rsReceived = 2,
        //rsProcessing = 3,
        rsRejected = 4,
        rsCancelled = 5,
        rsPartialyDone = 6,
        rsDone = 7,
        rsCancelRequested = 8
    };
}