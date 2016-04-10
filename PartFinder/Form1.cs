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
using Tools;

namespace PartFinder
{
    public partial class Form1 : Form, IPrint
    {
        const string PathInfo = "pathinfo.dat";
        private bool ignorePathChanges = false;

        private string _programTitle = "Partfinder v0.2";
        private string _installPath;
        public string InstallPath
        {
            get { return _installPath; }
            private set
            {
                _installPath = value;
                Text = _programTitle + " " + _installPath;
            }
        }

        private string _search;

        private PartFinder _finder;

        private bool _needsUpdate;

        private PartEntry _selectedPartEntry;


        /// <summary>
        /// The form which cann apply the changes
        /// </summary>
        public ApplyLists ApplyLists;



        /// <summary>
        /// Constructor.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            ignorePathChanges = true;
            InstallPath = @"C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program";

            // remember the path
            if (File.Exists(PathInfo))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(PathInfo, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    var path = sr.ReadLine();
                    if (Directory.Exists(path))
                    {
                        InstallPath = path;
                    }
                    var prune = sr.ReadLine();
                    if (File.Exists(prune))
                    {
                        textBoxPrune.Text = prune;
                    }
                    else if (Directory.Exists(path + @"Autopruner\PRNLs\"))
                    {
                        textBoxPrune.Text = path + @"Autopruner\PRNLs\Custom.prnl";
                    }

                    if (!sr.EndOfStream)
                    {
                        checkBoxNoMods.Checked = sr.ReadLine().Equals("True");
                    }
                    if (!sr.EndOfStream)
                    {
                        checkBoxNoCopies.Checked = sr.ReadLine().Equals("True");
                    }
                    if (!sr.EndOfStream)
                    {
                        checkBoxNoDeleted.Checked = sr.ReadLine().Equals("True");
                    }
                    if (!sr.EndOfStream)
                    {
                        checkBoxNoWild.Checked = sr.ReadLine().Equals("True");
                    }
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


        // browse
        private void selectInstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = InstallPath;
            var res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                InstallPath = folderBrowserDialog1.SelectedPath;

                if (!InstallPath.Equals(""))
                {
                    textBoxPrune.Text = InstallPath + @"\AutoPruner\PRNLs\Custom.prnl";
                    PrintMessage("Selected prune file: " + textBoxPrune.Text);
                }
                PrintMessage("Selected ksp install: " + InstallPath);
            }
            SaveSettings();
            _finder = null;
        }

        // browse prune
        private void selectPruneListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool init = false;
            try
            {
                var fi = new FileInfo(textBoxPrune.Text);
                if (Directory.Exists(fi.DirectoryName))
                {
                    openFileDialog1.InitialDirectory = fi.DirectoryName;
                    openFileDialog1.FileName = fi.Exists ? fi.Name : "";
                    init = true;
                }
            }
            catch
            { }
            if (!init)
            {
                if (Directory.Exists(textBoxPrune.Text))
                {
                    openFileDialog1.InitialDirectory = textBoxPrune.Text;
                    openFileDialog1.FileName = "";
                }
            }
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBoxPrune.Text = openFileDialog1.FileName;
                if (!File.Exists(textBoxPrune.Text))
                {
                    File.Create(textBoxPrune.Text);
                    PrintMessage("created prune list.");
                }
            }
            PrintMessage("Selected prune file: \r\n" + textBoxPrune.Text);

            if (!File.Exists(textBoxPrune.Text))
            {
                try
                {
                    FileInfo fi = new FileInfo(textBoxPrune.Text);
                    if (Directory.Exists(fi.DirectoryName))
                    {
                        File.Create(fi.FullName);
                        PrintMessage("Created prune file");
                    }
                    else
                    {
                        PrintMessage("Cannot create prune file (path does not exits)");
                    }
                }
                catch
                { }
            }

            // reload
            _finder = null;

            SaveSettings();
        }

