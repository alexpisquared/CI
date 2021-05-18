﻿using CI.DS.ViewModel;
using CI.DS.ViewModel.VMs;
using CI.Standard.Lib.Extensions;
using DB.Inventory.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CI.DS.Visual.Views
{
  public partial class DynamicSPUICreatorView : UserControl
  {
    readonly InventoryContext _db = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;");
    StoredProcDetail? _spd;

    public DynamicSPUICreatorView() => InitializeComponent();

    async void onLoaded(object s, RoutedEventArgs e)
    {
      _spd = ((DynamicSPUICreatorVM)DataContext).StoredProcDetail;
      wpEntry.Children.Clear();
      var i = 0;
      foreach (var prm in _spd.Parameters.Split(',', StringSplitOptions.RemoveEmptyEntries))
      {
        var sp = new StackPanel();

        sp.Children.Add(new Label { Content = prm.Replace("@p", "").Replace("@", ""), });
        sp.Children.Add(new TextBox { Tag = prm, Text = $"{++i}" });

        wpEntry.Children.Add(sp);
      }

      await Task.Delay(99); focus0.Focus();

      //onRunSP(s, e);
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
      var sqlstring = "??";

      try
      {
        switch (mode)
        {
          case 1:
            sqlstring = $"{_spd?.SPName} "; // "usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName";
            foreach (var panel in wpEntry.Children)
            {
              if (panel is StackPanel spnl)
              {
                foreach (var tl in spnl.Children)
                {
                  if (tl is TextBox tbx)
                  {
                    var f = tbx.Tag.ToString()?.Split(' ').First();
                    sqlstring += $" {f},"; // $" '{tbx.Text}',";
                  }
                }
              }
            }
            var dynamicRows = readTableDynamicly(sqlstring.TrimEnd(',')); //todo: await foreach (var number in readTableDynamicly(spsql))      {        Console.WriteLine(number);      }
            tbkError.Text = $"{sqlstring.TrimEnd(',')}   ►   {dynamicRows.Count()} rows returned";
            tbkError.Foreground = dynamicRows.Count() > 0 ? Brushes.Green : Brushes.DarkOrange;
            dg1.ItemsSource = dynamicRows.ToDataTable().DefaultView;
            break;
          case 2:
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
                    var e = tbx.Tag.ToString()?.Split(' ');
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
            break;
          case 3:
            List<SqlParameter> spParams3 = new();
            sqlstring = $"EXEC @ReturnValue = {_spd?.Schema}.{_spd?.SPName}";
            foreach (var panel in wpEntry.Children)
            {
              if (panel is StackPanel spnl)
              {
                foreach (var tl in spnl.Children)
                {
                  if (tl is TextBox tbx)
                  {
                    var param = tbx.Tag.ToString()?.Split(' ') ?? new[] { "name", "type", "max_len", "precision", "0-in, 1-out" };
                    spParams3.Add(new SqlParameter
                    {
                      ParameterName = param.First(),
                      Value = tbx.Text,
                      DbType = param[1] == "int" ? DbType.Int32 : DbType.String,
                      SqlDbType = param[1] == "int" ? SqlDbType.Int : SqlDbType.NVarChar,
                      Direction = param.Last() == "0" ? ParameterDirection.Input : ParameterDirection.Output // .InputOutput is problematic
                    });
                    sqlstring += $" {param.First()}{(param?.Last() == "0" ? "" : " OUTPUT")},"; // $" '{tbx.Text}',";
                  }
                }
              }
            }

            spParams3.Add(new SqlParameter { ParameterName = "@ReturnValue", Direction = ParameterDirection.ReturnValue, Value = -1 });
            sqlstring = sqlstring.TrimEnd(',');

            var spNameOnly = sqlstring = $"{_spd?.Schema}.{_spd?.SPName}";
            var dynamcRows = readTableDynamicly_NoYield(spNameOnly, spParams3); //todo: await foreach (var number in readTableDynamicly(spsql))      {        Console.WriteLine(number);      }

            tbkError.Foreground = dynamcRows.Count() > 0 ? Brushes.Green : Brushes.Brown;
            tbkError.Text = $"{spNameOnly}   ►   {dynamcRows.Count()} rows returned \r\n\t";
            spParams3.Where(p => p.Direction != ParameterDirection.Input).ToList().ForEach(p => tbkError.Text += ($"{p.ParameterName} = {p.Value}   "));
            dg1.ItemsSource = dynamcRows.ToDataTable().DefaultView;
            break;
          default: break;
        }
      }
      catch (Exception ex)
      {
        tbkError.Text = $"{sqlstring}\r\n{ex.GetType().Name}: {ex.Message}";
        tbkError.Foreground = Brushes.Red;
      }
    }

    IEnumerable<dynamic> readTableDynamicly(string spSql) //todo: async System.Collections.Generic.IAsyncEnumerable<dynamic> readTableDynamicly(string _sql)
    {
      var connection = _db.Database.GetDbConnection();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
          connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = spSql;

        using var reader = cmd.ExecuteReader();
        if (!reader.HasRows)
          yield break;
        else
          while (reader.Read())
          {
            yield return GetDynamicData(reader);
          }

        reader.Close();
      }
      finally
      {
        connection.Close();
      }
    }
    IEnumerable<dynamic> readTableDynamicly_NoYield(string spSql, List<SqlParameter> sqlParameters) //todo: async System.Collections.Generic.IAsyncEnumerable<dynamic> readTableDynamicly(string _sql)
    {
      var connection = _db.Database.GetDbConnection();
      var rv = new List<dynamic>();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
          connection.Open();

        using var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = spSql;
        sqlParameters.ForEach(p => cmd.Parameters.Add(p));

        using var reader = cmd.ExecuteReader();
        if (reader.HasRows)
          while (reader.Read())
          {
            rv.Add(GetDynamicData(reader));
          }

        reader.Close();
      }
      finally
      {
        connection.Close();
      }

      return rv;
    }
    dynamic GetDynamicData(System.Data.Common.DbDataReader reader)
    {
      var expandoObject = new ExpandoObject() as IDictionary<string, object>;
      for (var i = 0; i < reader.FieldCount; i++)
      {
        expandoObject.Add(reader.GetName(i), reader[i]);
      }
      return expandoObject;
    }
  }
}