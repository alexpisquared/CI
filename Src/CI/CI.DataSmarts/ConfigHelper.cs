using CI.DS.Visual.Views;
using CI.GUI.Support.WpfLibrary.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CI.DataSmarts
{
  public class ConfigHelper
  {
    public static IConfigurationRoot InitConfig(string appsettingsFile)
    {
      try
      {
        for (var i = 0; i < 3; i++)
        {
          try
          {
            return new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile(appsettingsFile)
              .AddUserSecrets<MainView>()
              .Build();
          }
          catch (InvalidOperationException ex)
          {
            ex.Pop(null, optl: "Disaster ...");
          }
          catch (Exception ex)
          {
            if (!tryToCreateDefaultFile(appsettingsFile))
              ex.Pop(null, optl: "The default values will be used  ...maybe");
          }
        }

        throw new Exception($"Unable to create default  {appsettingsFile}  file");
      }
      catch (Exception ex)
      {
        ex.Pop(null, optl: "The default values will be used  ...maybe");
      }

      throw new Exception($"Unable to create default  {appsettingsFile}  file");
    }

    static bool tryToCreateDefaultFile(string appsettingsFile)
    {
      try
      {
        if (!File.Exists(appsettingsFile))
          File.WriteAllText(appsettingsFile, @"
{
  ""WhereAmI"": "" ??\\PermMgrClient\\appsettings.CI.PM.json  DFLT"",
  ""LogFolder"": ""\\\\bbsfile01\\Public\\AlexPi\\Misc\\Logs\\PermMgr.DFLT..txt"",
  ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
  ""SqlConStr"": ""Server=MTdevSQLDB;Database=Inventory;Trusted_Connection=True;"",
  ""AppSettings"": {
    ""ServerList"": ""mtDEVsqldb mtUATsqldb mtPRDsqldb"",
    ""RmsDbConStr"": ""Server={0};Database={1};Trusted_Connection=True;"",
    ""KeyVaultURL"": ""<moved to a safer place>""
  }
}");
        return true;
      }
      catch (Exception ex)
      {
        ex.Pop(null);
        return false;
      }
    }
  }
}
