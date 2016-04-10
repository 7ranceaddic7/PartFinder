﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace PartFinder
{
    public class PartFinder
    {
        /// <summary>
        /// Where is the ksp install
        /// </summary>
        private string _installPath;

        /// <summary>
        /// Path of the prune file.
        /// </summary>
        private string _prunePath;

        private string _unprunePath
        {
            get
            {
                return _prunePath.Substring(0, _prunePath.LastIndexOf(".") + 1) + "unprune.prnl";
            }
        }
        /// <summary>
        /// All parts of the game data
        /// </summary>
        public List<PartEntry> PartEntries;

        public Dictionary<string, List<PartEntry>> PartDict;

        /// <summary>
        /// The actual prune list as a list of stings (paths)
        /// </summary>
        public List<string> PruneList;

        /// <summary>
        /// Creates an extra list for things which are pruned but
        /// shall not be
        /// </summary>
        public List<string> UnPruneList;

        /// <summary>
        /// How many parts are already in a prune list or
        /// in the actual prune list
        /// </summary>
        public int NofPrunedParts;

        public bool NeedsUpdate;

        private IPrint _output;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="installPath"></param>
        /// <param name="output"></param>
        public PartFinder(IPrint output, string installPath, string prunePath)
        {
            _output = output;
            _prunePath = prunePath;
            _installPath = installPath;
            NeedsUpdate = true;

            LoadPruneLists();
        }

        private void LoadPruneLists()
        {
            string err;
            PruneList = Functions.ReadPruneList(_prunePath, out err);
            if (err != null)
            {
                PrintMessage(err);
                PruneList = new List<string>();
                SavePruneList(_prunePath, PruneList);
                PrintMessage("Created new prune list");
            }
            UnPruneList = Functions.ReadPruneList(_unprunePath, out err);
            if (err != null)
            {
                PrintMessage(err);
                UnPruneList = new List<string>();
                SavePruneList(_unprunePath, UnPruneList);
                PrintMessage("Created new unprune list");
            }
        }


        public void SearchStuff()
        {
            PrintMessage("read prune list ...");

            string err;
            PruneList = Functions.ReadPruneList(_prunePath, out err);
            if (err != null)
            {
                PrintMessage(err);
                UnPruneList = new List<string>();
            }
            UnPruneList = Functions.ReadPruneList(_unprunePath, out err);
            if (err != null)
            {
                PrintMessage(err);
                UnPruneList = new List<string>();
            }

            PrintMessage("searching ...");

            // find all parts in the files files
            var files = Functions.FindFilesRec(_installPath + "\\GameData", ".cfg");
            var prunedFiles = Functions.FindFilesRec(_installPath + "\\GameData", ".cfg.pruned");

            PrintMessage("found " + (files.Count + prunedFiles.Count) + " files (" + prunedFiles.Count + " pruned) ...");
            PrintMessage("extracting names ...");

            // creating entries
            PartEntries = new List<PartEntry>();
            NofPrunedParts = 0;

            // get them parts from the files
            foreach (string f in files)
            {
                if (f.EndsWith("RemoteTech_Tech_Node.cfg"))
                {
                    int j = 0;
                }
                var add = ExtractParts(f, false);
                PartEntries.AddRange(add);
            }
            foreach (string f in prunedFiles)
            {
                PartEntries.AddRange(ExtractParts(f, true));
            }

            // create hash set
            PartDict = new Dictionary<string, List<PartEntry>>();
            foreach (PartEntry entry in PartEntries)
            {
                if (entry.Name != null)
                {
                    string name = (entry.IsDeletion ? "!" : "") + entry.Name;
                    //if ((name == null || name.Length == 0) && entry.Parent != null && entry.Parent.Length > 0)
                    //{
                    //    name = entry.Parent;
                    //    entry.Name = name;
                    //}
                    if (name.Length > 0)
                    {
                        List<PartEntry> l;
                        if (!PartDict.TryGetValue(name, out l))
                        {
                            l = new List<PartEntry>();
                            PartDict.Add(name, l);
                        }
                        l.Add(entry);
                    }
                }
            }

            foreach (PartEntry entry in PartEntries)
            {
                bool pruned = entry.Pruned;
                // check wheter the part is on the prune list
                if (!pruned)
                {
                    foreach (string prune in PruneList)
                    {
                        if (entry.Path.StartsWith(prune))
                        {
                            pruned = true;
                            break;
                        }
                    }
                }
                // check wheter the part is on the unprune list
                if (pruned)
                {
                    foreach (string prune in UnPruneList)
                    {
                        if (entry.Path.StartsWith(prune))
                        {
                            pruned = false;
                            break;
                        }
                    }
                }
                entry.Pruned = pruned;
                if (pruned)
                {
                    NofPrunedParts++;
                }

                // add titles to the parts         
                // find root
                PartEntry root = entry;
                while (root.Parent != null)
                {
                    // search the parent
                    List<PartEntry> parent;
                    PartDict.TryGetValue(root.Parent, out parent);

                    if (parent != null)
                    {
                        // pick the most parentiest parent
                        PartEntry pick = null;
                        foreach (var p in parent)
                        {
                            if (pick == null
                                || (!p.IsDeletion
                                    && (pick.IsMod || !p.IsMod)
                                    && (pick.IsReplace || !p.IsReplace)
                                    && (pick.IsCopy || !p.IsCopy
                                    && pick.Parent != null)))
                            {
                                pick = p;
                            }
                        }

                        // if on the same level do not continue
                        if (root.Parent.Equals(pick.Parent))
                        {
                            root = null;
                            break;
                        }

                        root = pick;
                    }
                    else
                    {
                        root = null;
                        break;
                        Debug.Fail("must be in there");
                    }
                }

                if (root != entry && root != null)
                {
                    // use the parents name
                    //if (entry.Name == null || entry.Name.Length == 0)
                    //{
                    //    entry.Name = entry.Parent;
                    //}

                    // add the title to the root not
                    if (root.TitleFamily == null)
                    {
                        root.TitleFamily = new List<string>();
                    }
                    bool already = false;
                    foreach (string s in root.TitleFamily)
                    {
                        if (root.Equals(s))
                        {
                            already = true;
                            break;
                        }
                    }
                    if (!already && entry.Title != null)
                    {
                        root.TitleFamily.Add(entry.Title);
                    }
                }
            }


            // output status
            PrintMessage("found " + PartEntries.Count + " results (" + NofPrunedParts + " pruned) ...");
            PrintMessage("sorting ...");

            // sort results
            PartEntries = PartEntries.OrderBy((PartEntry x) => { return x.Name; }).ToList();

            PrintMessage("printing ...");
        }

        /// <summary>
        /// returns all parts created by the given list
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<PartEntry> ExtractParts(string path, bool pruned)
        {
            var entries = new List<PartEntry>();
            int subPathStart = path.IndexOf("\\GameData\\") + 10;
            int subPathEnd = path.LastIndexOf("\\");
            string subPath = path.Substring(subPathStart, subPathEnd - subPathStart);
            string fileName = path.Substring(path.LastIndexOf("\\") + 1);

            // open the file
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
                sr = new StreamReader(fs);

                // parse the file
                var current = new List<PartEntry>();
                string l;
                string todo = null;
                int partIndent = -1; // the indent level of the current part
                bool wasPartBody = false;
                int indent = 0;
                do
                {
                    #region READLINE
                    // readline and indention
                    if (todo != null)
                    {
                        l = todo;
                        todo = null;
                    }
                    else
                    {
                        l = sr.ReadLine();
                        // remove comments
                        int commentStart = l.IndexOf("//");
                        if (commentStart != -1)
                        {
                            l = l.Substring(0, commentStart);
                        }
                    }

                    // TODO: this is possibly never ever tested
                    if (l.Contains('{') || l.Contains('}'))
                    {
                        int brace = l.IndexOf('{');
                        int braceR = l.IndexOf('}');
                        brace = brace == -1 || (braceR != -1 && braceR < brace) ? braceR : brace;
                        if (brace != -1)
                        {
                            if (brace == 0)
                            {
                                indent += l[0] == '{' ? 1 : -1;
                                l = l.Substring(1);
                            }
                            else
                            {
                                string left = l.Substring(0, brace);
                                string right = l.Substring(left.Length);
                                left = left.Trim();

                                l = left;
                                todo = right;
                            }
                        }
                    }

                    // TODO: remove
                    #endregion

                    #region PART DEFINITION
                    var trimmed = l.Trim();
                    if (trimmed.Equals("PART")
                        || (trimmed.Length > 4 && trimmed.Substring(1).StartsWith("PART[")))
                    {
                        var parentNames = new List<string>(); ;
                        int oNameStart = l.IndexOf("[");
                        if (oNameStart != -1)
                        {
                            oNameStart++;
                            int oNameEnd = l.IndexOf("]");
                            if (oNameEnd == -1)
                            {
                                oNameEnd = l.Length;
                            }
                            parentNames.AddRange(l.Substring(oNameStart, oNameEnd - oNameStart).Split(','));
                        }
                        else
                        {
                            parentNames.Add(null);
                        }

                        current = new List<PartEntry>();
                        for (int i = 0; i < parentNames.Count; i++)
                        {
                            if (parentNames[i] == null)
                            {
                                int j = 0;
                            }
                            var c = new PartEntry(subPath + "\\" + fileName, path);
                            c.Name = parentNames[i];
                            c.Parent = parentNames[i];
                            c.Pruned = pruned;
                            current.Add(c);
                            entries.Add(c);
                        }

                        partIndent = indent + 1;
                        wasPartBody = false;

                        if (l.Contains("!PART") || l.Contains("-PART"))
                        {
                            foreach (var c in current)
                            {
                                c.IsDeletion = true;
                            }

                            partIndent = -1; // ignore the node
                            //current = new List<PartEntry>();
                        }
                        else if (l.Contains("@PART"))
                        {
                            foreach (var c in current)
                            {
                                c.IsMod = true;
                            }
                        }
                        else if (l.Contains("+PART") || l.Contains("$PART"))
                        {
                            foreach (var c in current)
                            {
                                c.IsCopy = true;
                            }
                        }
                        else if (l.Contains("%PART"))
                        {
                            foreach (var c in current)
                            {
                                c.IsReplace = true;
                            }
                        }
                        else if (l.Contains("|PART"))
                        {
                            throw new NotImplementedException();
                        }
                        else if (l.Contains("%PART"))
                        {
                            throw new NotImplementedException();
                        }
                        else if (l.Contains("#PART"))
                        {
                            throw new NotImplementedException();
                        }
                        else if (l.Contains("*PART"))
                        {
                            throw new NotImplementedException();
                        }
                    }
                    #endregion

                    // ignore anything inside and outside parts
                    if (indent != partIndent)
                    {
                        if (wasPartBody && indent < partIndent)
                        {
                            partIndent = -1;
                        }
                        continue;
                    }
                    wasPartBody = true;
                    //Debug.WriteLine(indent + ": " + l);

                    #region PARSE ENTRIES
                    // title 
                    {
                        string title = ParseLine(l, "title");
                        if (title != null)
                        {
                            foreach (var c in current)
                            {
                                c.Title = title;
                            }
                        }
                    }

                    // name 
                    {
                        string name = ParseLine(l, "name");
                        if (name != null)
                        {
                            foreach (var c in current)
                            {
                                c.Name = name;
                            }
                        }
                    }

                    // model 
                    string resPath = null;
                    {
                        string model = ParseLine(l, "model");
                        if (model != null)
                        {
                            resPath = model;
                        }
                    }

                    // texture 
                    {
                        string texture = ParseLine(l, "texture");
                        if (texture != null)
                        {
                            if (texture.Contains(","))
                            {
                                texture = texture.Substring(texture.IndexOf(",") + 1).Trim();
                            }
                            resPath = texture;
                        }
                    }

                    if (resPath != null)
                    {
                        resPath = resPath.Replace("/", "\\");
                        // if relative path
                        if (!resPath.Contains("\\"))
                        {
                            resPath = subPath + "\\" + resPath;
                        }

                        var resPathes = GetResourcePaths(resPath);
                    }
                    #endregion

                } while (!sr.EndOfStream);


                return entries;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<PartEntry>(); // "_XX file not found (" + ex + ")XX";
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        private static string ParseLine(string l, string searchFor)
        {
            l = l.Trim().ToLower();
            if (l.StartsWith(searchFor + " =") || l.StartsWith(searchFor + "="))
            {
                int tagStart = l.IndexOf('=') + 1;
                if (tagStart != -1)
                {
                    return l.Substring(tagStart).Trim();
                }
                else
                {
                    Debug.Fail("fishy");
                }
            }
            return null;
        }

        /// <summary>
        /// finds all resource path given a prefix (ignores
        /// .cfg files)
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private List<string> GetResourcePaths(string dir)
        {
            var res = new List<string>();
            var relDir = dir.Substring(0, dir.LastIndexOf("\\"));
            var absDir = _installPath + "\\GameData\\" + relDir;
            var prefix = dir.Substring(dir.LastIndexOf("\\") + 1);
            try
            {
                DirectoryInfo di = new DirectoryInfo(absDir);
                if (di.Exists)
                {
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        if (fi.Name.StartsWith(prefix)
                            && !(fi.Name.EndsWith(".cfg") || fi.Name.EndsWith(".cfg.pruned")))
                        {
                            var name = fi.Name;
                            if (name.EndsWith(".pruned"))
                            {
                                name = name.Substring(0, name.IndexOf(".pruned"));
                            }
                            res.Add(relDir + "\\" + fi.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exceptoin: GetResourcePaths:\r\n" + ex);
            }

            return res;
        }

        /// <summary>
        /// Removes all entries from prune and unprune list
        /// </summary>
        public void ResetPruneList()
        {
            PruneList = new List<string>();
            UnPruneList = new List<string>();
            SavePruneList(_prunePath, PruneList);
            SavePruneList(_unprunePath, UnPruneList);
            NeedsUpdate = true;

            PrintMessage("Removed all entries from prune/unprune list");
        }

        /// <summary>
        /// Set prune, remove from listBox and set to prunelist
        /// </summary>
        public List<PartEntry> PruneFile(PartEntry entry, bool prune)
        {
            string path = null;
            path = CreatePruneEntry(entry);

            // set/unset pruned
            if (prune)
            {
                if (UnPruneList.Contains(path))
                {
                    UnPruneList.Remove(path);
                    PrintMessage("Remove Unprune path: " + path);
                }
                else
                {
                    PruneList.Add(path);
                    PrintMessage("Add prune path: " + path);
                }
            }
            else
            {
                if (PruneList.Contains(path))
                {
                    PruneList.Remove(path);
                    PrintMessage("Remove prune path: " + path);
                }
                else
                {
                    UnPruneList.Add(path);
                    PrintMessage("Add Unprune path: " + path);
                }

            }

            var changed = new List<PartEntry>();

            foreach (PartEntry pe in PartEntries)
            {
                if (pe.Path.StartsWith(path))
                {
                    Debug.Assert(pe.Pruned != prune);
                    if (pe.Pruned != prune)
                    {
                        pe.Pruned = prune;
                        NofPrunedParts += prune ? 1 : -1;
                        changed.Add(pe);
                    }
                }
            }

            SavePruneList(_prunePath, PruneList);
            SavePruneList(_unprunePath, UnPruneList);

            return changed;
        }

        /// <summary>
        /// If a part shall be pruned, this creates the best entry for a prune
        /// file.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static string CreatePruneEntry(PartEntry entry)
        {
            string path;

            // subdirectory
            string partPath = entry.FullPath.Substring(0, entry.FullPath.LastIndexOf("\\"));
            var cfgs = Functions.FindFilesRec(partPath, ".cfg");
            var prunedCfgs = Functions.FindFilesRec(partPath, ".cfg.pruned");

            if (cfgs.Count + prunedCfgs.Count == 1)
            {
                path = entry.Path.Substring(0, entry.Path.LastIndexOf("\\"));
            }
            else
            {
                // add the path without the ending, so all files with the same prefix will be pruned
                path = entry.Path.Substring(0, entry.Path.LastIndexOf("cfg"));
            }

            return path;
        }

        public void MergeList(string other)
        {
            var name = other;
            bool succ = false;
            if (File.Exists(name))
            {
                // load
                string err;
                var loaded = Functions.ReadPruneList(name, out err);

                if (err != null)
                {
                    // merge
                    foreach (string s in loaded)
                    {
                        if (!PruneList.Contains(s))
                        {
                            PruneList.Add(s);
                        }
                    }

                    // save 
                    SavePruneList(_prunePath, PruneList);

                    // reload
                    NeedsUpdate = true;

                    PrintMessage("Added entries to the list.");
                    succ = true;
                }
                else
                {
                    PrintMessage(err);
                }
            }
            if (!succ)
            {
                //File.Create(_prunePath);
                PrintMessage("Merging was not successful.");
            }
        }

        /// <summary>
        /// laves the prune list to the file
        /// </summary>
        private void SavePruneList(string path, List<string> pruneList)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                foreach (string p in pruneList)
                {
                    // remove /gamedata/
                    var s = p;
                    if (p.Contains("GameData"))
                    {
                        s = p.Substring(10);
                    }
                    sw.WriteLine(s.Replace('\\', '/'));
                }
                sw.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                PrintMessage("ERROR. Cannot save prune list");
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// Creates two lits. One prunes all unused resources
        /// The other unprunes every used resource. Takes
        /// into account which files are pruned and the actual prune list
        /// </summary>
        public void CreateRepairLists()
        {
            // create prune list
            var resourcePaths = new List<string>();
            foreach (PartEntry pe in PartEntries)
            {
                if (pe.Pruned && pe.Resources != null)
                {
                    foreach (string res in pe.Resources)
                    {
                        if (!resourcePaths.Contains(res))
                        {
                            var r = res;
                            if (r.EndsWith(".pruned"))
                            {
                                r = r.Substring(0, r.LastIndexOf('.'));
                            }
                            resourcePaths.Add(r);
                        }
                    }
                }
            }
            CreateFromList(resourcePaths, "res.prune", false);

            // create unprune list
            resourcePaths = new List<string>();
            foreach (PartEntry pe in PartEntries)
            {
                // TODO: check if the resource is really unavailable
                if (!pe.Pruned && pe.Resources != null)
                {
                    foreach (string res in pe.Resources)
                    {
                        if (!resourcePaths.Contains(res))
                        {
                            var r = res;
                            if (r.EndsWith(".pruned"))
                            {
                                r = r.Substring(0, r.LastIndexOf('.'));
                            }
                            resourcePaths.Add(r);
                        }
                    }
                }
            }
            CreateFromList(resourcePaths, "res.unprune", false);
        }

        /// <summary>
        /// Creates a list which exclued every part which has a non rp0 label in
        /// its title in the module manager cache
        /// </summary>
        public void CreateRP0Lists()
        {
            if (PartEntries == null)
            {
                SearchStuff();
            }
            string path = _installPath + "\\GameData\\ModuleManager.ConfigCache";
            string path2 = _installPath + "\\GameData\\ModuleManager.ConfigCache2";
            // use cached version
            if (File.Exists(path2))
            {
                path = path2;
            }
            // lcreate prune lists
            FileStream fs = null;
            var namesNoRP0 = new List<string>();
            var namesNoCost = new List<string>();
            var namesNoRO = new List<string>();
            try
            {
                PrintMessage("Scanning " + path.Substring(path.LastIndexOf("\\")) + " (parts from last ksp launch)");
                fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string title = null;
                string name = null;
                while (!sr.EndOfStream)
                {
                    string l = sr.ReadLine();

                    // found a part node
                    if (l.Contains("PART"))
                    {
                        title = null;
                    }

                    // found a title tag
                    int titleStart = l.LastIndexOf("title =");
                    if (titleStart != -1)
                    {
                        titleStart += "title = ".Length;
                        title = l.Substring(titleStart);
                        if (title.Contains("non RP-0") && name != null)
                        {
                            namesNoRP0.Add(name);
                        }
                        if (title.Contains("RP-0 nocost") && name != null)
                        {
                            namesNoCost.Add(name);
                        }
                        if (title.Contains("non RO") && name != null)
                        {
                            namesNoRO.Add(name);
                        }
                    }

                    // found a name tag
                    int nameStart = l.LastIndexOf("name =");
                    if (nameStart != -1)
                    {
                        nameStart += "name = ".Length;
                        name = l.Substring(nameStart);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                PrintMessage("ERROR. Cannot read prune list");
                return;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            // produce parts (weakest)
            CreateFromList(namesNoRP0, "non_rp0");

            // nocost 
            namesNoCost.AddRange(namesNoRP0);
            CreateFromList(namesNoCost, "rp0_nocost");

            // non ro (strongest)
            namesNoRO.AddRange(namesNoCost);
            CreateFromList(namesNoRO, "non_ro");
        }

        //private List<string> CreateTagPruneList(List<string> tags)
        //{

        //}

        /// <summary>
        /// Creates a prune list given a list of names and saves is to 
        /// the given location. (PartEntries need to be initialized)
        /// </summary>
        /// <param name="pruneNames"></param>
        /// <param name="savePath"></param>
        private void CreateFromList(List<string> pruneNames, string suffix, bool byTitle = true)
        {
            var saveDir = _prunePath.Substring(0, _prunePath.LastIndexOf('\\') + 1);
            if (!Directory.Exists(saveDir))
            {
                PrintMessage("ERROR: cannot write to prune list loaction:\r\n" + saveDir);
                return;
            }

            var savePath = saveDir + _prunePath.Substring(
                    saveDir.Length,
                    _prunePath.LastIndexOf('.') - saveDir.Length)
                        + "." + suffix + ".prnl";

            var pathes = new List<string>();
            if (byTitle)
            {
                PrintMessage("searching parts");
                // search the path for every parts name
                foreach (string name in pruneNames)
                {
                    // delete the root, or the +copy, or the %repalcement and the original
                    // not the mod.
                    // and do nothing if the mod modifies an original part, which is
                    // used by some other non-pruned part
                    TryPrune(name, pruneNames, pathes);
                }
            }
            else
            {
                // otherwise just use the given strings
                pathes = pruneNames;
            }

            try
            {
                // save it
                SavePruneList(savePath, pathes);
                PrintMessage("saved as\r\n" + savePath);
            }
            catch
            {
                PrintMessage("ERROR: cannot write to prune list loaction:\r\n" + saveDir);
            }
        }

        /// <summary>
        /// If the part with the given name can be pruned the path will be
        /// put on the prune list
        /// </summary>
        /// <param name="name">the name of the part as defined in a cfg file</param>
        /// <param name="pruneNames">names of all parts which shall be pruned</param>
        /// <param name="prunePathList"></param>
        /// <returns>if it was possible to only put that file on the list</returns>
        private bool TryPrune(string name, List<string> pruneNames, List<string> prunePathList)
        {
            List<PartEntry> parts;
            PartDict.TryGetValue(name, out parts);
            foreach (PartEntry pe in parts)
            {
                if (pe.Pruned)
                {
                    continue;
                }
                // only remove if there are no childs from
                // this part or they are tagged as well
                if (!AllChildrenTagged(pe.Name, pruneNames))
                {
                    return false;
                }
                if (pe.IsMod)
                {
                    if (pe.Parent != name)
                    {
                        // follow the original part
                        return TryPrune(pe.Parent, pruneNames, prunePathList);
                    }
                    else
                    {
                        // just let the mods hang in there
                        continue;
                    }
                }
                else if (IsOnlyEntry(pe))
                {
                    if (pe.IsReplace)
                    {
                        // only remove if the original and the part
                        // itself is prunable
                        if (TryPrune(pe.Parent, pruneNames, prunePathList))
                        {
                            // can be pruned
                            prunePathList.Add(CreatePruneEntry(pe));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // can be pruned
                        prunePathList.Add(CreatePruneEntry(pe));
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns true if the given part is the only thing
        /// specified in its file
        /// </summary>
        /// <param name="pe"></param>
        /// <returns></returns>
        private bool IsOnlyEntry(PartEntry pe)
        {
            foreach (PartEntry other in PartEntries)
            {
                if (other.Path.Equals(pe.Path)
                    && pe != other)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// True if all children are tagged (or there are no children)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        private bool AllChildrenTagged(string name, List<string> tags)
        {
            // search all children
            foreach (PartEntry pe in PartEntries)
            {
                // check if this is a child
                if (!pe.Pruned && pe.Parent.Equals(name) && !IsDeleted(pe.Name))
                {
                    // check whether the child is tagged  
                    if (!(tags.Contains(pe.Name)
                        // or if its children as well
                        || AllChildrenTagged(pe.Name, tags)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// True if there is a deletion for the part (name)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsDeleted(string name)
        {
            List<PartEntry> parts;
            PartDict.TryGetValue(name, out parts);
            foreach (PartEntry pe in parts)
            {
                if (!pe.Pruned && pe.IsDeletion)
                {
                    return true;
                }
            }
            return false;
        }


        private void PrintMessage(string v)
        {
            _output.PrintMessage(v);
        }
    }
}
