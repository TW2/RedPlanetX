using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public class CreationObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; } // Peut être interverti avec le ratio
        public double Angle_X { get; set; }
        public double Angle_Y { get; set; }
        public double Angle_Z { get; set; }
        public double Ratio { get; set; } // Peut être interverti avec Z
        public double Quake_X { get; set; } // Il s'agit de tremblement dont on peut définir la variation et qui ont lieu chaque millisecondes
        public double Quake_Y { get; set; } // Il s'agit de tremblement dont on peut définir la variation et qui ont lieu chaque millisecondes
        public double Quake_Z { get; set; } // Il s'agit de tremblement dont on peut définir la variation et qui ont lieu chaque millisecondes
        public int Thickness { get; set; } // Définit un paramètre émulant la 3D lors de rotation
        public int Border { get; set; }
        public int Shadow { get; set; }
        public Color FrontColor { get; set; } // Définit la couleur de face
        public Color BackColor { get; set; } // Définit la couleur de derrière
        public Color ThicknessColor { get; set; } // Définit la couleur de l'épaisseur
        public Color BorderColor { get; set; }
        public Color ShadowColor { get; set; }
        public Rainbow FrontRainbow { get; set; } // Définit la couleur de face
        public Rainbow BackRainbow { get; set; } // Définit la couleur de derrière
        public Rainbow ThicknessRainbow { get; set; } // Définit la couleur de l'épaisseur
        public Rainbow BorderRainbow { get; set; }
        public Rainbow ShadowRainbow { get; set; }
        public bool UseFrontRainbow = false;
        public bool UseBackRainbow = false;
        public bool UseThicknessRainbow = false;
        public bool UseBorderRainbow = false;
        public bool UseShadowRainbow = false;

        public List<GeometryObject> Array = new List<GeometryObject>();
        public Point Coordinates = new Point(-1, -1);
        public bool Selected = false;

        public void init()
        {
            X = 0;
            Y = 0;
            Z = 0;
            Angle_X = 0d;
            Angle_Y = 0d;
            Angle_Z = 0d;
            Ratio = 1d;
            Quake_X = 0d;
            Quake_Y = 0d;
            Quake_Z = 0d;
            Thickness = 0;
            Border = 1;
            Shadow = 0;
            FrontColor = Color.Red;
            BackColor = Color.Blue;
            ThicknessColor = Color.Yellow;
            BorderColor = Color.Green;
            ShadowColor = Color.Black;
        }
    }
}
