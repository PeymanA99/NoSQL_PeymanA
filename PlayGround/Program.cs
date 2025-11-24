using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PlayGround
{
      class Program
      {
            static bool _done;
            static readonly object _locker = new object();

            static void Main(string[] args)
            {
                  //Thread t = new Thread(WriteY);
                  //t.Start();
                  //WriteY();
                  //t.Join();  //sleep and join thred is blocked
                  //bool blocked = (t.ThreadState != 0);
                  //if (blocked)
                  //      Console.WriteLine("Thread is blocked for now");

                  //Console.WriteLine("Thread t has ended");
                  ////for (int i = 0; i < 1000; i++) Console.Write("x");

                  //bool done = false;
                  //ThreadStart action = () =>
                  //{
                  //      if (!done)
                  //      { done = true; Console.WriteLine("Done"); }
                  //};
                  //new Thread(action).Start();
                  //action();
                  //new Thread(Go).Start();

                  //Thread t = new Thread(() => Print(" HEllo This is print"));
                  //t.Start();
                  //t.Join();

                  string text = "T1";
                  Thread t1 = new Thread(() => Console.WriteLine(text));
                  t1.Start();
                  text = "Tool";
                  Thread t2 = new Thread(() => Console.WriteLine(text));
                  t2.Start();
                  //t2.Sleep(10000); 
 
             }

            static void Print (string message) { Console.WriteLine(message); }
            static void Go() 
            {
              lock(_locker)
              {
                if (!_done) { Console.WriteLine("Done"); _done = true; }
              }
            }
            static void WriteY()
            {
                 
                  for (int i = 0; i < 1000; i++) Console.Write("Y");

            }
      }
}
