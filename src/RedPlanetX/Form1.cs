using AVS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetX
{
    public partial class Form1 : Form
    {
        //===============================================================================
        //EN : Global variables
        //FR : Variables globales
        //===============================================================================
        private Color GLOBAL_lineModeColor;
        private object GLOBAL_lastObjectUsed = null;
        private String GLOBAL_lastObjectUsedBy = "";
        private GraphicsPath GLOBAL_GP = null;
        private Bitmap GLOBAL_bitmap = null;
        private AviSynthClip GLOBAL_Clip = null;
        private String GLOBAL_script = "";

        #region Initialization

        public Form1()
        {
            InitializeComponent();

            //EN : Initialization of screen size
            //FR : Initialisation de la taille de l'écran
            StartPosition = FormStartPosition.Manual;

            Size = new Size(
                Screen.PrimaryScreen.WorkingArea.Width,
                Screen.PrimaryScreen.WorkingArea.Height);

            Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left,
                Screen.PrimaryScreen.WorkingArea.Top);

            //EN : Configuration of elements
            //FR : Configuration des élements
            tvVideo.Size = new Size(tvVideo.Width, 330);        //tvVideo = Video mode
            tvParticles.Size = new Size(tvVideo.Width, 330);    //tvParticles = Particles mode
            tvParticles.Location = tvVideo.Location;
            tvParticles.BackColor = Color.White;
            tvParticles.Visible = false;
            tvCreation.Size = new Size(tvVideo.Width, 330);     //tvCreation = Creation mode
            tvCreation.Location = tvVideo.Location;
            tvCreation.BackColor = Color.White;
            tvCreation.Visible = false;

            propertyGridEx1.Size = new Size(
                tvVideo.Width, 
                Screen.PrimaryScreen.WorkingArea.Height - tvVideo.Height - tsMain.Height - 60);

            tabcMain.Size = new Size(
                Screen.PrimaryScreen.WorkingArea.Width - tvVideo.Width - 40,
                Screen.PrimaryScreen.WorkingArea.Height - tsMain.Height - 53);

            trackbVideo.Width = tabcMain.Width - 20;

            panVideo.Size = new Size(
                tabcMain.Width - 20,
                tabcMain.Height - tsVideo.Height - trackbVideo.Height - 45);

            picbVideo.Width = 1280;
            picbVideo.Height = 720;
            picbVideo.Left = (panVideo.Width - picbVideo.Width) / 2;
            picbVideo.Top = (panVideo.Height - picbVideo.Height) / 2;
            panVideo.BackColor = Color.White;
            picbVideo.BackColor = Color.Blue;

            lblDuration.Location = new Point(tabcMain.Width - listCrObj.Width - 15, lblDuration.Top);
            txtDuration.Location = new Point(tabcMain.Width - listCrObj.Width - 10, txtDuration.Top);
            rbSeconds.Location = new Point(tabcMain.Width - 85, rbSeconds.Top);
            rbMilliseconds.Location = new Point(tabcMain.Width - 50, rbMilliseconds.Top);
            lblObjects.Location = new Point(tabcMain.Width - listCrObj.Width - 15, lblObjects.Top);
            listCrObj.Location = new Point(tabcMain.Width - listCrObj.Width - 10, listCrObj.Top);
            listCrObj.Size = new Size(
                listCrObj.Width,
                tabcMain.Height - tsCreation.Height - lblDuration.Height - txtDuration.Height - lblObjects.Height - 40);
            //lblLayers.Location = new Point(tabcMain.Width - listCrObj.Width - 15, lblLayers.Top);
            //listCrLay.Location = new Point(tabcMain.Width - listCrLay.Width - 10, listCrLay.Top);
            //listCrLay.Size = new Size(
            //    listCrLay.Width,
            //    tabcMain.Height - tsCreation.Height - lblDuration.Height - txtDuration.Height - lblObjects.Height - listCrObj.Height - lblLayers.Height - 50);

            trackbCreation.Width = tabcMain.Width - listCrObj.Width - 30;

            panCreation.Size = new Size(
                tabcMain.Width - listCrObj.Width - 30,
                tabcMain.Height - tsCreation.Height - trackbCreation.Height - 45);

            picbCreation.Width = 512;
            picbCreation.Height = 512;
            picbCreation.Left = (panCreation.Width - picbCreation.Width) / 2;
            picbCreation.Top = (panCreation.Height - picbCreation.Height) / 2;
            panCreation.BackColor = Color.White;
            picbCreation.BackColor = Color.WhiteSmoke;

            lblPrParticles.Location = new Point(tabcMain.Width - listPrParticles.Width - 15, lblPrParticles.Top);
            listPrParticles.Location = new Point(tabcMain.Width - listPrParticles.Width - 10, listPrParticles.Top);
            listPrParticles.Size = new Size(listPrParticles.Width, 400);
            lblPrDuration.Location = new Point(tabcMain.Width - listPrParticles.Width - 15, lblPrDuration.Top + 130);
            txtPrDuration.Location = new Point(tabcMain.Width - listPrParticles.Width - 10, txtPrDuration.Top + 130);
            rbPrSeconds.Location = new Point(tabcMain.Width - 85, rbPrSeconds.Top + 130);
            rbPrMilliseconds.Location = new Point(tabcMain.Width - 50, rbPrMilliseconds.Top + 130);
            lblPrObjects.Location = new Point(tabcMain.Width - listPrParticles.Width - 15, lblPrObjects.Top + 130);
            listPrObjects.Location = new Point(tabcMain.Width - listPrObjects.Width - 10, listPrObjects.Top + 130);
            listPrObjects.Size = new Size(
                listPrObjects.Width,
                tabcMain.Height - tsParticles.Height - lblPrParticles.Height - listPrParticles.Height - lblPrDuration.Height - txtPrDuration.Height - lblPrObjects.Height - 40);
            trackbParticles.Width = tabcMain.Width - listPrParticles.Width - 30;
            panParticles.Size = new Size(
                tabcMain.Width - listPrObjects.Width - 30,
                tabcMain.Height - tsParticles.Height - trackbParticles.Height - 45);
            picbParticles.Width = 512;
            picbParticles.Height = 512;
            picbParticles.Left = (panParticles.Width - picbParticles.Width) / 2;
            picbParticles.Top = (panParticles.Height - picbParticles.Height) / 2;
            panParticles.BackColor = Color.White;
            picbParticles.BackColor = Color.WhiteSmoke;

            //EN : Others initializations
            //FR : Autres initialisations
            InitOfParticleMode();
            InitOfCreationMode();



        }

        #endregion


        #region Video mode

        //===============================================================================
        //EN : Variables of Video mode
        //FR : Variables du mode Vidéo
        //===============================================================================
        private double video_fpm = 25d / 1000d;
        private FXSubtitle video_fxs = null;
        private long video_milliseconds = 0;

        private long GetMilliseconds(int frames)
        {
            long ms = Convert.ToInt64(Math.Round(frames / video_fpm));
            return ms;
        }

        private void trackbVideo_ValueChanged(object sender, EventArgs e)
        {
            GLOBAL_bitmap = ReadFrameBitmap(GLOBAL_Clip, trackbVideo.Value);
            video_milliseconds = GetMilliseconds(trackbVideo.Value);
            picbVideo.Refresh();
        }

        private void picbVideo_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (GLOBAL_bitmap != null)
                {
                    e.Graphics.DrawImage(GLOBAL_bitmap, 0, 0);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            

            if(video_fxs != null){
                Drawing.DrawFXSubtitle(e.Graphics, video_fxs, video_milliseconds);
            }
        }

        private void tsmiChooseParticle_Click(object sender, EventArgs e)
        {
            ChooseParticle cp = new ChooseParticle();
            cp.LoadParticles();
            DialogResult dr = cp.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                GenericParticle gp = cp.GetParticle();
                if (gp != null)
                {
                    if (video_fxs.GetAssLines().ContainsKey(tvVideo.SelectedNode))
                    {
                        AssLine al = new AssLine();
                        video_fxs.GetAssLines().TryGetValue(tvVideo.SelectedNode, out al);
                        al.AddParticle(gp);

                        TreeNode tn = new TreeNode(gp.ToString());
                        video_fxs.GetParticles().Add(tn, gp);
                        tvVideo.SelectedNode.Nodes.Add(tn);
                    }
                    else if (video_fxs.GetAssAllSyllables().ContainsKey(tvVideo.SelectedNode))
                    {
                        AssAllSyllables aas = new AssAllSyllables();
                        video_fxs.GetAssAllSyllables().TryGetValue(tvVideo.SelectedNode, out aas);
                        aas.AddParticle(gp);

                        TreeNode tn = new TreeNode(gp.ToString());
                        video_fxs.GetParticles().Add(tn, gp);
                        tvVideo.SelectedNode.Nodes.Add(tn);
                    }
                    else if (video_fxs.GetAssSyllables().ContainsKey(tvVideo.SelectedNode))
                    {
                        AssSyllable _as = new AssSyllable();
                        video_fxs.GetAssSyllables().TryGetValue(tvVideo.SelectedNode, out _as);
                        _as.AddParticle(gp);

                        TreeNode tn = new TreeNode(gp.ToString());
                        video_fxs.GetParticles().Add(tn, gp);
                        tvVideo.SelectedNode.Nodes.Add(tn);
                    }
                }
            }
        }

        #endregion

        #region Particles mode

        #region Drawing of Particle Mode

        /** Sélectionne les géométries qui sont proche du point. */
        private void PrSelectGeometry(InsertPoint ip, int x, int y, int width, int height)
        {
            int minX = x - width / 2; int maxX = x + width / 2;
            int minY = y - height / 2; int maxY = y + height / 2;
            foreach (GeometryObject go in ip.GetTrajectorySpline().GetTrajectory())
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    if (l.Start.X >= minX && l.Start.X <= maxX && l.Start.Y >= minY && l.Start.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Start;
                        particle_geoPoints.Add(geoPoint);
                    }
                    if (l.Stop.X >= minX && l.Stop.X <= maxX && l.Stop.Y >= minY && l.Stop.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Stop;
                        particle_geoPoints.Add(geoPoint);
                    }
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    if (b.Start.X >= minX && b.Start.X <= maxX && b.Start.Y >= minY && b.Start.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Start;
                        particle_geoPoints.Add(geoPoint);
                    }
                    if (b.Stop.X >= minX && b.Stop.X <= maxX && b.Stop.Y >= minY && b.Stop.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Stop;
                        particle_geoPoints.Add(geoPoint);
                    }
                    if (b.CP1.X >= minX && b.CP1.X <= maxX && b.CP1.Y >= minY && b.CP1.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.CP1;
                        particle_geoPoints.Add(geoPoint);
                    }
                    if (b.CP2.X >= minX && b.CP2.X <= maxX && b.CP2.Y >= minY && b.CP2.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.CP2;
                        particle_geoPoints.Add(geoPoint);
                    }
                }
            }
            picbParticles.Refresh();
        }

        /** Change les géomtries des formes sélectionnées avec un SelectGeometry. */
        private void PrChangeGeometry(int x, int y)
        {
            foreach (GeometryPoint geoPoint in particle_geoPoints)
            {
                if (geoPoint.SelectedObject.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)geoPoint.SelectedObject;
                    if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                    {
                        l.Start = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                    {
                        l.Stop = new Point(x, y);
                    }
                }
                else if (geoPoint.SelectedObject.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)geoPoint.SelectedObject;
                    if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                    {
                        b.Start = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                    {
                        b.Stop = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP1)
                    {
                        b.CP1 = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP2)
                    {
                        b.CP2 = new Point(x, y);
                    }
                }
            }
            picbParticles.Refresh();
        }

        private void DrawInParticleMode(Graphics g)
        {
            if (tvParticles.TopNode.Nodes.Count > 0)
            {
                foreach (TreeNode tn in tvParticles.TopNode.Nodes)
                {
                    if (particles.ContainsKey(tn))
                    {
                        GenericParticle gp = (GenericParticle)particles[tn];

                        if (gp.GetType() == typeof(ParamTypeParticle))
                        {
                            ParamTypeParticle ptp = (ParamTypeParticle)gp;

                            foreach (TreeNode tntp in ptp.GetVolmuesNode().Nodes)
                            {
                                if (ptp.GetVolumes().ContainsKey(tntp))
                                {
                                    Volume v = (Volume)ptp.GetVolumes()[tntp];

                                    Drawing.DrawVolumeAndInsertPoint(g, v, trackbParticles.Value, trackbParticles.Maximum, particle_geoPoints);

                                }
                            }

                            //foreach (TreeNode tntp in ptp.GetPointsNode().Nodes)
                            //{
                            //    if (ptp.GetPoints().ContainsKey(tntp))
                            //    {
                            //        InsertPoint ip = (InsertPoint)ptp.GetPoints()[tntp];

                            //        Drawing.DrawInsertPoint(g, ip, trackbParticles.Value);

                            //    }
                            //}
                        }
                        else if (gp.GetType() == typeof(ScriptTypeParticle))
                        {
                            ScriptTypeParticle stp = (ScriptTypeParticle)gp;


                        }
                    }

                }
            }
        }

        #endregion
        //===============================================================================
        //EN : Variables of Particles mode
        //FR : Variables du mode Particules
        //===============================================================================
        private Dictionary<TreeNode, GenericParticle> particles = new Dictionary<TreeNode, GenericParticle>();
        private int countParticles = 0;
        private object particle_lastVolumeUsed = null;
        private List<GeometryPoint> particle_geoPoints = new List<GeometryPoint>();

        private void InitOfParticleMode()
        {
            IO.FillParticleVolumeList(listPrObjects);
            IO.FillParticlesList(listPrParticles);
        }

        

        //===============================================================================
        //EN : After a selection on the treview, we refresh the property grid with parameters of a particle
        //FR : Après une sélection sur le treview, on rafraichit la grille de propriétés avec les paramètres d'une particule
        //===============================================================================
        private void tvParticles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                foreach (TreeNode tn in tvParticles.TopNode.Nodes)
                {
                    if (particles.ContainsKey(tn))
                    {
                        GenericParticle gp = (GenericParticle)particles[tn];

                        if (gp.GetType() == typeof(ParamTypeParticle))
                        {
                            ParamTypeParticle ptp = (ParamTypeParticle)gp;

                            if (ptp.GetVolumes().ContainsKey(e.Node))
                            {
                                propertyGridEx1.Item.Clear();
                                GLOBAL_lastObjectUsed = ptp.GetVolumes()[e.Node];
                                GLOBAL_lastObjectUsedBy = "Particle";
                                foreach (Parameter p in ptp.GetVolumes()[e.Node].Parameters.GetValues())
                                {
                                    propertyGridEx1.Item.Add(p.Name, p.Object, false, p.Category, p.Summary, true);
                                }
                                propertyGridEx1.Refresh();

                                //displayObjects();
                            }
                            else if (ptp.GetEvents().ContainsKey(e.Node))
                            {
                                propertyGridEx1.Item.Clear();
                                GLOBAL_lastObjectUsed = ptp.GetEvents()[e.Node];
                                GLOBAL_lastObjectUsedBy = "Particle";
                                foreach (Parameter p in ptp.GetEvents()[e.Node].Parameters.GetValues())
                                {
                                    propertyGridEx1.Item.Add(p.Name, p.Object, false, p.Category, p.Summary, true);
                                }
                                propertyGridEx1.Refresh();
                            }
                            else if (ptp.GetPoints().ContainsKey(e.Node))
                            {
                                propertyGridEx1.Item.Clear();
                                GLOBAL_lastObjectUsed = ptp.GetPoints()[e.Node];
                                GLOBAL_lastObjectUsedBy = "Particle";
                                particle_lastVolumeUsed = ptp.GetVolumes()[e.Node.Parent.Parent];
                                foreach (Parameter p in ptp.GetPoints()[e.Node].Parameters.GetValues())
                                {
                                    propertyGridEx1.Item.Add(p.Name, p.Object, false, p.Category, p.Summary, true);
                                }
                                propertyGridEx1.Refresh();
                            }
                        }
                        else if (gp.GetType() == typeof(ScriptTypeParticle))
                        {
                            ScriptTypeParticle stp = (ScriptTypeParticle)gp;


                        }
                    }
                }
            }
        }

        //===============================================================================
        //EN : Actions of context menu in Particles treeview
        //FR : Actions du menu contextuel dans le treeview des Particules
        //===============================================================================
        private void tsmiPartPP_Click(object sender, EventArgs e)
        {
            countParticles++;
            ParamTypeParticle ptp = new ParamTypeParticle();
            TreeNode particle_treenode = new TreeNode("Particle " + countParticles + " (ParamType)");
            particles.Add(particle_treenode, ptp);
            tvParticles.TopNode.Nodes.Add(particle_treenode);
            ptp.InitTopNode(particle_treenode);
        }

        private void tsmiPartSP_Click(object sender, EventArgs e)
        {

        }

        private void tsmiPartSV_Click(object sender, EventArgs e)
        {
            if (particles.ContainsKey(tvParticles.SelectedNode))
            {
                GenericParticle gp;
                particles.TryGetValue(tvParticles.SelectedNode, out gp);

                if (gp.GetType() == typeof(ParamTypeParticle))
                {
                    ParamTypeParticle ptp = (ParamTypeParticle)gp;
                    Volume v = (Volume)listPrObjects.SelectedItem;
                    ptp.AddVolume(v.GetClone());
                    picbParticles.Refresh();
                }
                else if (gp.GetType() == typeof(ScriptTypeParticle))
                {
                    ScriptTypeParticle stp = (ScriptTypeParticle)gp;
                }
            }
        }

        private void tsmiPartIP_Click(object sender, EventArgs e)
        {
            //if (particles.ContainsKey(tvParticles.SelectedNode))
            //{
            //    GenericParticle gp;
            //    particles.TryGetValue(tvParticles.SelectedNode, out gp);

            //    if (gp.GetType() == typeof(ParamTypeParticle))
            //    {
            //        ParamTypeParticle ptp = (ParamTypeParticle)gp;
            //        InsertPoint ip = new InsertPoint();
            //        ptp.AddInsertPoint(ip);
            //        picbParticles.Refresh();
            //    }
            //    else if (gp.GetType() == typeof(ScriptTypeParticle))
            //    {
            //        ScriptTypeParticle stp = (ScriptTypeParticle)gp;
            //    }
            //}
        }

        private void tsmiModifyPart_Click(object sender, EventArgs e)
        {
            if (listPrParticles.SelectedItem != null)
            {
                if (listPrParticles.SelectedItem.GetType() == typeof(ParamTypeParticle))
                {
                    particles.Clear();
                    tvParticles.TopNode.Nodes.Clear();
                    ParamTypeParticle ptp = (ParamTypeParticle)listPrParticles.SelectedItem;
                    countParticles++;
                    TreeNode particle_treenode = new TreeNode("Particle " + countParticles + " (ParamType)");
                    particles.Add(particle_treenode, ptp);
                    tvParticles.TopNode.Nodes.Add(particle_treenode);
                    ptp.InitTopNode(particle_treenode);
                    picbParticles.Refresh();
                }
                else if (listPrParticles.SelectedItem.GetType() == typeof(ScriptTypeParticle))
                {
                    ScriptTypeParticle stp = (ScriptTypeParticle)listPrParticles.SelectedItem;

                }
            }
        }

        private void trackbParticles_ValueChanged(object sender, EventArgs e)
        {
            picbParticles.Refresh();
        }

        private void picbParticles_Paint(object sender, PaintEventArgs e)
        {
            Drawing.DrawLandmark(e.Graphics, picbParticles.Width, picbParticles.Height);
            DrawInParticleMode(e.Graphics);
        }

        private void picbParticles_MouseClick(object sender, MouseEventArgs e)
        {
            if (GLOBAL_lastObjectUsed.GetType() == typeof(InsertPoint) && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                InsertPoint ip = (InsertPoint)GLOBAL_lastObjectUsed;

                if (ip.TrajectoryStart == null)
                {
                    ip.TrajectoryStart = new JPoint(e.X, e.Y);
                }
                else
                {
                    ip.GetTrajectorySpline().AddBezier(ip.TrajectoryStart.ToPoint(), new Point(e.X, e.Y));
                    ip.TrajectoryStart = new JPoint(e.X, e.Y);
                }

                picbParticles.Refresh();
            }
        }

        private void picbParticles_MouseDown(object sender, MouseEventArgs e)
        {
            if (GLOBAL_lastObjectUsed != null)
            {
                if (GLOBAL_lastObjectUsed.GetType() == typeof(InsertPoint) && e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    InsertPoint ip = (InsertPoint)GLOBAL_lastObjectUsed;
                    PrSelectGeometry(ip, e.X, e.Y, 6, 6);
                }
            }            
        }

        private void picbParticles_MouseUp(object sender, MouseEventArgs e)
        {
            if (GLOBAL_lastObjectUsed != null)
            {
                if (GLOBAL_lastObjectUsed.GetType() == typeof(InsertPoint) && e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    particle_geoPoints = new List<GeometryPoint>();
                    picbParticles.Refresh();
                }
            }            
        }

        private void picbParticles_MouseMove(object sender, MouseEventArgs e)
        {
            if (GLOBAL_lastObjectUsed != null)
            {
                if (GLOBAL_lastObjectUsed.GetType() == typeof(InsertPoint) && e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    PrChangeGeometry(e.X, e.Y);
                }
            }            
        }

        private void tsbPrNewPart_Click(object sender, EventArgs e)
        {

        }

        private void tsbPrOpenPart_Click(object sender, EventArgs e)
        {

        }

        private void tsbPrSavePart_Click(object sender, EventArgs e)
        {
            DialogResult dr = sfdPart.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                List<GenericParticle> gps = new List<GenericParticle>(particles.Values);

                IO.ParticlesToXML(gps, sfdPart.FileName);
            }
        }

        #endregion

        #region Creation mode

        //===============================================================================
        //EN : Variables of Creation mode
        //FR : Variables du mode Création
        //===============================================================================
        private Dictionary<TreeNode, Volume> volumes = new Dictionary<TreeNode, Volume>();
        private Dictionary<TreeNode, Event> events_of_volumes = new Dictionary<TreeNode, Event>();
        private int countVolumes = 0;        
        private List<GeometryPoint> volume_geoPoints = new List<GeometryPoint>();
        private int volume_lineMode = 0;
        private int volume_withstruct = 0;
        private int volume_change = 0;
        private JPoint volume_point = null;

        //===============================================================================
        //EN : Functions of Creation mode
        //FR : Fonctions du mode Création
        //===============================================================================
        #region Drawing of Creation Mode

        

        /** Sélectionne les géométries qui sont proche du point. */
        private void SelectGeometry(CreationObject co, int x, int y, int width, int height)
        {
            int minX = x - width / 2; int maxX = x + width / 2;
            int minY = y - height / 2; int maxY = y + height / 2;
            foreach (GeometryObject go in co.Array)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    if (l.Start.X >= minX && l.Start.X <= maxX && l.Start.Y >= minY && l.Start.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Start;
                        volume_geoPoints.Add(geoPoint);
                    }
                    if (l.Stop.X >= minX && l.Stop.X <= maxX && l.Stop.Y >= minY && l.Stop.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Stop;
                        volume_geoPoints.Add(geoPoint);
                    }
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    if (b.Start.X >= minX && b.Start.X <= maxX && b.Start.Y >= minY && b.Start.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Start;
                        volume_geoPoints.Add(geoPoint);
                    }
                    if (b.Stop.X >= minX && b.Stop.X <= maxX && b.Stop.Y >= minY && b.Stop.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.Stop;
                        volume_geoPoints.Add(geoPoint);
                    }
                    if (b.CP1.X >= minX && b.CP1.X <= maxX && b.CP1.Y >= minY && b.CP1.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.CP1;
                        volume_geoPoints.Add(geoPoint);
                    }
                    if (b.CP2.X >= minX && b.CP2.X <= maxX && b.CP2.Y >= minY && b.CP2.Y <= maxY)
                    {
                        GeometryPoint geoPoint = new GeometryPoint();
                        geoPoint.Init();
                        geoPoint.SelectedObject = go;
                        geoPoint.SelectedPoint = GeometryPoint.PointType.CP2;
                        volume_geoPoints.Add(geoPoint);
                    }
                }
            }
            picbCreation.Refresh();
        }

        /** Change les géomtries des formes sélectionnées avec un SelectGeometry. */
        private void ChangeGeometry(int x, int y)
        {
            foreach (GeometryPoint geoPoint in volume_geoPoints)
            {
                if (geoPoint.SelectedObject.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)geoPoint.SelectedObject;
                    if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                    {
                        l.Start = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                    {
                        l.Stop = new Point(x, y);
                    }
                }
                else if (geoPoint.SelectedObject.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)geoPoint.SelectedObject;
                    if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                    {
                        b.Start = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                    {
                        b.Stop = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP1)
                    {
                        b.CP1 = new Point(x, y);
                    }
                    else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP2)
                    {
                        b.CP2 = new Point(x, y);
                    }
                }
            }
            picbCreation.Refresh();
        }

        private void DrawSomething(Graphics g)
        {
            if (tvCreation.TopNode.Nodes.Count > 0)
            {
                foreach (TreeNode tn in tvCreation.TopNode.Nodes)
                {
                    if (volumes.ContainsKey(tn))
                    {
                        Volume v = (Volume)volumes[tn];

                        Drawing.DrawVolume(g, v, trackbCreation.Value);
                                                
                    }                    
                }                
            }
        }

        #endregion

        private void InitOfCreationMode()
        {
            GLOBAL_lineModeColor = tsbCrLine.BackColor;
            tsbCrLine.BackColor = Color.Yellow;
            tsbCrWIthStruct.BackColor = Color.Yellow;
            tsbCrDoNothing.BackColor = Color.Yellow;
        }

        private void displayObjects()
        {
            if (volumes.ContainsKey(tvCreation.SelectedNode))
            {
                Volume v = new Volume();
                volumes.TryGetValue(tvCreation.SelectedNode, out v);

                listCrObj.Items.Clear();
                foreach (CreationObject il in v.Objects)
                {
                    listCrObj.Items.Add(il);
                }
            }
        }

        

        //===============================================================================
        //EN : Actions of context menu in Volumes treeview
        //FR : Actions du menu contextuel dans le treeview des Volumes
        //===============================================================================
        private void tsmiAddVolume_Click(object sender, EventArgs e)
        {
            countVolumes++;
            Volume v = new Volume();
            v.Configure(trackbCreation.Minimum.ToString(), trackbCreation.Maximum.ToString(), countVolumes);
            TreeNode topnode = tvCreation.TopNode;
            TreeNode a_node = v.GetTreeNode(volumes, events_of_volumes);
            topnode.Nodes.Add(a_node);
        }

        private void tsmiDeleteVolume_Click(object sender, EventArgs e)
        {

        }

        private void tsmiAddVolumeInstants_Click(object sender, EventArgs e)
        {

        }

        private void tsmiDeleteVolumeInstants_Click(object sender, EventArgs e)
        {

        }

        //===============================================================================
        //EN : Actions of context menu in Objects list
        //FR : Actions du menu contextuel dans la liste des objets
        //===============================================================================
        private void tsmiAddVolObjects_Click(object sender, EventArgs e)
        {
            if (volumes.ContainsKey(tvCreation.SelectedNode))
            {
                Volume v = new Volume();
                volumes.TryGetValue(tvCreation.SelectedNode, out v);

                CreationObject co = new CreationObject();
                v.Objects.Add(co);
                displayObjects();
            }
        }

        //===============================================================================
        //EN : After a selection on the treview, we refresh the property grid with parameters of a volume
        //FR : Après une sélection sur le treview, on rafraichit la grille de propriétés avec les paramètres d'un volume
        //===============================================================================
        private void tvCreation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (volumes.ContainsKey(e.Node))
                {
                    propertyGridEx1.Item.Clear();
                    GLOBAL_lastObjectUsed = volumes[e.Node];
                    GLOBAL_lastObjectUsedBy = "Creation";
                    foreach (Parameter p in volumes[e.Node].Parameters.GetValues())
                    {
                        propertyGridEx1.Item.Add(p.Name, p.Object, false, p.Category, p.Summary, true);
                    }
                    propertyGridEx1.Refresh();

                    displayObjects();
                }
                else if (events_of_volumes.ContainsKey(e.Node))
                {
                    propertyGridEx1.Item.Clear();
                    GLOBAL_lastObjectUsed = events_of_volumes[e.Node];
                    GLOBAL_lastObjectUsedBy = "Creation";
                    foreach (Parameter p in events_of_volumes[e.Node].Parameters.GetValues())
                    {
                        propertyGridEx1.Item.Add(p.Name, p.Object, false, p.Category, p.Summary, true);
                    }
                    propertyGridEx1.Refresh();
                }
            }
        }

        //===============================================================================
        //EN : Duration of the action of the volume
        //FR : Durée de la phase d'action du volume
        //===============================================================================
        private void txtDuration_TextChanged(object sender, EventArgs e)
        {
            // Si on change le texte des secondes ou millisecondes
            try
            {
                int nombre = Convert.ToInt32(txtDuration.Text);
                nombre = nombre > 0 ? nombre : 1;
                if (rbSeconds.Checked == true)
                {
                    //Secondes affichées en millisecondes
                    trackbCreation.Maximum = nombre * 1000;
                }
                else
                {
                    //Millisecondes
                    trackbCreation.Maximum = nombre;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //===============================================================================
        //EN : Activation of mode of the button of the toolbar
        //FR : Activation des modes sur les boutons de la barre d'outils
        //===============================================================================
        private void tsbCrLine_Click(object sender, EventArgs e)
        {
            volume_lineMode = 0;
            tsbCrLine.BackColor = Color.Yellow;
            tsbCrBezier.BackColor = GLOBAL_lineModeColor;
        }

        private void tsbCrBezier_Click(object sender, EventArgs e)
        {
            volume_lineMode = 1;
            tsbCrLine.BackColor = GLOBAL_lineModeColor;
            tsbCrBezier.BackColor = Color.Yellow;
        }

        private void tsbCrWIthStruct_Click(object sender, EventArgs e)
        {
            volume_withstruct = 0;
            tsbCrWIthStruct.BackColor = Color.Yellow;
            tsbCrWithoutStruct.BackColor = GLOBAL_lineModeColor;
            picbCreation.Refresh();
        }

        private void tsbCrWithoutStruct_Click(object sender, EventArgs e)
        {
            volume_withstruct = 1;
            tsbCrWIthStruct.BackColor = GLOBAL_lineModeColor;
            tsbCrWithoutStruct.BackColor = Color.Yellow;
            picbCreation.Refresh();
        }

        private void tsbCrDoNothing_Click(object sender, EventArgs e)
        {
            volume_change = 0;
            tsbCrTranslate.BackColor = GLOBAL_lineModeColor;
            tsbCrRotate.BackColor = GLOBAL_lineModeColor;
            tsbCrResize.BackColor = GLOBAL_lineModeColor;
            tsbCrShear.BackColor = GLOBAL_lineModeColor;
            tsbCrSelection.BackColor = GLOBAL_lineModeColor;
            tsbCrDoNothing.BackColor = Color.Yellow;
        }

        private void tsbCrTranslate_Click(object sender, EventArgs e)
        {
            volume_change = 1;
            tsbCrTranslate.BackColor = Color.Yellow;
            tsbCrRotate.BackColor = GLOBAL_lineModeColor;
            tsbCrResize.BackColor = GLOBAL_lineModeColor;
            tsbCrShear.BackColor = GLOBAL_lineModeColor;
            tsbCrSelection.BackColor = GLOBAL_lineModeColor;
            tsbCrDoNothing.BackColor = GLOBAL_lineModeColor;
        }

        private void tsbCrRotate_Click(object sender, EventArgs e)
        {
            volume_change = 2;
            tsbCrTranslate.BackColor = GLOBAL_lineModeColor;
            tsbCrRotate.BackColor = Color.Yellow;
            tsbCrResize.BackColor = GLOBAL_lineModeColor;
            tsbCrShear.BackColor = GLOBAL_lineModeColor;
            tsbCrSelection.BackColor = GLOBAL_lineModeColor;
            tsbCrDoNothing.BackColor = GLOBAL_lineModeColor;
        }

        private void tsbCrResize_Click(object sender, EventArgs e)
        {
            volume_change = 3;
            tsbCrTranslate.BackColor = GLOBAL_lineModeColor;
            tsbCrRotate.BackColor = GLOBAL_lineModeColor;
            tsbCrResize.BackColor = Color.Yellow;
            tsbCrShear.BackColor = GLOBAL_lineModeColor;
            tsbCrSelection.BackColor = GLOBAL_lineModeColor;
            tsbCrDoNothing.BackColor = GLOBAL_lineModeColor;
        }

        private void tsbCrShear_Click(object sender, EventArgs e)
        {
            volume_change = 4;
            tsbCrTranslate.BackColor = GLOBAL_lineModeColor;
            tsbCrRotate.BackColor = GLOBAL_lineModeColor;
            tsbCrResize.BackColor = GLOBAL_lineModeColor;
            tsbCrShear.BackColor = Color.Yellow;
            tsbCrSelection.BackColor = GLOBAL_lineModeColor;
            tsbCrDoNothing.BackColor = GLOBAL_lineModeColor;
        }

        private void tsbCrSelection_Click(object sender, EventArgs e)
        {
            volume_change = 5;
            tsbCrTranslate.BackColor = GLOBAL_lineModeColor;
            tsbCrRotate.BackColor = GLOBAL_lineModeColor;
            tsbCrResize.BackColor = GLOBAL_lineModeColor;
            tsbCrShear.BackColor = GLOBAL_lineModeColor;
            tsbCrSelection.BackColor = Color.Yellow;
            tsbCrDoNothing.BackColor = GLOBAL_lineModeColor;
        }

        //===============================================================================
        //EN : Action on the graphic component
        //FR : Action sur le composant graphique
        //===============================================================================
        private void picbCreation_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && listCrObj.SelectedIndex > -1)
            {
                CreationObject co = (CreationObject)listCrObj.SelectedItem;

                if (co.Coordinates.X == -1)
                {
                    co.Coordinates = new Point(e.X, e.Y);
                }
                if (volume_lineMode == 0)
                {
                    LineObject l = new LineObject();
                    l.Start = co.Coordinates;
                    l.Stop = new Point(e.X, e.Y);
                    co.Array.Add(l);
                }
                else
                {
                    BezierObject b = new BezierObject();
                    b.Start = co.Coordinates;
                    int xdiff = e.X - co.Coordinates.X;
                    int ydiff = e.Y - co.Coordinates.Y;
                    int x1_3 = co.Coordinates.X + xdiff / 3;
                    int x2_3 = co.Coordinates.X + xdiff * 2 / 3;
                    int y1_3 = co.Coordinates.Y + ydiff / 3;
                    int y2_3 = co.Coordinates.Y + ydiff * 2 / 3;
                    b.CP1 = new Point(x1_3, y1_3);
                    b.CP2 = new Point(x2_3, y2_3);
                    b.Stop = new Point(e.X, e.Y);
                    co.Array.Add(b);
                }
                co.Coordinates = new Point(e.X, e.Y);
                picbCreation.Refresh();
            }
        }

        private void picbCreation_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && listCrObj.SelectedIndex > -1 && volume_change == 0)
            {
                CreationObject co = (CreationObject)listCrObj.SelectedItem;
                SelectGeometry(co, e.X, e.Y, 6, 6);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 1)
            {
                volume_point = new JPoint(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 2)
            {
                volume_point = new JPoint(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 3)
            {
                volume_point = new JPoint(e.X, e.Y);
            }
        }

        private void picbCreation_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 0)
            {
                volume_geoPoints = new List<GeometryPoint>();
                picbCreation.Refresh();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 1)
            {
                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        Translation t = new Translation(volume_point);
                        t.setDistance(e.X, e.Y);
                        t.SetTranslation(cro.Array);
                    }                    
                }
                volume_point = null;
                picbCreation.Refresh();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 2)
            {
                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        Rotation r = new Rotation(volume_point);
                        r.setRotation(e.X, e.Y);
                        r.SetRotation(cro.Array, 0d);
                    }
                }
                volume_point = null;
                picbCreation.Refresh();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 3)
            {
                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        Resize r = new Resize(volume_point);
                        r.setDistance(e.X, e.Y);
                        r.SetResize(cro.Array, 0d);
                    }
                }
                volume_point = null;
                picbCreation.Refresh();
            }
        }

        private void picbCreation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 0)
            {
                ChangeGeometry(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 1 && volume_point != null)
            {
                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        Translation t = new Translation(volume_point);
                        t.setDistance(e.X, e.Y);
                        GLOBAL_GP = t.GetPreview(cro.Array);
                    }
                    picbCreation.Refresh();
                }

            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 2 && volume_point != null)
            {
                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        Rotation r = new Rotation(volume_point);
                        r.setRotation(e.X, e.Y);
                        GLOBAL_GP = r.GetPreview(cro.Array);
                    }
                    picbCreation.Refresh();
                }

            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle && volume_change == 3 && volume_point != null)
            {
                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        Resize r = new Resize(volume_point);
                        r.setDistance(e.X, e.Y);
                        GLOBAL_GP = r.GetPreview(cro.Array);
                    }
                    picbCreation.Refresh();
                }

            }
        }

        //===============================================================================
        //EN : Redraw of the graphic component
        //FR : Regénération du dessin sur le composant graphique
        //===============================================================================
        private void picbCreation_Paint(object sender, PaintEventArgs e)
        {
            Drawing.DrawLandmark(e.Graphics, picbCreation.Width, picbCreation.Height);

            if (volume_withstruct == 0)
            {
                if (tvCreation.SelectedNode != null)
                {
                    if (volumes.ContainsKey(tvCreation.SelectedNode))
                    {
                        Volume v = new Volume();
                        volumes.TryGetValue(tvCreation.SelectedNode, out v);

                        Drawing.DrawVolumeStructure(e.Graphics, v, trackbCreation.Value, volume_geoPoints);
                    }
                }
            }
            else
            {
                DrawSomething(e.Graphics);
            }

            if (volume_change == 1 | volume_change == 2 | volume_change == 3)
            {
                if (GLOBAL_GP != null)
                {
                    e.Graphics.DrawPath(new Pen(Brushes.Purple, 1), GLOBAL_GP);
                    GLOBAL_GP = null;
                }
            }
        }

        //===============================================================================
        //EN : Fill with green color when selected or white when it's not selected (structure mode)
        //FR : Dessin en vert du volume sélectionné et en blanc des non sélectionnés en mode structure
        //===============================================================================        
        private void listCrObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listCrObj.Items.Count > 0)
            {
                CreationObject co = (CreationObject)listCrObj.SelectedItem;

                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        cro.Selected = false;
                        if (co == cro)
                        {
                            cro.Selected = true;
                        }
                    }

                    picbCreation.Refresh();
                }
            }
        }

        //===============================================================================
        //EN : Action on trackbar and redraw
        //FR : Action sur la barre de sélection et regénération du dessin
        //===============================================================================
        private void trackbCreation_Scroll(object sender, EventArgs e)
        {
            picbCreation.Refresh();
        }

        //===============================================================================
        //EN : Load and save the volumes
        //FR : Ouverture et sauvegarde de volumes
        //===============================================================================
        private void tsbCrLoad_Click(object sender, EventArgs e)
        {
            DialogResult dr = ofdCreation.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                List<Volume> vols = IO.XMLToVolumes(ofdCreation.FileName);
                Volume.CleanAndAddTreeNodes(volumes, events_of_volumes, vols, tvCreation.TopNode);
            }
        }

        private void tsbCrSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = sfdCreation.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                List<Volume> vols = new List<Volume>();

                foreach(Volume v in volumes.Values){
                    vols.Add(v);
                    //TODO gérer nom identique dans VolumesParticlesList (update)
                    listPrObjects.Items.Add(v);
                }

                IO.VolumesToXML(vols, sfdCreation.FileName);
            }
        }

        //===============================================================================
        //EN : Volumes operations (see Selection class)
        //FR : Opérations sur les volumes (voir la classe Selection)
        //===============================================================================
        private void tsmiCrCopy_Click(object sender, EventArgs e)
        {
            if (listCrObj.Items.Count > 0)
            {
                CreationObject co = (CreationObject)listCrObj.SelectedItem;

                if (volumes.ContainsKey(tvCreation.SelectedNode))
                {
                    Volume v = new Volume();
                    volumes.TryGetValue(tvCreation.SelectedNode, out v);

                    foreach (CreationObject cro in v.Objects)
                    {
                        Selection s = new Selection();
                        s.SetOriginalList(cro.Array);
                        s.AddSelection(cro.Array);
                        s.Operation_Copy();
                    }

                    picbCreation.Refresh();
                }
            }
        }

        #endregion

        #region GLOBAL Functions

        public Bitmap ReadFrameBitmap(AviSynthClip asc, int position)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(asc.VideoWidth, asc.VideoHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                // Lock the bitmap's bits.
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                try
                {
                    // Get the address of the first line.
                    IntPtr ptr = bmpData.Scan0;
                    // Read data
                    asc.ReadFrame(ptr, bmpData.Stride, position);
                }
                finally
                {
                    // Unlock the bits.
                    bmp.UnlockBits(bmpData);
                }
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                return bmp;
            }
            catch (Exception)
            {
                if (bmp != null) bmp.Dispose();
                throw;
            }
        }

        #endregion

        private void tabcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabcMain.SelectedIndex == 0)
            {
                //Video mode
                tvCreation.Visible = false;
                tvParticles.Visible = false;
                tvVideo.Visible = true;
            }
            else if (tabcMain.SelectedIndex == 1)
            {
                //Particles mode
                tvCreation.Visible = false;                
                tvVideo.Visible = false;
                tvParticles.Visible = true;
            }
            else if (tabcMain.SelectedIndex == 2)
            {
                //Creation mode                
                tvVideo.Visible = false;
                tvParticles.Visible = false;
                tvCreation.Visible = true;
            }
        }

        private void propertyGridEx1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyGridEx.PropertyGridEx pge = (PropertyGridEx.PropertyGridEx)s;
            if (GLOBAL_lastObjectUsed.GetType() == typeof(Volume) && GLOBAL_lastObjectUsedBy == "Creation")
            {
                Volume v = (Volume)GLOBAL_lastObjectUsed;
                foreach (Parameter p in v.Parameters.GetValues())
                {
                    if (p.Name.Equals(pge.SelectedGridItem.Label) == true)
                    {
                        p.Object = pge.SelectedGridItem.Value;
                    }
                }
                picbCreation.Refresh();
            }
            else if (GLOBAL_lastObjectUsed.GetType() == typeof(Event) && GLOBAL_lastObjectUsedBy == "Creation")
            {
                Event ev = (Event)GLOBAL_lastObjectUsed;
                foreach (Parameter p in ev.Parameters.GetValues())
                {
                    if (p.Name.Equals(pge.SelectedGridItem.Label) == true)
                    {
                        p.Object = pge.SelectedGridItem.Value;
                    }
                }
                picbCreation.Refresh();
            }
            else if (GLOBAL_lastObjectUsed.GetType() == typeof(Volume) && GLOBAL_lastObjectUsedBy == "Particle")
            {
                Volume v = (Volume)GLOBAL_lastObjectUsed;
                foreach (Parameter p in v.Parameters.GetValues())
                {
                    if (p.Name.Equals(pge.SelectedGridItem.Label) == true)
                    {
                        p.Object = pge.SelectedGridItem.Value;
                    }
                }
                picbParticles.Refresh();
            }
            else if (GLOBAL_lastObjectUsed.GetType() == typeof(Event) && GLOBAL_lastObjectUsedBy == "Particle")
            {
                Event ev = (Event)GLOBAL_lastObjectUsed;
                foreach (Parameter p in ev.Parameters.GetValues())
                {
                    if (p.Name.Equals(pge.SelectedGridItem.Label) == true)
                    {
                        p.Object = pge.SelectedGridItem.Value;
                    }
                }
                picbParticles.Refresh();
            }
            else if (GLOBAL_lastObjectUsed.GetType() == typeof(InsertPoint) && GLOBAL_lastObjectUsedBy == "Particle")
            {
                InsertPoint ip = (InsertPoint)GLOBAL_lastObjectUsed;
                foreach (Parameter p in ip.Parameters.GetValues())
                {
                    if (p.Name.Equals(pge.SelectedGridItem.Label) == true)
                    {
                        p.Object = pge.SelectedGridItem.Value;
                    }
                }
                picbParticles.Refresh();
            }
            
        }

        private void tsbAssOpen_Click(object sender, EventArgs e)
        {
            DialogResult dr = ofdAss.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                List<string> rawLines = IO.GetRawLinesFromAss(ofdAss.FileName);

                video_fxs = new FXSubtitle();
                TreeNode t = video_fxs.GetMappedTreeNodeForASS(rawLines, new Font("Arial", 40f), 640, 50);
                tvVideo.TopNode.Nodes.Add(t);
            }
        }

        private void tsbAvsOpen_Click(object sender, EventArgs e)
        {
            DialogResult dr = ofdAvs.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                GLOBAL_script = ofdAvs.FileName;
                AviSynthScriptEnvironment asse = new AviSynthScriptEnvironment();
                GLOBAL_Clip = asse.OpenScriptFile(GLOBAL_script);
                GLOBAL_bitmap = ReadFrameBitmap(GLOBAL_Clip, 0);
                trackbVideo.Maximum = GLOBAL_Clip.num_frames;
                video_fpm = (Convert.ToDouble(GLOBAL_Clip.raten) / Convert.ToDouble(GLOBAL_Clip.rated)) / 1000d;
                Drawing.FPM = video_fpm;
                picbVideo.Refresh();
            }
        }

        

        

        

        

        

        


        


    }
}
