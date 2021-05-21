using CI.DS.ViewModel;
using CI.DS.ViewModel.Helpers;
using CI.DS.ViewModel.VMs;
using CI.Standard.Lib.Extensions;
using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CI.DS.Visual.Views
{
  public partial class SPParamManagerView : UserControl
  {
    readonly InventoryContext _db = new(@"Server=mtUATsqldb;Database=Inventory;Trusted_Connection=True;");
    SpdAdm? _spd;
    SqlSpRuner _runner = new();
    bool _isdbg = false;

    public SPParamManagerView() => InitializeComponent();

    async void onLoaded(object s, RoutedEventArgs e)
    {
#if DEBUG
      _isdbg = true;
#endif
      _spd = ((SPParamManagerVM)DataContext).SpdAdm;
      wpEntry.Children.Clear();
      if (_spd is null) return;

      var i = 0;
      foreach (var prm in _spd.Parameters.Split(',', StringSplitOptions.RemoveEmptyEntries))
      {
        var sp = new StackPanel();
        var spl = prm.Split(' ');
        bool v = int.TryParse(spl[2], out int width);
        width = 20 + (v && width > 40 ? 260 : width * 10);

        sp.Children.Add(new Label { Content = _isdbg ? prm : spl.First().Replace("@p", "").Replace("@", "").ToSentence(), });
        sp.Children.Add(new TextBox
        {
          Tag = prm,
          Width = width,
          Text = $"{(_isdbg ? ++i : "")}"
        });

        wpEntry.Children.Add(sp);
      }

      await Task.Delay(99); focus0.Focus();
    }
    void onShowSP(object s, RoutedEventArgs e) => MessageBox.Show(_spd?.Definition, _spd?.SPName);
    async void onGetDataSet(object s, RoutedEventArgs e) => await runSP(1);
    async void onUpdateOnly(object s, RoutedEventArgs e) => await runSP(2);
    async void onUpdateOnl0(object s, RoutedEventArgs e) => await runSP(3);

    async Task runSP(int mode)
    {
      tbkError.Text = $"Launching ...";
      tbkError.Foreground = Brushes.Blue;
      await Task.Delay(75);
      var commandText = "??";

      try
      {
        switch (mode)
        {
          case 1:
            commandText = _spd?.SPName ?? "Sorry, that is odd....";
            wpEntry.Children.OfType<StackPanel>().ToList().ForEach(stackPanel => stackPanel.Children.OfType<TextBox>().ToList().ForEach(tbx => commandText += $" {tbx.Text},"));

            var dynamicRows = _runner.ExecuteReader(_db, commandText = commandText.TrimEnd(',')); //todo: await foreach (var number in readTableDynamicly(spsql))      {        Console.WriteLine(number);      }
            tbkError.Text = $"{commandText}   ►   {dynamicRows.Count()} rows returned";
            tbkError.Text = $"Rows returned  {dynamicRows.Count()} \r\n";
            tbkError.Foreground = dynamicRows.Count() > 0 ? Brushes.Green : Brushes.DarkOrange;
            dg1.ItemsSource = dynamicRows.ToDataTable().DefaultView;
            break;
          case 2: commandText = execSqlRaw(); break;
          case 3:
            List<SqlParameter> spParams = new();
            wpEntry.Children.OfType<StackPanel>().ToList().ForEach(stackPanel => stackPanel.Children.OfType<TextBox>().ToList().ForEach(tbx => addSqlParam(spParams, tbx)));
            spParams.Add(new SqlParameter { ParameterName = "@ReturnValue", Direction = ParameterDirection.ReturnValue, Value = -1 });

            commandText = $"{_spd?.Schema}.{_spd?.SPName}";
            var dynamcRows = _runner.ExecuteReader(_db, commandText, spParams); //todo: await foreach (var number in readTableDynamicly(spsql))      {        Console.WriteLine(number);      }

            tbkError.Foreground = dynamcRows.Count() > 0 ? Brushes.Green : Brushes.Brown;
            tbkError.Text = $"Rows returned  {dynamcRows.Count()} \r\n";
            spParams.Where(p => p.Direction != ParameterDirection.Input).ToList().ForEach(p => tbkError.Text += ($"{p.ParameterName.ToSentence()} = {p.Value}   "));
            dg1.ItemsSource = dynamcRows.ToDataTable().DefaultView;
            break;
          default: break;
        }
      }
      catch (Exception ex)
      {
        tbkError.Text = $"{_spd?.Schema}.{_spd?.SPName}\r\n{ex.GetType().Name}: {ex.Message}";
        tbkError.Foreground = Brushes.Red;
      }
    }

    string execSqlRaw()
    {
      string sqlstring;
      List<SqlParameter> spParams2 = new();
      sqlstring = $"EXEC @ReturnValue = {_spd?.Schema}.{_spd?.SPName}";
      foreach (var panel in wpEntry.Children)
      {
        if (panel is StackPanel spnl)
        {
          foreach (var tl in spnl.Children)
          {
            if (tl is TextBox tbx)
            {
              var e = tbx.Tag.ToString()?.Split(' ') ?? new[] { "name", "type", "max_len", "precision", "0-in, 1-out" };
              var paramName = e.First();
              var direction = e.Last() == "0" ? ParameterDirection.Input : ParameterDirection.Output;
              var outputStr = direction == ParameterDirection.Output ? " OUTPUT" : "";
              spParams2.Add(new SqlParameter { ParameterName = paramName, Value = tbx.Text, Direction = direction });
              sqlstring += $" {paramName}{outputStr},"; // $" '{tbx.Text}',";
            }
          }
        }
      }

      spParams2.Add(new SqlParameter { ParameterName = "@ReturnValue", Value = -1, Direction = ParameterDirection.Output });
      sqlstring = sqlstring.TrimEnd(',');

      var rowsAffected = _db.Database.ExecuteSqlRaw(sqlstring, spParams2.ToArray()); // Modify Data Using a Stored Procedure -- https://www.codemag.com/Article/2101031/Calling-Stored-Procedures-with-the-Entity-Framework-in-.NET-5Modify Data Using a Stored Procedure -- https://www.codemag.com/Article/2101031/Calling-Stored-Procedures-with-the-Entity-Framework-in-.NET-5

      tbkError.Foreground = Brushes.DarkCyan;
      tbkError.Text = $"Rows Affected:{rowsAffected}   ...OUTPUTs: \r\n\t";
      spParams2.Where(p => p.Direction == ParameterDirection.Output).ToList().ForEach(p => tbkError.Text += ($"{p.ParameterName} = {p.Value}   "));
      return sqlstring;
    }

    void addSqlParam(List<SqlParameter> spParams, TextBox tbx) => _runner.AddSqlParam(spParams, tbx.Tag.ToString()?.Split(' ') ?? new[] { "name", "type", "max_len", "precision", "0-in, 1-out" }, tbx.Text);
  }
}