///////////////////////////////////////////////////////////////////
// TestExec.cs - Test Requirements for Project #2                //
// Ver 2.2                                                       //
// Application: Demonstration for CSE687-OOD, Project#2          //
// Language:    C#, ver 6.0, Visual Studio 2015                  //
// Platform:    Dell XPS2700, Core-i7, Windows 10                //
///////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package demonstrates that the NoSqlDb meets all requirements
 * of Project #2, F2015.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *   TestExec.cs,  DBElement.cs, DBEngine, PayloadWrapper, Display, 
 *   QueryEngine.cs, ItemFactory.cs, 
 *   DBExtensions.cs, UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 2.3 : 13 Nov 15  Peyman Ardekani Modifed and added new Methods to file for Project 4
 * ver 2.2 : 13 Oct 15
 * - modified TestR8() for modified VirtualDB cloning capability
 * - added dispatching of tests based on command line arguments 
 * ver 2.1 : 11 Oct 15
 * - small changes to TestR5() due to changes in the persistance interfaces
 *   and FromXml(...) implementations.
 * ver 2.0 : 10 Oct 15
 * - added code for all requirement tests and a demonstration of views.
 * - added alias type definitions and alias functions, using lambdas.
 * - added command line argument, Pause, to allow pausing after each
 *   demonstration.  
 * - If no commandline argument is present the demonstrations do not pause, 
 *   as requested in class.
 * ver 1.1 : 24 Sep 15
 * - minor changes to function TestR2() to accommodate changes in
 *   DBElement<Key, Data> and DBEngine<Key, Value>
 * ver 1.0 : 18 Sep 15
 * - first release
 *
 */
//ToDo: 6 - add Persistance package

using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Xml.Linq;
using System.Threading;
using static System.Console;
namespace Project4Starter
{
      ///////////////////////////////////////////////////////////////////////
      // These aliases simplify code text, but make it harder
      // to remember exactly what the code is trying to do.
      //
      using DBElemS = DBElement<int, PL_String>;
      using DBElemL = DBElement<string, PL_ListOfStrings>;
      using DBS = DBEngine<int, DBElement<int, PL_String>>;
      using DBL = DBEngine<string, DBElement<string, PL_ListOfStrings>>;
      using ItmFactS = ItemFactory<int, PL_String>;
      using ItmFactL = ItemFactory<string, PL_ListOfStrings>;
      using System.Xml;
      using System.Text;

      ///////////////////////////////////////////////////////////////////////
      // TestExec class
      // - organizes tests for each requirement
      // - supports dispatching one or more tests
      //
      class TestExec
      {
            List<int> tests = new List<int>();
            public static int keyId = 5;
            private DBS db1 = new DBS();
            private DBL db2 = new DBL();
            public TestExec()
            {
                  DBElemS elem = ItmFactS.create();
                  elem.name = "Microsoft";
                  elem.descr = "Today and Future";
                  elem.children.AddRange(new List<int> { 1, 2, 3});
                  elem.payload = new PL_String("Address: One Microsoft Way");
                  //Write(elem.ToString());
                  db1.insert(1, elem);
          
                  DBElemS elem1 = ItmFactS.create();
                  elem1.name = "H.A.L. 9000";
                  elem1.descr = "Good afternoon. I am a HAL 9000 computer. I became operational at the H.A.L. plant in Urbana, Illinois on the 12th of January 1992. My instructor was Mr. Langley, and he taught me to sing a song. If you'd like to hear it I can sing it for you.";
                  elem1.payload = new PL_String("Daisy, Daisy, give me your answer do. I'm half crazy all for the love of you. It won't be a stylish marriage, I can't afford a carriage. But you'll look sweet upon the seat of a bicycle built for two.");
                  db1.insert(2, elem1);
              
                  showDBS(db1);
                  //this.TestR2();
                  //WriteLine();

                  try
                  {
                        //DBS pdb = new DBS();
                        //String checkPath = System.IO.Path.GetFullPath("../../Req5.xml");
                        
                        ////pdb.FromXml<int, DBElement<int, PL_String>, PL_String>(doc);
                        //pdb.FromXml<int, DBElemS, PL_String>(doc);
                        //showDBS(pdb);
                        //DBL newdb = new DBL();
                        XDocument doc = XDocument.Load("../../Req5.xml");
                        this.db2.FromXml<string, DBElement<string, PL_ListOfStrings>, PL_ListOfStrings>(doc);
                        showDBL(db2);
                  }
                  catch (Exception e)
                  {
                        Console.WriteLine(e.StackTrace);
                  }


            }
            /////////////////////////////////////////////////////////////////////
            // These are function aliases - same comments as above
            //
            Action<IQuery<int, DBElemS>> showDBS = (IQuery<int, DBElemS> dbs) =>
    {
      dbs.showDB<int, DBElement<int, PL_String>, PL_String>();
    };

