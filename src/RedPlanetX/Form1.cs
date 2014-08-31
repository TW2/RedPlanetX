using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetX
{
    public partial class Form1 : Form
    {
        private Dictionary<TreeNode, ObjectEvents> objects = new Dictionary<TreeNode, ObjectEvents>();
        private Dictionary<TreeNode, Event> events = new Dictionary<TreeNode, Event>();
        private int countSCRIPT = 0, countTA = 0, countTH = 0, countTV = 0, countSHAPE = 0, countDRAWING = 0, countPICTURE = 0, countVIDEO = 0;
        private object lastObjectUsed = null;
        private bool regen = false;

        private string video_path = "---";
        private string extract_path = "---"; private int imagescount = 0;
        private string modify_path = "---";
        private string video_width = "";
        private string video_height = "";
        private string video_framerate = "";
        private string video_frames = "";

        #region "ObjectEvents"

        public ObjectEvents ConfigureHorizontalText(int count, string start, string end)
        {
            ObjectEvents oe = new ObjectEvents();
            oe.Name = "Horizontal text " + count;
            oe.Object = "Sample text";
            oe.Type = ObjectEvents.ObjectType.HorizontalText;

            //-------------------------------------------------------------------
            // Paramètres de l'objet principal/maître (PROPRIETES STATIQUES)
            Parameters main_prs = new Parameters();

            Parameter parent = new Parameter();
            parent.Name = "Parent";
            parent.Object = "";
            parent.Category = "Link";
            parent.Summary = "A parent can share its properties if set, otherwise type a blank string to cancel them.";
            main_prs.Add(parent.Name, parent);

            Parameter fontname = new Parameter();
            fontname.Name = "Font name";
            fontname.Object = "Arial";
            fontname.Category = "Text";
            fontname.Summary = "The font name of the text.";
            main_prs.Add(fontname.Name, fontname);

            Parameter bold = new Parameter();
            bold.Name = "Bold";
            bold.Object = false;
            bold.Category = "Text";
            bold.Summary = "The style of the text.";
            main_prs.Add(bold.Name, bold);

            Parameter italic = new Parameter();
            italic.Name = "Italic";
            italic.Object = false;
            italic.Category = "Text";
            italic.Summary = "The style of the text.";
            main_prs.Add(italic.Name, italic);

            Parameter underline = new Parameter();
            underline.Name = "Underline";
            underline.Object = false;
            underline.Category = "Text";
            underline.Summary = "Underline of the text.";
            main_prs.Add(underline.Name, underline);

            Parameter strikeout = new Parameter();
            strikeout.Name = "Strike out";
            strikeout.Object = false;
            strikeout.Category = "Text";
            strikeout.Summary = "Strike out of the text.";
            main_prs.Add(strikeout.Name, strikeout);

            Parameter str = new Parameter();
            str.Name = "String";
            str.Object = "Type your text";
            str.Category = "Text";
            str.Summary = "The text to display.";
            main_prs.Add(str.Name, str);

            Parameter anchorX = new Parameter();
            anchorX.Name = "Anchor X";
            anchorX.Object = 100;
            anchorX.Category = "Position";
            anchorX.Summary = "The anchor is a static position used to quickly set up the position.";
            main_prs.Add(anchorX.Name, anchorX);

            Parameter anchorY = new Parameter();
            anchorY.Name = "Anchor Y";
            anchorY.Object = 100;
            anchorY.Category = "Position";
            anchorY.Summary = "The anchor is a static position used to quickly set up the position.";
            main_prs.Add(anchorY.Name, anchorY);

            Parameter pos = new Parameter();
            pos.Name = "Position";
            pos.Object = "";
            pos.Category = "Position";
            pos.Summary = "Set up the position at a corner, at the middle of a side or at the center.";
            main_prs.Add(pos.Name, pos);


            oe.Parameters = main_prs;
            //===================================================================

            //-------------------------------------------------------------------
            // Paramètres de l'objet secondaire/esclave (PROPRIETES DYNAMIQUES)
            Parameters leaf_prs = new Parameters();
            Parameter fontsize1 = new Parameter();
            fontsize1.Name = "Font size";
            fontsize1.Object = 20.25f;
            fontsize1.Category = "Text";
            fontsize1.Summary = "The font size of the text.";
            leaf_prs.Add(fontsize1.Name, fontsize1);

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

            Parameter sx1 = new Parameter();
            sx1.Name = "Scale X";
            sx1.Object = 100f;
            sx1.Category = "Scale";
            sx1.Summary = "Scale of the text on X axis in percent (100% = no changes).";
            leaf_prs.Add(sx1.Name, sx1);

            Parameter sy1 = new Parameter();
            sy1.Name = "Scale Y";
            sy1.Object = 100f;
            sy1.Category = "Scale";
            sy1.Summary = "Scale of the text on Y axis in percent (100% = no changes).";
            leaf_prs.Add(sy1.Name, sy1);

            Parameter angle1 = new Parameter();
            angle1.Name = "Angle";
            angle1.Object = 0f;
            angle1.Category = "Angle";
            angle1.Summary = "Angle of the text.";
            leaf_prs.Add(angle1.Name, angle1);

            Parameter color11 = new Parameter();
            color11.Name = "Color at corner 7";
            color11.Object = new Color();
            color11.Category = "Color";
            color11.Summary = "Color at the corner 7 of the numeric keypad.";
            leaf_prs.Add(color11.Name, color11);

            Parameter color21 = new Parameter();
            color21.Name = "Color at corner 9";
            color21.Object = new Color();
            color21.Category = "Color";
            color21.Summary = "Color at the corner 9 of the numeric keypad.";
            leaf_prs.Add(color21.Name, color21);

            Parameter color31 = new Parameter();
            color31.Name = "Color at corner 3";
            color31.Object = new Color();
            color31.Category = "Color";
            color31.Summary = "Color at the corner 3 of the numeric keypad.";
            leaf_prs.Add(color31.Name, color31);

            Parameter color41 = new Parameter();
            color41.Name = "Color at corner 1";
            color41.Object = new Color();
            color41.Category = "Color";
            color41.Summary = "Color at the corner 1 of the numeric keypad.";
            leaf_prs.Add(color41.Name, color41);

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Parameters leaf_prs2 = new Parameters();
            Parameter fontsize2 = new Parameter();
            fontsize2.Name = "Font size";
            fontsize2.Object = 20.25f;
            fontsize2.Category = "Text";
            fontsize2.Summary = "The font size of the text.";
            leaf_prs2.Add(fontsize2.Name, fontsize2);

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

            Parameter sx2 = new Parameter();
            sx2.Name = "Scale X";
            sx2.Object = 100f;
            sx2.Category = "Scale";
            sx2.Summary = "Scale of the text on X axis in percent (100% = no changes).";
            leaf_prs2.Add(sx2.Name, sx2);

            Parameter sy2 = new Parameter();
            sy2.Name = "Scale Y";
            sy2.Object = 100f;
            sy2.Category = "Scale";
            sy2.Summary = "Scale of the text on Y axis in percent (100% = no changes).";
            leaf_prs2.Add(sy2.Name, sy2);

            Parameter angle2 = new Parameter();
            angle2.Name = "Angle";
            angle2.Object = 0f;
            angle2.Category = "Angle";
            angle2.Summary = "Angle of the text.";
            leaf_prs2.Add(angle2.Name, angle2);

            Parameter color12 = new Parameter();
            color12.Name = "Color at corner 7";
            color12.Object = new Color();
            color12.Category = "Color";
            color12.Summary = "Color at the corner 7 of the numeric keypad.";
            leaf_prs2.Add(color12.Name, color12);

            Parameter color22 = new Parameter();
            color22.Name = "Color at corner 9";
            color22.Object = new Color();
            color22.Category = "Color";
            color22.Summary = "Color at the corner 9 of the numeric keypad.";
            leaf_prs2.Add(color22.Name, color22);

            Parameter color32 = new Parameter();
            color32.Name = "Color at corner 3";
            color32.Object = new Color();
            color32.Category = "Color";
            color32.Summary = "Color at the corner 3 of the numeric keypad.";
            leaf_prs2.Add(color32.Name, color32);

            Parameter color42 = new Parameter();
            color42.Name = "Color at corner 1";
            color42.Object = new Color();
            color42.Category = "Color";
            color42.Summary = "Color at the corner 1 of the numeric keypad.";
            leaf_prs2.Add(color42.Name, color42);
            //===================================================================

            Events evts = new Events();
            evts.Add(start, leaf_prs);
            evts.Add(end, leaf_prs2);
            oe.Events = evts;

            return oe;
        }

        #endregion

        #region "Event"

        private Event GetCurrentEventHorizontalText(TreeNode main, int anchorX, int anchorY, int current_frame)
        {
            int meantime = current_frame, frame_before = 0, frame_after = 0;
            Event before = null, after = null, current = null;

            if (objects.ContainsKey(main))
            {
                ObjectEvents oe = (ObjectEvents)objects[main];
                Events evts = oe.Events;

                if (oe.Type == ObjectEvents.ObjectType.HorizontalText)
                {
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

                    if (after.Equals(before))
                    {
                        return before;
                    }

                    Parameters leaf_prs = new Parameters();

                    foreach (Parameter p1 in before.Parameters.GetValues())
                    {
                        foreach (Parameter p2 in after.Parameters.GetValues())
                        {
                            if (p1.Name.Equals(p2.Name))
                            {
                                if (p1.Name == "Font size") 
                                {
                                    float currentSize = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                                    Parameter p = new Parameter();
                                    p.Name = "Font size";
                                    p.Object = currentSize;
                                    p.Category = "Text";
                                    p.Summary = "A size.";
                                    leaf_prs.Add(p.Name, p);
                                }

                                if (p1.Name == "Position X")
                                {
                                    float currentPosition = GetPosition((float)p1.Object, (float)p2.Object, anchorX, frame_before, frame_after, meantime);
                                    Parameter p = new Parameter();
                                    p.Name = "Position X";
                                    p.Object = currentPosition;
                                    p.Category = "Position";
                                    p.Summary = "A position.";
                                    leaf_prs.Add(p.Name, p);
                                }

                                if (p1.Name == "Position Y")
                                {
                                    float currentPosition = GetPosition((float)p1.Object, (float)p2.Object, anchorY, frame_before, frame_after, meantime);
                                    Parameter p = new Parameter();
                                    p.Name = "Position Y";
                                    p.Object = currentPosition;
                                    p.Category = "Position";
                                    p.Summary = "A position.";
                                    leaf_prs.Add(p.Name, p);
                                }

                                if (p1.Name == "Scale X")
                                {
                                    float currentScale = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                                    Parameter p = new Parameter();
                                    p.Name = "Scale X";
                                    p.Object = currentScale;
                                    p.Category = "Scale";
                                    p.Summary = "A scale.";
                                    leaf_prs.Add(p.Name, p);
                                }

                                if (p1.Name == "Scale Y")
                                {
                                    float currentScale = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                                    Parameter p = new Parameter();
                                    p.Name = "Scale Y";
                                    p.Object = currentScale;
                                    p.Category = "Scale";
                                    p.Summary = "A scale.";
                                    leaf_prs.Add(p.Name, p);
                                }

                                if (p1.Name == "Angle")
                                {
                                    float currentAngle = GetFloat((float)p1.Object, (float)p2.Object, frame_before, frame_after, meantime);
                                    Parameter p = new Parameter();
                                    p.Name = "Angle";
                                    p.Object = currentAngle;
                                    p.Category = "Angle";
                                    p.Summary = "An angle.";
                                    leaf_prs.Add(p.Name, p);
                                }

                                if (p1.Name == "Color at corner 7" | p1.Name == "Color at corner 9" | p1.Name == "Color at corner 3" | p1.Name == "Color at corner 1")
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

                }
            }


            return current;
        }

        #endregion

        #region "Get object in time"

        private Color GetColor(Color before, Color after, int frame_before, int frame_after, int meantime)
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

        private float GetFloat(float before, float after, int frame_before , int frame_after, int meantime)
        {
            if (frame_before == meantime)
            {
                return before;
            }

            if (frame_after == meantime)
            {
                return after;
            }

            if(frame_before==0)
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

        private float GetPosition(float before, float after, int anchor, int frame_before, int frame_after, int meantime)
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

        private FontStyle GetFontStyle(bool bold, bool italic, bool underline, bool strikeout)
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

        #endregion

        #region "JSON"

        private void SaveJSON()
        {
            //Source @ http://james.newtonking.com/json
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                
                writer.WriteStartObject();
                writer.WritePropertyName("Video path");
                writer.WriteValue(video_path);
                writer.WritePropertyName("Extract path");
                writer.WriteValue(extract_path);
                writer.WritePropertyName("Modify path");
                writer.WriteValue(modify_path);

                writer.WritePropertyName("Video width");
                writer.WriteValue(video_width);
                writer.WritePropertyName("Video height");
                writer.WriteValue(video_height);
                writer.WritePropertyName("Video framerate");
                writer.WriteValue(video_framerate);
                writer.WritePropertyName("Video frames");
                writer.WriteValue(video_frames);

                writer.WriteEndObject();

            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\save.json"))
            {
                file.Write(sb.ToString());
            }
        }

        private void LoadJSON()
        {
            string readtoend = "";

            FileInfo fi = new FileInfo(Application.StartupPath + "\\save.json");

            if (fi.Exists)
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(fi.FullName))
                {
                    readtoend = file.ReadToEnd();
                }

                JsonTextReader reader = new JsonTextReader(new StringReader(readtoend));
                string lastValue = "", currentValue = "";
                while (reader.Read())
                {                    
                    if (reader.Value != null)
                    {
                        currentValue = reader.Value.ToString();

                        if (lastValue.Equals("Video path")) { video_path = currentValue; }
                        if (lastValue.Equals("Extract path")) { extract_path = currentValue; }
                        if (lastValue.Equals("Modify path")) { modify_path = currentValue; }

                        if (lastValue.Equals("Video width")) { video_width = currentValue; }
                        if (lastValue.Equals("Video height")) { video_height = currentValue; }
                        if (lastValue.Equals("Video framerate")) { video_framerate = currentValue; }
                        if (lastValue.Equals("Video frames")) { video_frames = currentValue; }

                        lastValue = reader.Value.ToString();
                    }

                }

                ConfigureVideo();
            }

        }

        #endregion

        #region "Video Informations"

        private void ConfigureVideo()
        {
            if (extract_path != "---")
            {
                imagescount = 0;
                DirectoryInfo di = new DirectoryInfo(extract_path);
                foreach (FileInfo fi in di.GetFiles())
                {
                    imagescount++;
                }

                trackBar1.Maximum = imagescount;
                trackBar1.Minimum = 1;
                trackBar1.Value = imagescount / 2;
            }

            if (video_width != "" && video_height != "")
            {
                pictureBox1.Width = Convert.ToInt32(video_width);
                pictureBox1.Height = Convert.ToInt32(video_height);
                pictureBox1.Left = (panel1.Width - pictureBox1.Width)/2;
                pictureBox1.Top = (panel1.Height - pictureBox1.Height)/2;
            }
        }

        private void GetInformations(string commandLine, string batchFile, string infoFile)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(batchFile))
            {
                file.Write(commandLine);
            }

            Process.Start(batchFile);

            string readtoend = "";

            FileInfo fi = new FileInfo(infoFile);

            if (fi.Exists)
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(fi.FullName))
                {
                    readtoend = file.ReadToEnd();
                }

                JsonTextReader reader = new JsonTextReader(new StringReader(readtoend));
                string lastValue = "", currentValue = "";
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        currentValue = reader.Value.ToString();

                        if (lastValue.Equals("width")) { video_width = currentValue; }
                        if (lastValue.Equals("height")) { video_height = currentValue; }
                        if (lastValue.Equals("r_frame_rate")) { video_framerate = video_framerate.Equals("") ? currentValue : video_framerate; }
                        if (lastValue.Equals("nb_frames")) { video_frames = video_frames.Equals("") ? currentValue : video_frames; }
                        
                        lastValue = reader.Value.ToString();
                        
                    }

                }
            }

        }

        #endregion
        
        #region "Bitmap"

        

        #endregion



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            propertyGridEx1.Item.Clear();
            propertyGridEx1.Item.Add("My String", "My Value", false, "Simple type", "This is a string", true);
            propertyGridEx1.Item.Add("My Integer", 100, false, "Simple type", "This is an integer", true);
            propertyGridEx1.Item.Add("My Double", 10.4, false, "Simple type", "This is a double", true);
            propertyGridEx1.Item.Add("My Font", new Font("Arial", 9), false, "Classes", "This is a font class", true);
            propertyGridEx1.Item.Add("My Color", new Color(), false, "Classes", "This is a color class", true);
            propertyGridEx1.Item.Add("My Point", new Point(10, 10), false, "Classes", "This is point class", true);
            propertyGridEx1.Refresh();

            LoadJSON();
        }
        
        private void cmsTVmnuAddO_Click(object sender, EventArgs e)
        {
            ObjectEvents oe = new ObjectEvents();
            countSCRIPT++;
            oe.Name = "Script "+countSCRIPT;
            oe.Object = null;
            Parameters main_prs = new Parameters();
            Parameter first_param = new Parameter();
            first_param.Name = "Example";
            first_param.Object = "Sample";
            first_param.Category = "Cat 1";
            first_param.Summary = "This is the first example.";
            main_prs.Add("Example", first_param);
            oe.Parameters = main_prs;
            Parameters leaf_prs = new Parameters();
            Parameter sub_param = new Parameter();
            sub_param.Name = "Example0";
            sub_param.Object = "Sample0";
            sub_param.Category = "Cat 2";
            sub_param.Summary = "This is the second example.";
            leaf_prs.Add("Example0", sub_param);
            Parameters leaf_prs2 = new Parameters();
            Parameter sub_param2 = new Parameter();
            sub_param2.Name = "Example0";
            sub_param2.Object = "Sample0";
            sub_param2.Category = "Cat 2";
            sub_param2.Summary = "This is the second example.";
            leaf_prs2.Add("Example0", sub_param2);
            Events evts = new Events();
            evts.Add("0", leaf_prs);
            evts.Add("100", leaf_prs2);
            oe.Events = evts;
            TreeNode topnode = treeView1.TopNode;
            TreeNode a_node = oe.GetTreeNode(objects, events);
            topnode.Nodes.Add(a_node);
        }

        private void cmsTVmnuAddE_Click(object sender, EventArgs e)
        {

        }

        private void cmsTVmnuRemove_Click(object sender, EventArgs e)
        {

        }

        private void cmsTVmnuAddTA_Click(object sender, EventArgs e)
        {

        }

        private void cmsTVmnuAddTH_Click(object sender, EventArgs e)
        {
            countTH++;
            ObjectEvents oe = ConfigureHorizontalText(countTH, trackBar1.Minimum.ToString(), trackBar1.Maximum.ToString());
            TreeNode topnode = treeView1.TopNode;
            TreeNode a_node = oe.GetTreeNode(objects, events);
            topnode.Nodes.Add(a_node);
        }

        private void cmsTVmnuAddTV_Click(object sender, EventArgs e)
        {

        }

        private void cmsTVmnuAddShape_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSeparator2_Click(object sender, EventArgs e)
        {

        }

        private void cmsTVmnuAddDrawing_Click(object sender, EventArgs e)
        {

        }

        private void cmsTVmnuAddPicture_Click(object sender, EventArgs e)
        {

        }

        private void cmsTVmnuAddVideo_Click(object sender, EventArgs e)
        {

        }

        private void treeview1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (objects.ContainsKey(e.Node))
                {
                    //MessageBox.Show(e.Node.FullPath + " est contenu dans OBJECTS");
                    propertyGridEx1.Item.Clear();
                    lastObjectUsed = objects[e.Node];
                    foreach(Parameter p in objects[e.Node].Parameters.GetValues())
                    {
                        propertyGridEx1.Item.Add(p.Name, p.Object, false, p.Category, p.Summary, true);
                    }                    
                    propertyGridEx1.Refresh();
                }else if (events.ContainsKey(e.Node))
                {
                    //MessageBox.Show(e.Node.FullPath + " est contenu dans EVENTS");
                    propertyGridEx1.Item.Clear();
                    lastObjectUsed = events[e.Node];
                    foreach (Parameter p in events[e.Node].Parameters.GetValues())
                    {
                        propertyGridEx1.Item.Add(p.Name, p.Object, false, p.Category, p.Summary, true);
                    }
                    propertyGridEx1.Refresh();
                }
                
            }
        }

        private void propertyGridEx1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyGridEx.PropertyGridEx pge = (PropertyGridEx.PropertyGridEx)s;

            if (lastObjectUsed.GetType() == typeof(ObjectEvents))
            {
                ObjectEvents oe = (ObjectEvents)lastObjectUsed;
                foreach (Parameter p in oe.Parameters.GetValues())
                {
                    if (p.Name.Equals(pge.SelectedGridItem.Label) == true)
                    {
                        p.Object = pge.SelectedGridItem.Value;
                    }
                }
            }
            else if (lastObjectUsed.GetType() == typeof(Event))
            {
                Event evt = (Event)lastObjectUsed;
                foreach (Parameter p in evt.Parameters.GetValues())
                {
                    if (p.Name.Equals(pge.SelectedGridItem.Label) == true)
                    {
                        p.Object = pge.SelectedGridItem.Value;
                    }
                }
            }

            pictureBox1.Refresh();
        }
        
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (extract_path != "---")
            {
                Bitmap bmp = new Bitmap(extract_path + "image-" + trackBar1.Value + ".png");
                e.Graphics.DrawImage(bmp, 0, 0);
            }

            foreach (TreeNode tn in treeView1.TopNode.Nodes)
            {
                if (objects.ContainsKey(tn))
                {
                    ObjectEvents oe = (ObjectEvents)objects[tn];

                    if (oe.Type == ObjectEvents.ObjectType.HorizontalText)
                    {
                        String str = "Hello world !";
                        String fontname = "Arial";
                        int anchorX = 0, anchorY = 0;
                        bool bold = false, italic = false, underline = false, strikeout = false;

                        foreach (Parameter p in oe.Parameters.GetValues())
                        {
                            if (p.Name == "Parent") {  }
                            if (p.Name == "Font name") { fontname = p.Object.ToString(); }
                            if (p.Name == "Bold") { bold = (bool)p.Object; }
                            if (p.Name == "Italic") { italic = (bool)p.Object; }
                            if (p.Name == "Underline") { underline = (bool)p.Object; }
                            if (p.Name == "Strike out") { strikeout = (bool)p.Object; }
                            if (p.Name == "String") { str = p.Object.ToString(); }
                            if (p.Name == "Anchor X") { anchorX = (int)p.Object; }
                            if (p.Name == "Anchor Y") { anchorY = (int)p.Object; }
                            if (p.Name == "Position") {  }
                        }

                        float fontsize = 20.25f, positionX = 0f, positionY = 0f, scaleX = 100f, scaleY = 100f, angle = 0f;
                        Color c7 = new Color(), c9 = new Color(), c3 = new Color(), c1 = new Color();

                        Event evt = GetCurrentEventHorizontalText(tn, anchorX, anchorY, trackBar1.Value);
                        foreach (Parameter p in evt.Parameters.GetValues())
                        {
                            if (p.Name == "Font size") { fontsize = (float)p.Object; }
                            if (p.Name == "Position X") { positionX = (float)p.Object; }
                            if (p.Name == "Position Y") { positionY = (float)p.Object; }
                            if (p.Name == "Scale X") { scaleX = (float)p.Object;  }
                            if (p.Name == "Scale Y") { scaleY = (float)p.Object; }
                            if (p.Name == "Angle") { angle = (float)p.Object; }
                            if (p.Name == "Color at corner 7") { c7 = (Color)p.Object; }
                            if (p.Name == "Color at corner 9") { c9 = (Color)p.Object; }
                            if (p.Name == "Color at corner 3") { c3 = (Color)p.Object; }
                            if (p.Name == "Color at corner 1") { c1 = (Color)p.Object; }
                        }

                        Font fn = new Font(fontname, fontsize, GetFontStyle(bold, italic, underline, strikeout));
                        e.Graphics.RotateTransform(angle);
                        e.Graphics.ScaleTransform(scaleX/100, scaleY/100);
                        RPXGraphics.DrawStringWithFourCornersGradient(e.Graphics, str, fn, positionX, positionY, c7, c9, c3, c1);
                        
                        //e.Graphics.DrawString(str, fn, Brushes.Yellow, positionX, positionY);
                        fn.Dispose();
                    }
                }


            }

            if (regen == true)
            {
                Bitmap bmp = new Bitmap(Convert.ToInt32(video_width), Convert.ToInt32(video_height));
                pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(modify_path + "image-" + trackBar1.Value + ".png");
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void tsbOpenVideo_Click(object sender, EventArgs e)
        {
            
            DialogResult dr = ofdVideo.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Source @ http://linuxers.org/tutorial/how-extract-images-video-using-ffmpeg
                FileInfo fi = new FileInfo(ofdVideo.FileName);
                fi.Directory.CreateSubdirectory("extract");
                fi.Directory.CreateSubdirectory("modify");

                video_path = fi.FullName;
                extract_path = fi.Directory + "\\extract\\";
                modify_path = fi.Directory + "\\modify\\";                

                //Extraction d'images
                String commandLine = "\"" + Application.StartupPath + "\\ffmpeg.exe\" -i \"" + ofdVideo.FileName + "\" -f image2 \"" + fi.Directory + "\\extract\\image-%1d.png\"";

                Console.Out.WriteLine(commandLine);

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = commandLine;
                p.Start();
                string consoleOut = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                //Obtention d'informations
                commandLine = "\"" + Application.StartupPath + "\\ffprobe.exe\" -v quiet -print_format json -show_format -show_streams \"" + ofdVideo.FileName + "\" > \"" + fi.Directory + "\\out.json\"";

                GetInformations(commandLine, fi.Directory + "\\batch.bat", fi.Directory + "\\out.json");

                SaveJSON();

                ConfigureVideo();
            }
        }

        private void tsbRegenerate_Click(object sender, EventArgs e)
        {
            for (int i = trackBar1.Minimum; i <= trackBar1.Maximum; i++)
            {
                Bitmap bmp = new Bitmap(Convert.ToInt32(video_width), Convert.ToInt32(video_height));
                Graphics g = Graphics.FromImage(bmp);

                Bitmap rbmp = new Bitmap(extract_path + "image-" + i + ".png");
                g.DrawImage(rbmp, 0, 0);
                rbmp.Dispose();

                foreach (TreeNode tn in treeView1.TopNode.Nodes)
                {
                    if (objects.ContainsKey(tn))
                    {
                        ObjectEvents oe = (ObjectEvents)objects[tn];

                        if (oe.Type == ObjectEvents.ObjectType.HorizontalText)
                        {
                            String str = "Hello world !";
                            String fontname = "Arial";
                            int anchorX = 0, anchorY = 0;
                            bool bold = false, italic = false, underline = false, strikeout = false;

                            foreach (Parameter p in oe.Parameters.GetValues())
                            {
                                if (p.Name == "Parent") { }
                                if (p.Name == "Font name") { fontname = p.Object.ToString(); }
                                if (p.Name == "Bold") { bold = (bool)p.Object; }
                                if (p.Name == "Italic") { italic = (bool)p.Object; }
                                if (p.Name == "Underline") { underline = (bool)p.Object; }
                                if (p.Name == "Strike out") { strikeout = (bool)p.Object; }
                                if (p.Name == "String") { str = p.Object.ToString(); }
                                if (p.Name == "Anchor X") { anchorX = (int)p.Object; }
                                if (p.Name == "Anchor Y") { anchorY = (int)p.Object; }
                                if (p.Name == "Position") { }
                            }

                            float fontsize = 20.25f, positionX = 0f, positionY = 0f, scaleX = 100f, scaleY = 100f, angle = 0f;
                            Color c7 = new Color(), c9 = new Color(), c3 = new Color(), c1 = new Color();

                            Event evt = GetCurrentEventHorizontalText(tn, anchorX, anchorY, i);
                            foreach (Parameter p in evt.Parameters.GetValues())
                            {
                                if (p.Name == "Font size") { fontsize = (float)p.Object; }
                                if (p.Name == "Position X") { positionX = (float)p.Object; }
                                if (p.Name == "Position Y") { positionY = (float)p.Object; }
                                if (p.Name == "Scale X") { scaleX = (float)p.Object; }
                                if (p.Name == "Scale Y") { scaleY = (float)p.Object; }
                                if (p.Name == "Angle") { angle = (float)p.Object; }
                                if (p.Name == "Color at corner 7") { c7 = (Color)p.Object; }
                                if (p.Name == "Color at corner 9") { c9 = (Color)p.Object; }
                                if (p.Name == "Color at corner 3") { c3 = (Color)p.Object; }
                                if (p.Name == "Color at corner 1") { c1 = (Color)p.Object; }
                            }

                            Font fn = new Font(fontname, fontsize, GetFontStyle(bold, italic, underline, strikeout));
                            g.RotateTransform(angle);
                            g.ScaleTransform(scaleX / 100, scaleY / 100);
                            RPXGraphics.DrawStringWithFourCornersGradient(g, str, fn, positionX, positionY, c7, c9, c3, c1);
                            fn.Dispose();
                        }
                    }

                    bmp.Save(modify_path + "image-" + i + ".png");
                    g.Dispose();
                    bmp.Dispose();
                }
            }

            

            MessageBox.Show("Regen OK");
        }

        private void tsbEncode_Click(object sender, EventArgs e)
        {
            FileInfo fn = new FileInfo(video_path);

            string framerate = "25";
            if(video_framerate.Contains("2997"))
            {
                framerate = "29.97";
            }

            string commandLine = "\"" + Application.StartupPath + "\\ffmpeg.exe\" -f image2 -framerate " + framerate + " -i \"" + modify_path + "image-%d.png\" -r " + framerate + " \"" + fn.Directory + "\\output.mp4\"";

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = commandLine;
            p.Start();
            string consoleOut = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }
    }
}
