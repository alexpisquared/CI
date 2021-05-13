using CI.DS.ViewModel;
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
using System.Windows;
using System.Windows.Controls;

namespace CI.DS.Visual.Views
{
  public partial class DynamicSPUICreatorView : UserControl
  {
    readonly InventoryContext _context = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;");
    StoredProcDetail _spd;

    public DynamicSPUICreatorView() => InitializeComponent();

    void onLoaded(object sender, RoutedEventArgs e)
    {
      _spd = ((CI.DS.ViewModel.VMs.DynamicSPUICreatorVM)DataContext).StoredProcDetail;
      wpEntry.Children.Clear();
      var i = 0;
      foreach (var prm in _spd.Parameters.Split(','))
      {
        var sp = new StackPanel();

        sp.Children.Add(new Label { Content = prm.Replace("@p", "").Replace("@", ""), });
        sp.Children.Add(new TextBox { Tag = prm, Text = $"{++i}" });

        wpEntry.Children.Add(sp);
      }

      onRunSP(sender, e);
    }
    void onRunSP(object sender, RoutedEventArgs e)
    {
      var spsql = "usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName";
      spsql = $"{_spd.SPName} '2', '2', '2', '2', '2'";
      spsql = $"{_spd.SPName} ";

      try
      {
        List<SqlParameter> spparams = new();
        foreach (var control in wpEntry.Children)//.ToList().Where(r=>r is TextBox))
        {
          if (control is StackPanel spnl)
          {
            foreach (var tl in spnl.Children)
            {
              if (tl is TextBox tbx)
              {
                spparams.Add(new SqlParameter(tbx.Tag.ToString(), tbx.Text));
                spsql += $" '{tbx.Text}',";
              }
            }
          }
        }

        var rows = readTableDynamicly(spsql.TrimEnd(',')); //todo: await foreach (var number in readTableDynamicly(spsql))      {        Console.WriteLine(number);      }

        dg1.ItemsSource = rows.ToDataTable().DefaultView;
        tbkError.Text = $"{spsql}   ►   {rows.Count()} rows found";
      }
      catch (Exception ex)
      {
        tbkError.Text = $"{spsql}\r\n{ex.Message}";
      }
    }

    IEnumerable<dynamic> readTableDynamicly(string _sql) //todo: async System.Collections.Generic.IAsyncEnumerable<dynamic> readTableDynamicly(string _sql)
    {
      var connection = _context.Database.GetDbConnection();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
        {
          connection.Open();
        }

        using var command = connection.CreateCommand();
        command.CommandText = _sql;

        using var reader = command.ExecuteReader();
        if (!reader.HasRows)
        {
          Debug.WriteLine("No rows found.");
        }
        else
        {
          while (reader.Read())
          {
            yield return GetDynamicData(reader);
          }
        }

        reader.Close();
      }
      //catch (SqlNullValueException ex) { _logger.LogError(ex.ToString()); }
      //catch (Exception ex) { Debug.WriteLine($"  ■ ■ ■ {ex.Message}"); }
      finally { connection.Close(); }
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