    Action<IQuery<string, DBElemL>> showDBL = (IQuery<string, DBElemL> dbl) =>
    {
      dbl.showDB<string, DBElement<string, PL_ListOfStrings>, PL_ListOfStrings>();
    };
            //private object retun;

            public bool pause { get; set; }
            public void checkPause()
    {
      if (pause)
      {
        Write("\n  Press any key to move to next demonstraton:");
        Console.ReadKey();
        WriteLine();
      }
    }
    //----< Testing Project #2, Requirement #2 >-------------------------

    void TestR2()
    {
      "Demonstrating Requirement #2".title('=');
      Write("\n    DBEngine works for two sets of Key/Value pair types");
      WriteLine();

      DBElemL los = ItmFactL.create();
      los.name = "DR. Evil";
      los.descr = "Dr. Evil is Star of Austain Power";
      los.children.AddRange(new List<string> { "one", "two", "three", "four"});
      los.payload = new PL_ListOfStrings(new List<string> { "item1", "Mini me", "Austin Power" });
      Write(los.ToString());
      db2.insert("Key1", los);
      showDBL(db2);

      DBElemL los2 = ItmFactL.create();
      los2.name = " Stevee Job-Waznick";
      los2.descr = "Two stevees who build a company called APPLE Computer";
      los2.children.AddRange(new List<string> { "one", "two", "three", "four" });
      los2.payload = new PL_ListOfStrings(new List<string> { "Apple", "Rocks", "Man!!" });
      Write(los2.ToString());
      db2.insert("Key2", los2);
      showDBL(db2);
       WriteLine();
      checkPause();
    }
    //----< Testing Project #2, Requirement #3 >-------------------------

    public void TestR3()
    {
      "Log items DBElement ".title('=');
       showDBS(db1);
      /*Write("\n    Adding and Deleting key/value pairs");
      WriteLine();
      "adding new element to DBEngine<string, PL_ListOfStrings>".title();
      DBElemL elem = ItmFactL.create();
      db2.insert("key2", elem);
      showDBL(db2);
      WriteLine();

      "removing the new element".title();
      db2.remove("key2");
      showDBL(db2);
      WriteLine();
      checkPause();*/
    }
    //----< Testing Project #2, Requirement #4 >-------------------------

    void TestR4()
    {
      "Demonstrating Requirement #4".title('=');
      Write("\n    Editing of MetaData and Payload");
      WriteLine();

      "original database".title();
      showDBL(db2);
      WriteLine();

      "database with edited element".title();
      DBElemL elem;
      db2.getValue("Key1", out elem);
      elem.name = elem.name + " modified";
      elem.descr = elem.descr + " modified";
      elem.children.AddRange(new List<string> { "alpha", "beta" });
      elem.payload.theWrappedData = new List<string> { "edit1", "edit2" };

      "demonstrating an insert of element created with ItemFactory".title();
      DBElemL elem1 = ItmFactL.create();
      db2.insert("Key2", elem1);
      showDBL(db2);
      WriteLine();
      checkPause();
    }
    //----< Testing Project #2, Requirement #5 >-------------------------

