using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Visual.Lib.Helpers
{
  public static class Bpr
  {
    const int _f1 = 5000, _f2 = 6000, _d1 = 100000;
    public static void Test()
    {
      Task.Run(async () =>
      {
        await Beep(_f1, _d1/1000);
        await Beep(_f2, _d1/1000);
        await Task.Delay(333);
        await Start();
        await Task.Delay(333);
        await Finish();
        await Task.Delay(333);
        await Error();
        await Task.Delay(333);
        await Task.Delay(333);
        await Task.Delay(333);
        await Task.Delay(333);
      });
    }
    public static async Task Start()
    {
      await BprAlt.BeepMks(
        new[] {
          new[] { _f1, BprAlt.FixDuration(_f1, _d1) },
          new[] { _f2, BprAlt.FixDuration(_f2, _d1) },
      }, ushort.MaxValue);
    }
    public static async Task Finish()
    {
      await BprAlt.BeepMks(
        new[] {
          new[] { _f2, BprAlt.FixDuration(_f2, _d1) },
          new[] { _f1, BprAlt.FixDuration(_f1, _d1) },
      }, ushort.MaxValue);
    }
    public static async Task Error()
    {
      await BprAlt.BeepMks(
        new[] {
          new[] { _f2, BprAlt.FixDuration(_f2, _d1) },
          new[] { _f2, BprAlt.FixDuration(_f2, _d1) },
          new[] { _f2, BprAlt.FixDuration(_f2, _d1) },
          new[] { _f1, BprAlt.FixDuration(_f1, _d1 * 3) },
      }, ushort.MaxValue);
    }

    public static async Task Beep(int freq = 98, int durationMs = 250) => await BprAlt.BeepMks(new[] { new[] { freq, BprAlt.FixDuration(freq, durationMs * 1000) } }, ushort.MaxValue);
    public static async Task BeepFD(int freq = 98, int durationMks = 250111, ushort volume = ushort.MaxValue) => await BprAlt.BeepMks(new[] { new[] { freq, BprAlt.FixDuration(freq, durationMks) } }, volume);
    public static async Task BeepFD(int freq = 98, double durationSec = .25, ushort volume = ushort.MaxValue) => await BprAlt.BeepMks(new[] { new[] { freq, BprAlt.FixDuration(freq, (int)(durationSec * 1000000)) } }, volume);
  }
}
