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

namespace EfStoredProcWpfApp.Views
{
  public partial class DynamicSPUI : UserControl
  {
    readonly InventoryContext _context = new(@"Server=.\sqlexpress;Database=Inventory;Trusted_Connection=True;");
    public DynamicSPUI() => InitializeComponent();

    void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      wpEntry.Children.Add(new Label { Content = "@pGroup_ID",      /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pGroup_ID",      /**/ Text = $"2" });
      wpEntry.Children.Add(new Label { Content = "@pDateType",      /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pDateType",      /**/ Text = $"2" });
      wpEntry.Children.Add(new Label { Content = "@pStartDateInt",  /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pStartDateInt",  /**/ Text = $"20210404" });
      wpEntry.Children.Add(new Label { Content = "@pEndDateInt",    /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pEndDateInt",    /**/ Text = $"{DateTime.Today:yyyyDDmm}" });
      wpEntry.Children.Add(new Label { Content = "@pGroupName",     /**/}); wpEntry.Children.Add(new TextBox { Tag = "@pGroupName",     /**/ Text = $"2" });

      Button_Click(sender, e);
    }

    void Button_Click(object sender, RoutedEventArgs e)
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

      /*
      ///tu: https://stackoverflow.com/questions/51693286/no-mapping-to-a-relational-type-can-be-found-for-the-clr-type-int32/51693945
      ///tu: https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#string-interpolation-in-fromsql-and-executesqlcommand
      var city = "London";
      var cTtl = "Sales Representative";
      FormattableString sqli = $@" SELECT * FROM ""Customers"" WHERE ""City"" = {city} AND ""ContactTitle"" = {cTtl}";
      var sqls = sqli.ToString();
      var l1 = _context.Set<BookReportView2>().FromSqlInterpolated(sqli).ToArray();
      var l2 = _context.Set<BookReportView2>().FromSqlRaw(sqls).ToArray(); */
    }

    IEnumerable<dynamic> readTableDynamicly(string _sql) //todo: async System.Collections.Generic.IAsyncEnumerable<dynamic> readTableDynamicly(string _sql)
    {
      List<StoredProcDetail> rv = new();
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
  public class StoredProcDetail
  {
    private string _v1;
    private string _v2;
    private string _v3;

    public StoredProcDetail(string v1, string v2, string v3)
    {
      _v1 = v1;
      _v2 = v2;
      _v3 = v3;
    }
  }
  public class BookReportView2
  {
    public int BookId { get; set; }
    public string? Name { get; set; }
    public string? Currency { get; set; }
    public string? ReportingCurrency { get; set; }
    public string? AccountNum { get; set; }
    public string? DefMarket { get; set; }
    public string? ShortName { get; set; }
    public int StrategyType { get; set; }
    public int Active { get; set; }
    public string? Groups { get; set; }
  }
}
