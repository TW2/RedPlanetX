using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetX
{
    public class Volume
    {
        //EN : A volume is a set of objects that contains states which are instants and that is linked with layers of effects
        //FR : Un volume désigne un ensemble d'objets contenant des états à des instants donnés et lié à des couches d'effets

        public string Name { get; set; }
        public Events Events { get; set; }
        public Parameters Parameters { get; set; }
        //public List<CreationLayer> Layers = new List<CreationLayer>();
        public List<CreationObject> Objects = new List<CreationObject>();
        public List<InsertPoint> Points = new List<InsertPoint>();

        // Retourne un treenode et enregistre les treenodes crées dans les collections passé en paramètres.
        public TreeNode GetTreeNode(Dictionary<TreeNode, Volume> objects, Dictionary<TreeNode, Event> events)
        {
            TreeNode main = new TreeNode(this.Name);
            main.Name = this.Name;
            objects.Add(main, this);
            foreach (Event e in Events.ToList())
            {
                TreeNode evt = new TreeNode(e.Name);
                evt.Name = e.Name;
                events.Add(evt, e);
                main.Nodes.Add(evt);
            }
            return main;
        }

        // Retourne un treenode et enregistre les treenodes crées dans les collections passé en paramètres.
        public static void CleanAndAddTreeNodes(Dictionary<TreeNode, Volume> objects, Dictionary<TreeNode, Event> events, List<Volume> vols, TreeNode top)
        {
            top.Nodes.Clear();

            foreach (Volume v in vols)
            {
                TreeNode main = new TreeNode(v.Name);
                main.Name = v.Name;
                objects.Add(main, v);
                foreach (Event e in v.Events.ToList())
                {
                    TreeNode evt = new TreeNode(e.Name);
                    evt.Name = e.Name;
                    events.Add(evt, e);
                    main.Nodes.Add(evt);
                }
                top.Nodes.Add(main);
            }
        }

        public void ModifyTreeNode(TreeNode main)
        {
            main.Name = this.Name;
            main.Nodes.Clear();
            foreach (Event e in Events.ToList())
            {
                TreeNode evt = new TreeNode(e.Name);
                evt.Name = e.Name;
                main.Nodes.Add(evt);
            }
        }

        public static Color GetColor(Color before, Color after, int frame_before, int frame_after, int meantime)
        {
            if (frame_before == meantime)
            {
                return before;
            }

            if (frame_after == meantime)
            {
                return after;
            }

            int bc_R = Convert.ToInt32(before.R);
            int bc_G = Convert.ToInt32(before.G);
            int bc_B = Convert.ToInt32(before.B);

            int ac_R = Convert.ToInt32(after.R);
            int ac_G = Convert.ToInt32(after.G);
            int ac_B = Convert.ToInt32(after.B);

            int diff_R = ac_R - bc_R;
            int diff_G = ac_G - bc_G;
            int diff_B = ac_B - bc_B;

            meantime = frame_before == 0 ? meantime : meantime - frame_before;
            frame_after = frame_before == 0 ? frame_after : frame_after - frame_before;

            int delta_R = diff_R * meantime / frame_after;
            int delta_G = diff_G * meantime / frame_after;
            int delta_B = diff_B * meantime / frame_after;

            int r = bc_R + delta_R;
            int g = bc_G + delta_G;
            int b = bc_B + delta_B;

            Color c = Color.FromArgb(r, g, b);

            return c;
        }

        public static float GetFloat(float before, float after, int frame_before, int frame_after, int meantime)
        {
            if (frame_before == meantime)
            {
                return before;
            }

            if (frame_after == meantime)
            {
                return after;
            }

            if (frame_before == 0)
            { //Particular case
                float diff = after - before;
                float delta = diff * meantime / frame_after;
                return before + delta;
            }
            else
            {
                float diff = after - before;
                float delta = diff * (meantime - frame_before) / (frame_after - frame_before);
                return before + delta;
            }
        }

        public static float GetPosition(float before, float after, int anchor, int frame_before, int frame_after, int meantime)
        {
            float bc = anchor + before;
            float ac = anchor + after;

            if (frame_before == meantime)
            {
                return bc;
            }

            if (frame_after == meantime)
            {
                return ac;
            }

            if (frame_before == 0)
            { //Particular case
                float diff = ac - bc;
                float delta = diff * meantime / frame_after;
                return bc + delta;
            }
            else
            {
                float diff = ac - bc;
                float delta = diff * (meantime - frame_before) / (frame_after - frame_before);
                return bc + delta;
            }
        }

        public static FontStyle GetFontStyle(bool bold, bool italic, bool underline, bool strikeout)
        {
            if (bold == false && italic == false && underline == false && strikeout == false)
            {
                return FontStyle.Regular;
            }
            else if (bold == true && italic == false && underline == false && strikeout == false)
            {
                return FontStyle.Bold;
            }
            else if (bold == true && italic == true && underline == false && strikeout == false)
            {
                return FontStyle.Bold | FontStyle.Italic;
            }
            else if (bold == false && italic == true && underline == false && strikeout == false)
            {
                return FontStyle.Italic;
            }
            else if (bold == false && italic == false && underline == true && strikeout == false)
            {
                return FontStyle.Regular | FontStyle.Underline;
            }
            else if (bold == true && italic == false && underline == true && strikeout == false)
            {
                return FontStyle.Bold | FontStyle.Underline;
            }
            else if (bold == true && italic == true && underline == true && strikeout == false)
            {
                return FontStyle.Bold | FontStyle.Italic | FontStyle.Underline;
            }
            else if (bold == false && italic == true && underline == true && strikeout == false)
            {
                return FontStyle.Italic | FontStyle.Underline;
            }
            else if (bold == false && italic == false && underline == false && strikeout == true)
            {
                return FontStyle.Regular | FontStyle.Strikeout;
            }
            else if (bold == true && italic == false && underline == false && strikeout == true)
            {
                return FontStyle.Bold | FontStyle.Strikeout;
            }
            else if (bold == true && italic == true && underline == false && strikeout == true)
            {
                return FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout;
            }
            else if (bold == false && italic == true && underline == false && strikeout == true)
            {
                return FontStyle.Italic | FontStyle.Strikeout;
            }
            else if (bold == false && italic == false && underline == true && strikeout == true)
            {
                return FontStyle.Regular | FontStyle.Underline | FontStyle.Strikeout;
            }
            else if (bold == true && italic == false && underline == true && strikeout == true)
            {
                return FontStyle.Bold | FontStyle.Underline | FontStyle.Strikeout;
            }
            else if (bold == true && italic == true && underline == true && strikeout == true)
            {
                return FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout;
            }
            else if (bold == false && italic == true && underline == true && strikeout == true)
            {
                return FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout;
            }
        return FontStyle.Regular;
        }

        public static int GetInt(int before, int after, int frame_before, int frame_after, int meantime)
        {
            if (frame_before == meantime)
            {
                return before;
            }

            if (frame_after == meantime)
            {
                return after;
            }

            if (frame_before == 0)
            { //Particular case
                int diff = after - before;
                float delta = diff * meantime / frame_after;
                return (int)(before + delta);
            }
            else
            {
                int diff = after - before;
                float delta = diff * (meantime - frame_before) / (frame_after - frame_before);
                return (int)(before + delta);
            }
        }

        public void Configure(string start, string end, int count)
        {
            this.Name = "Volume " + count;

            //-------------------------------------------------------------------
            // Paramètres de l'objet principal/maître (PROPRIETES STATIQUES)

            Parameters main_prs = new Parameters();

            Parameter parent = new Parameter();
            parent.Name = "Parent";
            parent.Object = "";
            parent.Category = "Link";
            parent.Summary = "A parent can share its properties if set, otherwise type a blank string to cancel them.";
            main_prs.Add(parent.Name, parent);

            Parameter usefrontrainbow = new Parameter();
            usefrontrainbow.Name = "Use front rainbow";
            usefrontrainbow.Object = false;
            usefrontrainbow.Category = "Rainbow";
            usefrontrainbow.Summary = "The rainbow for this element.";
            main_prs.Add(usefrontrainbow.Name, usefrontrainbow);

            //Parameter usebackrainbow = new Parameter();
            //usebackrainbow.Name = "Use back rainbow";
            //usebackrainbow.Object = false;
            //usebackrainbow.Category = "Rainbow";
            //usebackrainbow.Summary = "The rainbow for this element.";
            //main_prs.Add(usebackrainbow.Name, usebackrainbow);

            //Parameter usethicknessrainbow = new Parameter();
            //usethicknessrainbow.Name = "Use thickness rainbow";
            //usethicknessrainbow.Object = false;
            //usethicknessrainbow.Category = "Rainbow";
            //usethicknessrainbow.Summary = "The rainbow for this element.";
            //main_prs.Add(usethicknessrainbow.Name, usethicknessrainbow);

            Parameter useborderrainbow = new Parameter();
            useborderrainbow.Name = "Use border rainbow";
            useborderrainbow.Object = false;
            useborderrainbow.Category = "Rainbow";
            useborderrainbow.Summary = "The rainbow for this element.";
            main_prs.Add(useborderrainbow.Name, useborderrainbow);

            Parameter useshadowrainbow = new Parameter();
            useshadowrainbow.Name = "Use shadow rainbow";
            useshadowrainbow.Object = false;
            useshadowrainbow.Category = "Rainbow";
            useshadowrainbow.Summary = "The rainbow for this element.";
            main_prs.Add(useshadowrainbow.Name, useshadowrainbow);
            
            Parameter anchorX = new Parameter();
            anchorX.Name = "Anchor X";
            anchorX.Object = 0;
            anchorX.Category = "Position";
            anchorX.Summary = "The anchor is a static position used to quickly set up the position.";
            main_prs.Add(anchorX.Name, anchorX);

            Parameter anchorY = new Parameter();
            anchorY.Name = "Anchor Y";
            anchorY.Object = 0;
            anchorY.Category = "Position";
            anchorY.Summary = "The anchor is a static position used to quickly set up the position.";
            main_prs.Add(anchorY.Name, anchorY);

            Parameter pos = new Parameter();
            pos.Name = "Position";
            pos.Object = "";
            pos.Category = "Position";
            pos.Summary = "Set up the position at a corner, at the middle of a side or at the center.";
            main_prs.Add(pos.Name, pos);

            this.Parameters = main_prs;

            //===================================================================

            //-------------------------------------------------------------------
            // Paramètres de l'objet secondaire/esclave (PROPRIETES DYNAMIQUES)

            Parameters leaf_prs = new Parameters();

            Parameter x1 = new Parameter();
            x1.Name = "Position X";
            x1.Object = 0f;
            x1.Category = "Position";
            x1.Summary = "A position that can be the start.";
            leaf_prs.Add(x1.Name, x1);

            Parameter y1 = new Parameter();
            y1.Name = "Position Y";
            y1.Object = 0f;
            y1.Category = "Position";
            y1.Summary = "A position that can be the start.";
            leaf_prs.Add(y1.Name, y1);

            //Parameter z1 = new Parameter();
            //z1.Name = "Position Z";
            //z1.Object = 0f;
            //z1.Category = "Position";
            //z1.Summary = "A position that can be the start.";
            //leaf_prs.Add(z1.Name, z1);

            Parameter sx1 = new Parameter();
            sx1.Name = "Scale X";
            sx1.Object = 100f;
            sx1.Category = "Scale";
            sx1.Summary = "Scale of the element on X axis in percent (100% = no changes).";
            leaf_prs.Add(sx1.Name, sx1);

            Parameter sy1 = new Parameter();
            sy1.Name = "Scale Y";
            sy1.Object = 100f;
            sy1.Category = "Scale";
            sy1.Summary = "Scale of the element on Y axis in percent (100% = no changes).";
            leaf_prs.Add(sy1.Name, sy1);

            Parameter anglex1 = new Parameter();
            anglex1.Name = "Angle X";
            anglex1.Object = 0f;
            anglex1.Category = "Angle";
            anglex1.Summary = "Angle of the element.";
            leaf_prs.Add(anglex1.Name, anglex1);

            Parameter angley1 = new Parameter();
            angley1.Name = "Angle Y";
            angley1.Object = 0f;
            angley1.Category = "Angle";
            angley1.Summary = "Angle of the element.";
            leaf_prs.Add(angley1.Name, angley1);

            Parameter anglez1 = new Parameter();
            anglez1.Name = "Angle Z";
            anglez1.Object = 0f;
            anglez1.Category = "Angle";
            anglez1.Summary = "Angle of the element.";
            leaf_prs.Add(anglez1.Name, anglez1);

            Parameter quakex1 = new Parameter();
            quakex1.Name = "Quake X";
            quakex1.Object = 0f;
            quakex1.Category = "Quake";
            quakex1.Summary = "Quake of the element.";
            leaf_prs.Add(quakex1.Name, quakex1);

            Parameter quakey1 = new Parameter();
            quakey1.Name = "Quake Y";
            quakey1.Object = 0f;
            quakey1.Category = "Quake";
            quakey1.Summary = "Quake of the element.";
            leaf_prs.Add(quakey1.Name, quakey1);

            //Parameter quakez1 = new Parameter();
            //quakez1.Name = "Quake Z";
            //quakez1.Object = 0f;
            //quakez1.Category = "Quake";
            //quakez1.Summary = "Quake of the element.";
            //leaf_prs.Add(quakez1.Name, quakez1);

            //Parameter thickness1 = new Parameter();
            //thickness1.Name = "Thickness";
            //thickness1.Object = 0;
            //thickness1.Category = "Value";
            //thickness1.Summary = "Thickness of the element.";
            //leaf_prs.Add(thickness1.Name, thickness1);

            Parameter border1 = new Parameter();
            border1.Name = "Border";
            border1.Object = 1;
            border1.Category = "Value";
            border1.Summary = "Border of the element.";
            leaf_prs.Add(border1.Name, border1);

            Parameter shadow1 = new Parameter();
            shadow1.Name = "Shadow";
            shadow1.Object = 0;
            shadow1.Category = "Value";
            shadow1.Summary = "Shadow of the element.";
            leaf_prs.Add(shadow1.Name, shadow1);

            Parameter frontcolor1 = new Parameter();
            frontcolor1.Name = "Front color";
            frontcolor1.Object = Color.LightBlue;
            frontcolor1.Category = "Color";
            frontcolor1.Summary = "Color of a part of the element.";
            leaf_prs.Add(frontcolor1.Name, frontcolor1);

            //Parameter backcolor1 = new Parameter();
            //backcolor1.Name = "Back color";
            //backcolor1.Object = Color.Blue;
            //backcolor1.Category = "Color";
            //backcolor1.Summary = "Color of a part of the element.";
            //leaf_prs.Add(backcolor1.Name, backcolor1);

            //Parameter thicknesscolor1 = new Parameter();
            //thicknesscolor1.Name = "Thickness color";
            //thicknesscolor1.Object = Color.Green;
            //thicknesscolor1.Category = "Color";
            //thicknesscolor1.Summary = "Color of a part of the element.";
            //leaf_prs.Add(thicknesscolor1.Name, thicknesscolor1);

            Parameter bordercolor1 = new Parameter();
            bordercolor1.Name = "Border color";
            bordercolor1.Object = Color.Red;
            bordercolor1.Category = "Color";
            bordercolor1.Summary = "Color of a part of the element.";
            leaf_prs.Add(bordercolor1.Name, bordercolor1);

            Parameter shadowcolor1 = new Parameter();
            shadowcolor1.Name = "Shadow color";
            shadowcolor1.Object = Color.Gray;
            shadowcolor1.Category = "Color";
            shadowcolor1.Summary = "Color of a part of the element.";
            leaf_prs.Add(shadowcolor1.Name, shadowcolor1);

            //Parameter frontrainbow1 = new Parameter();
            //frontrainbow1.Name = "Front rainbow";
            //frontrainbow1.Object = new Rainbow();
            //frontrainbow1.Category = "Color";
            //frontrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(frontrainbow1.Name, frontrainbow1);

            //Parameter backrainbow1 = new Parameter();
            //backrainbow1.Name = "Back rainbow";
            //backrainbow1.Object = new Rainbow();
            //backrainbow1.Category = "Color";
            //backrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(backrainbow1.Name, backrainbow1);

            //Parameter thicknessrainbow1 = new Parameter();
            //thicknessrainbow1.Name = "Thickness rainbow";
            //thicknessrainbow1.Object = new Rainbow();
            //thicknessrainbow1.Category = "Color";
            //thicknessrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(thicknessrainbow1.Name, thicknessrainbow1);

            //Parameter borderrainbow1 = new Parameter();
            //borderrainbow1.Name = "Border rainbow";
            //borderrainbow1.Object = new Rainbow();
            //borderrainbow1.Category = "Color";
            //borderrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(borderrainbow1.Name, borderrainbow1);

            //Parameter shadowrainbow1 = new Parameter();
            //shadowrainbow1.Name = "Shadow rainbow";
            //shadowrainbow1.Object = new Rainbow();
            //shadowrainbow1.Category = "Color";
            //shadowrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(shadowrainbow1.Name, shadowrainbow1);

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Parameters leaf_prs2 = new Parameters();

            Parameter x2 = new Parameter();
            x2.Name = "Position X";
            x2.Object = 0f;
            x2.Category = "Position";
            x2.Summary = "A position that can be the end.";
            leaf_prs2.Add(x2.Name, x2);

            Parameter y2 = new Parameter();
            y2.Name = "Position Y";
            y2.Object = 0f;
            y2.Category = "Position";
            y2.Summary = "A position that can be the end.";
            leaf_prs2.Add(y2.Name, y2);

            //Parameter z2 = new Parameter();
            //z2.Name = "Position Z";
            //z2.Object = 0f;
            //z2.Category = "Position";
            //z2.Summary = "A position that can be the end.";
            //leaf_prs2.Add(z2.Name, z2);

            Parameter sx2 = new Parameter();
            sx2.Name = "Scale X";
            sx2.Object = 100f;
            sx2.Category = "Scale";
            sx2.Summary = "Scale of the element on X axis in percent (100% = no changes).";
            leaf_prs2.Add(sx2.Name, sx2);
            
            Parameter sy2 = new Parameter();
            sy2.Name = "Scale Y";
            sy2.Object = 100f;
            sy2.Category = "Scale";
            sy2.Summary = "Scale of the element on Y axis in percent (100% = no changes).";
            leaf_prs2.Add(sy2.Name, sy2);

            Parameter anglex2 = new Parameter();
            anglex2.Name = "Angle X";
            anglex2.Object = 0f;
            anglex2.Category = "Angle";
            anglex2.Summary = "Angle of the element.";
            leaf_prs2.Add(anglex2.Name, anglex2);

            Parameter angley2 = new Parameter();
            angley2.Name = "Angle Y";
            angley2.Object = 0f;
            angley2.Category = "Angle";
            angley2.Summary = "Angle of the element.";
            leaf_prs2.Add(angley2.Name, angley2);

            Parameter anglez2 = new Parameter();
            anglez2.Name = "Angle Z";
            anglez2.Object = 0f;
            anglez2.Category = "Angle";
            anglez2.Summary = "Angle of the element.";
            leaf_prs2.Add(anglez2.Name, anglez2);
            
            Parameter quakex2 = new Parameter();
            quakex2.Name = "Quake X";
            quakex2.Object = 0f;
            quakex2.Category = "Quake";
            quakex2.Summary = "Quake of the element.";
            leaf_prs2.Add(quakex2.Name, quakex2);

            Parameter quakey2 = new Parameter();
            quakey2.Name = "Quake Y";
            quakey2.Object = 0f;
            quakey2.Category = "Quake";
            quakey2.Summary = "Quake of the element.";
            leaf_prs2.Add(quakey2.Name, quakey2);

            //Parameter quakez2 = new Parameter();
            //quakez2.Name = "Quake Z";
            //quakez2.Object = 0f;
            //quakez2.Category = "Quake";
            //quakez2.Summary = "Quake of the element.";
            //leaf_prs2.Add(quakez2.Name, quakez2);

            //Parameter thickness2 = new Parameter();
            //thickness2.Name = "Thickness";
            //thickness2.Object = 0;
            //thickness2.Category = "Value";
            //thickness2.Summary = "Thickness of the element.";
            //leaf_prs2.Add(thickness2.Name, thickness2);

            Parameter border2 = new Parameter();
            border2.Name = "Border";
            border2.Object = 1;
            border2.Category = "Value";
            border2.Summary = "Border of the element.";
            leaf_prs2.Add(border2.Name, border2);

            Parameter shadow2 = new Parameter();
            shadow2.Name = "Shadow";
            shadow2.Object = 0;
            shadow2.Category = "Value";
            shadow2.Summary = "Shadow of the element.";
            leaf_prs2.Add(shadow2.Name, shadow2);
            
            Parameter frontcolor2 = new Parameter();
            frontcolor2.Name = "Front color";
            frontcolor2.Object = Color.LightCyan;
            frontcolor2.Category = "Color";
            frontcolor2.Summary = "Color of a part of the element.";
            leaf_prs2.Add(frontcolor2.Name, frontcolor2);

            //Parameter backcolor2 = new Parameter();
            //backcolor2.Name = "Back color";
            //backcolor2.Object = Color.Cyan;
            //backcolor2.Category = "Color";
            //backcolor2.Summary = "Color of a part of the element.";
            //leaf_prs2.Add(backcolor2.Name, backcolor2);

            //Parameter thicknesscolor2 = new Parameter();
            //thicknesscolor2.Name = "Thickness color";
            //thicknesscolor2.Object = Color.Lime;
            //thicknesscolor2.Category = "Color";
            //thicknesscolor2.Summary = "Color of a part of the element.";
            //leaf_prs2.Add(thicknesscolor2.Name, thicknesscolor2);

            Parameter bordercolor2 = new Parameter();
            bordercolor2.Name = "Border color";
            bordercolor2.Object = Color.Orange;
            bordercolor2.Category = "Color";
            bordercolor2.Summary = "Color of a part of the element.";
            leaf_prs2.Add(bordercolor2.Name, bordercolor2);

            Parameter shadowcolor2 = new Parameter();
            shadowcolor2.Name = "Shadow color";
            shadowcolor2.Object = Color.LightGray;
            shadowcolor2.Category = "Color";
            shadowcolor2.Summary = "Color of a part of the element.";
            leaf_prs2.Add(shadowcolor2.Name, shadowcolor2);

            //Parameter frontrainbow2 = new Parameter();
            //frontrainbow2.Name = "Front rainbow";
            //frontrainbow2.Object = new Rainbow();
            //frontrainbow2.Category = "Color";
            //frontrainbow2.Summary = "Rainbow of a part of the element.";
            //leaf_prs2.Add(frontrainbow2.Name, frontrainbow2);

            //Parameter backrainbow2 = new Parameter();
            //backrainbow2.Name = "Back rainbow";
            //backrainbow2.Object = new Rainbow();
            //backrainbow2.Category = "Color";
            //backrainbow2.Summary = "Rainbow of a part of the element.";
            //leaf_prs2.Add(backrainbow2.Name, backrainbow2);
            
            //Parameter thicknessrainbow2 = new Parameter();
            //thicknessrainbow2.Name = "Thickness rainbow";
            //thicknessrainbow2.Object = new Rainbow();
            //thicknessrainbow2.Category = "Color";
            //thicknessrainbow2.Summary = "Rainbow of a part of the element.";
            //leaf_prs2.Add(thicknessrainbow2.Name, thicknessrainbow2);

            //Parameter borderrainbow2 = new Parameter();
            //borderrainbow2.Name = "Border rainbow";
            //borderrainbow2.Object = new Rainbow();
            //borderrainbow2.Category = "Color";
            //borderrainbow2.Summary = "Rainbow of a part of the element.";
            //leaf_prs2.Add(borderrainbow2.Name, borderrainbow2);

            //Parameter shadowrainbow2 = new Parameter();
            //shadowrainbow2.Name = "Shadow rainbow";
            //shadowrainbow2.Object = new Rainbow();
            //shadowrainbow2.Category = "Color";
            //shadowrainbow2.Summary = "Rainbow of a part of the element.";
            //leaf_prs2.Add(shadowrainbow2.Name, shadowrainbow2);

            //===================================================================
            
            Events evts = new Events();
            evts.Add(start, leaf_prs);
            evts.Add(end, leaf_prs2);
            this.Events = evts;
        }

        public void AddOneEvent(string instant)
        {
            //-------------------------------------------------------------------
            // Paramètres de l'objet secondaire/esclave (PROPRIETES DYNAMIQUES)

            Parameters leaf_prs = new Parameters();

            Parameter x1 = new Parameter();
            x1.Name = "Position X";
            x1.Object = 0f;
            x1.Category = "Position";
            x1.Summary = "A position that can be the start.";
            leaf_prs.Add(x1.Name, x1);

            Parameter y1 = new Parameter();
            y1.Name = "Position Y";
            y1.Object = 0f;
            y1.Category = "Position";
            y1.Summary = "A position that can be the start.";
            leaf_prs.Add(y1.Name, y1);

            //Parameter z1 = new Parameter();
            //z1.Name = "Position Z";
            //z1.Object = 0f;
            //z1.Category = "Position";
            //z1.Summary = "A position that can be the start.";
            //leaf_prs.Add(z1.Name, z1);

            Parameter sx1 = new Parameter();
            sx1.Name = "Scale X";
            sx1.Object = 100f;
            sx1.Category = "Scale";
            sx1.Summary = "Scale of the element on X axis in percent (100% = no changes).";
            leaf_prs.Add(sx1.Name, sx1);

            Parameter sy1 = new Parameter();
            sy1.Name = "Scale Y";
            sy1.Object = 100f;
            sy1.Category = "Scale";
            sy1.Summary = "Scale of the element on Y axis in percent (100% = no changes).";
            leaf_prs.Add(sy1.Name, sy1);

            Parameter anglex1 = new Parameter();
            anglex1.Name = "Angle X";
            anglex1.Object = 0f;
            anglex1.Category = "Angle";
            anglex1.Summary = "Angle of the element.";
            leaf_prs.Add(anglex1.Name, anglex1);

            Parameter angley1 = new Parameter();
            angley1.Name = "Angle Y";
            angley1.Object = 0f;
            angley1.Category = "Angle";
            angley1.Summary = "Angle of the element.";
            leaf_prs.Add(angley1.Name, angley1);

            Parameter anglez1 = new Parameter();
            anglez1.Name = "Angle Z";
            anglez1.Object = 0f;
            anglez1.Category = "Angle";
            anglez1.Summary = "Angle of the element.";
            leaf_prs.Add(anglez1.Name, anglez1);

            Parameter quakex1 = new Parameter();
            quakex1.Name = "Quake X";
            quakex1.Object = 0f;
            quakex1.Category = "Quake";
            quakex1.Summary = "Quake of the element.";
            leaf_prs.Add(quakex1.Name, quakex1);

            Parameter quakey1 = new Parameter();
            quakey1.Name = "Quake Y";
            quakey1.Object = 0f;
            quakey1.Category = "Quake";
            quakey1.Summary = "Quake of the element.";
            leaf_prs.Add(quakey1.Name, quakey1);

            //Parameter quakez1 = new Parameter();
            //quakez1.Name = "Quake Z";
            //quakez1.Object = 0f;
            //quakez1.Category = "Quake";
            //quakez1.Summary = "Quake of the element.";
            //leaf_prs.Add(quakez1.Name, quakez1);

            //Parameter thickness1 = new Parameter();
            //thickness1.Name = "Thickness";
            //thickness1.Object = 0;
            //thickness1.Category = "Value";
            //thickness1.Summary = "Thickness of the element.";
            //leaf_prs.Add(thickness1.Name, thickness1);

            Parameter border1 = new Parameter();
            border1.Name = "Border";
            border1.Object = 1;
            border1.Category = "Value";
            border1.Summary = "Border of the element.";
            leaf_prs.Add(border1.Name, border1);

            Parameter shadow1 = new Parameter();
            shadow1.Name = "Shadow";
            shadow1.Object = 0;
            shadow1.Category = "Value";
            shadow1.Summary = "Shadow of the element.";
            leaf_prs.Add(shadow1.Name, shadow1);

            Parameter frontcolor1 = new Parameter();
            frontcolor1.Name = "Front color";
            frontcolor1.Object = Color.FromArgb(255, 255, 255);
            frontcolor1.Category = "Color";
            frontcolor1.Summary = "Color of a part of the element.";
            leaf_prs.Add(frontcolor1.Name, frontcolor1);

            //Parameter backcolor1 = new Parameter();
            //backcolor1.Name = "Back color";
            //backcolor1.Object = Color.FromArgb(255, 255, 255);
            //backcolor1.Category = "Color";
            //backcolor1.Summary = "Color of a part of the element.";
            //leaf_prs.Add(backcolor1.Name, backcolor1);

            //Parameter thicknesscolor1 = new Parameter();
            //thicknesscolor1.Name = "Thickness color";
            //thicknesscolor1.Object = Color.FromArgb(255, 255, 255);
            //thicknesscolor1.Category = "Color";
            //thicknesscolor1.Summary = "Color of a part of the element.";
            //leaf_prs.Add(thicknesscolor1.Name, thicknesscolor1);

            Parameter bordercolor1 = new Parameter();
            bordercolor1.Name = "Border color";
            bordercolor1.Object = Color.FromArgb(255, 255, 255);
            bordercolor1.Category = "Color";
            bordercolor1.Summary = "Color of a part of the element.";
            leaf_prs.Add(bordercolor1.Name, bordercolor1);

            Parameter shadowcolor1 = new Parameter();
            shadowcolor1.Name = "Shadow color";
            shadowcolor1.Object = Color.FromArgb(255, 255, 255);
            shadowcolor1.Category = "Color";
            shadowcolor1.Summary = "Color of a part of the element.";
            leaf_prs.Add(shadowcolor1.Name, shadowcolor1);

            //Parameter frontrainbow1 = new Parameter();
            //frontrainbow1.Name = "Front rainbow";
            //frontrainbow1.Object = new Rainbow();
            //frontrainbow1.Category = "Color";
            //frontrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(frontrainbow1.Name, frontrainbow1);

            //Parameter backrainbow1 = new Parameter();
            //backrainbow1.Name = "Back rainbow";
            //backrainbow1.Object = new Rainbow();
            //backrainbow1.Category = "Color";
            //backrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(backrainbow1.Name, backrainbow1);

            //Parameter thicknessrainbow1 = new Parameter();
            //thicknessrainbow1.Name = "Thickness rainbow";
            //thicknessrainbow1.Object = new Rainbow();
            //thicknessrainbow1.Category = "Color";
            //thicknessrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(thicknessrainbow1.Name, thicknessrainbow1);

            //Parameter borderrainbow1 = new Parameter();
            //borderrainbow1.Name = "Border rainbow";
            //borderrainbow1.Object = new Rainbow();
            //borderrainbow1.Category = "Color";
            //borderrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(borderrainbow1.Name, borderrainbow1);

            //Parameter shadowrainbow1 = new Parameter();
            //shadowrainbow1.Name = "Shadow rainbow";
            //shadowrainbow1.Object = new Rainbow();
            //shadowrainbow1.Category = "Color";
            //shadowrainbow1.Summary = "Rainbow of a part of the element.";
            //leaf_prs.Add(shadowrainbow1.Name, shadowrainbow1);
            
            this.Events.Add(instant, leaf_prs);
        }

        public static Event GetCurrentEvent(Volume v, int anchorX, int anchorY, int current_frame)
        {
            int meantime = current_frame, frame_before = 0, frame_after = 0;
            Event before = null, after = null, current = null;

            Events evts = v.Events;

            foreach (Event e in evts.ToList())
            {
                int value = Convert.ToInt32(e.Name);
                if (value <= meantime)
                {
                    before = e;
                    frame_before = value;
                }
            }

            foreach (Event e in evts.ToReverseList())
            {
                int value = Convert.ToInt32(e.Name);
                if (value >= meantime)
                {
                    after = e;
                    frame_after = value;
                }
            }

            Parameters leaf_prs = new Parameters();

            foreach (Parameter p1 in before.Parameters.GetValues())
            {
                foreach (Parameter p2 in after.Parameters.GetValues())
                {
                    if (p1.Name.Equals(p2.Name))
                    {
                        if (p1.Name == "Position X")
                        {
                            float currentPosX = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentPosX;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        if (p1.Name == "Position Y")
                        {
                            float currentPosY = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentPosY;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        //if (p1.Name == "Position Z")
                        //{
                        //    float currentPosZ = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                        //    Parameter p = new Parameter();
                        //    p.Name = p1.Name;
                        //    p.Object = currentPosZ;
                        //    p.Category = p1.Category;
                        //    p.Summary = p1.Summary;
                        //    leaf_prs.Add(p.Name, p);
                        //}


                        if (p1.Name == "Scale X")
                        {
                            float currentScaleX = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentScaleX;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        if (p1.Name == "Scale Y")
                        {
                            float currentScaleY = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentScaleY;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }


                        if (p1.Name == "Angle X")
                        {
                            float currentAngleX = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentAngleX;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        if (p1.Name == "Angle Y")
                        {
                            float currentAngleY = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentAngleY;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        if (p1.Name == "Angle Z")
                        {
                            float currentAngleZ = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentAngleZ;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }


                        if (p1.Name == "Quake X")
                        {
                            float currentQuakeX = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentQuakeX;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        if (p1.Name == "Quake Y")
                        {
                            float currentQuakeY = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentQuakeY;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        //if (p1.Name == "Quake Z")
                        //{
                        //    float currentQuakeZ = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                        //    Parameter p = new Parameter();
                        //    p.Name = p1.Name;
                        //    p.Object = currentQuakeZ;
                        //    p.Category = p1.Category;
                        //    p.Summary = p1.Summary;
                        //    leaf_prs.Add(p.Name, p);
                        //}


                        //if (p1.Name == "Thickness")
                        //{
                        //    int currentThickness = GetInt((int)p1.Object, (int)p2.Object, frame_before, frame_after, meantime);
                        //    Parameter p = new Parameter();
                        //    p.Name = p1.Name;
                        //    p.Object = currentThickness;
                        //    p.Category = p1.Category;
                        //    p.Summary = p1.Summary;
                        //    leaf_prs.Add(p.Name, p);
                        //}

                        if (p1.Name == "Border")
                        {
                            int currentBorder = GetInt((int)p1.Object, (int)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentBorder;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        if (p1.Name == "Shadow")
                        {
                            int currentShadow = GetInt((int)p1.Object, (int)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentShadow;
                            p.Category = p1.Category;
                            p.Summary = p1.Summary;
                            leaf_prs.Add(p.Name, p);
                        }

                        if (p1.Name == "Front color" | p1.Name == "Back color" | p1.Name == "Thickness color" | p1.Name == "Border color" | p1.Name == "Shadow color")
                        {
                            Color currentColor = GetColor((Color)p1.Object, (Color)p2.Object, frame_before, frame_after, meantime);
                            Parameter p = new Parameter();
                            p.Name = p1.Name;
                            p.Object = currentColor;
                            p.Category = "Color";
                            p.Summary = "A color.";
                            leaf_prs.Add(p.Name, p);
                        }
                    }
                }
            }

            current = new Event();
            current.Name = meantime.ToString();
            current.Parameters = leaf_prs;

            return current;
        }

        public override string ToString()
        {
            return Name + " :: " + base.ToString();
        }

        public void AddtInsertPoint(InsertPoint ip)
        {
            Points.Add(ip);
        }

        public List<InsertPoint> GettInsertPoints()
        {
            return Points;
        }

        public Volume GetClone()
        {
            return IO.GetClone(this);
        }

    }
    
}