    void TestR5()
    {
      "Demonstrating Requirement #5".title('=');
      //Write("\n    Persist and Unpersist");
      showDBL(db2);
      DBElemL elem;
      db2.getValue("key1", out elem);
      elem.showElement();
      WriteLine();

                  elem.descr = "sanitized description has no markup characters";
      string XML = db2.ToXml<PL_ListOfStrings>();
      Write("\n --- XML from DBEngine<string, DBElement<string, PL_ListOfStrings> ---");
      Write(XML);
      XDocument doc = XDocument.Parse(XML);
      doc.Save("Req5.xml");
      WriteLine();

      Write("\n --- Creating new database from XML ---");
      DBL newdb = new DBL();
      newdb.FromXml<string, DBElement<string, PL_ListOfStrings>, PL_ListOfStrings>(doc);
      showDBL(newdb);
      WriteLine();
      checkPause();
    }
    //----< Testing Project #2, Requirement #6 >-------------------------

    void TestR6()
    {
      "Demonstrating Requirement #6".title('=');
      Write("\n    Scheduled persistance");
      WriteLine();
      Write("\n --- scheduling two persists 1 second apart ---\n");
      System.Timers.Timer schedular = new System.Timers.Timer();
      schedular.Interval = 1000;
      schedular.AutoReset = true;
      int count = 0;
      schedular.Elapsed += (object source, ElapsedEventArgs e) =>
      {
        ++count;
        string XML = db2.ToXml<PL_ListOfStrings>();
        Write("\n  Persisting Database");
        Write(XML);
        WriteLine();
        if (count == 2)
        {
          schedular.Enabled = false;
        }
      };
      schedular.Enabled = true;
      Thread.Sleep(3000);
      checkPause();
    }
    //----< QueryPredicate for Testing Project #2, Requirement #7d >-----

    Func<string, bool> MakeQueryForStringInMetadata(string testString, DBL pdb)
    {
      Func<string, bool> qp = (queryKey) =>
      {
        bool succeeded = false;
        DBElemL qelem;
        if (!pdb.getValue(queryKey, out qelem))
        {
          return false;
        }
        if (qelem.name.Contains(testString))
        {
          Write("\n  key {0} has name containing \"{1}\"", queryKey, testString);
          succeeded = true;
        }
        if (qelem.descr.Contains(testString))
        {
          Write("\n  key {0} has descr containing \"{1}\"", queryKey, testString);
          succeeded = true;
        }
        foreach (var child in qelem.children)
        {
          if (child.Contains(testString))
          {
            Write("\n  key {0} has child containing \"{1}\"", queryKey, testString);
            succeeded = true;
            break;
          }
        }
        return succeeded;
      };
      return qp;
    }
    //----< QueryPredicate for Testing Project #2, Requirement #7d >-----

    Func<string, bool> MakeQueryForNoChildren(DBL pdb)
    {
      Func<string, bool> qp = (queryKey) =>
      {
        DBElemL qelem;
        if (!pdb.getValue(queryKey, out qelem))
        {
          return false;
        }
        if (qelem.children.Count() > 0)
          return false;
        return true;
      };
      return qp;
    }
    //----< QueryPredicate for Testing Project #2, Requirement #7d >-----

    Func<string, bool> MakeQueryForTimeStamp(DateTime first, DateTime second, DBL pdb)
    {
      Func<string, bool> qp = (queryKey) =>
      {
        DBElemL qelem;
        if (!pdb.getValue(queryKey, out qelem))
        {
          return false;
        }
        if (qelem.timeStamp > first && qelem.timeStamp <= second)
          return true;
        return false;
      };
      return qp;
    }
    //----< Testing Project #2, Requirement #7a >-------------------------

