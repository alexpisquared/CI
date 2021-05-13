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

namespace CI.DS.Visual.Views
{
  public partial class DynamicSPUICreatorView : UserControl
  {
    readonly InventoryContext _context = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;");
    public DynamicSPUICreatorView()
    {
      InitializeComponent();
    }

    void onLoaded(object sender, RoutedEventArgs e)
    {
      wpEntry.Children.Add(new Label { Content = "@pGroup_ID",      /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pGroup_ID",      /**/ Text = $"2" });
      wpEntry.Children.Add(new Label { Content = "@pDateType",      /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pDateType",      /**/ Text = $"2" });
      wpEntry.Children.Add(new Label { Content = "@pStartDateInt",  /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pStartDateInt",  /**/ Text = $"20210404" });
      wpEntry.Children.Add(new Label { Content = "@pEndDateInt",    /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pEndDateInt",    /**/ Text = $"{DateTime.Today:yyyyDDmm}" });
      wpEntry.Children.Add(new Label { Content = "@pGroupName",     /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pGroupName",     /**/ Text = $"2" });

      onRunSP(sender, e);
    }
    void onRunSP(object sender, RoutedEventArgs e)
    {
      List<SqlParameter> spparams = new();
      foreach (var control in wpEntry.Children)//.ToList().Where(r=>r is TextBox))
      {
        if (control is TextBox tbx)
        {
          spparams.Add(new SqlParameter(tbx.Tag.ToString(), tbx.Text));
        }
      }

      var spsql = "usp_Report_AllCashByGroup @pGroup_ID, @pDateType, @pStartDateInt, @pEndDateInt, @pGroupName";
      spsql = "usp_Report_AllCashByGroup '2', '2', '2', '2', '2'";

      var rows = readTableDynamicly(spsql); //todo: await foreach (var number in readTableDynamicly(spsql))      {        Console.WriteLine(number);      }

      dg1.ItemsSource = rows.ToDataTable().DefaultView;
      tbkError.Text = $"{rows.Count()} rows found";
    }

    IEnumerable<dynamic> readTableDynamicly(string _sql) //todo: async System.Collections.Generic.IAsyncEnumerable<dynamic> readTableDynamicly(string _sql)
    {
      System.Data.Common.DbConnection? connection = _context.Database.GetDbConnection();

      try
      {
        if (connection.State.Equals(ConnectionState.Closed))
        {
          connection.Open();
        }

        using System.Data.Common.DbCommand command = connection.CreateCommand();
        command.CommandText = _sql;

        using System.Data.Common.DbDataReader? reader = command.ExecuteReader();
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
      for (int i = 0; i < reader.FieldCount; i++)
      {
        expandoObject.Add(reader.GetName(i), reader[i]);
      }
      return expandoObject;
    }
  }
}
