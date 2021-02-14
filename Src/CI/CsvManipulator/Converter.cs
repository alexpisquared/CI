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
      var report = "";
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
        var allCsvRecords = csv.GetRecords<dynamic>().ToList();
        var columnCount = ((IDictionary<string, object>)allCsvRecords.FirstOrDefault()).Values.Count;

        var nonEmptyRows = allCsvRecords.Skip(_ignoreHeaderColumnName ? 1 : 0).ToList().Where(kvp => ((ExpandoObject)kvp).Any(v => !string.IsNullOrEmpty(v.Value?.ToString())));
        report += ($"Rows * Cols: {nonEmptyRows.Count(),7} / {allCsvRecords.Count(),-7} * {columnCount}\n");

        var ecrv = findEmptyColumns(columnCount, nonEmptyRows);
        report += ecrv.report;

        report += ($"Empty columns:  ");
        ecrv.ec.ToList().ForEach(r => report += (r ? "#" : "·"));
        report += ($"\n");

        var ourv = removeEmptyColumns(nonEmptyRows, ecrv);
        report += ourv.report;

        using (var writer = new StreamWriter(_filename2))
        {
          using var csv3 = new CsvWriter(writer, config);
          csv3.WriteRecords(ourv.ou);
        }
      }
      catch (Exception ex) { report += (ex); }

      return report;
      //printTopLines();
    }

    static (List<dynamic> ou, string report) removeEmptyColumns(IEnumerable<dynamic> nonEmptyRows, (bool[] ec, string report) emptyColumns)
    {
      var report = "";
      var outCsvRecords = new List<dynamic>();

      nonEmptyRows.ToList().ForEach(kvp =>
      {
        var outCsvRecord = new ExpandoObject() as IDictionary<string, object>;

        var c = 0;
        foreach (var column in kvp)
        {
          if (emptyColumns.ec[c++])
          {
            outCsvRecord.Add(column.Key, column.Value);
            report += ($"  '{column.Value}'\t");
          }
        }

        outCsvRecords.Add(outCsvRecord);
        report += ($"\n");
      });

      return (outCsvRecords, report); ;
    }

    (bool[] ec, string report) findEmptyColumns(int colCnt, IEnumerable<dynamic> rows)
    {
      var report = "";
      var nonemptyColumns = new bool[colCnt];
      rows.Skip(_ignoreHeaderColumnName ? 1 : 0).ToList().ForEach(kvp =>
      {
        var c = 0;
        foreach (var cell in ((IDictionary<string, object>)kvp).Values)
        {
          report += ($"  '{cell}'\t");

          if (!string.IsNullOrEmpty(cell?.ToString()))
            nonemptyColumns[c] = true;

          c++;
        }
        report += ($"\n");
      });

      return (nonemptyColumns, report);
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