using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Useful
{
  public static class Logger
  {
    public static bool FullTime = false;
    public static bool DeepLog = false;
    public static readonly List<string> Logs = new List<string>(100);

    public static void Log(object o,bool deep)
    {
        if (DeepLog || !deep)
        {
            string str = "[" + (FullTime ? DateTime.Now.ToLongTimeString() : DateTime.Now.ToShortTimeString()) + "]  " +
                         o;
            Console.WriteLine(str);
            Logs.Add(str);
        }
    }

    public static void Save(string path)
    {
      try
      {
        File.WriteAllLines(path, Logs, Encoding.Unicode);
      }
      catch
      {
        Console.WriteLine("Couldn't save logs!");
      }
    }
  }
}
