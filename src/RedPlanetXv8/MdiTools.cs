using RedPlanetXv8.ASS;
using RedPlanetXv8.AviSynth;
using RedPlanetXv8.Composition;
using RedPlanetXv8.Node;
using System;
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
            }


            //================================================================================
            // AVS TREENODE
            //================================================================================ 
            if (selected.GetType() == typeof(AVSTreeNode))
            {
                selected.SelectedImageIndex = 1;
            }


            //================================================================================
            // ASS TREENODE
            //================================================================================
            if (selected.GetType() == typeof(ASSTreeNode))
            {
                selected.SelectedImageIndex = 4;
            }


            //================================================================================
            // SENTENCE TREENODE
            //================================================================================
            if (selected.GetType() == typeof(SentenceTreeNode))
            {
                selected.SelectedImageIndex = 8;
            }


            //================================================================================
            // SYLLABLE TREENODE
            //================================================================================
            if (selected.GetType() == typeof(SyllableTreeNode))
            {
                selected.SelectedImageIndex = 8;
            }


            //================================================================================
            // LETTER TREENODE
            //================================================================================
            if (selected.GetType() == typeof(LetterTreeNode))
            {
                selected.SelectedImageIndex = 8;
            }


            //================================================================================
            // PARENT TREENODE
            //================================================================================
            if (selected.GetType() == typeof(ParentTreeNode))
            {
                selected.SelectedImageIndex = 2;
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

                PathTreeNode ptn = (PathTreeNode)selected;
                PathObject po = ptn.PathObject;
                po.Hide = false;
                form1.MdiView.View.LastPathTreeNode = ptn;
                form1.MdiView.View.UpdatePaintOnly();
            }

            lastSelectedTreeNode = selected;
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
        }
    }
}
