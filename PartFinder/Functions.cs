using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartFinder
{
    public static class Functions
    {
        /// <summary>
        /// Returns a list of all .cfg files in path and sub directories
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> FindFilesRec(string path, string extension)
        {
            List<string> result = new List<string>();
            if (!Directory.Exists(path))
            {
                return result;
            }
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var dir in di.GetDirectories())
            {
                result.AddRange(FindFilesRec(dir.FullName, extension));
            }
            foreach (var p in di.GetFiles())
            {
                if (p.FullName.EndsWith(extension))
                {
                    result.Add(p.FullName);
                }
            }
            return result;
        }


        /// <summary>
        /// Reads the prune list from file
        /// </summary>
        public static List<string> ReadPruneList(string path, out string error)
        {
            error = null;
            // load prune list
            FileStream fs = null;
            var list = new List<string>();
            try
            {
                fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    list.Add("\\GameData\\" + sr.ReadLine().Replace('/', '\\'));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                error = ("ERROR. Cannot read prune list: " + path.Substring(path.LastIndexOf("\\") + 1));
                return new List<string>();
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return list;
        }
    }
}
