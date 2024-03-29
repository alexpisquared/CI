﻿using CsvHelper;
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
    readonly List<CsvLine> _linesIn = new();
    readonly bool _ignoreHeaderColumnName = false; // for rare case scenario could be false.
    const int _topN = 3;
    CsvConfiguration? _config;
    List<dynamic>? _allCsvRecords;

    public CsvConverter(string filename)
    {
      _filename0 = filename;
      var ext = Path.GetExtension(filename);
      _filename2 = filename.Replace(ext, $".~{ext}");
    }

    public async Task<string> GetFileStats()
    {
      await Task.Delay(33);
      var report = "";
      try
      {
        _config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          HasHeaderRecord = false, // !!! otherwise must have unique non-empty column names
          //HeaderValidated = null,
          MissingFieldFound = null,
          IgnoreBlankLines = true,
          TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(_filename0);
        using var csvrdr = new CsvReader(reader, _config);
        _allCsvRecords = csvrdr.GetRecords<dynamic>().ToList();
        report += DumpTopRows("BEFORE", _allCsvRecords);
      }
      catch (Exception ex) { report += (ex); }

      return report;
    }
    public async Task<string> CleanEmptyRowsColumns()
    {
      var report = "";
      try
      {
        var allCsvHeaders = ((IDictionary<string, object>?)_allCsvRecords?.FirstOrDefault())?.Values;
        var columnCount = allCsvHeaders?.Count ?? 0;

        var nonEmptyRows = _allCsvRecords?.Skip(_ignoreHeaderColumnName ? 1 : 0).ToList().Where(kvp => ((ExpandoObject)kvp).Any(v => !string.IsNullOrEmpty(v.Value?.ToString())));

        //allCsvHeaders.ToList().ForEach(header => report += $"  {header}\t"); report += ($"\n");

        var ecrv = FindEmptyColumnsTemplate(columnCount, nonEmptyRows?.Skip(1));

        report += ($"Empty columns:  "); ecrv.ToList().ForEach(r => report += (r ? "#" : "·")); report += ($"\n");

        var ourv = RemoveEmptyColumns(nonEmptyRows, ecrv);
        report += DumpTopRows("AFTER", ourv);

        using var writer = new StreamWriter(_filename2);
        using var csv3 = new CsvWriter(writer, _config);
        csv3.WriteRecords(ourv);
      }
      catch (Exception ex) { report += (ex); }
      await Task.Delay(33);

      return report;
    }

    static string DumpTopRows(string note, List<dynamic> allCsvRecords, int topCount = _topN)
    {
      var report = $"{note}   Top {_topN} from total {allCsvRecords.Count:N0} rows: \n";

      allCsvRecords.Take(topCount).ToList().ForEach(row =>
      {
        ((IDictionary<string, object>)row).Values.ToList().ForEach(cell => report += ($"  '{cell}'\t"));
        report += $"\n";
      });

      return report + $"  ... + {allCsvRecords.Count - topCount} more rows.\n\n";
    }

    static List<dynamic> RemoveEmptyColumns(IEnumerable<dynamic>? nonEmptyRows, bool[] emptyColumns)
    {
      var outCsvRecords = new List<dynamic>();

      nonEmptyRows?.ToList().ForEach(kvp =>
      {
        var outCsvRecord = new ExpandoObject() as IDictionary<string, object>;

        var c = 0;
        foreach (var column in kvp)
        {
          if (emptyColumns[c++])
          {
            outCsvRecord.Add(column.Key, column.Value);
          }
        }

        outCsvRecords.Add(outCsvRecord);
      });

      return outCsvRecords;
    }
    static bool[] FindEmptyColumnsTemplate(int colCnt, IEnumerable<dynamic>? rows, int topCount = _topN)
    {
      var emptyColumnFlags = new bool[colCnt];
      rows?.Take(topCount).ToList().ForEach(row =>
      {
        var c = 0;
        ((IDictionary<string, object>)row).Values.ToList().ForEach(cell =>
        {
          if (!string.IsNullOrEmpty(cell?.ToString()))
            emptyColumnFlags[c] = true;

          c++;
        });
      });

      return emptyColumnFlags;
    }
  }
}
///C:\OI\SSCNETSettlementExtract\CsvToJsonConverter\CsvToJsonConverter.csproj