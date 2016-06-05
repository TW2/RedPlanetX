using RedPlanetXv8.AviSynth;
using RedPlanetXv8.Composition;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedPlanetXv8
{
    public partial class MdiView : Form
    {
        Settings composition = null;
        Composition.View avsPanel = null;
        AviSynthObject avso = null;

        public MdiView()
        {
            InitializeComponent();
            trackBar1.ValueChanged += TrackBar1_ValueChanged;
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (avsPanel != null & composition != null)
            {
                Text = "RedPlanetX :: Odyssée :: " + composition.ProjectName + " project by " + composition.AuthorName +
                    " :: " + trackBar1.Value + "/" + trackBar1.Maximum;
                avso.Update(trackBar1.Value);
                avsPanel.ChangeViewImage(avso.Image);
                avsPanel.ChangeFrameAndRefresh(trackBar1.Value);
            }
        }

        private void MdiView_SizeChanged(object sender, EventArgs e)
        {
            panel1.Location = new Point(0, trackBar1.Height);
            panel1.Size = new Size(trackBar1.Width, Height - trackBar1.Height - 0);
        }

        public void SetAVSView(AviSynthObject avso)
        {
            this.avso = avso;
            panel1.Controls.Clear();
            avsPanel = new Composition.View();
            avsPanel.ChangeViewSize(new Size(composition.Size.Width, composition.Size.Height));
            panel1.Controls.Add(avsPanel);
            avsPanel.Dock = DockStyle.Fill;
        }

        public TrackBar TrackBar
        {
            get { return trackBar1; }
            set { trackBar1 = value; }
        }

        public Composition.View View
        {
            get { return avsPanel; }
            set { avsPanel = value; }
        }

        public Settings Composition
        {
            get { return composition; }
            set { composition = value; }
        }
    }
}
