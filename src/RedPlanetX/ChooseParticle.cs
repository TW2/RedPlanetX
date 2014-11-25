using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetX
{
    public partial class ChooseParticle : Form
    {
        public ChooseParticle()
        {
            InitializeComponent();
        }

        public void LoadParticles()
        {
            listBox1.Items.Clear();
            IO.FillParticlesList(listBox1);
        }

        public GenericParticle GetParticle()
        {
            if (listBox1.SelectedItem != null)
            {
                return (GenericParticle)listBox1.SelectedItem;
            }
            else
            {
                return null;
            }
        }

        private void DrawParticle(Graphics g)
        {
            if (listBox1.SelectedItem != null)
            {
                if (listBox1.SelectedItem.GetType() == typeof(ParamTypeParticle))
                {
                    ParamTypeParticle ptp = (ParamTypeParticle)listBox1.SelectedItem;

                    foreach (TreeNode tntp in ptp.GetVolmuesNode().Nodes)
                    {
                        if (ptp.GetVolumes().ContainsKey(tntp))
                        {
                            Volume v = (Volume)ptp.GetVolumes()[tntp];

                            Drawing.DrawVolumeAndInsertPoint(g, v, trackBar1.Value, trackBar1.Maximum);

                        }
                    }
                }
                else if (listBox1.SelectedItem.GetType() == typeof(ScriptTypeParticle))
                {
                    ScriptTypeParticle stp = (ScriptTypeParticle)listBox1.SelectedItem;


                }
            }            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Drawing.DrawLandmark(e.Graphics, pictureBox1.Width, pictureBox1.Height);
            DrawParticle(e.Graphics);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (listBox1.SelectedItem.GetType() == typeof(ParamTypeParticle))
                {
                    ParamTypeParticle ptp = (ParamTypeParticle)listBox1.SelectedItem;

                    foreach (TreeNode tntp in ptp.GetVolmuesNode().Nodes)
                    {
                        if (ptp.GetVolumes().ContainsKey(tntp))
                        {
                            Volume v = (Volume)ptp.GetVolumes()[tntp];

                            int num = v.Events.ToList().Count;
                            Event topevent = v.Events.ToList()[num - 1];
                            trackBar1.Maximum = Convert.ToInt32(topevent.Name);
                            pictureBox1.Refresh();
                        }
                    }
                }
                else if (listBox1.SelectedItem.GetType() == typeof(ScriptTypeParticle))
                {
                    ScriptTypeParticle stp = (ScriptTypeParticle)listBox1.SelectedItem;


                }
            }            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }


    }
}