    DBElemS TestR7a(DBS pdb, int testString)
    {
      Write(" --- retrieving value for specified key: {0} ---", testString);
      DBElemS elem;
      if (pdb.getValue(testString, out elem))
      {
        Write(elem.showElement());
      }
      else
        Write("\n  retrieval failed\n");
      WriteLine();
      return elem;
    }

      DBElemL TestR7a(DBL pdb, string testString)
      {
            Write(" --- retrieving value for specified key: {0} ---", testString);
            DBElemL elem;
            if (pdb.getValue(testString, out elem))
            {
                  Write(elem.showElement());
            }
            else
                  Write("\n  retrieval failed\n");
            WriteLine();
            return elem;
      }
 
    //----< Testing Project #2, Requirement #7b >-------------------------

    public String TestR7b( int testString)
    {
                  DBElemS elem;
                  String msg = "\n --- retrieving children of key:" + testString + "--- \n" ;
                  if (db1.getValue(testString, out elem))
                  {
                        //DisplayExtensions.DisplayCommaSeparatedList<string>(elem.children.);
                        //elem.children.DisplayCommaSeparatedList<string>;
                         bool first = true;
                        foreach (var t in elem.children)
                        { 
                              if (first)
                              {
                                    if (msg.Length > 0)
                                          msg += t.ToString() + ",";
                                    else
                                          msg += t.ToString(); 
                                    first = false;
                              }
                              else
                              {
                                    msg += t.ToString() + ",";
                              }
                        }
                        return msg;
                  }
                  else
                  {
                        Write("\n  retrieval failed\n");
                        return "No Children for this record";
                  }
    }

            //----< Testing Project #2,  String Key Requirement #7b >-------------------------

            public String TestR7b(string testString)
            {
                  DBElemL elem;
                  String msg = "\n --- retrieving children of key:" + testString + "--- \n";
                  if (db2.getValue(testString, out elem))
                  {
                        //DisplayExtensions.DisplayCommaSeparatedList<string>(elem.children.);
                        //elem.children.DisplayCommaSeparatedList<string>;
                        bool first = true;
                        foreach (var t in elem.children)
                        {
                              if (first)
                              {
                                    if (msg.Length > 0)
                                          msg += t.ToString() + ",";
                                    else
                                          msg += t.ToString();
                                    first = false;
                              }
                              else
                              {
                                    msg += "," + t.ToString();
                              }
                        }
                        return msg;
                  }
                  else
                  {
                        Write("\n  retrieval failed\n");
                        return "No Children for this record";
                  }
            }


      public string TestR7c(string testString)
      {
                  Write("\n --- set of all keys matching pattern \"{0}\" : --- ", testString);
                  List<string> matchingKeys = new List<string>();
                  foreach (var key in db2.Keys())
                  {
                        if (key.Contains(testString))
                              matchingKeys.Add(key);
                  }
                  bool first = true; String myKeys = " Set of all Keys matching \n  " ;
                  foreach (var t in matchingKeys)
                  {
                        if (first)
                        {
                              myKeys +=  t.ToString();
                              first = false;
                        }
                        else
                        {
                              myKeys += ", " + t.ToString();
                        }
                  }
                  return myKeys;
        }
   
     //----< Testing Project #2, Requirement #7c >-------------------------

    public void TestR7c(DBL pdb, string testString)
    {
      Write("\n --- set of all keys matching pattern \"{0}\": ---", testString);
      List<string> matchingKeys = new List<string>();
      foreach (var key in pdb.Keys())
      {
        if (key.Contains("Two"))
          matchingKeys.Add(key);
      }
      //matchingKeys.DisplayCommaSeparatedList<string>();
      WriteLine();
    }

