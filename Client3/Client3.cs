using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Project4Starter 
{
      using System.Diagnostics;
      using System.Xml.Linq;
      using Util = Utilities;

      class Client
      {
            string localUrl { get; set; } = "http://localhost:8084/CommService";
            string remoteUrl { get; set; } = "http://localhost:8080/CommService";
            
            //----< retrieve urls from the CommandLine if there are any >--------
            public void processCommandLine(string[] args)
            {
                  if (args.Length == 0)
                        return;
                  localUrl = Util.processCommandLineForLocal(args, localUrl);
                  remoteUrl = Util.processCommandLineForRemote(args, remoteUrl);
            }

            static void Main(string[] args)
            {
                  Console.Write("\n  starting CommService client");
                  Console.Write("\n =============================\n");

                  Console.Title = "Client #3 is a Writer Process an XML files with Insert, Edit, Delete ";
                  Stopwatch stopWatch = new Stopwatch();
                  Client clnt = new Client();
                  clnt.processCommandLine(args);

                 
                  string localPort = Util.urlPort(clnt.localUrl);
                  string localAddr = Util.urlAddress(clnt.localUrl);
                  Receiver rcvr = new Receiver(localPort, localAddr);
                  Message msg = new Message();
                  msg.fromUrl = clnt.localUrl;
                  msg.toUrl = clnt.remoteUrl;
                  if (rcvr.StartService())
                  {
                        rcvr.doService(rcvr.defaultServiceAction());
                        //stopWatch.Stop();
                        //rcvr.serverProcessMessage(msg, stopWatch);
                        //rcvr.showTime(stopWatch, out aTime);
                  }

                  Sender sndr = new Sender(clnt.localUrl);  // Sender needs localUrl for start message

                  //Message msg = new Message();
                  //msg.fromUrl = clnt.localUrl;
                  //msg.toUrl = clnt.remoteUrl;

                  Console.Write("\n  sender's url is {0}", msg.fromUrl);
                  Console.Write("\n  attempting to connect to {0}\n", msg.toUrl);

                  if (!sndr.Connect(msg.toUrl))
                  {
                        Console.Write("\n  could not connect in {0} attempts", sndr.MaxConnectAttempts);
                        sndr.shutdown();
                        rcvr.shutDown();
                        return;
                  }
                  XDocument anXML = null; 
                  try
                  {
                        msg = new Message();
                        msg.fromUrl = clnt.localUrl;
                        msg.toUrl = clnt.remoteUrl;
                        anXML = XDocument.Load("../../Writers.xml"); //EditDelete
                        stopWatch.Start();
                        foreach (XElement item in anXML.Root.Elements())
                        {
                              //"Insert&key" + (++counter).ToString();
                              msg.content = item.ToString();
                              Console.Write("\n  Sending {0}", msg.content);
                              if (!sndr.sendMessage(msg))
                                    return;
                              Thread.Sleep(100);
                        }
                  }
                  catch (Exception ex)
                  {     
                        Console.WriteLine("\nError occured in the file loading.  " + ex.Message);
                  }
                  
                  stopWatch.Stop();
                  TimeSpan ts = stopWatch.Elapsed;
                  // Format and display the TimeSpan value.
                  string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                      ts.Hours, ts.Minutes, ts.Seconds,
                      ts.Milliseconds / 10);
                  Console.WriteLine("\n\n Req # 5 Time to process msgs from xml file: " + elapsedTime);
                  msg = new Message();
                  msg.fromUrl = clnt.localUrl;
                  msg.toUrl = clnt.remoteUrl;
                  msg.content = "done";
                  sndr.sendMessage(msg);

                  // Wait for user to press a key to quit.
                  // Ensures that client has gotten all server replies.
                  Util.waitForUser();

                  // shut down this client's Receiver and Sender by sending close messages
                  rcvr.shutDown();
                  sndr.shutdown();

                  Console.Write("\n\n");
            }
      }
}
