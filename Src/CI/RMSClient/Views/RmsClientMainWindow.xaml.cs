using Microsoft.Data.SqlClient;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace RMSClient
{
  public partial class RmsClientMainWindow : Window
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

      MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZVa += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZVa:{ZVa}"); }; //tu:
      MouseLeftButtonDown += (s, e) => DragMove();
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
    }
    public static readonly DependencyProperty ZVaProperty = DependencyProperty.Register("ZVa", typeof(double), typeof(RmsClientMainWindow), new PropertyMetadata(1.25)); public double ZVa { get => (double)GetValue(ZVaProperty); set => SetValue(ZVaProperty, value); }

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

#if !DIRECT

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

        _logger.LogInformation($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {Title}   params: {dt1.SelectedDate} - {dt2.SelectedDate}   {acnt}   {(cnkDirein.IsChecked == true ? "Direct Reinvest" : "")}");
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

      LoadStatuses();
      ServerSession.Instance.SetMainForm(this);
      ServerSession.Instance.Connect(_appSettings.IpAddress, _appSettings.Port);
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
        _logger.LogInformation($" +{(DateTime.Now - App._started):mm\\:ss\\.ff}  {Title}   ");
      }
    }
    void onPopup(object s, MouseButtonEventArgs e)
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

        if (dialogue.ShowDialog() == true &&
          MessageBox.Show($"Sending new order status  {dialogue.NewOrderStatus}  \n\nwith note \n\n{dialogue.Note}\n\nto upstairs ...", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
          ServerSession.Instance.SendChangeRequest(request.OrderId, dialogue.NewOrderStatus.ToString(), (uint)(dialogue.Quantity ?? 0), dialogue.Note);
        }
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

    readonly Dictionary<string, ServerSession.RequestStatus> m_statusDict = new Dictionary<string, ServerSession.RequestStatus>();
    delegate void NewRequestDelegate(int n);
    void GetRequests(int requestID)
    {
      try
      {
        using var con = new SqlConnection(_constr);
        con.Open();
        using (var a = new SqlDataAdapter(@"select RequestID, CreatorID, r.SourceID, s.SourceName, AccountID, a.ShortName, r.TypeID, t.Name, r.SubtypeID, st.Name, r.ActionID, act.Name,
                            r.StatusID, stat.Name, CreationDate, SecADPNumber, CUSIP, Symbol, CurrencyCode, Quantity, DoneQty, Price, OtherInfo, BBSNote
                            from request r, source s, Inventory.dbo.Account a, RequestType t, Subtype st, [Action] act, [Status] stat
                            where r.SourceID = s.SourceID and
                            r.AccountID = a.Account_ID and
                            r.TypeID = t.TypeID and
                            r.TypeID = st.TypeID and
                            r.SubtypeID = st.SubtypeID and
                            r.typeID = act.typeID and
                            r.subtypeID = act.SubtypeID and
                            r.ActionID = act.ActionID and
                            r.StatusID = stat.StatusID and r.StatusID not in(4, 5, 7)", con))
        {
          var t = new DataTable();
          a.Fill(t);
          dg1.ItemsSource = t.Rows;

          if (requestID != 0)
          {
            for (var i = t.Rows.Count - 2; i >= 0; i--)
            {
              //select the row:
              //if (Convert.ToInt32(dg1.rows.Rows[i].Cells[0].Value.ToString()) == requestID)
              //{
              //  dg1.CurrentCell = dg1.Rows[i].Cells[0];
              break;
              //}
            }
          }
        }
      }
      catch (Exception ex) { _logger.LogError($"{ex}"); MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    void LoadStatuses()
    {
      using (var con = new SqlConnection(_constr))
      {
        con.Open();
        var command = new SqlCommand("select statusID, name from [Status]", con);
        var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
          while (reader.Read())
          {
            m_statusDict[reader.GetString(1)] = (ServerSession.RequestStatus)reader.GetInt32(0);
          }
        }
      }
    }
    void OnNewRequestHandler(int n) => GetRequests(n);
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