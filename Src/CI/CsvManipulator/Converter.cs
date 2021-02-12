using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace CsvManipulator
{
  public class Converter
  {
    readonly string _filename;
    readonly List<CsvLine> _linesIn = new List<CsvLine>();

    public Converter(string filename) => _filename = filename;
    public void CleanEmptyRowsColumns()
    {
      try
      {
        //checkColumns();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          HasHeaderRecord = false, // !!! otherwise must have unique non-empty column names
          //HeaderValidated = null,
          MissingFieldFound = null,
          IgnoreBlankLines = true,
          TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(_filename);
        using var csv = new CsvReader(reader, config);
        var linesIn = csv.GetRecords<dynamic>().ToList();
        var kvp = linesIn.FirstOrDefault();
        var colCnt = ((IDictionary<string, object>)kvp).Values.Count;
        bool[] nonempties = new bool[colCnt];
        

        Debug.WriteLine($"Rows * Cols: {linesIn.Count,7} * {colCnt}");
        linesIn.ToList().ForEach(kvp =>
        {
          var c = 0;
          //Debug.WriteLine($"Cols: {((IDictionary<string, object>)kvp).Values.Count,7}");
          foreach (var cell in ((IDictionary<string, object>)kvp).Values)
          {
            Debug.Write($"  '{cell}',");

            if (!string.IsNullOrEmpty(cell?.ToString()))
              nonempties[c] = true;
            c++;
          }
          Debug.WriteLine($"");          
        });

        nonempties.ToList().ForEach(r => Debug.Write(r ? "#" : "·"));
      }
      catch (Exception ex) { Debug.WriteLine(ex); throw; }

      //printTopLines();
    }

    void checkColumns()
    {
      using var reader1 = new StreamReader(_filename);
      var firstLine = reader1.ReadLine() ?? "";

      dynamic eo = new ExpandoObject();
      var csvDynaObj = (IDictionary<string, object>)eo;
      foreach (var column in firstLine.Split(',', StringSplitOptions.RemoveEmptyEntries))
      {
        csvDynaObj.Add(column, "");
      }

    }

    void printTopLines(int top = 4)
    {
      Debug.WriteLine($" - Import done: {_linesIn.Count} lines imported!  Showing the 1st {top} rows:");
      _linesIn.Take(top).ToList().ForEach(l => Debug.WriteLine(l));
    }
  }
}
///C:\OI\SSCNETSettlementExtract\CsvToJsonConverter\CsvToJsonConverter.csproj