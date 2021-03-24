using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.Framework.Client;
using System.IO;

namespace TFS
{
  class Program
  {
    static string[] textPatterns = new[] { "traderaccount_view2" };  //Text to search
    static string[] filePatterns = new[] { "*.cs", "*.xml", "*.config", "*.asp", "*.aspx", "*.js", "*.h", "*.cpp", "*.vb", "*.asax", "*.ashx", "*.asmx", "*.ascx", "*.master", "*.svc" }; //file extensions

    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      try
      {
        var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(
          new Uri("http://dev-tfs1.pariotech.com:8080/tfs")//, new ServiceIdentityCredentialsProvider(@"BBSSecurities\alex.pigida","")      
          ); 

      //tfs.EnsureAuthenticated();

        var versionControl = tfs.GetService<VersionControlServer>();


        StreamWriter outputFile = new StreamWriter(@"C:\Find.txt");
        var allProjs = versionControl.GetAllTeamProjects(true);
        foreach (var teamProj in allProjs)
        {
          foreach (var filePattern in filePatterns)
          {
            var items = versionControl.GetItems(teamProj.ServerItem + "/" + filePattern, RecursionType.Full).Items
                        .Where(i => !i.ServerItem.Contains("_ReSharper"));  //skipping resharper stuff
            foreach (var item in items)
            {
              List<string> lines = SearchInFile(item);
              if (lines.Count > 0)
              {
                outputFile.WriteLine("FILE:" + item.ServerItem);
                outputFile.WriteLine(lines.Count.ToString() + " occurence(s) found.");
                outputFile.WriteLine();
              }
              foreach (string line in lines)
              {
                outputFile.WriteLine(line);
              }
              if (lines.Count > 0)
              {
                outputFile.WriteLine();
              }
            }
          }
          outputFile.Flush();
        }
      }
      catch (Exception e)
      {
        string ex = e.Message;
        Console.WriteLine("!!EXCEPTION: " + e.Message);
        Console.WriteLine("Continuing... ");
      }
      Console.WriteLine("========");
      Console.Read();
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
            result.Add("=== Line " + lineIndex + ": " + line.Trim());

          line = stream.ReadLine();
          lineIndex++;
        }
      }
      catch (Exception e)
      {
        string ex = e.Message;
        Console.WriteLine("!!EXCEPTION: " + e.Message);
        Console.WriteLine("Continuing... ");
      }

      return result;
    }
  }
}

