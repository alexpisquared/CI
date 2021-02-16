using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvManipulator
{
  public class CsvConverter : ICsvConverter
  {
    readonly string _filename0, _filename2;
    readonly List<CsvLine> _linesIn = new List<CsvLine>();
    readonly bool _ignoreHeaderColumnName = true; // for rare case scenario could be false.

    public CsvConverter(string filename)
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
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          HasHeaderRecord = false, // !!! otherwise must have unique non-empty column names
          //HeaderValidated = null,
          MissingFieldFound = null,
          IgnoreBlankLines = true,
          TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(_filename0);
        using var csvrdr = new CsvReader(reader, config);
        var allCsvRecords = csvrdr.GetRecords<dynamic>().ToList();
        var allCsvHeaders = ((IDictionary<string, object>)allCsvRecords.FirstOrDefault()).Values;
        var columnCount = allCsvHeaders.Count;

        var nonEmptyRows = allCsvRecords.Skip(_ignoreHeaderColumnName ? 1 : 0).ToList().Where(kvp => ((ExpandoObject)kvp).Any(v => !string.IsNullOrEmpty(v.Value?.ToString())));
        report += ($"Rows * Cols: {nonEmptyRows.Count(),7} / {allCsvRecords.Count,-7} * {columnCount}\n");

        foreach (var item in allCsvHeaders)
        {
          report += $"  {item}\t";
        }
        report += ($"\n");

        var ecrv = findEmptyColumns(columnCount, nonEmptyRows);
        report += ecrv.tnr;

        report += ($"Empty columns:  ");
        ecrv.ecf.ToList().ForEach(r => report += (r ? "#" : "·"));
        report += ($"\n");

        var ourv = removeEmptyColumns(nonEmptyRows, ecrv);
        report += ourv.report;

        using var writer = new StreamWriter(_filename2);
        using var csv3 = new CsvWriter(writer, config);
        csv3.WriteRecords(ourv.ou);
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

    static (bool[] ecf, string tnr) findEmptyColumns(int colCnt, IEnumerable<dynamic> rows, int topCount = 3)
    {
      var topNrowsReport = "";
      var emptyColumnFlags = new bool[colCnt];
      rows.Take(topCount).ToList().ForEach(kvp =>
      {
        var c = 0;
        ((IDictionary<string, object>)kvp).Values.ToList().ForEach(cell =>
        {
          topNrowsReport += ($"  '{cell}'\t");

          if (!string.IsNullOrEmpty(cell?.ToString()))
            emptyColumnFlags[c] = true;

          c++;
        });
        topNrowsReport += ($"\n");
      });

      topNrowsReport += ($"  ... + {rows.Count() - topCount} more rows.\n\n");

      return (emptyColumnFlags, topNrowsReport);
    }


    void printTopLines(int top = 4)
    {
      Debug.WriteLine($" - Import done: {_linesIn.Count} lines imported!  Showing the 1st {top} rows:");
      _linesIn.Take(top).ToList().ForEach(l => Debug.WriteLine(l));
    }

    public async Task<string> GetFileStats()
    {
      await Task.Delay(333);
      return "Under COnstruction...";
    }
  }
}
///C:\OI\SSCNETSettlementExtract\CsvToJsonConverter\CsvToJsonConverter.csproj