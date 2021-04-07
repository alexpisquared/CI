namespace CI.GUI.Support.WpfLibrary.Helpers
{
  public class Bpr
  {
    public static void ErrorFaF() => NativeMethods.Beep(4444, 333);
    public static void Beep(int freq, int dur) => NativeMethods.Beep(freq, dur);
  }
}
