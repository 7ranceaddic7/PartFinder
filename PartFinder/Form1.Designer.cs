namespace PartFinder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPrune = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxPruned = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.listBoxParts = new System.Windows.Forms.ListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.checkBoxNoMods = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectInstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectPruneListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openApplyWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nonRP0ListsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resourceRepairListsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aintNoHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxNoCopies = new System.Windows.Forms.CheckBox();
            this.buttonResetList = new System.Windows.Forms.Button();
            this.checkBoxNoDeleted = new System.Windows.Forms.CheckBox();
            this.checkBoxNoWild = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(662, 27);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(656, 124);
            this.textBoxStatus.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Update";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button3_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(495, 115);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Search";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(51, 115);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(429, 20);
            this.textBoxSearch.TabIndex = 0;
            this.textBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearch_KeyDown);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(581, 114);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "List All";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "found 0 entries";
            // 
            // textBoxPrune
            // 
            this.textBoxPrune.AutoSize = true;
            this.textBoxPrune.Location = new System.Drawing.Point(82, 30);
            this.textBoxPrune.Name = "textBoxPrune";
            this.textBoxPrune.Size = new System.Drawing.Size(40, 13);
            this.textBoxPrune.TabIndex = 1;
            this.textBoxPrune.Text = "<path>";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.CheckFileExists = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Filter:";
            // 
            // listBoxPruned
            // 
            this.listBoxPruned.ContextMenuStrip = this.contextMenuStrip1;
            this.listBoxPruned.FormattingEnabled = true;
            this.listBoxPruned.Location = new System.Drawing.Point(662, 177);
            this.listBoxPruned.Name = "listBoxPruned";
            this.listBoxPruned.Size = new System.Drawing.Size(656, 654);
            this.listBoxPruned.TabIndex = 6;
            this.listBoxPruned.DoubleClick += new System.EventHandler(this.unpruneEntry);
            this.listBoxPruned.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Parts:";
            // 
            // listBoxParts
            // 
            this.listBoxParts.ContextMenuStrip = this.contextMenuStrip1;
            this.listBoxParts.FormattingEnabled = true;
            this.listBoxParts.Location = new System.Drawing.Point(16, 177);
            this.listBoxParts.Name = "listBoxParts";
            this.listBoxParts.Size = new System.Drawing.Size(640, 654);
            this.listBoxParts.TabIndex = 6;
            this.listBoxParts.DoubleClick += new System.EventHandler(this.pruneEntry);
            this.listBoxParts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDown);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(495, 143);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 2;
            this.button6.Text = ">Prune>";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.pruneEntry);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(581, 143);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 2;
            this.button7.Text = "<Unprune<";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.unpruneEntry);
            // 
            // checkBoxNoMods
            // 
            this.checkBoxNoMods.AutoSize = true;
            this.checkBoxNoMods.Location = new System.Drawing.Point(509, 91);
            this.checkBoxNoMods.Name = "checkBoxNoMods";
            this.checkBoxNoMods.Size = new System.Drawing.Size(66, 17);
            this.checkBoxNoMods.TabIndex = 5;
            this.checkBoxNoMods.Text = "no mods";
            this.checkBoxNoMods.UseVisualStyleBackColor = true;
            this.checkBoxNoMods.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.applyToolStripMenuItem,
            this.createListToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1330, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectInstallToolStripMenuItem,
            this.selectPruneListToolStripMenuItem,
            this.mergeListToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectInstallToolStripMenuItem
            // 
            this.selectInstallToolStripMenuItem.Name = "selectInstallToolStripMenuItem";
            this.selectInstallToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.selectInstallToolStripMenuItem.Text = "Select Install";
            this.selectInstallToolStripMenuItem.Click += new System.EventHandler(this.selectInstallToolStripMenuItem_Click);
            // 
            // selectPruneListToolStripMenuItem
            // 
            this.selectPruneListToolStripMenuItem.Name = "selectPruneListToolStripMenuItem";
            this.selectPruneListToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.selectPruneListToolStripMenuItem.Text = "Select PruneList";
            this.selectPruneListToolStripMenuItem.Click += new System.EventHandler(this.selectPruneListToolStripMenuItem_Click);
            // 
            // mergeListToolStripMenuItem
            // 
            this.mergeListToolStripMenuItem.Name = "mergeListToolStripMenuItem";
            this.mergeListToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.mergeListToolStripMenuItem.Text = "Merge with existing list";
            this.mergeListToolStripMenuItem.Click += new System.EventHandler(this.mergeListToolStripMenuItem_Click);
            // 
            // applyToolStripMenuItem
            // 
            this.applyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openApplyWindowToolStripMenuItem});
            this.applyToolStripMenuItem.Name = "applyToolStripMenuItem";
            this.applyToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.applyToolStripMenuItem.Text = "Apply";
            // 
            // openApplyWindowToolStripMenuItem
            // 
            this.openApplyWindowToolStripMenuItem.Name = "openApplyWindowToolStripMenuItem";
            this.openApplyWindowToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openApplyWindowToolStripMenuItem.Text = "Open Apply Window";
            this.openApplyWindowToolStripMenuItem.Click += new System.EventHandler(this.openApplyWindowToolStripMenuItem_Click);
            // 
            // createListToolStripMenuItem
            // 
            this.createListToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nonRP0ListsToolStripMenuItem,
            this.resourceRepairListsToolStripMenuItem});
            this.createListToolStripMenuItem.Name = "createListToolStripMenuItem";
            this.createListToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.createListToolStripMenuItem.Text = "Create list";
            // 
            // nonRP0ListsToolStripMenuItem
            // 
            this.nonRP0ListsToolStripMenuItem.Name = "nonRP0ListsToolStripMenuItem";
            this.nonRP0ListsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.nonRP0ListsToolStripMenuItem.Text = "nonRP0 lists";
            this.nonRP0ListsToolStripMenuItem.Click += new System.EventHandler(this.nonRP0ListsToolStripMenuItem_Click);
            // 
            // resourceRepairListsToolStripMenuItem
            // 
            this.resourceRepairListsToolStripMenuItem.Name = "resourceRepairListsToolStripMenuItem";
            this.resourceRepairListsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.resourceRepairListsToolStripMenuItem.Text = "resource repair lists";
            this.resourceRepairListsToolStripMenuItem.Click += new System.EventHandler(this.resourceRepairListsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aintNoHelpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aintNoHelpToolStripMenuItem
            // 
            this.aintNoHelpToolStripMenuItem.Name = "aintNoHelpToolStripMenuItem";
            this.aintNoHelpToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.aintNoHelpToolStripMenuItem.Text = "aint no help";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Prune File:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(659, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Pruned Parts:";
            // 
            // checkBoxNoCopies
            // 
            this.checkBoxNoCopies.AutoSize = true;
            this.checkBoxNoCopies.Location = new System.Drawing.Point(581, 91);
            this.checkBoxNoCopies.Name = "checkBoxNoCopies";
            this.checkBoxNoCopies.Size = new System.Drawing.Size(72, 17);
            this.checkBoxNoCopies.TabIndex = 5;
            this.checkBoxNoCopies.Text = "no copies";
            this.checkBoxNoCopies.UseVisualStyleBackColor = true;
            this.checkBoxNoCopies.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // buttonResetList
            // 
            this.buttonResetList.Location = new System.Drawing.Point(567, 57);
            this.buttonResetList.Name = "buttonResetList";
            this.buttonResetList.Size = new System.Drawing.Size(88, 23);
            this.buttonResetList.TabIndex = 2;
            this.buttonResetList.Text = "Reset Pruning";
            this.buttonResetList.UseVisualStyleBackColor = true;
            this.buttonResetList.Click += new System.EventHandler(this.buttonResetList_Click);
            // 
            // checkBoxNoDeleted
            // 
            this.checkBoxNoDeleted.AutoSize = true;
            this.checkBoxNoDeleted.Location = new System.Drawing.Point(427, 92);
            this.checkBoxNoDeleted.Name = "checkBoxNoDeleted";
            this.checkBoxNoDeleted.Size = new System.Drawing.Size(76, 17);
            this.checkBoxNoDeleted.TabIndex = 5;
            this.checkBoxNoDeleted.Text = "no deleted";
            this.checkBoxNoDeleted.UseVisualStyleBackColor = true;
            this.checkBoxNoDeleted.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxNoWild
            // 
            this.checkBoxNoWild.AutoSize = true;
            this.checkBoxNoWild.Location = new System.Drawing.Point(335, 92);
            this.checkBoxNoWild.Name = "checkBoxNoWild";
            this.checkBoxNoWild.Size = new System.Drawing.Size(88, 17);
            this.checkBoxNoWild.TabIndex = 5;
            this.checkBoxNoWild.Text = "no wild cards";
            this.checkBoxNoWild.UseVisualStyleBackColor = true;
            this.checkBoxNoWild.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 843);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.listBoxParts);
            this.Controls.Add(this.listBoxPruned);
            this.Controls.Add(this.checkBoxNoCopies);
            this.Controls.Add(this.checkBoxNoWild);
            this.Controls.Add(this.checkBoxNoDeleted);
            this.Controls.Add(this.checkBoxNoMods);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.buttonResetList);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPrune);
            this.Controls.Add(this.textBoxSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Partfinder v0.2";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label textBoxPrune;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxPruned;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBoxParts;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox checkBoxNoMods;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectInstallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectPruneListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aintNoHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openApplyWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nonRP0ListsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resourceRepairListsToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxNoCopies;
        private System.Windows.Forms.Button buttonResetList;
        private System.Windows.Forms.CheckBox checkBoxNoDeleted;
        private System.Windows.Forms.CheckBox checkBoxNoWild;
    }
}