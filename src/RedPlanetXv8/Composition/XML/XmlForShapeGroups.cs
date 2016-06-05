using RedPlanetXv8.Composition.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition.XML
{
    public class XmlForShapeGroups
    {
        public XmlForShapeGroups()
        {

        }

        public static void Serialize(List<Group> gs, string filename)
        {
            Generic gen = new Generic();
            string doc = "";
            Dictionary<string, string> coordinates = new Dictionary<string, string>();

            //On commence par un groupe de groupes
            doc += gen.GetTag_ON("Groups") + Environment.NewLine;

            foreach (Group g in gs)
            {
                //On continue avec un groupe
                doc += gen.GetTag_ON("Group") + Environment.NewLine;

                foreach (IGraphicObject go in g.GetGroup())
                {
                    if (go.GetType() == typeof(Line))
                    {
                        Line l = (Line)go;

                        //On continue avec une ligne
                        doc += gen.GetTag_ON("Line") + Environment.NewLine;

                        coordinates.Clear();
                        coordinates.Add("X", Convert.ToString(l.Start.X));
                        coordinates.Add("Y", Convert.ToString(l.Start.Y));
                        //On écrit la ligne qui concerne le paramètre Start
                        doc += gen.GetTag_VOID("Start", coordinates) + Environment.NewLine;

                        coordinates.Clear();
                        coordinates.Add("X", Convert.ToString(l.End.X));
                        coordinates.Add("Y", Convert.ToString(l.End.Y));
                        //On écrit la ligne qui concerne le paramètre End
                        doc += gen.GetTag_VOID("End", coordinates) + Environment.NewLine;

                        //On ferme la ligne en cours
                        doc += gen.GetTag_OFF("Line") + Environment.NewLine;
                    }

                    if (go.GetType() == typeof(Curve))
                    {
                        Curve c = (Curve)go;

                        //On continue avec une courbe
                        doc += gen.GetTag_ON("Curve") + Environment.NewLine;

                        coordinates.Clear();
                        coordinates.Add("X", Convert.ToString(c.Start.X));
                        coordinates.Add("Y", Convert.ToString(c.Start.Y));
                        //On écrit la ligne qui concerne le paramètre Start
                        doc += gen.GetTag_VOID("Start", coordinates) + Environment.NewLine;

                        coordinates.Clear();
                        coordinates.Add("X", Convert.ToString(c.CP1.X));
                        coordinates.Add("Y", Convert.ToString(c.CP1.Y));
                        //On écrit la ligne qui concerne le paramètre CP1
                        doc += gen.GetTag_VOID("CP1", coordinates) + Environment.NewLine;

                        coordinates.Clear();
                        coordinates.Add("X", Convert.ToString(c.CP2.X));
                        coordinates.Add("Y", Convert.ToString(c.CP2.Y));
                        //On écrit la ligne qui concerne le paramètre CP2
                        doc += gen.GetTag_VOID("CP2", coordinates) + Environment.NewLine;

                        coordinates.Clear();
                        coordinates.Add("X", Convert.ToString(c.End.X));
                        coordinates.Add("Y", Convert.ToString(c.End.Y));
                        //On écrit la ligne qui concerne le paramètre End
                        doc += gen.GetTag_VOID("End", coordinates) + Environment.NewLine;

                        //On ferme la courbe en cours
                        doc += gen.GetTag_OFF("Curve") + Environment.NewLine;
                    }
                }

                //On ferme le groupe en cours
                doc += gen.GetTag_OFF("Group") + Environment.NewLine;
            }

            //On ferme le groupe de groupes en cours
            doc += gen.GetTag_OFF("Groups") + Environment.NewLine;

            System.IO.File.WriteAllText(filename, doc);
        }

        public static List<Group> Deserialize(string filename)
        {
            List<Group> gs = new List<Group>();
            Group group = new Group();
            Line _line = new Line();
            Curve _curve = new Curve();

            string doc = System.IO.File.ReadAllText(filename);

            Generic gen = new Generic();
            Dictionary<string, string> coordinates = new Dictionary<string, string>();

            string line = "", expr = "", oldexpr = "", coor = "";
            StringReader sr = new StringReader(doc);
            while ((line = sr.ReadLine()) != null)
            {
                //1 - Groups ON (List<Group> gs)
                //2 - Group ON (Group group)
                //3 - Line ON (Line _line) ou Curve ON (Curve _curve)
                expr = gen.FromON(line, out coordinates);
                if (expr == "Group")
                {
                    group = new Group();
                    gs.Add(group);
                }
                else if (expr == "Line")
                {
                    _line = new Line();
                    group.AddLine(_line);
                    oldexpr = "Line";
                }
                else if (expr == "Curve")
                {
                    _curve = new Curve();
                    group.AddCurve(_curve);
                    oldexpr = "Curve";
                }

                //4 - Coordinates
                coor = gen.FromVOID(line, out coordinates);
                if (coor == "Start" && oldexpr == "Line")
                {
                    _line.Start = new Point(Convert.ToInt32(coordinates["X"]), Convert.ToInt32(coordinates["Y"]));
                }
                else if (coor == "End" && oldexpr == "Line")
                {
                    _line.End = new Point(Convert.ToInt32(coordinates["X"]), Convert.ToInt32(coordinates["Y"]));
                }
                else if (coor == "Start" && oldexpr == "Curve")
                {
                    _curve.Start = new Point(Convert.ToInt32(coordinates["X"]), Convert.ToInt32(coordinates["Y"]));
                }
                else if (coor == "CP1" && oldexpr == "Curve")
                {
                    _curve.CP1 = new Point(Convert.ToInt32(coordinates["X"]), Convert.ToInt32(coordinates["Y"]));
                }
                else if (coor == "CP2" && oldexpr == "Curve")
                {
                    _curve.CP2 = new Point(Convert.ToInt32(coordinates["X"]), Convert.ToInt32(coordinates["Y"]));
                }
                else if (coor == "End" && oldexpr == "Curve")
                {
                    _curve.End = new Point(Convert.ToInt32(coordinates["X"]), Convert.ToInt32(coordinates["Y"]));
                }
            }

            return gs;
        }
    }
}
