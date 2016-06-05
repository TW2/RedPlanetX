using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetXv8.Composition
{
    public partial class ShapeManagerForm : Form
    {
        public ShapeManagerForm()
        {
            InitializeComponent();
            figureDrawArea1.SetLineMode();
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            List<Drawing.Group> gs = LoadFigure();
            figureDrawArea1.SetGroups(gs);
            figureDrawArea1.Refresh();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveFigure(figureDrawArea1.GetGroups());
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            List<Drawing.Group> groups = figureDrawArea1.GetGroups();

            if (groups.Count != 0)
            {
                DialogResult dr = MessageBox.Show("Do you want to save the previous figure ?", "Save option", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    SaveFigure(groups);
                }
            }

            figureDrawArea1.GetGroups().Clear();
            figureDrawArea1.NewGroup();
            figureDrawArea1.Refresh();
        }

        private void tsbAddLine_Click(object sender, EventArgs e)
        {
            tsbAddLine.BackColor = Color.Gold;
            tsbAddCurve.BackColor = SystemColors.Control;
            figureDrawArea1.SetLineMode();
        }

        private void tsbAddCurve_Click(object sender, EventArgs e)
        {
            tsbAddLine.BackColor = SystemColors.Control;
            tsbAddCurve.BackColor = Color.Gold;
            figureDrawArea1.SetCurveMode();
        }

        private void SaveFigure(List<Drawing.Group> groups)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RedPlanet Xpress Figure|*.rpxf";
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                XML.XmlForShapeGroups.Serialize(groups, sfd.FileName);
            }
        }

        private List<Drawing.Group> LoadFigure()
        {
            List<Drawing.Group> gs = new List<Drawing.Group>();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RedPlanet Xpress Figure|*.rpxf";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                gs = XML.XmlForShapeGroups.Deserialize(ofd.FileName);
            }

            return gs;
        }

        public List<Drawing.Group> Groups
        {
            get { return figureDrawArea1.GetGroups(); }
        }
    }
}
