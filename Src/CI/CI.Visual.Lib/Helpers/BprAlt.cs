﻿using CI.Standard.Lib.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;

namespace CI.Visual.Lib.Helpers
{
  public static class BprAlt
  {
    public static async Task BeepMks(int[][] HzMks, ushort volume = ushort.MaxValue)
    {
      using (MemoryStream? mStrm = new MemoryStream())
      using (BinaryWriter? writer = new BinaryWriter(mStrm))
      {
        const double TAU = 2 * Math.PI;
        const int formatChunkSize = 16, headerSize = 8, samplesPerSecond = 44100, waveSize = 4;
        const short formatType = 1, tracks = 1, bitsPerSample = 16, frameSize = tracks * ((bitsPerSample + 7) / 8);
        const int bytesPerSecond = samplesPerSecond * frameSize;

        int ttlms = HzMks.Sum(r => r[1]);
        int samples = (int)(samplesPerSecond * 0.000001m * ttlms);
        int dataChunkSize = samples * frameSize;
        int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
        // var encoding = new System.Text.UTF8Encoding();
        writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
        writer.Write(fileSize);
        writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
        writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
        writer.Write(formatChunkSize);
        writer.Write(formatType);
        writer.Write(tracks);
        writer.Write(samplesPerSecond);
        writer.Write(bytesPerSecond);
        writer.Write(frameSize);
        writer.Write(bitsPerSample);
        writer.Write(0x61746164); // = encoding.GetBytes("data")
        writer.Write(dataChunkSize);

        foreach (int[]? hzms in HzMks)
        {
          int hz = hzms[0];
          long ms = hzms[1];
          double theta = hz * TAU / samplesPerSecond;

          //Console.WriteLine($" ** Beep():  {hz,8:N0} hz   {ms,8:N0} ms     =>     2Pi *{hz,5} hz  /  {samplesPerSecond} sampl/s  =  {theta:N4}");

          // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
          // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
          double amp = volume >> 2; // so we simply set amp = volume / 2
          int stepCount = (int)(samples * ms / ttlms);
          for (int step = 0; step < stepCount; step++)
          {
            //System.Diagnostics.Debug.WriteLine((short)(amp * Math.Sin(theta * step)));
            writer.Write((short)(amp * Math.Sin(theta * step)));
          }
        }

        mStrm.Seek(0, SeekOrigin.Begin);
        _p.Stream = mStrm;
        try
        {
#if sync
          _p.Play();          
          await Task.Delay(ttlms / 1000);
#else
          _p.PlaySync();
          await Task.Delay(10);
#endif

          writer.Close();
          mStrm.Close();
        }
        catch (Exception ex) { ex.Log(); throw; }
      }
    }

    static readonly SoundPlayer _p = new();    //public static async Task Beep(int f = 50, int t = 200, ushort v = ushort.MaxValue) => await Beep((uint)f, (uint)t, v);
    public static int FixDuration(int frqHz, int durMks)
    {
      double times = durMks * .000001 * frqHz;
      double timesm = Math.Round(times);
      if (timesm <= 0)
      {
        timesm = 1;
      }

      int durmks = (int)(/*1.03125 * */1000000 * timesm / frqHz);

      return durmks;
    }
  }
}