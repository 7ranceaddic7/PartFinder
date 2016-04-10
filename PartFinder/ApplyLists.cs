using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartFinder
{
    public partial class ApplyLists : Form
    {
        public ApplyLists()
        {
            InitializeComponent();
        }

        const string PathLists = "pathlist.dat";
        private bool ignorePathChanges = false;

        public Form1 _parent { get; private set; }

        private void ApplyLists_Load(object sender, EventArgs e)
        {
            ignorePathChanges = true;

            // remember all entries
            if (File.Exists(PathLists))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(PathLists, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    bool unprune = false;
                    bool check = false;
                    do
                    {
                        var line = sr.ReadLine();
                        if (line.Equals("unprune"))
                        {
                            unprune = true;
                        }
                        if (line.Equals("check"))
                        {
                            check = true;
                        }
                        if (line.Equals("uncheck"))
                        {
                            check = false;
                        }

                        if (File.Exists(line))
                        {
                            if (unprune)
                            {
                                listBoxUnPrunePath.Items.Add(new ListEntry(line));
                                listBoxUnPrunePath.SetItemChecked(listBoxUnPrunePath.Items.Count - 1, check);
                            }
                            else
                            {
                                listBoxPrunePath.Items.Add(new ListEntry(line));
                                listBoxPrunePath.SetItemChecked(listBoxPrunePath.Items.Count - 1, check);
                            }
                        }
                    } while (!sr.EndOfStream);
                }
                catch
                { }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
            ignorePathChanges = false;
            PrintMessage("Ready...");
        }

        // add prune
        private void button1_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (string fn in openFileDialog1.FileNames)
                {
                    if (File.Exists(fn)
                        && !listBoxPrunePath.Items.Contains(fn))
                    {
                        listBoxPrunePath.Items.Add(new ListEntry(fn));
                    }
                }
            }
            SaveLists();
        }

        // remove prune
        private void button3_Click(object sender, EventArgs e)
        {
            var oldIndex = listBoxPrunePath.SelectedIndex;
            if (oldIndex != -1)
            {
                listBoxPrunePath.Items.RemoveAt(oldIndex);
                if (oldIndex < listBoxPrunePath.Items.Count)
                {
                    listBoxPrunePath.SelectedIndex = oldIndex;
                }
                else
                {
                    listBoxPrunePath.SelectedIndex = listBoxPrunePath.Items.Count - 1;
                }
            }
            SaveLists();
        }

        // apply prune
        private void button8_Click(object sender, EventArgs e)
        {
            if (listBoxPrunePath.SelectedIndex == -1)
            {
                return;
            }
            string err;
            var list = Functions.ReadPruneList(((ListEntry)listBoxPrunePath.Items[listBoxPrunePath.SelectedIndex]).Path, out err);
            if (err == null)
            {
                ApplyList(list, new List<string>());
                PrintMessage("Pruned selected list on the left");
                _parent.StateChanged();
            }
            else
            {
                PrintMessage(err);
            }
        }

        // add unprune
        private void button2_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (string fn in openFileDialog1.FileNames)
                {
                    if (File.Exists(fn)
                    && !listBoxUnPrunePath.Items.Contains(fn))
                    {
                        listBoxUnPrunePath.Items.Add(new ListEntry(fn));
                    }
                }
            }
            SaveLists();
        }

        // remove unprune
        private void button4_Click(object sender, EventArgs e)
        {
            var oldIndex = listBoxUnPrunePath.SelectedIndex;
            if (oldIndex != -1)
            {
                listBoxUnPrunePath.Items.RemoveAt(oldIndex);
                if (oldIndex < listBoxUnPrunePath.Items.Count)
                {
                    listBoxUnPrunePath.SelectedIndex = oldIndex;
                }
                else
                {
                    listBoxUnPrunePath.SelectedIndex = listBoxUnPrunePath.Items.Count - 1;
                }
            }
            SaveLists();
        }

        // apply unprune    
        private void button9_Click(object sender, EventArgs e)
        {
            if (listBoxUnPrunePath.SelectedIndex == -1)
            {
                return;
            }
            string err;
            var list = Functions.ReadPruneList(((ListEntry)listBoxUnPrunePath.Items[listBoxUnPrunePath.SelectedIndex]).Path, out err);
            if (err == null)
            {
                ApplyList(new List<string>(), list);
                PrintMessage("Unpruned selected list on the right");
                _parent.StateChanged();
            }
            else
            {
                PrintMessage(err);
            }
        }

        // prune all
        private void button6_Click(object sender, EventArgs e)
        {
            var prune = new List<string>();
            // prune
            foreach (ListEntry entry in listBoxUnPrunePath.CheckedItems)
            {
                string err;
                var list = Functions.ReadPruneList(entry.Path, out err);
                if (err == null)
                {
                    prune.AddRange(list);
                }
                else
                {
                    PrintMessage(err);
                }
            }
            ApplyList(prune, new List<string>());
            PrintMessage("Pruned all lists on the left");
            _parent.StateChanged();
        }

        // unprune all
        private void button7_Click(object sender, EventArgs e)
        {
            var unprune = new List<string>();

            // unprune
            foreach (ListEntry entry in listBoxPrunePath.CheckedItems)
            {
                string err;
                var list = Functions.ReadPruneList(entry.Path, out err);
                if (err == null)
                {
                    unprune.AddRange(list);
                }
                else
                {
                    PrintMessage(err);
                }
            }
            ApplyList(new List<string>(), unprune);
            PrintMessage("Unpruned all lists on the right");
            _parent.StateChanged();
        }

        // global reset
        private void button5_Click(object sender, EventArgs e)
        {
            // unprune all
            ApplyList(new List<string>(), new List<string>() { "\\GameData\\" });
            PrintMessage("Done.");

            _parent.StateChanged();
        }

        // global apply
        private void button10_Click(object sender, EventArgs e)
        {
            // unprune all
            ApplyList(new List<string>(), new List<string>() { "\\GameData\\" });

            var prune = new List<string>();
            var unprune = new List<string>();
            // prune
            foreach (ListEntry entry in listBoxPrunePath.CheckedItems)
            {
                string err;
                var list = Functions.ReadPruneList(entry.Path, out err);
                if (err == null)
                {
                    prune.AddRange(list);
                }
                else
                {
                    PrintMessage(err);
                }
            }
            // unprune
            foreach (ListEntry entry in listBoxUnPrunePath.CheckedItems)
            {
                string err;
                var list = Functions.ReadPruneList(entry.Path, out err);
                if (err == null)
                {
                    unprune.AddRange(list);
                }
                else
                {
                    PrintMessage(err);
                }
            }
            ApplyList(prune, unprune);
            _parent.StateChanged();
            PrintMessage("Done.");
        }

        // lists changed
        private void listBox_Changed(object sender, EventArgs e)
        {
            SaveLists();
        }
        private void listBox_Changed(object sender, KeyPressEventArgs e)
        {
            SaveLists();
        }

        /// <summary>
        /// Prunes or unprunes every file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="prune"></param>
        private void ApplyList(List<string> pruneList, List<string> unPruneList)
        {
            string path = _parent.InstallPath + "\\GameData\\";
            var all = Functions.FindFilesRec(path, "");
            var pruned = Functions.FindFilesRec(path, ".pruned");
            var unpruned = new List<string>();
            foreach (string s in all)
            {
                if (!pruned.Contains(s))
                {
                    unpruned.Add(s);
                }
            }

            PrintMessage("Start pruning: prune paths " + pruneList.Count + ", unprune paths " + unPruneList.Count);

            int nofPruned = 0;
            int nofUnpruned = 0;
            
            // prune 
            foreach (string f in unpruned)
            {
                var subpath = f.Substring(path.Length - 10);

                // check wheter the part is on the prune list
                bool doPrune = false;
                foreach (string prefix in pruneList)
                {
                    if (subpath.StartsWith(prefix))
                    {
                        // unprune file
                        doPrune = true;
                        break;
                    }
                }
                if (doPrune)
                {
                    // check wheter the part is on the unprune list
                    foreach (string prefix in unPruneList)
                    {
                        if (subpath.StartsWith(prefix))
                        {
                            // unprune file
                            doPrune = false;
                            break;
                        }
                    }
                }

                if (doPrune)
                {
                    // prune the file
                    Debug.Assert(!f.EndsWith(".pruned"));
                    File.Move(f, f + ".pruned");
                    nofPruned++;
                }
            }

            // unprune 
            foreach (string f in pruned)
            {
                var subpath = f.Substring(path.Length - 10);

                // check wheter the part is on the unprune list
                foreach (string prefix in unPruneList)
                {
                    if (subpath.StartsWith(prefix))
                    {
                        // unprune file
                        Debug.Assert(f.EndsWith(".pruned"));
                        if (f.EndsWith(".pruned"))
                        {
                            File.Move(f, f.Substring(0, f.LastIndexOf('.')));
                            nofUnpruned++;
                        }
                        break;
                    }
                }
            }

            PrintMessage("Pruning finished: pruned files " + nofPruned + ", unpruned files " + nofUnpruned);
        }





        private void SaveLists()
        {
            if (ignorePathChanges)
            {
                return;
            }
            FileStream fs = null;
            try
            {
                fs = new FileStream(PathLists, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                foreach (ListEntry p in listBoxPrunePath.Items)
                {
                    sw.WriteLine(listBoxPrunePath.CheckedItems.Contains(p) ? "check" : "uncheck");
                    sw.WriteLine(p.Path);
                }
                sw.WriteLine("unprune");
                foreach (ListEntry p in listBoxUnPrunePath.Items)
                {
                    sw.WriteLine(listBoxUnPrunePath.CheckedItems.Contains(p) ? "check" : "uncheck");
                    sw.WriteLine(p.Path);
                }
                sw.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                PrintMessage("\r\nERROR. Cannot save prune list");
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// Prints a message (and a newline) to the textbox
        /// updates it and sets the cursor to the bottom.
        /// </summary>
        /// <param name="s"></param>
        private void PrintMessage(string s)
        {
            if (textBoxStatus.Text.Length != 0)
            {
                textBoxStatus.Text += "\r\n";
            }
            textBoxStatus.AppendText(s);
            textBoxStatus.Invalidate();
            textBoxStatus.Update();
            textBoxStatus.Refresh();
        }

        internal void SetParent(Form1 parent)
        {
            _parent = parent;
        }

        private class ListEntry
        {
            public string Path;

            public ListEntry(string path)
            {
                Path = path;
            }

            public override string ToString()
            {
                return Path.Substring(Path.LastIndexOf("\\") + 1);
            }
        }

        private void ApplyLists_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.ApplyLists = null;
        }
    }
}
