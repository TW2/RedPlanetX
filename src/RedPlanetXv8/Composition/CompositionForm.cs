using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetXv8.Composition
{
    public partial class CompositionForm : Form
    {
        public CompositionForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            cbbFPS.Items.Add(FrameRate._FILM);
            cbbFPS.Items.Add(FrameRate._NTSC24);
            cbbFPS.Items.Add(FrameRate._NTSC30);
            cbbFPS.Items.Add(FrameRate._PAL25);
            cbbFPS.SelectedItem = FrameRate._NTSC24;

            cbbSize.Items.Add(FrameSize._SD_SQUARE);
            cbbSize.Items.Add(FrameSize._SD_RECT);
            cbbSize.Items.Add(FrameSize._HD720p);
            cbbSize.Items.Add(FrameSize._HD1080p);
            cbbSize.Items.Add(FrameSize._4K);
            cbbSize.Items.Add(FrameSize._8K);
            cbbSize.SelectedItem = FrameSize._HD720p;
        }

        public Settings GetComposition()
        {
            Settings set = new Settings();
            set.ProjectName = txtProject.Text;
            set.AuthorName = txtAuthor.Text;
            set.FPS = (FrameRate)cbbFPS.SelectedItem;
            set.Size = (FrameSize)cbbSize.SelectedItem;
            set.SetDuration(Convert.ToInt32(txtDurMin.Text), Convert.ToInt32(txtDurSec.Text));

            return set;
        }
    }
}
