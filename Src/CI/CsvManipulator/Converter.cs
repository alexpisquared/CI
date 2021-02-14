using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvManipulator
{
  public class Converter
  {
    readonly string _filename0, _filename2;
    readonly List<CsvLine> _linesIn = new List<CsvLine>();
    readonly bool _ignoreHeaderColumnName = true; // for rare case scenario could be false.

    public Converter(string filename)
    {
      _filename0 = filename;
      var ext = Path.GetExtension(filename);
      _filename2 = filename.Replace(ext, $".~{ext}");
    }

    public string CleanEmptyRowsColumns()
    {
      var report = "haha";
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

        using var reader = new StreamReader(_filename0);
        using var csv = new CsvReader(reader, config);
        var linesIn = csv.GetRecords<dynamic>().ToList();
        var kvp = linesIn.FirstOrDefault();
        var colCnt = ((IDictionary<string, object>)kvp).Values.Count;
        var nonempties = new bool[colCnt];


        report = ($"Rows * Cols: {linesIn.Count,7} * {colCnt}\n");
        linesIn.Skip(_ignoreHeaderColumnName ? 1 : 0).ToList().ForEach(kvp =>
        {
          var c = 0;
          //rv+=($"Cols: {((IDictionary<string, object>)kvp).Values.Count,7}\n");
          foreach (var cell in ((IDictionary<string, object>)kvp).Values)
          {
            report += ($"  '{cell}',");

            if (!string.IsNullOrEmpty(cell?.ToString()))
              nonempties[c] = true;

            c++;
          }
          report += ($"\n");
        });

        nonempties.ToList().ForEach(r => report += (r ? "#" : "·"));        report += ($"\n");


        var linesOu = new List<dynamic>();

        linesIn.ToList().ForEach(kvp =>
        {
          var csvItem = new ExpandoObject() as IDictionary<string, object>;

          var c = 0;
          foreach (var column in kvp)
          {
            if (nonempties[c++])
            {
              csvItem.Add(column.Key, column.Value);
              report += ($"  '{column.Value}',");
            }
          }

          linesOu.Add(csvItem);
          report += ($"\n");
        });

        using (var writer = new StreamWriter(_filename2))
        {
          using var csv3 = new CsvWriter(writer, config);
          csv3.WriteRecords(linesOu);
        }

        return report;
      }
      catch (Exception ex) { report += (ex); throw; }

      //printTopLines();
    }

    void checkColumns()
    {
      using var reader1 = new StreamReader(_filename0);
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