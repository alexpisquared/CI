using CI.GUI.Support.WpfLibrary.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace CI.GUI.Support.WpfLibrary.Helpers
{
  public class ConfigHelper //todo: Copy to the main Shared.WPF and replace all 'new ConfigurationBuilder()' with it.
  {
    public static IConfigurationRoot AutoInitConfig(string defaultValues = _defaultValues) => InitConfig(@$"AppSettings\{AppDomain.CurrentDomain.FriendlyName}.json", defaultValues);

    public static IConfigurationRoot InitConfig(string appsettingsFile, string defaultValues = _defaultValues)
    {
      try
      {
        for (var i = 0; i < 3; i++)
          try
          {
            return new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile(appsettingsFile)
              .AddUserSecrets<WhatIsThatForType>()
              .Build();
          }
          catch (InvalidOperationException ex)
          {
            ex.Pop(null, optl: "Disaster ...");
          }
          catch (FileNotFoundException ex)
          {
            if (!tryCreateDefaultFile(appsettingsFile, defaultValues))
              ex.Log("Retrying 3 times ...");
          }
          catch (FormatException ex)
          {
            new Process { StartInfo = new ProcessStartInfo("Notepad.exe", $"\"{appsettingsFile}\"") { RedirectStandardError = true, UseShellExecute = false } }.Start();
            ex.Pop(null, optl: $"Try to edit the errors out from \n\t {appsettingsFile}");
          }
          catch (Exception ex)
          {
            if (!tryCreateDefaultFile(appsettingsFile, defaultValues))
              ex.Pop(null, optl: "██  ██  ██  Take a look!");
          }

        throw new Exception($"Unable to create default  {appsettingsFile}  file");
      }
      catch (Exception ex)
      {
        ex.Pop(null, optl: "The default values will be used  ...maybe");
      }

      throw new Exception($"Unable to create default  {appsettingsFile}  file");
    }

    static bool tryCreateDefaultFile(string appsettingsFile, string defaultValues)
    {
      try
      {
        if (Directory.Exists(Path.GetDirectoryName(appsettingsFile)) != true)
          Directory.CreateDirectory(Path.GetDirectoryName(appsettingsFile));

        if (!File.Exists(appsettingsFile))
          File.WriteAllText(appsettingsFile, string.Format(defaultValues, appsettingsFile.Replace(@"\", @"\\")));

        return true;
      }
      catch (Exception ex)
      {
        ex.Pop(null);
        return false;
      }
    }
    class WhatIsThatForType { public string MyProperty { get; set; } = "<Default Value of Nothing Special>"; }
    const string _defaultValues = @"{{
      ""WhereAmI"": ""{0}"",
      ""LogFolder"": ""\\\\bbsfile01\\Public\\AlexPi\\Misc\\Logs\\[RenameMe].DFLT..txt"",
      ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
      ""SqlConStr"": ""Server=mtDEVsqldb;Database=Inventory;Trusted_Connection=True;"",
      ""AppSettings"": {{
        ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
        ""RmsDbConStr"": ""Server={{0}};Database={{1}};Trusted_Connection=True;"",
        ""KeyVaultURL"": ""<moved to a safer place>""
      }}
}}";
  }
}
