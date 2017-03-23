using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Useful.Other
{
    /// <summary>
    ///     Simple Logger class
    /// </summary>
    public static class Logger
    {
        /// <summary>
        ///     Write all messages to console.
        /// </summary>
        public static bool ConsoleOut = true;

        /// <summary>
        ///     Prefix with full format datetime or short format/
        /// </summary>
        public static bool FullTime = false;

        /// <summary>
        ///     Log deeplog messages.
        /// </summary>
        public static bool DeepLog = false;

        /// <summary>
        ///     List of all logs received for now.
        /// </summary>
        public static readonly List<string> Logs = new List<string>(100);

        /// <summary>
        ///     Log and write to console.
        /// </summary>
        /// <param name="o">Object logged</param>
        /// <param name="deep">Is the log deep?</param>
        public static void Log(object o, bool deep)
        {
            if (!DeepLog && deep) return;
            var lines = o.ToString().Split('\n');
            var str = "[" + (FullTime ? DateTime.Now.ToLongTimeString() : DateTime.Now.ToShortTimeString()) + "]  ";
            foreach (var l in lines)
            {
                var s = str + l;
                if (ConsoleOut) Console.WriteLine(s);
                Logs.Add(s);
            }
        }

        /// <summary>
        ///     Save all logs to file.
        /// </summary>
        /// <param name="path">Path to save the file to.</param>
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