      public string TestR7d( string testString)
      {
                  Write(" --- queryset of all elements containing {0} in their MetaData: ---", testString);
                  QueryEngine<string, DBElemL> qe = new QueryEngine<string, DBElemL>(db2);

                  Func<string, bool> qp1 = MakeQueryForStringInMetadata(testString, db2);
                  qe.Query(qp1);
                  WriteLine();

                  Write("\n --- testing compound query ---" + testString);
                  Write("\n --- uses previous query followed by query for elements with no children ---");
                  WriteLine();

                  qp1 = MakeQueryForStringInMetadata("P", db2);
                  Func<string, bool> qp2 = MakeQueryForNoChildren(db2);

                  qe.add(qp1);
                  qe.add(qp2);
                  VirtualDB<string, DBElemL> vdb = qe.doQueries();
                  //List<string> cqueryKeys = vdb.Keys();
                  List<string> cqueryKeys = qe.doQueries().Keys() as List<string>;
                  if (cqueryKeys.Count() == 0)
                        return ("\n  compound query failed");
                  else
                  {
                        Write("\n  compound query succeeded\n  returned keyset is:");
                        //return DisplayExtensions.CommaSeparatedList(cqueryKeys);
                        bool first = true; String myKeys = "\n  compound query succeeded\n  returned keyset is: ";
                        foreach (var t in cqueryKeys)
                        {
                              if (first)
                              {
                                    myKeys += t.ToString();
                                    first = false;
                              }
                              else
                              {
                                    myKeys += ", " + t.ToString();
                              }
                        }
                        return myKeys;
                  }
            }

   //----< Testing Project #2, Requirement #7d >-------------------------
    void TestR7d(DBL pdb, string testString)
    {
      Write(" --- testing query for set of all elements containing the string \"{0}\" in their MetaData: ---", testString);
      QueryEngine<string, DBElemL> qe = new QueryEngine<string, DBElemL>(pdb);

      Func<string, bool> qp1 = MakeQueryForStringInMetadata(testString, pdb);
      qe.Query(qp1);
      WriteLine();

      Write("\n --- testing compound query ---");
      Write("\n --- uses previous query followed by query for elements with no children ---");
      WriteLine();

      qp1 = MakeQueryForStringInMetadata("Evil", pdb);
      Func<string, bool> qp2 = MakeQueryForNoChildren(pdb);

      qe.add(qp1);
      qe.add(qp2);
      VirtualDB<string, DBElemL> vdb = qe.doQueries();
      List<string> cqueryKeys = vdb.Keys();
      //List<string> cqueryKeys = qe.doQueries().Keys() as List<string>;
      WriteLine();

      if (cqueryKeys.Count() == 0)
        Write("\n  compound query failed");
      else
      {
        Write("\n  compound query succeeded\n  returned keyset is:");
        //cqueryKeys.DisplayCommaSeparatedList<string>();
      }
      Write("\n --- Virtual db resulting from query ---");
      showDBL(vdb);
      WriteLine();
    }
    //----< Testing Project #2, Requirement #7e >-------------------------

    public string TestR7e( string afirst, string asecond)
    {
      Write("\n --- testing timeStamp query ---");
      Write("\n  testing for time stamps in the range {0} to {1}", afirst, asecond);
      DateTime first = DateTime.Parse(afirst);
      DateTime second = DateTime.Parse(asecond);
      DBElemL elem1 = new DBElemL();
      elem1.timeStamp = DateTime.Now.AddDays(-1);
      DBElemL elem2 = new DBElemL();
      elem2.timeStamp = DateTime.Now.AddDays(-2);
      DBElemL elem3 = new DBElemL();
      elem3.timeStamp = DateTime.Now.AddDays(-3);
      DBL pdb = new DBL();
      pdb.insert("key1", elem1);
      pdb.insert("key2", elem2);
      pdb.insert("key3", elem3);
      showDBL(pdb);
      WriteLine();


      Func<string, bool> q = MakeQueryForTimeStamp(first, second, pdb);//db2
      QueryEngine<string, DBElemL> qe = new QueryEngine<string, DBElemL>(pdb);
      List<string> keys = qe.Query(q).Keys() as List<string>;
      if(keys.Count() == 0)
      {
        return ("\n There is no results with date between " + first.ToString() + " and " + second.ToString());
      }
      DBElemL elem;
      String dateReturn = "\n  result of query for timeStamp = \n"; 
      foreach(string key in keys)
      {
         pdb.getValue(key, out elem);
         dateReturn += "TimeStamp for " + key.ToString() + " : " + elem.timeStamp.ToString() + " \n";
      }
      return dateReturn; 
    }

