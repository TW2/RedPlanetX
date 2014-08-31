namespace RedPlanetX
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Objects");
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbOpenVideo = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.cmsTreeview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsTVmnuAddScript = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTVmnuAddTA = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTVmnuAddTH = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTVmnuAddTV = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTVmnuAddShape = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTVmnuAddDrawing = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTVmnuAddPicture = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTVmnuAddVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsTVmnuAddE = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsTVmnuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.propertyGridEx1 = new PropertyGridEx.PropertyGridEx();
            this.ofdVideo = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsbRegenerate = new System.Windows.Forms.ToolStripButton();
            this.tsbEncode = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.cmsTreeview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpenVideo,
            this.tsbRegenerate,
            this.tsbEncode});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1306, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbOpenVideo
            // 
            this.tsbOpenVideo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenVideo.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpenVideo.Image")));
            this.tsbOpenVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenVideo.Name = "tsbOpenVideo";
            this.tsbOpenVideo.Size = new System.Drawing.Size(23, 22);
            this.tsbOpenVideo.Text = "toolStripButton1";
            this.tsbOpenVideo.ToolTipText = "Open a video...";
            this.tsbOpenVideo.Click += new System.EventHandler(this.tsbOpenVideo_Click);
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.cmsTreeview;
            this.treeView1.Location = new System.Drawing.Point(12, 28);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "nnObjects";
            treeNode1.Text = "Objects";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.Size = new System.Drawing.Size(233, 314);
            this.treeView1.TabIndex = 3;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeview1_AfterSelect);
            // 
            // cmsTreeview
            // 
            this.cmsTreeview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsTVmnuAddScript,
            this.cmsTVmnuAddTA,
            this.cmsTVmnuAddTH,
            this.cmsTVmnuAddTV,
            this.cmsTVmnuAddShape,
            this.cmsTVmnuAddDrawing,
            this.cmsTVmnuAddPicture,
            this.cmsTVmnuAddVideo,
            this.toolStripSeparator1,
            this.cmsTVmnuAddE,
            this.toolStripSeparator2,
            this.cmsTVmnuRemove});
            this.cmsTreeview.Name = "cmsTreeview";
            this.cmsTreeview.Size = new System.Drawing.Size(232, 236);
            // 
            // cmsTVmnuAddScript
            // 
            this.cmsTVmnuAddScript.Name = "cmsTVmnuAddScript";
            this.cmsTVmnuAddScript.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddScript.Text = "Add a script...";
            this.cmsTVmnuAddScript.Click += new System.EventHandler(this.cmsTVmnuAddO_Click);
            // 
            // cmsTVmnuAddTA
            // 
            this.cmsTVmnuAddTA.Name = "cmsTVmnuAddTA";
            this.cmsTVmnuAddTA.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddTA.Text = "Add a text in an area...";
            this.cmsTVmnuAddTA.Click += new System.EventHandler(this.cmsTVmnuAddTA_Click);
            // 
            // cmsTVmnuAddTH
            // 
            this.cmsTVmnuAddTH.Name = "cmsTVmnuAddTH";
            this.cmsTVmnuAddTH.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddTH.Text = "Add a horizontal text...";
            this.cmsTVmnuAddTH.Click += new System.EventHandler(this.cmsTVmnuAddTH_Click);
            // 
            // cmsTVmnuAddTV
            // 
            this.cmsTVmnuAddTV.Name = "cmsTVmnuAddTV";
            this.cmsTVmnuAddTV.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddTV.Text = "Add a vertical text...";
            this.cmsTVmnuAddTV.Click += new System.EventHandler(this.cmsTVmnuAddTV_Click);
            // 
            // cmsTVmnuAddShape
            // 
            this.cmsTVmnuAddShape.Name = "cmsTVmnuAddShape";
            this.cmsTVmnuAddShape.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddShape.Text = "Add a shape...";
            this.cmsTVmnuAddShape.Click += new System.EventHandler(this.cmsTVmnuAddShape_Click);
            // 
            // cmsTVmnuAddDrawing
            // 
            this.cmsTVmnuAddDrawing.Name = "cmsTVmnuAddDrawing";
            this.cmsTVmnuAddDrawing.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddDrawing.Text = "Add a drawing...";
            this.cmsTVmnuAddDrawing.Click += new System.EventHandler(this.cmsTVmnuAddDrawing_Click);
            // 
            // cmsTVmnuAddPicture
            // 
            this.cmsTVmnuAddPicture.Name = "cmsTVmnuAddPicture";
            this.cmsTVmnuAddPicture.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddPicture.Text = "Add a picture...";
            this.cmsTVmnuAddPicture.Click += new System.EventHandler(this.cmsTVmnuAddPicture_Click);
            // 
            // cmsTVmnuAddVideo
            // 
            this.cmsTVmnuAddVideo.Name = "cmsTVmnuAddVideo";
            this.cmsTVmnuAddVideo.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddVideo.Text = "Add a video...";
            this.cmsTVmnuAddVideo.Click += new System.EventHandler(this.cmsTVmnuAddVideo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(228, 6);
            // 
            // cmsTVmnuAddE
            // 
            this.cmsTVmnuAddE.Name = "cmsTVmnuAddE";
            this.cmsTVmnuAddE.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuAddE.Text = "Add an event...";
            this.cmsTVmnuAddE.Click += new System.EventHandler(this.cmsTVmnuAddE_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(228, 6);
            this.toolStripSeparator2.Click += new System.EventHandler(this.toolStripSeparator2_Click);
            // 
            // cmsTVmnuRemove
            // 
            this.cmsTVmnuRemove.Name = "cmsTVmnuRemove";
            this.cmsTVmnuRemove.Size = new System.Drawing.Size(231, 22);
            this.cmsTVmnuRemove.Text = "Remove an event or an object";
            this.cmsTVmnuRemove.Click += new System.EventHandler(this.cmsTVmnuRemove_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(251, 28);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(1043, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Blue;
            this.pictureBox1.Location = new System.Drawing.Point(63, 118);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(921, 428);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // propertyGridEx1
            // 
            // 
            // 
            // 
            this.propertyGridEx1.DocCommentDescription.AccessibleName = "";
            this.propertyGridEx1.DocCommentDescription.AutoEllipsis = true;
            this.propertyGridEx1.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.propertyGridEx1.DocCommentDescription.Location = new System.Drawing.Point(3, 18);
            this.propertyGridEx1.DocCommentDescription.Name = "";
            this.propertyGridEx1.DocCommentDescription.Size = new System.Drawing.Size(227, 37);
            this.propertyGridEx1.DocCommentDescription.TabIndex = 1;
            this.propertyGridEx1.DocCommentImage = null;
            // 
            // 
            // 
            this.propertyGridEx1.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.propertyGridEx1.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.propertyGridEx1.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.propertyGridEx1.DocCommentTitle.Name = "";
            this.propertyGridEx1.DocCommentTitle.Size = new System.Drawing.Size(227, 15);
            this.propertyGridEx1.DocCommentTitle.TabIndex = 0;
            this.propertyGridEx1.DocCommentTitle.UseMnemonic = false;
            this.propertyGridEx1.Location = new System.Drawing.Point(12, 348);
            this.propertyGridEx1.Name = "propertyGridEx1";
            this.propertyGridEx1.SelectedObject = ((object)(resources.GetObject("propertyGridEx1.SelectedObject")));
            this.propertyGridEx1.ShowCustomProperties = true;
            this.propertyGridEx1.Size = new System.Drawing.Size(233, 385);
            this.propertyGridEx1.TabIndex = 6;
            // 
            // 
            // 
            this.propertyGridEx1.ToolStrip.AccessibleName = "ToolBar";
            this.propertyGridEx1.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.propertyGridEx1.ToolStrip.AllowMerge = false;
            this.propertyGridEx1.ToolStrip.AutoSize = false;
            this.propertyGridEx1.ToolStrip.CanOverflow = false;
            this.propertyGridEx1.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.propertyGridEx1.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.propertyGridEx1.ToolStrip.Location = new System.Drawing.Point(0, 1);
            this.propertyGridEx1.ToolStrip.Name = "";
            this.propertyGridEx1.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.propertyGridEx1.ToolStrip.Size = new System.Drawing.Size(233, 25);
            this.propertyGridEx1.ToolStrip.TabIndex = 1;
            this.propertyGridEx1.ToolStrip.TabStop = true;
            this.propertyGridEx1.ToolStrip.Text = "PropertyGridToolBar";
            this.propertyGridEx1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridEx1_PropertyValueChanged);
            // 
            // ofdVideo
            // 
            this.ofdVideo.FileName = "openFileDialog1";
            this.ofdVideo.Filter = "Videos|*.mp4;*.avi;*.mkv";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(251, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1043, 654);
            this.panel1.TabIndex = 7;
            // 
            // tsbRegenerate
            // 
            this.tsbRegenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRegenerate.Image = ((System.Drawing.Image)(resources.GetObject("tsbRegenerate.Image")));
            this.tsbRegenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRegenerate.Name = "tsbRegenerate";
            this.tsbRegenerate.Size = new System.Drawing.Size(23, 22);
            this.tsbRegenerate.Text = "toolStripButton1";
            this.tsbRegenerate.Click += new System.EventHandler(this.tsbRegenerate_Click);
            // 
            // tsbEncode
            // 
            this.tsbEncode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEncode.Image = ((System.Drawing.Image)(resources.GetObject("tsbEncode.Image")));
            this.tsbEncode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEncode.Name = "tsbEncode";
            this.tsbEncode.Size = new System.Drawing.Size(23, 22);
            this.tsbEncode.Text = "toolStripButton1";
            this.tsbEncode.Click += new System.EventHandler(this.tsbEncode_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 745);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.propertyGridEx1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "RedPlanet Xpress";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsTreeview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private PropertyGridEx.PropertyGridEx propertyGridEx1;
        private System.Windows.Forms.ContextMenuStrip cmsTreeview;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddScript;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddE;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuRemove;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddTA;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddTH;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddTV;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddShape;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddDrawing;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddPicture;
        private System.Windows.Forms.ToolStripMenuItem cmsTVmnuAddVideo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbOpenVideo;
        private System.Windows.Forms.OpenFileDialog ofdVideo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton tsbRegenerate;
        private System.Windows.Forms.ToolStripButton tsbEncode;



    }
}

