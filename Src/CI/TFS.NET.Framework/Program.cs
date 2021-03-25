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
    const string _srch = "select distinct upper(trader_id + ':' + shortname) from inventory..traderaccount_view2";
    static readonly string[]
      textPatterns = new[] { _srch },
      filePatterns = new[] { "*.*" };//"*.?", "*.??", "*.???", "*.????", "*.?????", "*.??????" };// "*.cs", "*.xml", "*.config", "*.asp", "*.aspx", "*.js", "*.h", "*.cpp", "*.vb", "*.asax", "*.ashx", "*.asmx", "*.ascx", "*.master", "*.svc", "*.jar" }; //file extensions

    static void Main(string[] args)
    {
      Console.ForegroundColor = ConsoleColor.Gray;
      var now = DateTime.Now;
      var fnm = _srch.Replace(":", "-");
      var outputF = $@"C:\temp\Code Matches - {fnm} - {now:HHmm} - Filenames Only.txt";
      var details = $@"C:\temp\Code Matches - {fnm} - {now:HHmm} - Files & Lines.txt";
      var headerL = $"Times  Checked-in  Filename{Environment.NewLine}";


      try
      {
        var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://dev-tfs1.pariotech.com:8080/tfs/defaultcollection"));
        tfs.EnsureAuthenticated();

        var versionControl = tfs.GetService<VersionControlServer>();

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
            var files = versionControl.GetItems(teamProj.ServerItem + "/" + filePattern, RecursionType.Full).Items.Where(r => r.ItemType == ItemType.File && !r.ServerItem.Contains("_ReSharper"));  //skipping resharper stuff
            Console.WriteLine($"{++f} / {filePatterns.Length} - {filePattern}  {files.Count()}:");
            foreach (var item in files)
            {
              if ((++i) % 1000 == 0)
                Console.WriteLine($"{i,6} / {files.Count(),-6} {item.ServerItem}");

              var lines = SearchInFile(item);
              if (lines.Count > 0)
              {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
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
      catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"{ex}"); Console.ForegroundColor = ConsoleColor.Gray; }
      finally
      {
        File.AppendAllText(details, $"\n from - to = took:  {now:HH:mm} - {DateTime.Now:HH:mm} = {(DateTime.Now - now).TotalMinutes:N1} min");
      }

      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine("======== Press any key ");
      Console.ResetColor();
      Console.ReadKey();
    }

    static List<string> SearchInFile(Item file)
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
      catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"{ex}"); Console.ForegroundColor = ConsoleColor.Gray; }
      return result;
    }
  }
}

