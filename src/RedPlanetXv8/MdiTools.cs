using RedPlanetXv8.ASS;
using RedPlanetXv8.AviSynth;
using RedPlanetXv8.Composition;
using RedPlanetXv8.Node;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetXv8
{
    public partial class MdiTools : Form
    {
        ImageList imglist = new ImageList();
        MainTreeNode maintn = new MainTreeNode();
        AVSTreeNode avstn = new AVSTreeNode();
        ASSTreeNode asstn = null;
        TreeNode lastSelectedTreeNode = null;
        Form1 form1 = null;
        PropertyGridEx.PropertyGridEx pgex = new PropertyGridEx.PropertyGridEx();
        int parentCount = 1;
        int pathCount = 1;


        public MdiTools()
        {
            InitializeComponent();

            imglist.ImageSize = new Size(32, 32);
            imglist.Images.Add(Data._48px_Crystal_Clear_app_kcmdf); //0
            imglist.Images.Add(Data._48px_Crystal_Clear_app_camera); //1
            imglist.Images.Add(Data._48px_Crystal_Clear_app_kllckety); //2
            imglist.Images.Add(Data._48px_Crystal_Clear_app_ksplash); //3
            imglist.Images.Add(Data._48px_Crystal_Clear_app_applixware); //4
            imglist.Images.Add(Data._48px_Crystal_Clear_app_applixrouge); //5
            imglist.Images.Add(Data._48px_Crystal_Clear_app_applixvert); //6
            imglist.Images.Add(Data._48px_Crystal_Clear_app_applixbleu); //7
            imglist.Images.Add(Data._48px_Crystal_Clear_app_applixjaune); //8
            imglist.Images.Add(Data._48_path); //9

            treeProject.ImageList = imglist;

            pgex.Location = new Point(treeProject.Location.X, treeProject.Height + 20);
            pgex.Size = new Size(treeProject.Width, this.Height - treeProject.Height - 65);
            pgex.ShowCustomProperties = true;
            this.Controls.Add(pgex);

            treeProject.AfterSelect += TreeProject_AfterSelect;
            pgex.PropertyValueChanged += Pgex_PropertyValueChanged;
        }

        public Form1 ParentFormLink
        {
            set { form1 = value; }
        }

        private void Pgex_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Dictionary<ShapeTreeNode, ParentTreeNode> shapelink = form1.MdiView.View.ShapeLink;
            TreeNode selected = treeProject.SelectedNode;

            foreach (KeyValuePair<ShapeTreeNode, ParentTreeNode> pair in shapelink)
            {
                ShapeObject sho = pair.Key.ShapeObject;
                ParentObject p = pair.Value.ParentObject;

                if(selected.GetType() == typeof(ShapeTreeNode))
                {
                    ShapeTreeNode stn = (ShapeTreeNode)selected;
                    
                    if(stn.Equals(pair.Key))
                    {
                        CompareAndUpdateShapeObject(e.ChangedItem, e.OldValue, sho);
                    }
                }
            }

            
            if (selected.GetType() == typeof(LetterTreeNode))
            {
                LetterTreeNode ltn = (LetterTreeNode)selected;
                CompareAndUpdateLetter(e.ChangedItem, e.OldValue, ltn.String);
            }
            else if (selected.GetType() == typeof(SyllableTreeNode))
            {
                SyllableTreeNode stn = (SyllableTreeNode)selected;
                CompareAndUpdateSyllable(e.ChangedItem, e.OldValue, stn.String);
            }
            else if (selected.GetType() == typeof(SentenceTreeNode))
            {
                SentenceTreeNode stn = (SentenceTreeNode)selected;
                CompareAndUpdateSentence(e.ChangedItem, e.OldValue, stn.String);
            }

            form1.MdiView.View.UpdatePaintOnly();
        }

        private void TreeProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (lastSelectedTreeNode != null && lastSelectedTreeNode.GetType() == typeof(PathTreeNode))
            {
                PathTreeNode ptn = (PathTreeNode)lastSelectedTreeNode;
                PathObject po = ptn.PathObject;
                po.Hide = true;
                form1.MdiView.View.UpdatePaintOnly();
            }

            TreeNode selected = treeProject.SelectedNode;
            //================================================================================
            // MAIN TREENODE
            //================================================================================
            if (selected.GetType() == typeof(MainTreeNode))
            {
                selected.SelectedImageIndex = 0;

                pgex.Item.Clear();
                pgex.Refresh();
            }


            //================================================================================
            // AVS TREENODE
            //================================================================================ 
            if (selected.GetType() == typeof(AVSTreeNode))
            {
                selected.SelectedImageIndex = 1;

                pgex.Item.Clear();
                pgex.Refresh();
            }


            //================================================================================
            // ASS TREENODE
            //================================================================================
            if (selected.GetType() == typeof(ASSTreeNode))
            {
                selected.SelectedImageIndex = 4;

                pgex.Item.Clear();
                pgex.Refresh();
            }


            //================================================================================
            // SENTENCE TREENODE
            //================================================================================
            if (selected.GetType() == typeof(SentenceTreeNode))
            {
                selected.SelectedImageIndex = 8;

                SentenceTreeNode stn = (SentenceTreeNode)selected;
                Sentence sen = stn.String;

                long S = sen.SentenceStart, E = sen.SentenceEnd;

                pgex.Item.Clear();
                pgex.Item.Add("Start time", S, false, "Time", "Time in milliseconds from sentence", true);
                pgex.Item.Add("End time", E, false, "Time", "Time in milliseconds from sentence", true);
                pgex.Item.Add("Angle X at start", sen.GetAngleX(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle X at end", sen.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at start", sen.GetAngleY(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at end", sen.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at start", sen.GetAngleZ(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at end", sen.GetAngleZ(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Front color at start", sen.GetFrontColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Front color at end", sen.GetFrontColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Back color at start", sen.GetBackColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Back color at end", sen.GetBackColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Border color at start", sen.GetBorderColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Border color at end", sen.GetBorderColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at start", sen.GetShadowColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at end", sen.GetShadowColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Position X at start", sen.GetRelativePositionX(S), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position X at end", sen.GetRelativePositionX(E), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position Y at start", sen.GetRelativePositionY(S), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Position Y at end", sen.GetRelativePositionY(E), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Quake X at start", sen.GetQuakeX(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake X at end", sen.GetQuakeX(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at start", sen.GetQuakeY(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at end", sen.GetQuakeY(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Scale X at start", sen.GetScaleX(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale X at end", sen.GetScaleX(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at start", sen.GetScaleY(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at end", sen.GetScaleY(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Border weight at start", sen.GetBorderWeight(S), false, "Scale", "Weight", true);
                pgex.Item.Add("Border weight at end", sen.GetBorderWeight(E), false, "Scale", "Weight", true);
                pgex.Item.Add("Shadow depth at start", sen.GetShadowDepth(S), false, "Scale", "Depth", true);
                pgex.Item.Add("Shadow depth at end", sen.GetShadowDepth(E), false, "Scale", "Depth", true);
                pgex.Refresh();
            }


            //================================================================================
            // SYLLABLE TREENODE
            //================================================================================
            if (selected.GetType() == typeof(SyllableTreeNode))
            {
                selected.SelectedImageIndex = 8;

                SyllableTreeNode stn = (SyllableTreeNode)selected;
                Syllable syl = stn.String;

                long S = syl.SyllableStart, E = syl.SyllableEnd;

                pgex.Item.Clear();
                pgex.Item.Add("Start time", S, false, "Time", "Time in milliseconds from syllable", true);
                pgex.Item.Add("End time", E, false, "Time", "Time in milliseconds from syllable", true);
                pgex.Item.Add("Angle X at start", syl.GetAngleX(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle X at end", syl.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at start", syl.GetAngleY(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at end", syl.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at start", syl.GetAngleZ(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at end", syl.GetAngleZ(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Front color at start", syl.GetFrontColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Front color at end", syl.GetFrontColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Back color at start", syl.GetBackColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Back color at end", syl.GetBackColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Border color at start", syl.GetBorderColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Border color at end", syl.GetBorderColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at start", syl.GetShadowColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at end", syl.GetShadowColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Position X at start", syl.GetRelativePositionX(S), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position X at end", syl.GetRelativePositionX(E), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position Y at start", syl.GetRelativePositionY(S), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Position Y at end", syl.GetRelativePositionY(E), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Quake X at start", syl.GetQuakeX(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake X at end", syl.GetQuakeX(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at start", syl.GetQuakeY(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at end", syl.GetQuakeY(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Scale X at start", syl.GetScaleX(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale X at end", syl.GetScaleX(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at start", syl.GetScaleY(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at end", syl.GetScaleY(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Border weight at start", syl.GetBorderWeight(S), false, "Scale", "Weight", true);
                pgex.Item.Add("Border weight at end", syl.GetBorderWeight(E), false, "Scale", "Weight", true);
                pgex.Item.Add("Shadow depth at start", syl.GetShadowDepth(S), false, "Scale", "Depth", true);
                pgex.Item.Add("Shadow depth at end", syl.GetShadowDepth(E), false, "Scale", "Depth", true);
                pgex.Refresh();
            }


            //================================================================================
            // LETTER TREENODE
            //================================================================================
            if (selected.GetType() == typeof(LetterTreeNode))
            {
                selected.SelectedImageIndex = 8;

                LetterTreeNode ltn = (LetterTreeNode)selected;
                Letter let = ltn.String;

                long S = let.LetterStart, E = let.LetterEnd;

                pgex.Item.Clear();
                pgex.Item.Add("Start time", S, false, "Time", "Time in milliseconds from letter", true);
                pgex.Item.Add("End time", E, false, "Time", "Time in milliseconds from letter", true);
                pgex.Item.Add("Angle X at start", let.GetAngleX(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle X at end", let.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at start", let.GetAngleY(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at end", let.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at start", let.GetAngleZ(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at end", let.GetAngleZ(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Front color at start", let.GetFrontColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Front color at end", let.GetFrontColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Back color at start", let.GetBackColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Back color at end", let.GetBackColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Border color at start", let.GetBorderColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Border color at end", let.GetBorderColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at start", let.GetShadowColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at end", let.GetShadowColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Position X at start", let.GetRelativePositionX(S), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position X at end", let.GetRelativePositionX(E), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position Y at start", let.GetRelativePositionY(S), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Position Y at end", let.GetRelativePositionY(E), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Quake X at start", let.GetQuakeX(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake X at end", let.GetQuakeX(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at start", let.GetQuakeY(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at end", let.GetQuakeY(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Scale X at start", let.GetScaleX(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale X at end", let.GetScaleX(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at start", let.GetScaleY(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at end", let.GetScaleY(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Border weight at start", let.GetBorderWeight(S), false, "Scale", "Weight", true);
                pgex.Item.Add("Border weight at end", let.GetBorderWeight(E), false, "Scale", "Weight", true);
                pgex.Item.Add("Shadow depth at start", let.GetShadowDepth(S), false, "Scale", "Depth", true);
                pgex.Item.Add("Shadow depth at end", let.GetShadowDepth(E), false, "Scale", "Depth", true);
                pgex.Refresh();
            }


            //================================================================================
            // PARENT TREENODE
            //================================================================================
            if (selected.GetType() == typeof(ParentTreeNode))
            {
                selected.SelectedImageIndex = 2;

                pgex.Item.Clear();
                pgex.Refresh();
            }


            //================================================================================
            // SHAPE TREENODE
            //================================================================================
            if (selected.GetType() == typeof(ShapeTreeNode))
            {
                selected.SelectedImageIndex = 3;

                ShapeTreeNode stn = (ShapeTreeNode)selected;
                ShapeObject sho = stn.ShapeObject;

                long S = sho.Start, E = sho.End;

                pgex.Item.Clear();
                PreparePropertyGridForPathTreeNode(pgex, sho);
                pgex.Item.Add("Start time", S, false, "Time", "Time in milliseconds from sentence", true);
                pgex.Item.Add("End time", E, false, "Time", "Time in milliseconds from sentence", true);
                pgex.Item.Add("Angle X at start", sho.GetAngleX(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle X at end", sho.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at start", sho.GetAngleY(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Y at end", sho.GetAngleY(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at start", sho.GetAngleZ(S), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Angle Z at end", sho.GetAngleZ(E), false, "Angle", "Angle in degrees", true);
                pgex.Item.Add("Front color at start", sho.GetFrontColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Front color at end", sho.GetFrontColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Back color at start", sho.GetBackColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Back color at end", sho.GetBackColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Border color at start", sho.GetBorderColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Border color at end", sho.GetBorderColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at start", sho.GetShadowColor(S), false, "Color", "Color", true);
                pgex.Item.Add("Shadow color at end", sho.GetShadowColor(E), false, "Color", "Color", true);
                pgex.Item.Add("Position X at start", sho.GetRelativePositionX(S), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position X at end", sho.GetRelativePositionX(E), false, "Position", "Relative position on X", true);
                pgex.Item.Add("Position Y at start", sho.GetRelativePositionY(S), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Position Y at end", sho.GetRelativePositionY(E), false, "Position", "Relative position on Y", true);
                pgex.Item.Add("Quake X at start", sho.GetQuakeX(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake X at end", sho.GetQuakeX(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at start", sho.GetQuakeY(S), false, "Quake", "Quake", true);
                pgex.Item.Add("Quake Y at end", sho.GetQuakeY(E), false, "Quake", "Quake", true);
                pgex.Item.Add("Scale X at start", sho.GetScaleX(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale X at end", sho.GetScaleX(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at start", sho.GetScaleY(S), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Scale Y at end", sho.GetScaleY(E), false, "Scale", "Scale from 0.0 to 1.0", true);
                pgex.Item.Add("Border weight at start", sho.GetBorderWeight(S), false, "Scale", "Weight", true);
                pgex.Item.Add("Border weight at end", sho.GetBorderWeight(E), false, "Scale", "Weight", true);
                pgex.Item.Add("Shadow depth at start", sho.GetShadowDepth(S), false, "Scale", "Depth", true);
                pgex.Item.Add("Shadow depth at end", sho.GetShadowDepth(E), false, "Scale", "Depth", true);
                pgex.Refresh();
            }


            //================================================================================
            // PATH TREENODE
            //================================================================================
            if (selected.GetType() == typeof(PathTreeNode))
            {
                selected.SelectedImageIndex = 9;

                pgex.Item.Clear();
                pgex.Refresh();

                PathTreeNode ptn = (PathTreeNode)selected;
                PathObject po = ptn.PathObject;
                po.Hide = false;
                form1.MdiView.View.LastPathTreeNode = ptn;
                form1.MdiView.View.UpdatePaintOnly();
            }

            lastSelectedTreeNode = selected;
        }

        private void PreparePropertyGridForPathTreeNode(PropertyGridEx.PropertyGridEx pgex, ShapeObject so)
        {
            List<PathTreeNode> ptn_list = new List<PathTreeNode>();
            PathTreeNode dummy = new PathTreeNode();
            dummy.Text = "No path";
            dummy.PathObject = null;
            ptn_list.Add(dummy);

            foreach (TreeNode tn in avstn.Nodes)
            {
                if(tn.GetType() == typeof(PathTreeNode))
                {
                    PathTreeNode ptn = (PathTreeNode)tn;
                    ptn_list.Add(ptn);
                }
            }

            ArrayList ary = new ArrayList(ptn_list);

            int v = 0;
            if (so.GetPathTreeNode().PathIndex != v) { v = so.GetPathTreeNode().PathIndex; }

            pgex.Item.Add("Path", ary[v], false, "Path", "Path of the shape", true);
            pgex.Item[pgex.Item.Count - 1].Choices = new PropertyGridEx.CustomChoices(ary);
        }

        public void SetROOT()
        {
            maintn.Text = "Project";
            maintn.ImageIndex = 0;
            treeProject.Nodes.Add(maintn);
        }

        public void SetCompositionAVS(Settings set, AviSynthObject avso)
        {
            avstn.Composition = set;
            avstn.Avs = avso;
            avstn.Text = "AviSynth";
            avstn.ImageIndex = 1;
            maintn.Nodes.Add(avstn);
        }

        public void SetMainASS(AssScript asss)
        {
            asstn = new ASSTreeNode();
            asstn.Script = asss;
            asstn.Text = "Strings";
            asstn.ImageIndex = 4;
            avstn.Nodes.Add(asstn);
        }

        public void AddSentences()
        {
            if(asstn != null)
            {
                foreach (Sentence sent in asstn.Script.Sentences)
                {
                    SentenceTreeNode senttn = new SentenceTreeNode(sent);
                    senttn.Text = sent.String;
                    senttn.ImageIndex = 5;

                    foreach (Syllable syl in sent.Syllables)
                    {
                        SyllableTreeNode syltn = new SyllableTreeNode(syl);
                        senttn.Nodes.Add(syltn);
                        syltn.Text = syl.Syl;
                        syltn.ImageIndex = 6;

                        foreach (Letter let in syl.Letters)
                        {
                            LetterTreeNode lettn = new LetterTreeNode(let);
                            syltn.Nodes.Add(lettn);
                            lettn.Text = let.String;
                            lettn.ImageIndex = 7;
                        }
                    }
                    asstn.Nodes.Add(senttn);
                }                
            }
        }

        public void AddParent()
        {
            ParentTreeNode partn = new ParentTreeNode(new ParentObject());
            partn.Text = "Parent " + parentCount;
            partn.ImageIndex = 2;
            avstn.Nodes.Add(partn);
            parentCount++;
        }

        public void AddShape(ShapeObject so, out ShapeTreeNode shptn, out ParentTreeNode partn)
        {
            TreeNode selected = treeProject.SelectedNode;
            if(selected.GetType() == typeof(ParentTreeNode))
            {
                partn = (ParentTreeNode)selected;

                shptn = new ShapeTreeNode(so);
                shptn.Text = "Shape";
                shptn.ImageIndex = 3;
                partn.Nodes.Add(shptn);
            }
            else
            {
                partn = null;
                shptn = null;
            }            
        }

        public void AddPath()
        {
            PathTreeNode pathtn = new PathTreeNode(new PathObject());
            pathtn.Text = "Path " + pathCount;
            pathtn.PathIndex = pathCount;
            pathtn.ImageIndex = 9;
            avstn.Nodes.Add(pathtn);
            pathCount++;
        }

        private void CompareAndUpdateShapeObject(GridItem changed, object oldvalue, ShapeObject sho)
        {
            long S = sho.Start, E = sho.End;

            if (S.Equals(oldvalue) & changed.Label.Equals("Start time")) { sho.Start = (long)changed.Value; }
            if (E.Equals(oldvalue) & changed.Label.Equals("End time")) { sho.End = (long)changed.Value; }
            if (sho.GetAngleX(S).Equals(oldvalue) & changed.Label.Equals("Angle X at start")) { sho.SetAngleX(S, (int)changed.Value); }
            if (sho.GetAngleX(E).Equals(oldvalue) & changed.Label.Equals("Angle X at end")) { sho.SetAngleX(E, (int)changed.Value); }
            if (sho.GetAngleY(S).Equals(oldvalue) & changed.Label.Equals("Angle Y at start")) { sho.SetAngleY(S, (int)changed.Value); }
            if (sho.GetAngleY(E).Equals(oldvalue) & changed.Label.Equals("Angle Y at end")) { sho.SetAngleY(E, (int)changed.Value); }
            if (sho.GetAngleZ(S).Equals(oldvalue) & changed.Label.Equals("Angle Z at start")) { sho.SetAngleZ(S, (int)changed.Value); }
            if (sho.GetAngleZ(E).Equals(oldvalue) & changed.Label.Equals("Angle Z at end")) { sho.SetAngleZ(E, (int)changed.Value); }
            if (sho.GetFrontColor(S).Equals(oldvalue) & changed.Label.Equals("Front color at start")) { sho.SetFrontColor(S, (Color)changed.Value); }
            if (sho.GetFrontColor(E).Equals(oldvalue) & changed.Label.Equals("Front color at end")) { sho.SetFrontColor(E, (Color)changed.Value); }
            if (sho.GetBackColor(S).Equals(oldvalue) & changed.Label.Equals("Back color at start")) { sho.SetBackColor(S, (Color)changed.Value); }
            if (sho.GetBackColor(E).Equals(oldvalue) & changed.Label.Equals("Back color at end")) { sho.SetBackColor(E, (Color)changed.Value); }
            if (sho.GetBorderColor(S).Equals(oldvalue) & changed.Label.Equals("Border color at start")) { sho.SetBorderColor(S, (Color)changed.Value); }
            if (sho.GetBorderColor(E).Equals(oldvalue) & changed.Label.Equals("Border color at end")) { sho.SetBorderColor(E, (Color)changed.Value); }
            if (sho.GetShadowColor(S).Equals(oldvalue) & changed.Label.Equals("Shadow color at start")) { sho.SetShadowColor(S, (Color)changed.Value); }
            if (sho.GetShadowColor(E).Equals(oldvalue) & changed.Label.Equals("Shadow color at end")) { sho.SetShadowColor(E, (Color)changed.Value); }
            if (sho.GetRelativePositionX(S).Equals(oldvalue) & changed.Label.Equals("Position X at start")) { sho.SetRelativePositionX(S, (float)changed.Value); }
            if (sho.GetRelativePositionX(E).Equals(oldvalue) & changed.Label.Equals("Position X at end")) { sho.SetRelativePositionX(E, (float)changed.Value); }
            if (sho.GetRelativePositionY(S).Equals(oldvalue) & changed.Label.Equals("Position Y at start")) { sho.SetRelativePositionY(S, (float)changed.Value); }
            if (sho.GetRelativePositionY(E).Equals(oldvalue) & changed.Label.Equals("Position Y at end")) { sho.SetRelativePositionY(E, (float)changed.Value); }
            if (sho.GetQuakeX(S).Equals(oldvalue) & changed.Label.Equals("Quake X at start")) { sho.SetQuakeX(S, (int)changed.Value); }
            if (sho.GetQuakeX(E).Equals(oldvalue) & changed.Label.Equals("Quake X at end")) { sho.SetQuakeX(E, (int)changed.Value); }
            if (sho.GetQuakeY(S).Equals(oldvalue) & changed.Label.Equals("Quake Y at start")) { sho.SetQuakeY(S, (int)changed.Value); }
            if (sho.GetQuakeY(E).Equals(oldvalue) & changed.Label.Equals("Quake Y at end")) { sho.SetQuakeY(E, (int)changed.Value); }
            if (sho.GetScaleX(S).Equals(oldvalue) & changed.Label.Equals("Scale X at start")) { sho.SetScaleX(S, (float)changed.Value); }
            if (sho.GetScaleX(E).Equals(oldvalue) & changed.Label.Equals("Scale X at end")) { sho.SetScaleX(E, (float)changed.Value); }
            if (sho.GetScaleY(S).Equals(oldvalue) & changed.Label.Equals("Scale Y at start")) { sho.SetScaleY(S, (float)changed.Value); }
            if (sho.GetScaleY(E).Equals(oldvalue) & changed.Label.Equals("Scale Y at end")) { sho.SetScaleY(E, (float)changed.Value); }
            if (sho.GetBorderWeight(S).Equals(oldvalue) & changed.Label.Equals("Border weight at start")) { sho.SetBorderWeight(S, (int)changed.Value); }
            if (sho.GetBorderWeight(E).Equals(oldvalue) & changed.Label.Equals("Border weight at end")) { sho.SetBorderWeight(E, (int)changed.Value); }
            if (sho.GetShadowDepth(S).Equals(oldvalue) & changed.Label.Equals("Shadow depth at start")) { sho.SetShadowDepth(S, (int)changed.Value); }
            if (sho.GetShadowDepth(E).Equals(oldvalue) & changed.Label.Equals("Shadow depth at end")) { sho.SetShadowDepth(E, (int)changed.Value); }

            if (changed.Label.Equals("Path"))
            {
                sho.SetPathTreeNode(PathTreeNode.GetFromString(avstn, changed.Value.ToString()));
                
            }
        }

        private void CompareAndUpdateLetter(GridItem changed, object oldvalue, Letter let)
        {
            long S = let.LetterStart, E = let.LetterEnd;

            if (S.Equals(oldvalue) & changed.Label.Equals("Start time")) { let.LetterStart = (long)changed.Value; }
            if (E.Equals(oldvalue) & changed.Label.Equals("End time")) { let.LetterEnd = (long)changed.Value; }
            if (let.GetAngleX(S).Equals(oldvalue) & changed.Label.Equals("Angle X at start")) { let.SetAngleX(S, (int)changed.Value); }
            if (let.GetAngleX(E).Equals(oldvalue) & changed.Label.Equals("Angle X at end")) { let.SetAngleX(E, (int)changed.Value); }
            if (let.GetAngleY(S).Equals(oldvalue) & changed.Label.Equals("Angle Y at start")) { let.SetAngleY(S, (int)changed.Value); }
            if (let.GetAngleY(E).Equals(oldvalue) & changed.Label.Equals("Angle Y at end")) { let.SetAngleY(E, (int)changed.Value); }
            if (let.GetAngleZ(S).Equals(oldvalue) & changed.Label.Equals("Angle Z at start")) { let.SetAngleZ(S, (int)changed.Value); }
            if (let.GetAngleZ(E).Equals(oldvalue) & changed.Label.Equals("Angle Z at end")) { let.SetAngleZ(E, (int)changed.Value); }
            if (let.GetFrontColor(S).Equals(oldvalue) & changed.Label.Equals("Front color at start")) { let.SetFrontColor(S, (Color)changed.Value); }
            if (let.GetFrontColor(E).Equals(oldvalue) & changed.Label.Equals("Front color at end")) { let.SetFrontColor(E, (Color)changed.Value); }
            if (let.GetBackColor(S).Equals(oldvalue) & changed.Label.Equals("Back color at start")) { let.SetBackColor(S, (Color)changed.Value); }
            if (let.GetBackColor(E).Equals(oldvalue) & changed.Label.Equals("Back color at end")) { let.SetBackColor(E, (Color)changed.Value); }
            if (let.GetBorderColor(S).Equals(oldvalue) & changed.Label.Equals("Border color at start")) { let.SetBorderColor(S, (Color)changed.Value); }
            if (let.GetBorderColor(E).Equals(oldvalue) & changed.Label.Equals("Border color at end")) { let.SetBorderColor(E, (Color)changed.Value); }
            if (let.GetShadowColor(S).Equals(oldvalue) & changed.Label.Equals("Shadow color at start")) { let.SetShadowColor(S, (Color)changed.Value); }
            if (let.GetShadowColor(E).Equals(oldvalue) & changed.Label.Equals("Shadow color at end")) { let.SetShadowColor(E, (Color)changed.Value); }
            if (let.GetRelativePositionX(S).Equals(oldvalue) & changed.Label.Equals("Position X at start")) { let.SetRelativePositionX(S, (float)changed.Value); }
            if (let.GetRelativePositionX(E).Equals(oldvalue) & changed.Label.Equals("Position X at end")) { let.SetRelativePositionX(E, (float)changed.Value); }
            if (let.GetRelativePositionY(S).Equals(oldvalue) & changed.Label.Equals("Position Y at start")) { let.SetRelativePositionY(S, (float)changed.Value); }
            if (let.GetRelativePositionY(E).Equals(oldvalue) & changed.Label.Equals("Position Y at end")) { let.SetRelativePositionY(E, (float)changed.Value); }
            if (let.GetQuakeX(S).Equals(oldvalue) & changed.Label.Equals("Quake X at start")) { let.SetQuakeX(S, (int)changed.Value); }
            if (let.GetQuakeX(E).Equals(oldvalue) & changed.Label.Equals("Quake X at end")) { let.SetQuakeX(E, (int)changed.Value); }
            if (let.GetQuakeY(S).Equals(oldvalue) & changed.Label.Equals("Quake Y at start")) { let.SetQuakeY(S, (int)changed.Value); }
            if (let.GetQuakeY(E).Equals(oldvalue) & changed.Label.Equals("Quake Y at end")) { let.SetQuakeY(E, (int)changed.Value); }
            if (let.GetScaleX(S).Equals(oldvalue) & changed.Label.Equals("Scale X at start")) { let.SetScaleX(S, (float)changed.Value); }
            if (let.GetScaleX(E).Equals(oldvalue) & changed.Label.Equals("Scale X at end")) { let.SetScaleX(E, (float)changed.Value); }
            if (let.GetScaleY(S).Equals(oldvalue) & changed.Label.Equals("Scale Y at start")) { let.SetScaleY(S, (float)changed.Value); }
            if (let.GetScaleY(E).Equals(oldvalue) & changed.Label.Equals("Scale Y at end")) { let.SetScaleY(E, (float)changed.Value); }
            if (let.GetBorderWeight(S).Equals(oldvalue) & changed.Label.Equals("Border weight at start")) { let.SetBorderWeight(S, (int)changed.Value); }
            if (let.GetBorderWeight(E).Equals(oldvalue) & changed.Label.Equals("Border weight at end")) { let.SetBorderWeight(E, (int)changed.Value); }
            if (let.GetShadowDepth(S).Equals(oldvalue) & changed.Label.Equals("Shadow depth at start")) { let.SetShadowDepth(S, (int)changed.Value); }
            if (let.GetShadowDepth(E).Equals(oldvalue) & changed.Label.Equals("Shadow depth at end")) { let.SetShadowDepth(E, (int)changed.Value); }

            //if (changed.Label.Equals("Path"))
            //{
            //    let.SetPathTreeNode(PathTreeNode.GetFromString(avstn, changed.Value.ToString()));

            //}
        }

        private void CompareAndUpdateSyllable(GridItem changed, object oldvalue, Syllable syl)
        {
            long S = syl.SyllableStart, E = syl.SyllableEnd;

            if (S.Equals(oldvalue) & changed.Label.Equals("Start time")) { syl.SyllableStart = (long)changed.Value; }
            if (E.Equals(oldvalue) & changed.Label.Equals("End time")) { syl.SyllableEnd = (long)changed.Value; }
            if (syl.GetAngleX(S).Equals(oldvalue) & changed.Label.Equals("Angle X at start")) { syl.SetAngleX(S, (int)changed.Value); }
            if (syl.GetAngleX(E).Equals(oldvalue) & changed.Label.Equals("Angle X at end")) { syl.SetAngleX(E, (int)changed.Value); }
            if (syl.GetAngleY(S).Equals(oldvalue) & changed.Label.Equals("Angle Y at start")) { syl.SetAngleY(S, (int)changed.Value); }
            if (syl.GetAngleY(E).Equals(oldvalue) & changed.Label.Equals("Angle Y at end")) { syl.SetAngleY(E, (int)changed.Value); }
            if (syl.GetAngleZ(S).Equals(oldvalue) & changed.Label.Equals("Angle Z at start")) { syl.SetAngleZ(S, (int)changed.Value); }
            if (syl.GetAngleZ(E).Equals(oldvalue) & changed.Label.Equals("Angle Z at end")) { syl.SetAngleZ(E, (int)changed.Value); }
            if (syl.GetFrontColor(S).Equals(oldvalue) & changed.Label.Equals("Front color at start")) { syl.SetFrontColor(S, (Color)changed.Value); }
            if (syl.GetFrontColor(E).Equals(oldvalue) & changed.Label.Equals("Front color at end")) { syl.SetFrontColor(E, (Color)changed.Value); }
            if (syl.GetBackColor(S).Equals(oldvalue) & changed.Label.Equals("Back color at start")) { syl.SetBackColor(S, (Color)changed.Value); }
            if (syl.GetBackColor(E).Equals(oldvalue) & changed.Label.Equals("Back color at end")) { syl.SetBackColor(E, (Color)changed.Value); }
            if (syl.GetBorderColor(S).Equals(oldvalue) & changed.Label.Equals("Border color at start")) { syl.SetBorderColor(S, (Color)changed.Value); }
            if (syl.GetBorderColor(E).Equals(oldvalue) & changed.Label.Equals("Border color at end")) { syl.SetBorderColor(E, (Color)changed.Value); }
            if (syl.GetShadowColor(S).Equals(oldvalue) & changed.Label.Equals("Shadow color at start")) { syl.SetShadowColor(S, (Color)changed.Value); }
            if (syl.GetShadowColor(E).Equals(oldvalue) & changed.Label.Equals("Shadow color at end")) { syl.SetShadowColor(E, (Color)changed.Value); }
            if (syl.GetRelativePositionX(S).Equals(oldvalue) & changed.Label.Equals("Position X at start")) { syl.SetRelativePositionX(S, (float)changed.Value); }
            if (syl.GetRelativePositionX(E).Equals(oldvalue) & changed.Label.Equals("Position X at end")) { syl.SetRelativePositionX(E, (float)changed.Value); }
            if (syl.GetRelativePositionY(S).Equals(oldvalue) & changed.Label.Equals("Position Y at start")) { syl.SetRelativePositionY(S, (float)changed.Value); }
            if (syl.GetRelativePositionY(E).Equals(oldvalue) & changed.Label.Equals("Position Y at end")) { syl.SetRelativePositionY(E, (float)changed.Value); }
            if (syl.GetQuakeX(S).Equals(oldvalue) & changed.Label.Equals("Quake X at start")) { syl.SetQuakeX(S, (int)changed.Value); }
            if (syl.GetQuakeX(E).Equals(oldvalue) & changed.Label.Equals("Quake X at end")) { syl.SetQuakeX(E, (int)changed.Value); }
            if (syl.GetQuakeY(S).Equals(oldvalue) & changed.Label.Equals("Quake Y at start")) { syl.SetQuakeY(S, (int)changed.Value); }
            if (syl.GetQuakeY(E).Equals(oldvalue) & changed.Label.Equals("Quake Y at end")) { syl.SetQuakeY(E, (int)changed.Value); }
            if (syl.GetScaleX(S).Equals(oldvalue) & changed.Label.Equals("Scale X at start")) { syl.SetScaleX(S, (float)changed.Value); }
            if (syl.GetScaleX(E).Equals(oldvalue) & changed.Label.Equals("Scale X at end")) { syl.SetScaleX(E, (float)changed.Value); }
            if (syl.GetScaleY(S).Equals(oldvalue) & changed.Label.Equals("Scale Y at start")) { syl.SetScaleY(S, (float)changed.Value); }
            if (syl.GetScaleY(E).Equals(oldvalue) & changed.Label.Equals("Scale Y at end")) { syl.SetScaleY(E, (float)changed.Value); }
            if (syl.GetBorderWeight(S).Equals(oldvalue) & changed.Label.Equals("Border weight at start")) { syl.SetBorderWeight(S, (int)changed.Value); }
            if (syl.GetBorderWeight(E).Equals(oldvalue) & changed.Label.Equals("Border weight at end")) { syl.SetBorderWeight(E, (int)changed.Value); }
            if (syl.GetShadowDepth(S).Equals(oldvalue) & changed.Label.Equals("Shadow depth at start")) { syl.SetShadowDepth(S, (int)changed.Value); }
            if (syl.GetShadowDepth(E).Equals(oldvalue) & changed.Label.Equals("Shadow depth at end")) { syl.SetShadowDepth(E, (int)changed.Value); }

            //if (changed.Label.Equals("Path"))
            //{
            //    syl.SetPathTreeNode(PathTreeNode.GetFromString(avstn, changed.Value.ToString()));

            //}
        }

        private void CompareAndUpdateSentence(GridItem changed, object oldvalue, Sentence sen)
        {
            long S = sen.SentenceStart, E = sen.SentenceEnd;

            if (S.Equals(oldvalue) & changed.Label.Equals("Start time")) { sen.SentenceStart = (long)changed.Value; }
            if (E.Equals(oldvalue) & changed.Label.Equals("End time")) { sen.SentenceEnd = (long)changed.Value; }
            if (sen.GetAngleX(S).Equals(oldvalue) & changed.Label.Equals("Angle X at start")) { sen.SetAngleX(S, (int)changed.Value); }
            if (sen.GetAngleX(E).Equals(oldvalue) & changed.Label.Equals("Angle X at end")) { sen.SetAngleX(E, (int)changed.Value); }
            if (sen.GetAngleY(S).Equals(oldvalue) & changed.Label.Equals("Angle Y at start")) { sen.SetAngleY(S, (int)changed.Value); }
            if (sen.GetAngleY(E).Equals(oldvalue) & changed.Label.Equals("Angle Y at end")) { sen.SetAngleY(E, (int)changed.Value); }
            if (sen.GetAngleZ(S).Equals(oldvalue) & changed.Label.Equals("Angle Z at start")) { sen.SetAngleZ(S, (int)changed.Value); }
            if (sen.GetAngleZ(E).Equals(oldvalue) & changed.Label.Equals("Angle Z at end")) { sen.SetAngleZ(E, (int)changed.Value); }
            if (sen.GetFrontColor(S).Equals(oldvalue) & changed.Label.Equals("Front color at start")) { sen.SetFrontColor(S, (Color)changed.Value); }
            if (sen.GetFrontColor(E).Equals(oldvalue) & changed.Label.Equals("Front color at end")) { sen.SetFrontColor(E, (Color)changed.Value); }
            if (sen.GetBackColor(S).Equals(oldvalue) & changed.Label.Equals("Back color at start")) { sen.SetBackColor(S, (Color)changed.Value); }
            if (sen.GetBackColor(E).Equals(oldvalue) & changed.Label.Equals("Back color at end")) { sen.SetBackColor(E, (Color)changed.Value); }
            if (sen.GetBorderColor(S).Equals(oldvalue) & changed.Label.Equals("Border color at start")) { sen.SetBorderColor(S, (Color)changed.Value); }
            if (sen.GetBorderColor(E).Equals(oldvalue) & changed.Label.Equals("Border color at end")) { sen.SetBorderColor(E, (Color)changed.Value); }
            if (sen.GetShadowColor(S).Equals(oldvalue) & changed.Label.Equals("Shadow color at start")) { sen.SetShadowColor(S, (Color)changed.Value); }
            if (sen.GetShadowColor(E).Equals(oldvalue) & changed.Label.Equals("Shadow color at end")) { sen.SetShadowColor(E, (Color)changed.Value); }
            if (sen.GetRelativePositionX(S).Equals(oldvalue) & changed.Label.Equals("Position X at start")) { sen.SetRelativePositionX(S, (float)changed.Value); }
            if (sen.GetRelativePositionX(E).Equals(oldvalue) & changed.Label.Equals("Position X at end")) { sen.SetRelativePositionX(E, (float)changed.Value); }
            if (sen.GetRelativePositionY(S).Equals(oldvalue) & changed.Label.Equals("Position Y at start")) { sen.SetRelativePositionY(S, (float)changed.Value); }
            if (sen.GetRelativePositionY(E).Equals(oldvalue) & changed.Label.Equals("Position Y at end")) { sen.SetRelativePositionY(E, (float)changed.Value); }
            if (sen.GetQuakeX(S).Equals(oldvalue) & changed.Label.Equals("Quake X at start")) { sen.SetQuakeX(S, (int)changed.Value); }
            if (sen.GetQuakeX(E).Equals(oldvalue) & changed.Label.Equals("Quake X at end")) { sen.SetQuakeX(E, (int)changed.Value); }
            if (sen.GetQuakeY(S).Equals(oldvalue) & changed.Label.Equals("Quake Y at start")) { sen.SetQuakeY(S, (int)changed.Value); }
            if (sen.GetQuakeY(E).Equals(oldvalue) & changed.Label.Equals("Quake Y at end")) { sen.SetQuakeY(E, (int)changed.Value); }
            if (sen.GetScaleX(S).Equals(oldvalue) & changed.Label.Equals("Scale X at start")) { sen.SetScaleX(S, (float)changed.Value); }
            if (sen.GetScaleX(E).Equals(oldvalue) & changed.Label.Equals("Scale X at end")) { sen.SetScaleX(E, (float)changed.Value); }
            if (sen.GetScaleY(S).Equals(oldvalue) & changed.Label.Equals("Scale Y at start")) { sen.SetScaleY(S, (float)changed.Value); }
            if (sen.GetScaleY(E).Equals(oldvalue) & changed.Label.Equals("Scale Y at end")) { sen.SetScaleY(E, (float)changed.Value); }
            if (sen.GetBorderWeight(S).Equals(oldvalue) & changed.Label.Equals("Border weight at start")) { sen.SetBorderWeight(S, (int)changed.Value); }
            if (sen.GetBorderWeight(E).Equals(oldvalue) & changed.Label.Equals("Border weight at end")) { sen.SetBorderWeight(E, (int)changed.Value); }
            if (sen.GetShadowDepth(S).Equals(oldvalue) & changed.Label.Equals("Shadow depth at start")) { sen.SetShadowDepth(S, (int)changed.Value); }
            if (sen.GetShadowDepth(E).Equals(oldvalue) & changed.Label.Equals("Shadow depth at end")) { sen.SetShadowDepth(E, (int)changed.Value); }

            //if (changed.Label.Equals("Path"))
            //{
            //    sen.SetPathTreeNode(PathTreeNode.GetFromString(avstn, changed.Value.ToString()));

            //}
        }
    }
}
