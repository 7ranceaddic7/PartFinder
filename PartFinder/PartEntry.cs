using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace PartFinder
{
    public class PartEntry
    {
        /// <summary>
        /// The (not shown) name of the part (used in the config files)
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The title the part got directly assigned
        /// </summary>
        public string Title;
        /// <summary>
        /// The relative path to the game data folder
        /// </summary>
        public string Path;
        /// <summary>
        /// The absolute path (so its easy to find)
        /// </summary>
        public string FullPath;
        /// <summary>
        /// If true, the part was already pruned or is on the actual prune list
        /// </summary>
        public bool Pruned;

        /// <summary>
        /// How many parts are defined in the file
        /// </summary>
        public int SharingFiles;

        /// <summary>
        /// If this is true the entry is just a alternation
        /// of another part @PART[]
        /// </summary>
        public bool IsMod;

        /// <summary>
        /// If this is true, the part is a copy of another part
        /// it can be removed but the other may not if this is not
        /// </summary>
        public bool IsCopy;

        /// <summary>
        /// If this is true, the part is a replacement of another part
        /// it can be removed but then the original part need to be removed as well
        /// </summary>
        public bool IsReplace;

        /// <summary>
        /// If this is true, the parent part is deleted
        /// </summary>
        public bool IsDeletion;

        /// <summary>
        /// If this is a copy, mod or replacement, the Parent
        /// stores the original node
        /// </summary>
        public string Parent;

        /// <summary>
        /// All titles which are from parent parts
        /// or derived parts. Or overwritten title
        /// </summary>
        public List<string> TitleFamily;

        /// <summary>
        /// A list of textures or models used for the part.
        /// (For creation of a resource unprune list)
        /// </summary>
        public List<string> Resources;


        public PartEntry(string path, string fullpath)
        {
            Path = path;
            FullPath = fullpath;
            Resources = new List<string>();
        }

        ///// <summary>
        ///// Constructor.
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="path"></param>
        ///// <param name="fullpath"></param>
        ///// <param name="pruned"></param>
        //public PartEntry(string name, string mainTitle, string path, string fullpath, bool pruned)
        //{
        //    IsMod = name != null && name.StartsWith("@@@");
        //    IsCopy = name != null && name.StartsWith("+++");
        //    if (IsMod || IsCopy)
        //    {
        //        Name = name.Substring(3);
        //    }
        //    else
        //    {
        //        Name = name;
        //    }
        //    Title = mainTitle;
        //    Path = path;
        //    FullPath = fullpath;
        //    Pruned = pruned;
        //    Debug.Assert(path.StartsWith("\\GameData\\"));
        //}

        /// <summary>
        /// Printable string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string prefix = IsMod ? "@" : (IsCopy ? "+" : (IsReplace? "%" : " "));
            string titles = "";

            if (TitleFamily != null)
            {
                foreach (string t in TitleFamily)
                {
                    if (!t.Equals(Title))
                    {
                        titles += (titles.Length > 0 ? ", " : "") + t;
                    }
                }
            }

            if (titles.Length > 0)
            {
                return prefix + "[" + Name + "] " + Title + " { " + titles + " } @ " + Path;//.Substring(9);
            }
            else
            {
                return prefix + "[" + Name + "] " + Title + " @ " + Path;//.Substring(9);
            }
        }

        internal bool MatchesPattern(string pattern)
        {
            if (TitleFamily != null)
            {
                foreach (string s in TitleFamily)
                {
                    if (s.ToLower().Contains(pattern))
                    {
                        return true;
                    }
                }
            }
            return (Title != null && Title.ToLower().Contains(pattern)) 
                || (Name != null && Name.ToLower().Contains(pattern));
        }
    }
}
