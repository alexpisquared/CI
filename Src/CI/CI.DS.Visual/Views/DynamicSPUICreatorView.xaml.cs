using CI.DS.ViewModel;
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
    readonly InventoryContext _context = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;");
    StoredProcDetail? _spd;

    public DynamicSPUICreatorView() => InitializeComponent();

    async void onLoaded(object sender, RoutedEventArgs e)
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

      //onRunSP(sender, e);
    }
    void onShowSP(object s, RoutedEventArgs e) => MessageBox.Show(_spd?.Definition, _spd?.SPName);
    async void onRunSP(object sender, RoutedEventArgs e)
    {
      tbkError.Text = $"Launching ...";
      tbkError.Foreground = Brushes.Blue;
      await Task.Delay(100);

      var spsql = $"{_spd?.SPName} "; // "usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName";

      try
      {
        List<SqlParameter> spParams = new();
        foreach (var panel in wpEntry.Children)
        {
          if (panel is StackPanel spnl)
          {
            foreach (var tl in spnl.Children)
            {
              if (tl is TextBox tbx)
              {
                spParams.Add(new SqlParameter(tbx.Tag.ToString(), tbx.Text)); //todo:
                spsql += $" '{tbx.Text}',";
              }
            }
          }
        }

        //spsql += $"@ReturnValue OUTPUT";

        Debug.WriteLine($" ■ ReturnValue = .   ■ ■ ■ ■ ■ ■ ■ ■ ■ ■ ■ ");
        var dynamicRows = readTableDynamicly(spsql.TrimEnd(',')); //todo: await foreach (var number in readTableDynamicly(spsql))      {        Console.WriteLine(number);      }
        Debug.WriteLine($" ■ ReturnValue = .   ■ ■ ■ ■ ■ ■ ■ ■ ■ ■ ■ ");

        tbkError.Text = $"{spsql.TrimEnd(',')}   ►   {dynamicRows.Count()} rows returned";
        tbkError.Foreground = dynamicRows.Count() > 0 ? Brushes.Green : Brushes.DarkOrange;

        dg1.ItemsSource = dynamicRows.ToDataTable().DefaultView;
      }
      catch (Exception ex)
      {
        tbkError.Text = $"{spsql.TrimEnd(',')}\r\n{ex.GetType().Name}: {ex.Message}";
        tbkError.Foreground = Brushes.DarkRed;
      }
    }

    IEnumerable<dynamic> readTableDynamicly(string spSql) //todo: async System.Collections.Generic.IAsyncEnumerable<dynamic> readTableDynamicly(string _sql)
    {
      var returnParam = new SqlParameter
      {
        ParameterName = "@ReturnValue",
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output
      };
      Debug.WriteLine($" ■ ReturnValue = {returnParam.Value}.   4 ■ ■ ■ ■ ■");

      var connection = _context.Database.GetDbConnection();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
        {
          connection.Open();
        }

        using var cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = spSql;

        //var added = cmd.Parameters.Add(returnParam);

        using var reader = cmd.ExecuteReader();
        if (!reader.HasRows)
        {
          Debug.WriteLine($" ■ ReturnValue = {returnParam.Value}.   3 ■ ■ ■ ■   (No rows found)  ");
          yield break;
        }
        else
        {
          while (reader.Read())
          {
            yield return GetDynamicData(reader);
          }
        }

        Debug.WriteLine($" ■ ReturnValue = {returnParam.Value}.   2 ■ ■ ■");
        reader.Close();
        Debug.WriteLine($" ■ ReturnValue = {returnParam.Value}.   1 ■ ■");
      }
      finally
      {
        connection.Close();
        Debug.WriteLine($" ■ ReturnValue = {returnParam.Value}.   0 ■");
      }
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
