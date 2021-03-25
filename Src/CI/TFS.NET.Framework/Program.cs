using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TFS
{
  class Program
  {
    static readonly string[] textPatterns = new[] { "traderaccount_view" };  //Text to search
    static readonly string[] filePatterns = new[] { "*.cs", "*.xml", "*.config", "*.asp", "*.aspx", "*.js", "*.h", "*.cpp", "*.vb", "*.asax", "*.ashx", "*.asmx", "*.ascx", "*.master", "*.svc" }; //file extensions

    static void Main(string[] args)
    {
      Console.ForegroundColor = ConsoleColor.Gray;

      try
      {
        var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://dev-tfs1.pariotech.com:8080/tfs/defaultcollection"));
        tfs.EnsureAuthenticated();

        var versionControl = tfs.GetService<VersionControlServer>();

        var outputF = @"C:\temp\Code Matches - Filenames Only.txt";
        var details = @"C:\temp\Code Matches - Files & Lines.txt";
        var headerL = $"Times  Checked-in  Filename{Environment.NewLine}";

        File.AppendAllText(outputF, headerL);
        File.AppendAllText(details, headerL);

        var allProjs = versionControl.GetAllTeamProjects(true);
        Console.WriteLine($"{allProjs.Count()} team projects: ");


        var i = 0;
        var f = 0;
        foreach (var teamProj in allProjs)
        {
          Console.WriteLine($"{teamProj.Name}");

          foreach (var filePattern in filePatterns)
          {
            var items = versionControl.GetItems(teamProj.ServerItem + "/" + filePattern, RecursionType.Full).Items.Where(r => !r.ServerItem.Contains("_ReSharper"));  //skipping resharper stuff
            Console.WriteLine($"{++f} / {filePatterns.Length} - {filePattern}  {items.Count()}:");
            foreach (var item in items)
            {
              if ((++i) % 100 == 0)
                Console.WriteLine($"{i,6} / {items.Count(),-6} {item.ServerItem}");

              var lines = SearchInFile(item);
              if (lines.Count > 0)
              {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var header = $"{lines.Count,5}  {item.CheckinDate:yyyy-MM-dd}  {item.ServerItem}   {Environment.NewLine}";
                Console.Write(header);
                File.AppendAllText(outputF, header);
                File.AppendAllText(details, header);

                foreach (var line in lines)
                {
                  Console.ForegroundColor = ConsoleColor.Green;
                  Console.WriteLine($"  {line}");
                  File.AppendAllText(details, $"  {line}{Environment.NewLine}");
                }

                Console.ForegroundColor = ConsoleColor.Gray;
              }
            }
          }
        }
      }
      catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"{ex}"); Console.ForegroundColor = ConsoleColor.Gray; }

      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine("======== Press any key ");
      Console.ResetColor();
      Console.ReadKey();
    }

    // Define other methods and classes here
    private static List<string> SearchInFile(Item file)
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
            result.Add($"{lineIndex}: {line.Trim()}");

          line = stream.ReadLine();
          lineIndex++;
        }
      }
      catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"{ex}"); Console.ForegroundColor = ConsoleColor.Gray; }
      return result;
    }
  }
}

