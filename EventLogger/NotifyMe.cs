using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventLogger
{
      public class Notifier
      {
            Thread t = null;
            object sync_ = new object();
            public Action<string> notify
            {
                  get;
                  set;
            } = null;
            public void join()
            { t.Join(); }
            virtual public void doNotification()
            { Console.Write("\n  no one is listening"); }

            public void asyncRun()
            {
                  ThreadStart ts = () =>
                  {
                        int count = 0;
                        while (true)
                        {
                              Console.Write("\n  running");
                              if (++count % 5 == 0)
                              {
                                    if (notify != null)
                                          notify.Invoke("this is a notification from delegate"); //notify the deleget
                                    doNotification(); // notify via virtual function call
                              }
                              if (count > 20)
                                    break;
                              Thread.Sleep(100);
                        }
                  };
                  t = new Thread(ts); t.Start();
            }
      }
      class MT3Q4
      {
            void notifyHandler(string msg) { Console.Write("\n {0}", msg); }
            class GetNotified : Notifier { public override void doNotification() { Console.Write("\n Got notification from overload of virtual method"); } }
            static void Main(string[] args)
            {
                  //"MT3Q4 - Asynchronous Notifier".title(); 
                  MT3Q4 mt4q7 = new MT3Q4(); GetNotified notifier = new GetNotified(); notifier.notify += (msg) => { mt4q7.notifyHandler(msg); }; // register
                  notifier.asyncRun();
                  notifier.join();
                  Console.Write("\n\n");
            }
      }
}
