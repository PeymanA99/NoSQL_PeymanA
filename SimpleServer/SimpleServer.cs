/////////////////////////////////////////////////////////////////////////
// SimpleServer.cs - Very simple server - good starting point for      //
// ver 1.0           Project #4 database server.                       //
//                                                                     //
/////////////////////////////////////////////////////////////////////////
/*
 * Purpose:
 *----------
 * This is an unadorned server - easy to understand and build upon.
 * Note: has fixed listen port = 8079
 *
 * Additions to C# Console Wizard generated code:
 * - references to ICommService, Sender, Receiver, and Utilities
 */
/*
 * Maintenance History:
 * --------------------
 * ver 1.0 : 29 Oct 2015
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4Starter
{
      using System.Diagnostics;
      using Util = Utilities;
      public class SimpleSender : Sender
  {
    public bool goodStatus { get; set; } = true;

    public override void sendMsgNotify(string msg)
    {
      if (msg.Contains("could not connect"))
        goodStatus = false;  // overwrote to false 
    }
  }
  class SimpleServer
  {
    string port = "8079";
    string address = "localhost";
    

    static void Main(string[] args)
    {
      
      SimpleServer srvr = new SimpleServer();
      Stopwatch serverWatch = new Stopwatch();
      //SimpleSender sndr = new SimpleSender();
      Sender sndr = new Sender(Util.makeUrl(srvr.address, srvr.port));
      Console.Title = "Simple Server";
      String.Format("Simple Server Started listing on {0}", srvr.port).title('=');
      Receiver rcvr = new Receiver(srvr.port, srvr.address);
                  
      rcvr.StartService();

      while(true)
      {
            serverWatch.Restart(); // reset clock and start the processing time for server
            Message msg = rcvr.getMessage();
            serverWatch.Stop();
            Console.Write("\n  Simple Server received:");
            Utilities.showMessage(msg);
            EventLogger alog = new EventLogger(msg.eventType, msg.whenOccured, msg.fromUrl);
            alog.toString();
            if (msg.content == "done")
            {
            Console.WriteLine();
            rcvr.shutDown();
            sndr.shutdown();
            break;
            }
            msg.content = "Simple Server received: " + msg.content;
            Utilities.swapUrls(ref msg);
            TimeSpan ts = serverWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                  ts.Hours, ts.Minutes, ts.Seconds,
                  ts.Milliseconds / 10);
             Console.WriteLine("\n\nServer Msg processing time :" + elapsedTime);
                        /*if(sndr.goodStatus == true)
                        {
                          sndr.sendMessage(msg);
                        }
                        else
                        {
                          Console.Write("\n  closing\n");
                          rcvr.shutDown();
                          sndr.shutdown();
                          break;
                        }*/
                        Console.WriteLine();
      }
    }
  }
}
