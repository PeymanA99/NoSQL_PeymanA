/////////////////////////////////////////////////////////////////////
// StartedProcess.cs - Will be run by ProcessStarter               //
//                                                                 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;

namespace StartedProcess
{
  class StartedProcess
  {
    public void showCommandLine(string[] args)
    {
      Console.Write("\n  Commandline arguments: ");
      foreach (string arg in args)
        Console.Write("{0} ", arg);
    }
    public void makeSound()
    {
      SystemSounds.Asterisk.Play();
    }
    static void Main(string[] args)
    {
      Console.Title = "Started Process";
      StartedProcess sp = new StartedProcess();
      sp.showCommandLine(args);
      sp.makeSound();
          
      for(int i=0; i<10; ++i)
      {
        Console.Write("\n  running");
        Thread.Sleep(500);
      }
      Console.Write("\n  -- press key to exit: ");
      Console.ReadKey();
    }
  }
}
