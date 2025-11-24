using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project4Starter
{
      public class EventLogger
      {
            public string eventType { get; }   // gets value from Msg WCF
            public DateTime occurrenceTime { get; }
            public string itsOrigin { get;  }
            TimeSpan timeDelay { get; set; }


            public EventLogger(string aEvent, DateTime aTime, string aOrigin)
            {
                  eventType = aEvent;
                  occurrenceTime = aTime;
                  itsOrigin = aOrigin;
             
            }

            public void DisplayLog()
            {
                  Console.Write(" \n Events From Test-Harness Server: \n");
                  Console.WriteLine("Event Type: {0} ", this.eventType);
                  Console.WriteLine("Event's Origin: {0} ", this.itsOrigin);
                  // Have to do some calculation of time od MSG - TIME.NOW
                  TimeSpan timeDelay = this.occurrenceTime - DateTime.Now;
                  Console.WriteLine("Event's Date and Time with Milliseconds: {0}",
                           this.occurrenceTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                  Console.WriteLine("Time of travel: {0}",
                           timeDelay.ToString(@"dd\.hh\:mm\:ss"));
            }

//#if (Test_EventLogger)
           static void Main(string[] args)
            {

                  Console.Title = " Event Logger ";
                  Console.Write("\n  Starting Event Logger");
                  Console.Write("\n ===============================\n");

                  EventLogger testHarness = new EventLogger("Build This Package", DateTime.Now, "Repository Server");

                  Console.Write(" \n An Event Log \n");
                  Console.WriteLine(testHarness.eventType);
                  Console.WriteLine(testHarness.itsOrigin);
                  Console.WriteLine("Date and Time with Milliseconds: {0}",
                           testHarness.occurrenceTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                  Console.WriteLine(testHarness.occurrenceTime.ToString("s.ffff"));
                  testHarness.occurrenceTime.AddMilliseconds(6.0);
                  testHarness.occurrenceTime.AddSeconds(20);
                  testHarness.DisplayLog();
                  testHarness.ToString();
            }
//#endif
     }
}

      

       


