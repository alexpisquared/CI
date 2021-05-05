namespace CI.GUI.Support.WpfLibrary.Helpers
{
  public class UserSettings
  {
    public void Save<T>() => JsonIsoFileSerializer.Save(this, iss: IsoConst.URoaA);
    public T? Load<T>() where T : new() => JsonIsoFileSerializer.Load<T>(iss: IsoConst.URoaA) ?? new T();
  }
}
