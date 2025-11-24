///////////////////////////////////////////////////////////////
// DBElement.cs - Define element for noSQL database          //
// Ver 1.1                                                   //
// Application: Demonstration for CSE687-OOD, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell XPS2700, Core-i7, Windows 10            //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements the DBElement<Key, Data> type, used by 
 * DBEngine<key, Value> where Value is DBElement<Key, Data>.
 *
 * The DBElement<Key, Data> state consists of metadata and an
 * instance of the Data type.
 *
 * I intend this DBElement type to be used by both:
 *
 *   ItemFactory - used to ensure that all db elements have the
 *                 same structure even if built by different
 *                 software parts.
 *   ItemEditor  - used to ensure that db elements are edited
 *                 correctly and maintain the intended structure.
 *
 *   The Factory and Edit classes can be quite simple, I think,
 *   if you use the DBElement class.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DBElement.cs, UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.1 : 24 Sep 15
 * - removed extension methods, removed tests from test stub
 * - Testing now  uses DBEngineTest.cs
 * ver 1.0 : 13 Sep 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project4Starter
{
      /////////////////////////////////////////////////////////////////////
      // DBElement<Key, Data> class
      // - Instances of this class are the "values" in our key/value 
      //   noSQL database.
      // - Key and Data are unspecified classes, to be supplied by the
      //   application that uses the noSQL database.
      //   See the teststub below for examples of use.
      // IClone interface
      // - contract for building copies of PayloadWrapper<Data>
      //   and DBElement<Key, Data>
      //


      public class DBElement<Key, Data> : IClone, IPersist where Data : class, IClone, IPersist, new()
      {
      public string name { get; set; }          // metadata    |
      public string descr { get; set; }         // metadata    |
      public DateTime timeStamp { get; set; }   // metadata   value
      public List<Key> children { get; set; }   // metadata    |
      public Data payload { get; set; }         // data        |

      public DBElement(string Name = "unnamed", string Descr = "undescribed")
      {
      name = Name;
      descr = Descr;
      timeStamp = DateTime.Now;
      children = new List<Key>(); //initialize 
      }
      public DBElement()
      {
            name = "unnamed";
            descr = "undescribed";
            timeStamp = DateTime.Now;
            children = new List<Key>();
      }
      public string MetaDataToString()
      {
            StringBuilder accum = new StringBuilder();
            accum.Append(String.Format("\n  name: {0}", name));
            accum.Append(String.Format("\n  desc: {0}", descr));
            accum.Append(String.Format("\n  time: {0}", timeStamp));
            if (children.Count() > 0)
            {
                  accum.Append(String.Format("\n  Children: "));
                  bool first = true;
                  foreach (Key key in children)
                  {
                        if (first)
                        {
                              accum.Append(String.Format("{0}", key.ToString()));
                              first = false;
                        }
                        else
                              accum.Append(String.Format(", {0}", key.ToString()));
                  }
            }
            return accum.ToString();
      }

      public override string ToString()
      {
            StringBuilder accum = new StringBuilder();
            accum.Append(MetaDataToString());
            if (payload != null)
            {
                  accum.Append("\n  ");
                  accum.Append(payload.ToString());
            }
            return accum.ToString();
      }

      public string ToXml()
      {
            StringBuilder accum = new StringBuilder();
            accum.Append("\n  <element>");
            accum.Append(String.Format("\n    <name>{0}</name>", name));
            accum.Append(String.Format("\n    <descr>{0}</descr>", descr));
            accum.Append(String.Format("\n    <timeStamp>{0}</timeStamp>", timeStamp.ToString()));
            if (children.Count > 0)
            {
                  accum.Append("\n    <keys>");
                  foreach (Key key in children)
                  {
                        accum.Append(String.Format("\n      <key>{0}</key>", key));
                  }
                  accum.Append("\n    </keys>");
            }
            //accum.Append(String.Format("{0}", payload.ToXml())); <------  Figure OUT where to put it in ?
            accum.Append("\n  </element>");
            return accum.ToString();
            }

            public IClone Clone()
            {
                  DBElement<Key, Data> cloned = new DBElement<Key, Data>();
                  cloned.name = String.Copy(name);
                  cloned.descr = String.Copy(descr);
                  cloned.timeStamp = DateTime.Parse(timeStamp.ToString());
                  cloned.children = new List<Key>();
                  cloned.children.AddRange(children);
                  cloned.payload = payload.Clone() as Data;
                  return cloned;
                  throw new NotImplementedException();
            }

            string IPersist.ToXml()
            {
                  throw new NotImplementedException();
            }

            IPersist IPersist.FromXml(string xml)
            {
                  throw new NotImplementedException();
            }

            //public IClone Clone()
            //{
            //      DBElement<Key, Data> cloned = new DBElement<Key, Data>();
            //      cloned.name = String.Copy(name);
            //      cloned.descr = String.Copy(descr);
            //      cloned.timeStamp = DateTime.Parse(timeStamp.ToString());
            //      cloned.children = new List<Key>();
            //      cloned.children.AddRange(children);
            //      //cloned.payload = payload.Clone() as Data;
            //      return cloned;
            //}
            //----< instance from XML is not implemented for DBElements >--------
            /*
             * This is here to complete implementation of the IPersist Interface.
             * Eventually much of the DBEngine fromXml(string xml) functionality
             * will be moved here.
             */
            //public IPersist FromXml(string xml)
            //{
            //      string msg =
            //        "\n\n  FromXml in DBElement has no useful functionality.\n"
            //      + "  All that processing occurs in FromXml in DBExtensions\n"
            //      + "  used by instances of DBEngine.";
            //      System.Exception ex = new Exception(msg);
            //      throw ex;
            //}
            //public IClone Clone()
            //{
            //      DBElement<Key, Data> cloned = new DBElement<Key, Data>();
            //      cloned.name = String.Copy(name);
            //      cloned.descr = String.Copy(descr);
            //      cloned.timeStamp = DateTime.Parse(timeStamp.ToString());
            //      cloned.children = new List<Key>();
            //      cloned.children.AddRange(children);
            //      cloned.payload = payload.Clone() as Data;
            //      return cloned;
            //}
      }

#if (TEST_DBELEMENT)
  class TestDBElement
  {
    static void Main(string[] args)
    {
      "Testing DBElement Package".title('=');
      WriteLine();

      Write("\n  All testing of DBElement class moved to DBElementTest package.");
      Write("\n  This allow use of DBExtensions package without circular dependencies.");

      Write("\n\n");
    }
  }
#endif
}