      public string TestR7a(String aKey)
      {
            "Reteriving <<<String>>>  Key".title('=');
            if (db2.containsKey(aKey))
            {
                  DBElemL elem = TestR7a(db2, aKey);
                  return elem.showElement();
            }
            return "No elements were found";
      }
            //----< Testing Project #2, Requirement #7 >--------------------------

      public string TestR7a(int aKey)
    {
      "Reteriving int Key".title('=');
      if (db1.containsKey(aKey))
      {
            DBElemS elem = TestR7a(db1, aKey);
            return elem.showElement();
      }
      return "No elements were found";
    }
    //----< Testing Project #2, Requirement #8 >-------------------------

    public void TestR8()
    {
      "Demonstrating Requirement #8".title('=');
      Write("\n    Creation of immutable database from query");
      WriteLine();

      DBS pdb = new DBS();
      XDocument doc = XDocument.Load("../../PackageStructure.xml");
      pdb.FromXml<int, DBElemS, PL_String>(doc);

      "making query for keys containing \"DB\" and length less than 10 chars from Project #2 db:".title();
      VirtualDB<int, DBElemS> db = new VirtualDB<int, DBElemS>(pdb);
      db.clear();
      foreach (int key in pdb.Keys())
      {
            //if (key.Contains("DB") && key.Count() < 10)
                  db.addKey(key);
      }
      showDBS(db);
      WriteLine();

                  "making  clone of VirtualDB returned by query:".title();
      db.cloneDB();
      showDBS(db);
      WriteLine();

      "editing clone's DBElement value:".title();
      DBElemS elem; 
      db.getValue(1, out elem);//"DBElement"
                              //elem.name = elem.name + " edited";   cause null exceptions
                              //elem.children.Add("edited child");
                              //Write(elem.showElement());
                              //WriteLine();

                  "modified clone:".title();
      showDBS(db);
      WriteLine();

      "view of original database for those elements:".title();
      Action<string, DBElemL> view = (string key, DBElemL velem) =>
      {
        if (key.Contains("DB") && key.Count() < 10)
        {
          Write("\n  -- Key = {0} --", key);
          Write(velem.showElement());
          WriteLine();
        }
      };
      //pdb.showView(view);

      Write("\n  Note that no change is observed in the original database.");
      WriteLine();

      Write("\n  - Examine interface of VirtualDB in package QueryEngine.");
      Write("\n    Note there are no functions that modify the database.");
      Write("\n  - Examine VirtualDB.makeClone()");
      Write("\n    Note that any editing operations on the clone can't affect");
      Write("\n    the original database.");
      WriteLine();
      checkPause();
    }
    //----< Testing Project #2, Requirement #9 >--------------------------

