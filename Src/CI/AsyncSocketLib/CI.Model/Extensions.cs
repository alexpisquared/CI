using System.Text;

namespace AsyncSocketLib.CI.Model
{
  public static class Extensions
  {
    public static unsafe void ToByteArray(this string str, byte* buffer)
    {
      var byteArray = Encoding.ASCII.GetBytes(str);
      for (var i = 0; i < str.Length; i++)
        buffer[i] = byteArray[i];

      buffer[str.Length] = 0;
    }
  }
}
