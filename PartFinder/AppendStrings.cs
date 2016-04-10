using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public static class AppendStrings
    {
        public static string Append(char separator, params object[] args)
        {
            if (args.Length < 1)
            {
                throw new Exception();
            }
            string s = args[0].ToString();
            for (int i = 1; i < args.Length; i++)
            {
                s += separator + (args[i] ?? "null").ToString();
            }
            return s;
        }

        public static string Append(string separator, params object[] args)
        {
            if (args.Length == 0)
            {
                return "";
            }
            string s = args[0].ToString();
            for (int i = 1; i < args.Length; i++)
            {
                s += separator + (args[i] ?? "null").ToString();
            }
            return s;
        }

        public static string Append<T>(string separator, List<T> args)
        {
            if (args.Count == 0)
            {
                return "";
            }
            string s = args[0].ToString();
            for (int i = 1; i < args.Count; i++)
            {
                s += separator + (args[i] != null ? args[i].ToString() : "null");
            }
            return s;
        }
    }
}