    void TestR9()
    {
      "Demonstrating Requirement #9".title('=');
      Write("\n    Project2 structure unpersisted from XML file");

      DBL pdb = new DBL();

      ////////////////////////////////////////////////////////////////////////////////
      // Uncomment the next two lines to demonstrate unpersisting with augmentation
      //DBElemL elem = new DBElemL();
      //pdb.insert("Test:dummy_element", elem);

      XDocument doc = XDocument.Load("../../PackageStructure.xml");
      pdb.FromXml<string, DBElemL, PL_ListOfStrings>(doc);
      showDBL(pdb);
      WriteLine();
      checkPause();
    }
    public void DemoViews()
    {
      /*
       * Demonstrates that users can easily construct views and apply
       * to each element of a DBEngine or VirtualDB.
       *
       * A view is an Action delegate that accepts a key and its
       * corresponding element and displays some information about
       * that key/value pair.
       *
       * showView(view) loops through all the keys in a database and,
       * for each key, extracts its value and invokes the view with
       * that key and value.
       */
      "Demonstrating Views functionality".title('=');
      WriteLine();

      Write("\n --- demonstrating relationships ---");
      WriteLine();

      DBL pdb = new DBL();
      XDocument doc = XDocument.Load("../../PackageStructure.xml");
      pdb.FromXml<string, DBElemL, PL_ListOfStrings>(doc);
      Action<string, DBElemL> relationshipView =
        (string key, DBElemL elem) =>
        {
          Write("\n  Key: {0}", key);
         // elem.children.DisplayCommaSeparatedList<string>("\n  ChildKeys:\n  ");
        };
      pdb.showView(relationshipView);
      WriteLine();

      Write("\n --- demonstrating view of payloads ---");
      WriteLine();

      Action<string, DBElemL> payloadView =
        (string key, DBElemL elem) =>
        {
          Write("\n  Key: {0}", key);
          //elem.payload.theWrappedData.DisplayCommaSeparatedList<string>("\n  ");
        };
      pdb.showView(payloadView);
      WriteLine();

      checkPause();
    }
    
//----< To Insert records from client writes com channel >-------------------------
 public bool toInsert(string anEntery)
  {
            Write("\n  - <<<<<<<<<< Inserting a record. >>>>>>>>>> \n");
            XDocument xmlTree;
            xmlTree = XDocument.Parse(anEntery);
            try
            {
                  string sName = "", sDesc="", aKey = "";
                  int numChild = 0, numPayload=0;
                  var name = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("name")
                              select (string)elements).ToList();
                  foreach (var item in name)
                  {
                        sName = item.ToString();
                  };

                  var descr = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("descr")
                              select (string)elements).ToList();
                        foreach (var item in descr)
                        {
                              sDesc = item.ToString();
                        };

                  var payload = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("payload")
                                    select (string)elements).ToList();

                  numPayload = payload.Count();
                  var children = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("children")
                  select (string)elements).ToList();
                  numChild = children.Count();
                  DBElemL elem = ItmFactL.create(sName, sDesc, numChild, numPayload);
                        var key = (from elements in xmlTree.Elements("msgType").Elements("key")
                                   select (string)elements).ToList();
                        foreach (var item in key)
                        {
                              aKey = item.ToString();
                        }; //Utilities.keyGen(keyId)
                        db2.insert(aKey, elem);
                  showDBL(db2);
                  return true;
            }
            catch (Exception e)
            {
                  Write("An error occured in write processing", e.StackTrace);
                  throw;
            }
     }
