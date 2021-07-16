using System;
using Microsoft.TeamFoundation;
//using Microsoft.TeamFoundation.Client;
//using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFS
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Hello World!  " +
        $"{Microsoft.TeamFoundation.Common.StructureType.ProjectLifecycle}" +
        $"{Microsoft.TeamFoundation.Common.StructureType.ProjectLifecycle}" +
        $"{Microsoft.TeamFoundation.Common.StructureType.ProjectLifecycle}" +
        $"");
    }
  }
}
