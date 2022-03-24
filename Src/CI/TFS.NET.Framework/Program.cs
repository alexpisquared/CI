using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFS
{
  class Program
  {
    static void Main(string[] args)
    {
      var filePatterns = args[0].Split(new char[] { '`' }, StringSplitOptions.RemoveEmptyEntries); // new[] { "*.jar", "*.cs", "*.cpp" };//"*.?", "*.??", "*.???", "*.????", "*.?????", "*.??????" };// "*.cs", "*.xml", "*.config", "*.asp", "*.aspx", "*.js", "*.h", "*.cpp", "*.vb", "*.asax", "*.ashx", "*.asmx", "*.ascx", "*.master", "*.svc", "*.jar" }; //file extensions
      var textPatterns = args.Length < 2 ? null : args[1].Split(new char[] { '`' }, StringSplitOptions.RemoveEmptyEntries);

      Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write($"  Searching for  ");
      Console.ForegroundColor = ConsoleColor.Cyan; Console.Write($"{(args.Length < 2 ? "" : args[1])}");
      Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write($"  in  ");
      Console.ForegroundColor = ConsoleColor.Cyan; Console.Write($"{args[0]} \n");

      var now = DateTime.Now;
      var fnm = (args.Length < 2 ? "fnm" : args[1]).Replace(":", "-").Replace("|", " ").Replace("?", "-");
      var filenamesOnlyFile = $@"C:\temp\TFS.Search\{fnm} - {now:HHmm} - Filenames Only.txt";
      var filesAndLinesFile = $@"C:\temp\TFS.Search\{fnm} - {now:HHmm} - Files & Lines.txt";
      var headerL = $"Times  Checked-in  Filename       Searching for  '{(args.Length < 2 ? "" : args[1])}'  in  '{args[0]}'    {Environment.NewLine}";

      try
      {
        var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("https://ciis-sourcecode.corporate.ciglobe.net/tfs/DefaultCollection")); // :new|old: http://dev-tfs1.pariotech.com:8080/tfs/defaultcollection"));
        tfs.EnsureAuthenticated();

        var versionControl = tfs.GetService<VersionControlServer>();

        File.AppendAllText(filenamesOnlyFile, headerL);
        File.AppendAllText(filesAndLinesFile, headerL);

        var allProjs = versionControl.GetAllTeamProjects(true);
        Console.ForegroundColor = ConsoleColor.Magenta;
        //Console.WriteLine($"  {allProjs.Count():N0} team projects: {string.Join<TeamProject>(", ", allProjs)}");
        Console.Write($"  {allProjs.Count():N0} team projects:  ");

        var i = 0;
        var f = 0;
        foreach (var teamProj in allProjs)
        {
          Console.ForegroundColor = ConsoleColor.Gray;
          Console.WriteLine($"{teamProj.Name}");

          foreach (var filePattern in filePatterns)
          {
            var files = versionControl.GetItems(teamProj.ServerItem + "/" + filePattern, RecursionType.Full).Items.Where(r => r.ItemType == ItemType.File && !r.ServerItem.Contains("_ReSharper")); //skipping resharper stuff
            Console.WriteLine($"  {++f} / {filePatterns.Length} - {filePattern} - {files.Count():N0} files:");
            foreach (var file in files)
            {
              if ((++i) % 11 == 0)
                Console.Write($"{i,8:N0} / {files.Count():N0} {file.ServerItem,-400}                                                        \r");

              if (textPatterns == null)
              {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                var header = $"{"",5}  {file.CheckinDate:yyyy-MM-dd}  {file.ServerItem}   {Environment.NewLine}";
                Console.Write(header);
                File.AppendAllText(filenamesOnlyFile, header);
              }
              else
              {
                var lines = SearchInFile(file, textPatterns);
                if (lines.Count > 0)
                {
                  Console.ForegroundColor = ConsoleColor.DarkYellow;
                  var header = $"{lines.Count,5}  {file.CheckinDate:yyyy-MM-dd}  {file.ServerItem}   {Environment.NewLine}";
                  Console.Write(header);
                  File.AppendAllText(filenamesOnlyFile, header);
                  File.AppendAllText(filesAndLinesFile, header);

                  foreach (var line in lines)
                  {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  {line}");
                    File.AppendAllText(filesAndLinesFile, $"  {line}{Environment.NewLine}");
                  }

                  Console.ForegroundColor = ConsoleColor.Gray;
                }
              }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  {f} / {filePatterns.Length} - {filePattern} - {files.Count():N0} files.");
          }
        }
      }
      catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"{ex}"); Console.ForegroundColor = ConsoleColor.Gray; }
      finally
      {
        var report = $"\n from - to = took:  {now:HH:mm} - {DateTime.Now:HH:mm} = {(DateTime.Now - now).TotalMinutes:N1} min";
        Console.WriteLine(report);
        File.AppendAllText(filesAndLinesFile, report);

        _ = Process.Start("Explorer.exe", $"/select, \"{filesAndLinesFile}\"");
      }

      //Console.ForegroundColor = ConsoleColor.DarkGreen;      Console.WriteLine("======== Press any key ");      Console.ResetColor();      Console.ReadKey();
    }

    static List<string> SearchInFile(Item file, string[] textPatterns)
    {
      var result = new List<string>();

      try
      {
        var stream = new StreamReader(file.DownloadFile(), Encoding.Default);

        var line = stream.ReadLine();
        var lineIndex = 0;

        while (!stream.EndOfStream)
        {
          if (textPatterns.Any(p => line.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0))
            result.Add($"{lineIndex,6}: {line.Trim()}");

          line = stream.ReadLine();
          lineIndex++;
        }
      }
      catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"{ex}"); Console.ForegroundColor = ConsoleColor.Gray; }
      return result;
    }
  }
}