//----< To Edit records of NoSQL from client writes com channel >-------------------------
      public bool toEdit(string anEntery)
      {
                  Write("\n  - <<<<<<<<<< To Edit a record. >>>>>>>>>> \n");
                  XDocument xmlTree;
                  xmlTree = XDocument.Parse(anEntery);
                  try
                  {     string aName = "", aDesc = "", aKey = "", aPayload = "", aChild ="";
                        int numChild = 0, numPayload = 0;
                        var name = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("name")
                                    select (string)elements).ToList();
                        foreach (var item in name)
                        {
                              aName = item.ToString();
                        };
                        var descr = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("descr")
                                     select (string)elements).ToList();
                        foreach (var item in descr)
                        {
                              aDesc = item.ToString();
                        };
                        var payload = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("payload")
                                       select (string)elements).ToList();

                        numPayload = payload.Count();
                        foreach (var item in payload)
                        {
                              aPayload += item.ToString();
                        };
                        var children = (from elements in xmlTree.Elements("msgType").Elements("element").Elements("children")
                                        select (string)elements).ToList();
                        numChild = children.Count();
                        foreach (var item in children)
                        {
                              aChild += item.ToString();
                        };
                        var key = (from elements in xmlTree.Elements("msgType").Elements("key")
                                   select (string)elements).ToList();
                        foreach (var item in key)
                        {
                              aKey = item.ToString();
                        }; //Utilities.keyGen(keyId)
                        "database with edited element".title();
                        DBElemL anEdit;
                        db2.getValue(aKey, out anEdit);
                        anEdit.name = aName;
                        anEdit.descr = aDesc;
                        anEdit.children.AddRange(new List<string> { aChild});
                        anEdit.payload.theWrappedData = new List<string> { aPayload };
                        showDBL(db2);
                        return true;
                  }
                  catch (Exception e)
                  {
                        Write("An error occured in write processing Edit",e.StackTrace);
                        return false;
                  }
            }
  //----< To Delete records of NoSQL from client writes com channel >-------------------------
  public bool toDelete(string anEntery)
  {
                  Write("\n  - <<<<<<<<<< To Delete a record. >>>>>>>>>> \n");
                  XDocument xmlTree;
                  xmlTree = XDocument.Parse(anEntery);
                  try
                  {
                        string aKey = "";
                        var key = (from elements in xmlTree.Elements("msgType").Elements("key")
                                   select (string)elements).ToList();
                        foreach (var item in key)
                        {
                              aKey = item.ToString();
                        }; //Utilities.keyGen(keyId)
                        "database with edited element".title();
                        db2.remove(aKey);
                        WriteLine("---------Spitting out DB---------");
                        showDBL(db2);
                        return true;
                  }
                  catch (Exception e)
                  {
                        Write("An error occured in write processing Edit", e.StackTrace);
                        return false;
                  } }

//----< Record Logs into NoSQLDB >---------------------
public bool insertLog(EventLogger aLog)
{
                  try
                  {
                        DBElemS elem = ItmFactS.create();
                        elem.name = aLog.eventType;
                        elem.descr = aLog.itsOrigin;
                        elem.timeStamp = aLog.occurrenceTime;
                        elem.children.AddRange(new List<int> { 1, 2, 3 });
                        elem.payload = new PL_String("An Event from {1} " + aLog.itsOrigin);
                        keyId++;
                        db1.insert(keyId, elem);
                        return true;
                  }
                  catch (Exception e)
                  {
                        Console.WriteLine(e.Data);
                        return false;
                  }
}

//----< collect test requests from commandline >---------------------
void processCommandLine(string[] args)
{
      if (args.Length > 0)
      {
        if (args[0] == "pause")
          pause = true;
      }
      foreach (string arg in args)
      {
        try
        {
          int testNum = Int32.Parse(arg);
          tests.Add(testNum);
        }
        catch
        {
          continue;
        }
      }
      if (tests.Count() == 0)
      {
        for (int i = 2; i <= 8; ++i)
          tests.Add(i);
      }
    }
    //void dispatchTests()
    //{
    //  foreach (int test in tests)
    //  {
    //    switch (test)
    //    {
    //      case 2:
    //        TestR2(); break;
    //      case 3:
    //        TestR3(); break;
    //      case 4:
    //        TestR4(); break;
    //      case 5:
    //        TestR5(); break;
    //      case 6:
    //        TestR6(); break;
    //      case 7:
    //        TestR7(); break;
    //      case 8:
    //        TestR8(); break;
    //      case 9:
    //        TestR9(); break;
    //      case 10:
    //        DemoViews(); break;
    //    }
    //  }
    }
            //----< Entry point >------------------------------------------------
#if (test_main)

            static void Main(string[] args)
            {
                  TestExec exec = new TestExec();
                  "Demonstrating Project#2 Requirements".title('=');
                  WriteLine();

                  exec.processCommandLine(args);
                  exec.dispatchTests();
                  exec.toDo();

                  Write("\n\n");
            } 
#endif
      }
//}
