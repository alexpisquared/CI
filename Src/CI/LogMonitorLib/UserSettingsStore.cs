using System;
using System.IO;
using CI.Standard.Lib.Helpers;

namespace LogMonitorConsoleApp
{
  public class UserSettingsStore_
  {
    public static string _store => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @$"AppSettings\{AppDomain.CurrentDomain.FriendlyName}\UserSettings.json");

    public static void Save<T>(T ths) => JsonFileSerializer.Save(ths, _store);                  //JsonIsoFileSerializer.Save(ths, iss: IsoConst.URoaA);
    public static T Load<T>() where T : new() => JsonFileSerializer.Load<T>(_store) ?? new T(); //JsonIsoFileSerializer.Load<T>(iss: IsoConst.URoaA) ?? new T();
  }
}