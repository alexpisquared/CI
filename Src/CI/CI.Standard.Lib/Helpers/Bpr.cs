namespace CI.Standard.Lib.Helpers
{
  public class Bpr
  {
    const int _a = 7000, _b = 8000, _c = 9000, _msA = 100, _msB = 200;

    public static void Beep(int freq = 999, int dur = 999) => NativeMethods.Beep(freq, dur);
    public static void Start() { Beep(_a, _msA); Beep(_b, _msB); }
    public static void Finish() { Beep(_b, _msB); Beep(_a, _msA); }
    public static void Tick() => NativeMethods.Beep(_c, 75);
    public static void ErrorFaF() => NativeMethods.Beep(_a, 333);
  }
}
