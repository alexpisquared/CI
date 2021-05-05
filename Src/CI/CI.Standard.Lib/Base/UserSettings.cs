namespace CI.GUI.Support.WpfLibrary.Helpers
{
  public class UserSettings
  {
    public static void Save<T>(T ths) => JsonIsoFileSerializer.Save(ths, iss: IsoConst.URoaA);
    public static T Load<T>() where T : new() => JsonIsoFileSerializer.Load<T>(iss: IsoConst.URoaA) ?? new T();
  }
}
