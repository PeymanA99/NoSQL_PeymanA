/////////////////////////////////////////////////////////////////////////
// MessageMaker.cs - Construct ICommService Messages                   //
// ver 1.0                                                             //
/////////////////////////////////////////////////////////////////////////
/*
 * Purpose:
 *----------
 * This is a placeholder for application specific message construction
 *
 * Additions to C# Console Wizard generated code:
 * - references to ICommService and Utilities
 */
/*
 * Maintenance History:
 * --------------------
 * Peyman Ardekani made changes for Project 5
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
  public class MessageMaker
  {
    public static int msgCount { get; set; } = 0;
    public Message makeMessage(string fromUrl, string toUrl)
    {
      Message msg = new Message();
      msg.fromUrl = fromUrl;
      msg.toUrl = toUrl;
      msg.whenOccured = DateTime.Now;
      msg.eventType = "Test Completed!";
      msg.actionCode = "Display";
      msg.content = String.Format("\n Tesing Message #{0}", ++msgCount);
      return msg;
    }

      public Message makeMessage(string fromUrl, string toUrl, string aContant)
      {
            Message msg = new Message();
            msg.fromUrl = fromUrl;
            msg.toUrl = toUrl;
            msg.whenOccured = DateTime.Now;
            msg.eventType = "Test Completed!";
            msg.actionCode = "Display";
            msg.content = aContant;
            return msg;
      }
#if (TEST_MESSAGEMAKER)
 static void Main(string[] args)
    {
      MessageMaker mm = new MessageMaker();
      Message msg = mm.makeMessage("fromFoo", "toBar");
      Utilities.showMessage(msg);
      Console.Write("\n\n");
    }
#endif
  }
}
