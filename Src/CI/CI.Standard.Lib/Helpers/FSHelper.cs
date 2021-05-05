using CI.Standard.Lib.Extensions;
using System;
using System.IO;

namespace CI.Standard.Lib.Helpers
{
  public static class FSHelper
  {
    public static bool ExistsOrCreated(string? folder) // true if created or exists; false if unable to create.
    {
      if (folder is null)
        throw new ArgumentNullException("Provide non-null folder value");

      try
      {
        if (folder is not null && !Directory.Exists(folder))
          Directory.CreateDirectory(folder);

        return true;
      }
      catch (IOException ex) { ex.Log($"Directory.CreateDirectory({folder})"); }
      catch (Exception ex) { ex.Log($"Directory.CreateDirectory({folder})"); throw; }

      return false;
    }
  }
}
