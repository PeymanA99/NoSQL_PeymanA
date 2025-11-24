/////////////////////////////////////////////////////////////////////
// ProcessStarter.cs - Demonstrate running a child program         //
//                                                                 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;

namespace ProcessStarter
{
  class ProcessStarter
  {
    public bool startProcess(string process)
    {
      process = Path.GetFullPath(process);
      Console.Write("\n  fileSpec - \"{0}\"", process);
      ProcessStartInfo psi = new ProcessStartInfo
      {
        FileName = process,
        Arguments = "one two three",
        // set UseShellExecute to true to see child console, false hides console
        UseShellExecute = true
      };
      try
      {
        Process p = Process.Start(psi);
        return true;
      }
      catch(Exception ex)
      {
        Console.Write("\n  {0}", ex.Message);
        return false;
      }
    }
    static void Main(string[] args)
    {
      Console.Write("\n  current directory is: \"{0}\"", Directory.GetCurrentDirectory());
      ProcessStarter ps = new ProcessStarter();
      ps.startProcess("../../../StartedProcess/bin/debug/StartedProcess.exe");
      ps.startProcess("../../../Server/bin/Debug/Server.exe");
      ps.startProcess("../../../Client3/bin/debug/Client3.exe");
    //  ps.startProcess("C:/Users/PeYman/Desktop/CSE681/CourseProjects/project4/WpfClient/bin/Debug/WpfApplication1.exe");

      Console.Write("\n  press key to exit: ");
      Console.ReadKey();
    }
  }
}
