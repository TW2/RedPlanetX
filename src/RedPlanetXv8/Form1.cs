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
    public partial class Form1 : Form
    {
        MdiTools mdiTools = null;
        MdiView mdiView = null;
        Settings composition = null;
        AviSynthObject avso = null;
        AssScript asss = null;
        double FPS = 23.976d;

        public Form1()
        {
            InitializeComponent();

            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height - 40;
            Size = new Size(w, h);
            StartPosition = FormStartPosition.Manual;
            Location = new Point(0, 0);
            mdiTools = new MdiTools();
            mdiTools.MdiParent = this;
            mdiTools.ParentFormLink = this;
            mdiTools.Show();
            mdiView = new MdiView();
            mdiView.MdiParent = this;            
            mdiView.Size = new Size(Width - mdiTools.Width - 50, Height - 100);
            mdiView.Show();
            mdiView.Location = new Point(mdiTools.Width, 0);
        }

        private void tsbAviSynth_Click(object sender, EventArgs e)
        {
            CompositionForm cf = new CompositionForm();
            DialogResult dr = cf.ShowDialog();
            if (dr == DialogResult.OK)
            {
                composition = new Settings();
                composition = cf.GetComposition();
            }

            mdiView.Composition = composition;            

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "AviSynth script|*.avs";
            dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                avso = new AviSynthObject(ofd.FileName);
                mdiView.SetAVSView(avso);
                mdiView.TrackBar.Maximum = avso.Clip.num_frames;
                mdiTools.SetROOT();
                mdiTools.SetCompositionAVS(composition, avso);
                avso.Update(0);
                mdiView.View.ChangeViewImage(avso.Image);
                FPS = Convert.ToDouble(avso.Clip.raten) / Convert.ToDouble(avso.Clip.rated);
                mdiView.View.ChangeFrameAndRefresh(0);
            }

        }

        private void tsbASS_Click(object sender, EventArgs e)
        {
            if(avso != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Advanced Sub Station|*.ass";
                DialogResult dr = ofd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    asss = mdiView.View.Script = new AssScript(ofd.FileName);
                    avso.Update(0);
                    mdiView.View.ChangeViewImage(avso.Image);
                    mdiView.View.ChangeFrameAndRefresh(0);
                    mdiTools.SetMainASS(asss);
                    mdiTools.AddSentences();
                }
            }
            
        }

        private void tsbParent_Click(object sender, EventArgs e)
        {
            if (avso != null && asss != null)
            {
                mdiTools.AddParent();
            }
        }

        private void tsbShape_Click(object sender, EventArgs e)
        {

            if (avso != null && asss != null)
            {
                ShapeManagerForm sm = new ShapeManagerForm();
                DialogResult dr = sm.ShowDialog();
                if(dr == DialogResult.OK)
                {
                    ShapeObject sho = new ShapeObject(sm.Groups);
                    sho.AddStart(0L);
                    sho.AddEnd(5000L);
                    ShapeTreeNode stn = null;
                    ParentTreeNode ptn = null;
                    mdiTools.AddShape(sho, out stn, out ptn);
                    if(stn != null && ptn != null)
                    {
                        mdiView.View.AddShapeLink(stn, ptn);
                    }
                }
            }
        }

        public MdiView MdiView
        {
            get { return mdiView; }
        }

        private void tsbAddPath_Click(object sender, EventArgs e)
        {
            if (avso != null && asss != null)
            {
                mdiTools.AddPath();
            }
        }
    }
}