        // merge prune list
        private void mergeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool init = false;
            try
            {
                var fi = new FileInfo(textBoxPrune.Text);
                if (Directory.Exists(fi.DirectoryName))
                {
                    openFileDialog1.InitialDirectory = fi.DirectoryName;
                    openFileDialog1.FileName = fi.Exists ? fi.Name : "";
                    init = true;
                }
            }
            catch
            { }
            if (!init)
            {
                if (Directory.Exists(textBoxPrune.Text))
                {
                    openFileDialog1.InitialDirectory = textBoxPrune.Text;
                    openFileDialog1.FileName = "";
                }
            }
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                _finder.MergeList(openFileDialog1.FileName);
                SearchStuff();
            }
        }

        // path text changed
        private void textBoxResults_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
            _finder = null;
            listBoxParts.Items.Clear();
            listBoxPruned.Items.Clear();
        }

        // list all
        private void button4_Click(object sender, EventArgs e)
        {
            _search = null;
            SearchStuff();
        }

        // search
        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                _search = textBoxSearch.Text;
                SearchStuff();
            }
        }

        // search with filter
        private void button2_Click(object sender, EventArgs e)
        {
            _search = textBoxSearch.Text;
            SearchStuff();
        }

        // update
        private void button3_Click(object sender, EventArgs e)
        {
            _finder = null;
            SearchStuff();
        }

        // set to prune
        private void pruneEntry(object sender, EventArgs e)
        {
            if (listBoxParts.SelectedIndex >= listBoxParts.Items.Count
                || listBoxParts.SelectedIndex < 0)
            {
                return;
            }
            int oldIndex = listBoxParts.SelectedIndex;
            PruneFile((PartEntry)listBoxParts.Items[listBoxParts.SelectedIndex], true);
            listBoxParts.SelectedIndex = oldIndex >= listBoxParts.Items.Count ? listBoxParts.Items.Count - 1 : oldIndex;
        }

        // remove from prune
        private void unpruneEntry(object sender, EventArgs e)
        {
            if (listBoxPruned.SelectedIndex >= listBoxPruned.Items.Count
                || listBoxPruned.SelectedIndex < 0)
            {
                return;
            }
            int oldIndex = listBoxPruned.SelectedIndex;
            PruneFile((PartEntry)listBoxPruned.Items[listBoxPruned.SelectedIndex], false);
            listBoxPruned.SelectedIndex = oldIndex >= listBoxPruned.Items.Count ? listBoxPruned.Items.Count - 1 : oldIndex;
        }

        // non rp0
        private void nonRP0ListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_finder == null)
            {
                SearchStuff();
            }
            _finder.CreateRP0Lists();
        }


        // create list for unpruning every texture
        private void resourceRepairListsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_finder == null)
            {
                SearchStuff();
            }

            _finder.CreateRepairLists();
        }

        // open right cklick menu
        private void listBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            ListBox listBox = (ListBox)sender;
            listBox.SelectedIndex = listBox.IndexFromPoint(e.Location);
            if (listBox.SelectedIndex != -1)
            {
                _selectedPartEntry = (PartEntry)listBox.Items[listBox.SelectedIndex];
                contextMenuStrip1.Show();
            }
            else
            {
                _selectedPartEntry = null;
            }
        }

        // right click
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedPartEntry == null)
            {
                return;
            }
            //clear the menu and add custom items
            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Items.Add("Part name: " + _selectedPartEntry.Name);
            contextMenuStrip1.Items.Add("Title: " + _selectedPartEntry.Title);
            contextMenuStrip1.Items.Add("Path: " + _selectedPartEntry.Path);
            //contextMenuStrip1.Items.Add(
            //    new OpenFile("Open", _selectedPartEntry.FullPath));
            contextMenuStrip1.Items.Add("Associated Parts:");
            if (_selectedPartEntry.TitleFamily != null)
            {
                foreach (string p in _selectedPartEntry.TitleFamily)
                {
                    contextMenuStrip1.Items.Add(p.Trim());
                }
            }
        }

        // contex menu click
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var text = e.ClickedItem.Text;
            if (text.StartsWith("Path:"))
            {
                text = text.Substring(text.IndexOf(':') + 2);
                if (_finder != null)
                {
                    foreach (PartEntry pe in _finder.PartEntries)
                    {
                        if (pe.Path.Equals(text) && File.Exists(pe.FullPath))
                        {
                            Process.Start(pe.FullPath);
                            return;
                        }
                    }
                }
                text = textBoxPrune.Text + text;
            }
            else if (text.StartsWith("Title:") || text.StartsWith("Part name:"))
            {
                text = text.Substring(text.IndexOf(':') + 2);
            }
            else if (text.StartsWith("Associated Parts:"))
            {
                return;
            }
            Clipboard.SetText(text);
        }

        // apply window
        private void openApplyWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ApplyLists == null)
            {
                ApplyLists = new ApplyLists();
            }
            ApplyLists.SetParent(this);
            ApplyLists.Show();
            ApplyLists.Focus();
        }

        // reset all pruning
        private void buttonResetList_Click(object sender, EventArgs e)
        {
            _finder.ResetPruneList();

            SearchStuff();
        }

        // ckeck changed
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            _needsUpdate = true;

            // reset
            listBoxParts.Items.Clear();
            listBoxPruned.Items.Clear();

            SaveSettings();
        }



        /// <summary>
        /// update/init parts and fill lists with stuff
        /// </summary>
        /// <param name="pattern"></param>
        private void SearchStuff()
        {
            if (textBoxSearch.Text == "")
            {
                _search = null;
            }
            else if (_search != null)
            {
                _search = _search.ToLower();
            }
            if (_finder == null)
            {
                _finder = new PartFinder(this, InstallPath, textBoxPrune.Text);
            }
            if (_finder.NeedsUpdate)
            {
                _finder.SearchStuff();
            }

            // reset
            listBoxParts.Items.Clear();
            listBoxPruned.Items.Clear();

            foreach (PartEntry pe in _finder.PartEntries)
            {
                if (_search == null || pe.MatchesPattern(_search))
                {
                    // ignore deletions
                    if (pe.IsDeletion)
                    {
                        continue;
                    }

                    // if mod/copy
                    if ((pe.IsMod && checkBoxNoMods.Checked)
                     || (pe.IsCopy && checkBoxNoCopies.Checked)
                     || (pe.Name.Contains("*") && checkBoxNoWild.Checked))
                    {
                        continue;
                    }

                    // ignore deleted parts
                    if (checkBoxNoDeleted.Checked)
                    {
                        List<PartEntry> deletion;
                        if (_finder.PartDict.TryGetValue("!" + pe.Name, out deletion))
                        {
                            foreach (var d in deletion)
                            {
                                if (!d.Pruned)
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    if (pe.Pruned)
                    {
                        listBoxPruned.Items.Add(pe);
                    }
                    else
                    {
                        listBoxParts.Items.Add(pe);
                    }
                }
            }
            _needsUpdate = false;
            UpdateLabel();
        }

        private void PruneFile(PartEntry entry, bool prune)
        {
            if (_finder == null)
            {
                return;
            }

            if (_needsUpdate)
            {
                SearchStuff();
            }

            var changed = _finder.PruneFile(entry, prune);

            var removeFrom = prune ? listBoxParts : listBoxPruned;
            var addTo = !prune ? listBoxParts : listBoxPruned;
            // consider every part associated with the path
            foreach (PartEntry pe in changed)
            {
                if (pe.IsDeletion)
                {
                    if (checkBoxNoDeleted.Checked)
                    {
                        List<PartEntry> deleted;
                        if (_finder.PartDict.TryGetValue(pe.Parent, out deleted) && deleted != null)
                        {
                            foreach (var d in deleted)
                            {
                                // if mod/copy
                                if ((d.IsMod && checkBoxNoMods.Checked)
                                 || (d.IsCopy && checkBoxNoCopies.Checked)
                                 || (d.Name.Contains("*") && checkBoxNoWild.Checked))
                                {
                                    continue;
                                }


                                if (prune)
                                {
                                    // remove the effects of the deletion
                                    if (d.Pruned)
                                    {
                                        listBoxPruned.Items.Add(d);
                                    }
                                    else
                                    {
                                        listBoxParts.Items.Add(d);
                                    }
                                }
                                else
                                {
                                    // add the effects of deleting
                                    // remove the effects of the deletion
                                    if (d.Pruned)
                                    {
                                        listBoxPruned.Items.Remove(d);
                                    }
                                    else
                                    {
                                        listBoxParts.Items.Remove(d);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Assert(pe.Pruned == prune);

                    // remove ad from the respective list box
                    if ((_search == null || pe.MatchesPattern(_search)
                         && (!pe.IsMod || !checkBoxNoMods.Checked)
                         && (!pe.IsCopy || !checkBoxNoCopies.Checked)))
                    {
                        removeFrom.Items.Remove(pe);
                        addTo.Items.Add(pe);
                    }
                }
            }

            if (changed.Count > 0)
            {
                PrintMessage((prune ? "pruned " : "unpruned ") + changed + " parts");
            }

            // since we chaned something
            UpdateLabel();
        }

        /// <summary>
        /// update status label
        /// </summary>
        private void UpdateLabel()
        {
            label2.Text = "found: " + _finder.PartEntries.Count + " files with " + _finder.NofPrunedParts + " pruned files, "
                        + "listing " + (listBoxParts.Items.Count + listBoxPruned.Items.Count) + " parts with " + listBoxPruned.Items.Count + " pruned parts";
        }

        /// <summary>
        /// Prints a message (and a newline) to the textbox
        /// updates it and sets the cursor to the bottom.
        /// </summary>
        /// <param name="s"></param>
        public void PrintMessage(string s)
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

        /// <summary>
        /// Save the path of the ksp install and prune list
        /// </summary>
        private void SaveSettings()
        {
            if (ignorePathChanges)
            {
                return;
            }
            // save the path
            FileStream fs = null;
            try
            {
                fs = new FileStream(PathInfo, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(InstallPath);
                sw.WriteLine(textBoxPrune.Text);
                sw.WriteLine(checkBoxNoMods.Checked);
                sw.WriteLine(checkBoxNoCopies.Checked);
                sw.WriteLine(checkBoxNoDeleted.Checked);
                sw.WriteLine(checkBoxNoWild.Checked);
                sw.Flush();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                PrintMessage("\r\nERROR, Cannot save path");
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        //private class OpenFile : ToolStripItem
        //{
        //    public string Path;
        //    public string Display;
        //    public OpenFile(string display, string path)
        //        : base(display, null, null)
        //    {
        //        Path = path;
        //        Display = display;                
        //        //Text = display;
        //    }
        //    public override string ToString()
        //    {
        //        return Display;
        //    }            
        //}
    }
}
