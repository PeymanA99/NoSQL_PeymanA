/////////////////////////////////////////////////////////////////////////
// Server.cs - CommService server                                      //
// ver 2.2                                                             //
/////////////////////////////////////////////////////////////////////////
/*
 * Additions to C# Console Wizard generated code:
 * - Added reference to ICommService, Sender, Receiver, Utilities
 *
 * Note:
 * - This server now receives and then sends back received messages.
 */
/*
 * Plans:
 * - Add message decoding and NoSqlDb calls in performanceServiceAction.
 * - Provide requirements testing in requirementsServiceAction, perhaps
 *   used in a console client application separate from Performance 
 *   Testing GUI.
 */
/*
 * Maintenance History:
 * --------------------
 * ver 2.4 : 12 Nov 2015 Peyman A. Added methods actions and other codes for Project 4
 * ver 2.3 : 29 Oct 2015
 * - added handling of special messages: 
 *   "connection start message", "done", "closeServer"
 * ver 2.2 : 25 Oct 2015
 * - minor changes to display
 * ver 2.1 : 24 Oct 2015
 * - added Sender so Server can echo back messages it receives
 * - added verbose mode to support debugging and learning
 * - to see more detail about what is going on in Sender and Receiver
 *   set Utilities.verbose = true
 * ver 2.0 : 20 Oct 2015
 * - Defined Receiver and used that to replace almost all of the
 *   original Server's functionality.
 * ver 1.0 : 18 Oct 2015
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
      using System.Threading;
      using System.Xml.Linq;
      using Util = Utilities;
     
     
   class Server
  {
    string address { get; set; } = "localhost";
    string port { get; set; } = "8080";

    //----< quick way to grab ports and addresses from commandline pass number of w and r define arguments for # e and r 10 >-----

    public void ProcessCommandLine(string[] args)
    {
      if (args.Length > 0)
      {
        port = args[0];
      }
      if (args.Length > 1)
      {
        address = args[1];
      }
    }

    static void Main(string[] args)
    {
      Util.verbose = false;
      TestExec noSQLdb = new TestExec();
      Server srvr = new Server();
      Stopwatch serverWatch = new Stopwatch();
      srvr.ProcessCommandLine(args);
      Console.Title = "Server";
      Console.Write(String.Format("\n  Starting CommService server listening on port {0}", srvr.port));
      Console.Write("\n ====================================================\n");

      Sender sndr = new Sender(Util.makeUrl(srvr.address, srvr.port));
      //Sender sndr = new Sender();
      Receiver rcvr = new Receiver(srvr.port, srvr.address);   // server declares reciever 
      // Peyman A. added these Funcs for Writers Transactions 
      Func<string, bool> writeAction = an_entry =>
      {
            Console.Write("<<<<<<<<<<<Write action is called>>>>>>>>> ");
            return  noSQLdb.toInsert(an_entry);
      };
      Func<string, bool> editAction = an_entry =>
      {
            Console.Write("<<<<<<<<<<<Edit action is called>>>>>>>>> ");
            return noSQLdb.toEdit(an_entry);
      };
      Func<string, bool> deleteAction = an_entry =>
      {
            Console.Write("<<<<<<<<<<<Delete action is called>>>>>>>>> ");
            return noSQLdb.toDelete(an_entry);
      };

      Func<EventLogger, bool> insertLog = an_entry =>
      {
            Console.Write("<<<<<<<<<<<Insert Log into DBElement>>>>>>>>> ");
            return noSQLdb.insertLog(an_entry);
      };

      Action showDBLogger = () =>
      {
            noSQLdb.TestR3();

      };
      // - serviceAction defines what the server does with received messages
      // - This serviceAction just announces incoming messages and echos them
      //   back to the sender.  
      // - Note that demonstrates sender routing works if you run more than
      //   one client.
      Action serviceAction = () =>
      {
            Message msg = null;
            
            while (true)
            {
                  msg = rcvr.getMessage();   // note use of non-service method to deQ messages
                  serverWatch.Restart(); // reset clock and start the processing time for server
                  Console.Write("\n  ^^-- Received --^^ message:");
                  Console.Write("\n  sender is {0}", msg.fromUrl);
                  Console.Write("\n  reciever is {0}", msg.toUrl);
                  Console.Write("\n  content is {0}\n", msg.content);
                  Console.Write("\n  Msg time is {0}\n", msg.whenOccured.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                  EventLogger alog = new EventLogger(msg.eventType,msg.whenOccured,msg.fromUrl);
                  alog.DisplayLog();
                  insertLog(alog);
                  showDBLogger();

                  //Process Message in Service action interpert the message 
                  // Create a Thread and Delegate , RUN the Query, return the results 
                  //Thread process = new Thread(start);
                  try 
                  { //this was for the testing project4 
                        if (msg.content.Contains("Insert"))
                             writeAction(msg.content);
                        if (msg.content.Contains("Edit"))
                              editAction(msg.content);
                        if (msg.content.Contains("Delete"))
                              deleteAction(msg.content);
                        if (msg.content.Contains("&"))
                        {
                              string[] codes = msg.content.Split('&');
                              Console.Write("\n Code and Key for query {0} {1}\n", codes[0], codes[1]);
                              var key = 0;
                              if ( !codes[1].Contains("k")) //!codes[1].Contains("k") ||
                                    key = Int32.Parse(codes[1]);
                              switch (codes[0])
                              {
                                    case "R7a":
                                          {
                                                if (key != 0)
                                                      msg.content = "Q" + noSQLdb.TestR7a(key);
                                                else
                                                      msg.content = "Q" + noSQLdb.TestR7a(codes[1]);
                                                break;
                                          }
                                    case "R7b":
                                          {
                                                if (key != 0)
                                                      msg.content = "Q" + noSQLdb.TestR7b(key);
                                                else
                                                      msg.content = "Q" + noSQLdb.TestR7b(codes[1]);
                                                break;
                                          }
                                    case "R7c": // matching pattern 
                                          {     
                                                if (key != 0)
                                                      msg.content = "Q" + noSQLdb.TestR7c(codes[1]);
                                                else
                                                      msg.content = "Q" + noSQLdb.TestR7c(codes[1]);
                                                break;
                                          }
                                    case "R7d":
                                          { msg.content = "Q" + noSQLdb.TestR7d(codes[2]); break; }
                                    case "R7e":
                                          { msg.content = "Q" + noSQLdb.TestR7e(codes[3],codes[4]); break;}
                              } //switch
                        }// if & 
                  } //end-try
                  catch (FormatException e)
                  {
                        Console.WriteLine(e.Message);
                  } 

                  if (msg.content == "connection start message")
                  {
                  continue; // don't send back start message
                  }
                  if (msg.content == "done")
                  {
                  Console.Write("\n  client has finished\n");
                  continue;
                  }
                  if (msg.content == "closeServer")
                  {
                  Console.Write("received closeServer");
                  break;
                  }
                  serverWatch.Stop();
                  msg.content = "received " + msg.content;
                // swap urls for outgoing message
                  Util.swapUrls(ref msg);
                  TimeSpan ts = serverWatch.Elapsed;
                  // Format and display the TimeSpan value.
                  string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                      ts.Hours, ts.Minutes, ts.Seconds,
                      ts.Milliseconds / 10);
                  Console.WriteLine("\n\nServer Msg processing time :" + elapsedTime);
                  sndr.sendMessage(msg);
        }
      };

      if (rcvr.StartService())
      {
        rcvr.doService(serviceAction); // This serviceAction asynchronous, so the call doesn't block.
      }
      Console.WriteLine("COCK SUCKER OF UINVERS IS YOU ");
      Util.waitForUser(); 
    }
  }
}
