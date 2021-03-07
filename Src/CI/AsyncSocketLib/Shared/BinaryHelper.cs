using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AsyncSocketLib.Shared
{
  public static class BinaryHelper
  {
public    static void MoveData(byte[] buffer, int offset, int size) { for (var i = 0; i < size; i++) { buffer[i] = buffer[i + offset]; } }

    public static byte[]? ToByteArray<T>(T? obj)
    {
      if (obj == null)
        return null;

      var bf = new BinaryFormatter();
      using var ms = new MemoryStream();
      bf.Serialize(ms, obj);
      return ms.ToArray();
    }
    public static T? FromByteArray<T>(byte[]? data)
    {
      if (data == null)
        return default;

      using var ms = new MemoryStream(data);
      var bf = new BinaryFormatter();
      var obj = bf.Deserialize(ms);
      return (T)obj;
    }

  }
}
