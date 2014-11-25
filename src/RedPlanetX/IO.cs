using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RedPlanetX
{
    class IO
    {      

        public static void CreateTextFile(string towrite, string path)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                file.Write(towrite);
            }
        }

        public static void VolumesToXML(List<Volume> volumes, string path)
        {
            XmlTextWriter textWriter = new XmlTextWriter(path, null);

            //Start of XML Document
            textWriter.WriteStartDocument();

            //Start -> VolumeCollection
            textWriter.WriteStartElement("VolumeCollection");

            foreach (Volume v in volumes)
            {
                //Start -> Volume
                textWriter.WriteStartElement("Volume");

                //Block -> Name
                textWriter.WriteStartElement("Name");
                textWriter.WriteString(v.Name);
                textWriter.WriteEndElement();

                //Start -> Parameters
                textWriter.WriteStartElement("Parameters");

                foreach (Parameter p in v.Parameters.GetValues())
                {
                    //Start -> Parameter
                    textWriter.WriteStartElement("Parameter");

                    //Block -> Name
                    textWriter.WriteStartElement("Name");
                    textWriter.WriteString(p.Name);
                    textWriter.WriteEndElement();

                    //Block -> Category
                    textWriter.WriteStartElement("Category");
                    textWriter.WriteString(p.Category);
                    textWriter.WriteEndElement();

                    //Block -> Summary
                    textWriter.WriteStartElement("Summary");
                    textWriter.WriteString(p.Summary);
                    textWriter.WriteEndElement();

                    //Block -> Object
                    textWriter.WriteStartElement("Object");
                    textWriter.WriteString(p.Object.ToString());
                    textWriter.WriteEndElement();

                    //End -> Parameter
                    textWriter.WriteEndElement();
                }

                //End -> Parameters
                textWriter.WriteEndElement();

                //Start -> Events
                textWriter.WriteStartElement("Events");

                foreach (Event ev in v.Events.ToList())
                {
                    //Start -> Event
                    textWriter.WriteStartElement("Event");

                    //Block -> Name
                    textWriter.WriteStartElement("Name");
                    textWriter.WriteString(ev.Name);
                    textWriter.WriteEndElement();

                    //Start -> Parameters
                    textWriter.WriteStartElement("Parameters");

                    foreach (Parameter p in ev.Parameters.GetValues())
                    {
                        //Start -> Parameter
                        textWriter.WriteStartElement("Parameter");

                        //Block -> Name
                        textWriter.WriteStartElement("Name");
                        textWriter.WriteString(p.Name);
                        textWriter.WriteEndElement();

                        //Block -> Category
                        textWriter.WriteStartElement("Category");
                        textWriter.WriteString(p.Category);
                        textWriter.WriteEndElement();

                        //Block -> Summary
                        textWriter.WriteStartElement("Summary");
                        textWriter.WriteString(p.Summary);
                        textWriter.WriteEndElement();

                        //Block -> Object
                        textWriter.WriteStartElement("Object");
                        textWriter.WriteString(p.Object.ToString());
                        textWriter.WriteEndElement();

                        //End -> Parameter
                        textWriter.WriteEndElement();
                    }

                    //End -> Parameters
                    textWriter.WriteEndElement();

                    //End -> Event
                    textWriter.WriteEndElement();
                }

                //End -> Events
                textWriter.WriteEndElement();

                //Start -> CreationObject list
                textWriter.WriteStartElement("CreationObjects");

                foreach (CreationObject cro in v.Objects)
                {
                    //Start -> CreationObject
                    textWriter.WriteStartElement("CreationObject");

                    foreach (GeometryObject go in cro.Array)
                    {
                        if (go.GetType() == typeof(LineObject))
                        {
                            LineObject l = (LineObject)go;

                            //Start -> LineObject
                            textWriter.WriteStartElement("LineObject");

                            //Block -> Start.X
                            textWriter.WriteStartElement("Start.X");
                            textWriter.WriteString(l.Start.X.ToString());
                            textWriter.WriteEndElement();

                            //Block -> Start.Y
                            textWriter.WriteStartElement("Start.Y");
                            textWriter.WriteString(l.Start.Y.ToString());
                            textWriter.WriteEndElement();

                            //Block -> Stop.X
                            textWriter.WriteStartElement("Stop.X");
                            textWriter.WriteString(l.Stop.X.ToString());
                            textWriter.WriteEndElement();

                            //Block -> Stop.Y
                            textWriter.WriteStartElement("Stop.Y");
                            textWriter.WriteString(l.Stop.Y.ToString());
                            textWriter.WriteEndElement();

                            //End -> LineObject
                            textWriter.WriteEndElement();
                        }
                        else if (go.GetType() == typeof(BezierObject))
                        {
                            BezierObject b = (BezierObject)go;

                            //Start -> BezierObject
                            textWriter.WriteStartElement("BezierObject");

                            //Block -> Start.X
                            textWriter.WriteStartElement("Start.X");
                            textWriter.WriteString(b.Start.X.ToString());
                            textWriter.WriteEndElement();

                            //Block -> Start.Y
                            textWriter.WriteStartElement("Start.Y");
                            textWriter.WriteString(b.Start.Y.ToString());
                            textWriter.WriteEndElement();

                            //Block -> CP1.X
                            textWriter.WriteStartElement("CP1.X");
                            textWriter.WriteString(b.CP1.X.ToString());
                            textWriter.WriteEndElement();

                            //Block -> CP1.Y
                            textWriter.WriteStartElement("CP1.Y");
                            textWriter.WriteString(b.CP1.Y.ToString());
                            textWriter.WriteEndElement();

                            //Block -> CP2.X
                            textWriter.WriteStartElement("CP2.X");
                            textWriter.WriteString(b.CP2.X.ToString());
                            textWriter.WriteEndElement();

                            //Block -> CP2.Y
                            textWriter.WriteStartElement("CP2.Y");
                            textWriter.WriteString(b.CP2.Y.ToString());
                            textWriter.WriteEndElement();

                            //Block -> Stop.X
                            textWriter.WriteStartElement("Stop.X");
                            textWriter.WriteString(b.Stop.X.ToString());
                            textWriter.WriteEndElement();

                            //Block -> Stop.Y
                            textWriter.WriteStartElement("Stop.Y");
                            textWriter.WriteString(b.Stop.Y.ToString());
                            textWriter.WriteEndElement();

                            //End -> BezierObject
                            textWriter.WriteEndElement();
                        }
                    }
                }

                //End -> CreationObject list
                textWriter.WriteEndElement();

                //End -> Volume
                textWriter.WriteEndElement();
            }

            //End -> VolumeCollection
            textWriter.WriteEndElement();

            //End of XML Document
            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        public static List<Volume> XMLToVolumes(string path)
        {
            XmlTextReader textReader = new XmlTextReader(path);
            List<Volume> volumes = new List<Volume>();

            Volume v = null;
            Parameter p = null;
            Parameters pev = null;
            Events evts = null;
            CreationObject cro = null;
            LineObject l = null;
            BezierObject b = null;
            string lastParentElement = "", lastString = "";

            while (textReader.Read())
            {
                if (textReader.IsStartElement())
                {
                    if (textReader.Name == "Volume")
                    {
                        v = new Volume();
                        lastParentElement = "Volume";
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "Volume")
                    {
                        v.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Parameters" && lastParentElement == "Volume")
                    {
                        pev = new Parameters();
                        lastParentElement = "VolumeParameters";
                    }
                    else if (textReader.Name == "Parameter" && lastParentElement == "VolumeParameters")
                    {
                        p = new Parameter();
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "VolumeParameters")
                    {
                        p.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Category" && lastParentElement == "VolumeParameters")
                    {
                        p.Category = textReader.ReadString();
                    }
                    else if (textReader.Name == "Summary" && lastParentElement == "VolumeParameters")
                    {
                        p.Summary = textReader.ReadString();
                    }
                    else if (textReader.Name == "Object" && lastParentElement == "VolumeParameters")
                    {
                        switch (p.Name)
                        {
                            case "Use front rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use back rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use thickness rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use border rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use shadow rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Anchor X": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Anchor Y": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            default: p.Object = textReader.ReadString(); break;
                        }
                        pev.Add(p.Name, p);
                        v.Parameters = pev;
                    }
                    else if (textReader.Name == "Events")
                    {
                        evts = new Events();
                        lastParentElement = "Events";
                    }
                    else if (textReader.Name == "Event")
                    {
                        pev = new Parameters();
                        lastParentElement = "Event";
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "Event")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Parameters" && lastParentElement == "Event")
                    {
                        lastParentElement = "EventParameters";
                    }
                    else if (textReader.Name == "Parameter" && lastParentElement == "EventParameters")
                    {
                        p = new Parameter();
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "EventParameters")
                    {
                        p.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Category" && lastParentElement == "EventParameters")
                    {
                        p.Category = textReader.ReadString();
                    }
                    else if (textReader.Name == "Summary" && lastParentElement == "EventParameters")
                    {
                        p.Summary = textReader.ReadString();
                    }
                    else if (textReader.Name == "Object" && lastParentElement == "EventParameters")
                    {
                        switch (p.Name)
                        {
                            case "Position X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Position Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Position Z": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Scale X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Scale Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle Z": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake Z": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Thickness": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Border": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Shadow": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Front color": p.Object = Color.Red; break;
                            case "Back color": p.Object = Color.Red; break;
                            case "Thickness color": p.Object = Color.Red; break;
                            case "Border color": p.Object = Color.Red; break;
                            case "Shadow color": p.Object = Color.Red; break;
                            default: p.Object = textReader.ReadString(); break;
                        }
                        pev.Add(p.Name, p);                     
                    }
                    else if (textReader.Name == "CreationObjects")
                    {
                        lastParentElement = "CreationObjects";
                    }
                    else if (textReader.Name == "CreationObject")
                    {
                        cro = new CreationObject();
                        lastParentElement = "CreationObject";
                    }
                    else if (textReader.Name == "LineObject")
                    {
                        l = new LineObject();
                        lastParentElement = "LineObject";
                    }
                    else if (textReader.Name == "Start.X" && lastParentElement == "LineObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Start.Y" && lastParentElement == "LineObject")
                    {
                        l.Start = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "Stop.X" && lastParentElement == "LineObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Stop.Y" && lastParentElement == "LineObject")
                    {
                        l.Stop = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                        cro.Array.Add(l);
                    }
                    else if (textReader.Name == "BezierObject")
                    {
                        b = new BezierObject();
                        lastParentElement = "BezierObject";
                    }
                    else if (textReader.Name == "Start.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Start.Y" && lastParentElement == "BezierObject")
                    {
                        b.Start = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "CP1.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "CP1.Y" && lastParentElement == "BezierObject")
                    {
                        b.CP1 = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "CP2.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "CP2.Y" && lastParentElement == "BezierObject")
                    {
                        b.CP2 = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "Stop.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Stop.Y" && lastParentElement == "BezierObject")
                    {
                        b.Stop = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                        cro.Array.Add(b);
                    }
                }
                else if (textReader.Name == "Parameters" && lastParentElement == "EventParameters")
                {
                    evts.Add(lastString, pev);
                    lastParentElement = "Event";
                }
                else if (textReader.Name == "Events")
                {
                    v.Events = evts;
                }
                else if (textReader.Name == "LineObject" | textReader.Name == "BezierObject")
                {
                    lastParentElement = "CreationObject";
                }
                else if (textReader.Name == "CreationObject")
                {
                    v.Objects.Add(cro);
                }
                else if (textReader.Name == "Volume")
                {
                    volumes.Add(v);
                }

            }

            return volumes;
        }

        public static void FillParticleVolumeList(ListBox lb)
        {
            DirectoryInfo diVolumes = new DirectoryInfo(Application.StartupPath + "\\Data");
            foreach (FileInfo fi in diVolumes.GetFiles())
            {
                if (fi.Extension == ".vol")
                {
                    List<Volume> vols = XMLToVolumes(fi.FullName);

                    foreach (Volume v in vols)
                    {
                        lb.Items.Add(v);
                    }
                }                
            }
        }

        public static void FillParticlesList(ListBox lb)
        {
            DirectoryInfo diVolumes = new DirectoryInfo(Application.StartupPath + "\\Data");
            foreach (FileInfo fi in diVolumes.GetFiles())
            {
                if (fi.Extension == ".prt")
                {
                    List<GenericParticle> gps = XMLToParticles(fi.FullName);

                    foreach (GenericParticle gp in gps)
                    {
                        lb.Items.Add(gp);
                    }
                }
            }
        }

        public static Volume GetClone(Volume vl)
        {
            // ====================================================================
            StringWriter sw = new StringWriter();
            XmlTextWriter textWriter = new XmlTextWriter(sw);

            //Start of XML Document
            textWriter.WriteStartDocument();

            //Start -> Volume
            textWriter.WriteStartElement("Volume");

            //Block -> Name
            textWriter.WriteStartElement("Name");
            textWriter.WriteString(vl.Name);
            textWriter.WriteEndElement();

            //Start -> Parameters
            textWriter.WriteStartElement("Parameters");

            foreach (Parameter p in vl.Parameters.GetValues())
            {
                //Start -> Parameter
                textWriter.WriteStartElement("Parameter");

                //Block -> Name
                textWriter.WriteStartElement("Name");
                textWriter.WriteString(p.Name);
                textWriter.WriteEndElement();

                //Block -> Category
                textWriter.WriteStartElement("Category");
                textWriter.WriteString(p.Category);
                textWriter.WriteEndElement();

                //Block -> Summary
                textWriter.WriteStartElement("Summary");
                textWriter.WriteString(p.Summary);
                textWriter.WriteEndElement();

                //Block -> Object
                textWriter.WriteStartElement("Object");
                textWriter.WriteString(p.Object.ToString());
                textWriter.WriteEndElement();

                //End -> Parameter
                textWriter.WriteEndElement();
            }

            //End -> Parameters
            textWriter.WriteEndElement();

            //Start -> Events
            textWriter.WriteStartElement("Events");

            foreach (Event ev in vl.Events.ToList())
            {
                //Start -> Event
                textWriter.WriteStartElement("Event");

                //Block -> Name
                textWriter.WriteStartElement("Name");
                textWriter.WriteString(ev.Name);
                textWriter.WriteEndElement();

                //Start -> Parameters
                textWriter.WriteStartElement("Parameters");

                foreach (Parameter p in ev.Parameters.GetValues())
                {
                    //Start -> Parameter
                    textWriter.WriteStartElement("Parameter");

                    //Block -> Name
                    textWriter.WriteStartElement("Name");
                    textWriter.WriteString(p.Name);
                    textWriter.WriteEndElement();

                    //Block -> Category
                    textWriter.WriteStartElement("Category");
                    textWriter.WriteString(p.Category);
                    textWriter.WriteEndElement();

                    //Block -> Summary
                    textWriter.WriteStartElement("Summary");
                    textWriter.WriteString(p.Summary);
                    textWriter.WriteEndElement();

                    //Block -> Object
                    textWriter.WriteStartElement("Object");
                    textWriter.WriteString(p.Object.ToString());
                    textWriter.WriteEndElement();

                    //End -> Parameter
                    textWriter.WriteEndElement();
                }

                //End -> Parameters
                textWriter.WriteEndElement();

                //End -> Event
                textWriter.WriteEndElement();
            }

            //End -> Events
            textWriter.WriteEndElement();

            //Start -> CreationObject list
            textWriter.WriteStartElement("CreationObjects");

            foreach (CreationObject cro in vl.Objects)
            {
                //Start -> CreationObject
                textWriter.WriteStartElement("CreationObject");

                foreach (GeometryObject go in cro.Array)
                {
                    if (go.GetType() == typeof(LineObject))
                    {
                        LineObject l = (LineObject)go;

                        //Start -> LineObject
                        textWriter.WriteStartElement("LineObject");

                        //Block -> Start.X
                        textWriter.WriteStartElement("Start.X");
                        textWriter.WriteString(l.Start.X.ToString());
                        textWriter.WriteEndElement();

                        //Block -> Start.Y
                        textWriter.WriteStartElement("Start.Y");
                        textWriter.WriteString(l.Start.Y.ToString());
                        textWriter.WriteEndElement();

                        //Block -> Stop.X
                        textWriter.WriteStartElement("Stop.X");
                        textWriter.WriteString(l.Stop.X.ToString());
                        textWriter.WriteEndElement();

                        //Block -> Stop.Y
                        textWriter.WriteStartElement("Stop.Y");
                        textWriter.WriteString(l.Stop.Y.ToString());
                        textWriter.WriteEndElement();

                        //End -> LineObject
                        textWriter.WriteEndElement();
                    }
                    else if (go.GetType() == typeof(BezierObject))
                    {
                        BezierObject b = (BezierObject)go;

                        //Start -> BezierObject
                        textWriter.WriteStartElement("BezierObject");

                        //Block -> Start.X
                        textWriter.WriteStartElement("Start.X");
                        textWriter.WriteString(b.Start.X.ToString());
                        textWriter.WriteEndElement();

                        //Block -> Start.Y
                        textWriter.WriteStartElement("Start.Y");
                        textWriter.WriteString(b.Start.Y.ToString());
                        textWriter.WriteEndElement();

                        //Block -> CP1.X
                        textWriter.WriteStartElement("CP1.X");
                        textWriter.WriteString(b.CP1.X.ToString());
                        textWriter.WriteEndElement();

                        //Block -> CP1.Y
                        textWriter.WriteStartElement("CP1.Y");
                        textWriter.WriteString(b.CP1.Y.ToString());
                        textWriter.WriteEndElement();

                        //Block -> CP2.X
                        textWriter.WriteStartElement("CP2.X");
                        textWriter.WriteString(b.CP2.X.ToString());
                        textWriter.WriteEndElement();

                        //Block -> CP2.Y
                        textWriter.WriteStartElement("CP2.Y");
                        textWriter.WriteString(b.CP2.Y.ToString());
                        textWriter.WriteEndElement();

                        //Block -> Stop.X
                        textWriter.WriteStartElement("Stop.X");
                        textWriter.WriteString(b.Stop.X.ToString());
                        textWriter.WriteEndElement();

                        //Block -> Stop.Y
                        textWriter.WriteStartElement("Stop.Y");
                        textWriter.WriteString(b.Stop.Y.ToString());
                        textWriter.WriteEndElement();

                        //End -> BezierObject
                        textWriter.WriteEndElement();
                    }
                }
            }

            //End -> CreationObject list
            textWriter.WriteEndElement();

            //End -> Volume
            textWriter.WriteEndElement();

            //End of XML Document
            textWriter.WriteEndDocument();
            textWriter.Close();
            // ====================================================================

            String volume_text = sw.ToString();
            sw.Close();

            StringReader sr = new StringReader(volume_text);
            // ====================================================================
            XmlTextReader textReader = new XmlTextReader(sr);

            Volume v2 = null;
            Parameter p2 = null;
            Parameters pev2 = null;
            Events evts2 = null;
            CreationObject cro2 = null;
            LineObject l2 = null;
            BezierObject b2 = null;
            string lastParentElement = "", lastString = "";

            while (textReader.Read())
            {
                if (textReader.IsStartElement())
                {
                    if (textReader.Name == "Volume")
                    {
                        v2 = new Volume();
                        lastParentElement = "Volume";
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "Volume")
                    {
                        v2.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Parameters" && lastParentElement == "Volume")
                    {
                        pev2 = new Parameters();
                        lastParentElement = "VolumeParameters";
                    }
                    else if (textReader.Name == "Parameter" && lastParentElement == "VolumeParameters")
                    {
                        p2 = new Parameter();
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "VolumeParameters")
                    {
                        p2.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Category" && lastParentElement == "VolumeParameters")
                    {
                        p2.Category = textReader.ReadString();
                    }
                    else if (textReader.Name == "Summary" && lastParentElement == "VolumeParameters")
                    {
                        p2.Summary = textReader.ReadString();
                    }
                    else if (textReader.Name == "Object" && lastParentElement == "VolumeParameters")
                    {
                        switch (p2.Name)
                        {
                            case "Use front rainbow": p2.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use back rainbow": p2.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use thickness rainbow": p2.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use border rainbow": p2.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use shadow rainbow": p2.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Anchor X": p2.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Anchor Y": p2.Object = Convert.ToInt32(textReader.ReadString()); break;
                            default: p2.Object = textReader.ReadString(); break;
                        }
                        pev2.Add(p2.Name, p2);
                        v2.Parameters = pev2;
                    }
                    else if (textReader.Name == "Events")
                    {
                        evts2 = new Events();
                        lastParentElement = "Events";
                    }
                    else if (textReader.Name == "Event")
                    {
                        pev2 = new Parameters();
                        lastParentElement = "Event";
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "Event")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Parameters" && lastParentElement == "Event")
                    {
                        lastParentElement = "EventParameters";
                    }
                    else if (textReader.Name == "Parameter" && lastParentElement == "EventParameters")
                    {
                        p2 = new Parameter();
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "EventParameters")
                    {
                        p2.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Category" && lastParentElement == "EventParameters")
                    {
                        p2.Category = textReader.ReadString();
                    }
                    else if (textReader.Name == "Summary" && lastParentElement == "EventParameters")
                    {
                        p2.Summary = textReader.ReadString();
                    }
                    else if (textReader.Name == "Object" && lastParentElement == "EventParameters")
                    {
                        switch (p2.Name)
                        {
                            case "Position X": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Position Y": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Position Z": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Scale X": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Scale Y": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle X": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle Y": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle Z": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake X": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake Y": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake Z": p2.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Thickness": p2.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Border": p2.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Shadow": p2.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Front color": p2.Object = Color.Red; break;
                            case "Back color": p2.Object = Color.Red; break;
                            case "Thickness color": p2.Object = Color.Red; break;
                            case "Border color": p2.Object = Color.Red; break;
                            case "Shadow color": p2.Object = Color.Red; break;
                            default: p2.Object = textReader.ReadString(); break;
                        }
                        pev2.Add(p2.Name, p2);
                    }
                    else if (textReader.Name == "CreationObjects")
                    {
                        lastParentElement = "CreationObjects";
                    }
                    else if (textReader.Name == "CreationObject")
                    {
                        cro2 = new CreationObject();
                        lastParentElement = "CreationObject";
                    }
                    else if (textReader.Name == "LineObject")
                    {
                        l2 = new LineObject();
                        lastParentElement = "LineObject";
                    }
                    else if (textReader.Name == "Start.X" && lastParentElement == "LineObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Start.Y" && lastParentElement == "LineObject")
                    {
                        l2.Start = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "Stop.X" && lastParentElement == "LineObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Stop.Y" && lastParentElement == "LineObject")
                    {
                        l2.Stop = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                        cro2.Array.Add(l2);
                    }
                    else if (textReader.Name == "BezierObject")
                    {
                        b2 = new BezierObject();
                        lastParentElement = "BezierObject";
                    }
                    else if (textReader.Name == "Start.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Start.Y" && lastParentElement == "BezierObject")
                    {
                        b2.Start = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "CP1.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "CP1.Y" && lastParentElement == "BezierObject")
                    {
                        b2.CP1 = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "CP2.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "CP2.Y" && lastParentElement == "BezierObject")
                    {
                        b2.CP2 = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "Stop.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Stop.Y" && lastParentElement == "BezierObject")
                    {
                        b2.Stop = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                        cro2.Array.Add(b2);
                    }
                }
                else if (textReader.Name == "Parameters" && lastParentElement == "EventParameters")
                {
                    evts2.Add(lastString, pev2);
                    lastParentElement = "Event";
                }
                else if (textReader.Name == "Events")
                {
                    v2.Events = evts2;
                }
                else if (textReader.Name == "LineObject" | textReader.Name == "BezierObject")
                {
                    lastParentElement = "CreationObject";
                }
                else if (textReader.Name == "CreationObject")
                {
                    v2.Objects.Add(cro2);
                }
            }
            // ====================================================================
            sr.Close();
            
            return v2;
        }

        public static void ParticlesToXML(List<GenericParticle> particles, string path)
        {
            XmlTextWriter textWriter = new XmlTextWriter(path, null);

            //Start of XML Document
            textWriter.WriteStartDocument();

            //Start -> ParticleCollection
            textWriter.WriteStartElement("ParticleCollection");

            foreach (GenericParticle gp in particles)
            {
                //Start -> Particle
                textWriter.WriteStartElement("Particle");

                List<Volume> volumes = new List<Volume>();

                if (gp.GetType() == typeof(ParamTypeParticle))
                {
                    //Block -> Type
                    textWriter.WriteStartElement("Type");
                    textWriter.WriteString("ParamTypeParticle");
                    textWriter.WriteEndElement();

                    volumes = ((ParamTypeParticle)gp).GetVolumeList();
                }
                else if (gp.GetType() == typeof(ScriptTypeParticle))
                {
                    //Block -> Type
                    textWriter.WriteStartElement("Type");
                    textWriter.WriteString("ScriptTypeParticle");
                    textWriter.WriteEndElement();

                    volumes = ((ScriptTypeParticle)gp).GetVolumeList();
                }

                //Start -> VolumeCollection
                textWriter.WriteStartElement("VolumeCollection");

                foreach (Volume v in volumes)
                {
                    //Start -> Volume
                    textWriter.WriteStartElement("Volume");

                    //Block -> Name
                    textWriter.WriteStartElement("Name");
                    textWriter.WriteString(v.Name);
                    textWriter.WriteEndElement();

                    //Start -> Parameters
                    textWriter.WriteStartElement("Parameters");

                    foreach (Parameter p in v.Parameters.GetValues())
                    {
                        //Start -> Parameter
                        textWriter.WriteStartElement("Parameter");

                        //Block -> Name
                        textWriter.WriteStartElement("Name");
                        textWriter.WriteString(p.Name);
                        textWriter.WriteEndElement();

                        //Block -> Category
                        textWriter.WriteStartElement("Category");
                        textWriter.WriteString(p.Category);
                        textWriter.WriteEndElement();

                        //Block -> Summary
                        textWriter.WriteStartElement("Summary");
                        textWriter.WriteString(p.Summary);
                        textWriter.WriteEndElement();

                        //Block -> Object
                        textWriter.WriteStartElement("Object");
                        textWriter.WriteString(p.Object.ToString());
                        textWriter.WriteEndElement();

                        //End -> Parameter
                        textWriter.WriteEndElement();
                    }

                    //End -> Parameters
                    textWriter.WriteEndElement();

                    //Start -> Events
                    textWriter.WriteStartElement("Events");

                    foreach (Event ev in v.Events.ToList())
                    {
                        //Start -> Event
                        textWriter.WriteStartElement("Event");

                        //Block -> Name
                        textWriter.WriteStartElement("Name");
                        textWriter.WriteString(ev.Name);
                        textWriter.WriteEndElement();

                        //Start -> Parameters
                        textWriter.WriteStartElement("Parameters");

                        foreach (Parameter p in ev.Parameters.GetValues())
                        {
                            //Start -> Parameter
                            textWriter.WriteStartElement("Parameter");

                            //Block -> Name
                            textWriter.WriteStartElement("Name");
                            textWriter.WriteString(p.Name);
                            textWriter.WriteEndElement();

                            //Block -> Category
                            textWriter.WriteStartElement("Category");
                            textWriter.WriteString(p.Category);
                            textWriter.WriteEndElement();

                            //Block -> Summary
                            textWriter.WriteStartElement("Summary");
                            textWriter.WriteString(p.Summary);
                            textWriter.WriteEndElement();

                            //Block -> Object
                            textWriter.WriteStartElement("Object");
                            textWriter.WriteString(p.Object.ToString());
                            textWriter.WriteEndElement();

                            //End -> Parameter
                            textWriter.WriteEndElement();
                        }

                        //End -> Parameters
                        textWriter.WriteEndElement();

                        //End -> Event
                        textWriter.WriteEndElement();
                    }

                    //End -> Events
                    textWriter.WriteEndElement();

                    //Start -> CreationObject list
                    textWriter.WriteStartElement("CreationObjects");

                    foreach (CreationObject cro in v.Objects)
                    {
                        //Start -> CreationObject
                        textWriter.WriteStartElement("CreationObject");

                        foreach (GeometryObject go in cro.Array)
                        {
                            if (go.GetType() == typeof(LineObject))
                            {
                                LineObject l = (LineObject)go;

                                //Start -> LineObject
                                textWriter.WriteStartElement("LineObject");

                                //Block -> Start.X
                                textWriter.WriteStartElement("Start.X");
                                textWriter.WriteString(l.Start.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Start.Y
                                textWriter.WriteStartElement("Start.Y");
                                textWriter.WriteString(l.Start.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.X
                                textWriter.WriteStartElement("Stop.X");
                                textWriter.WriteString(l.Stop.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.Y
                                textWriter.WriteStartElement("Stop.Y");
                                textWriter.WriteString(l.Stop.Y.ToString());
                                textWriter.WriteEndElement();

                                //End -> LineObject
                                textWriter.WriteEndElement();
                            }
                            else if (go.GetType() == typeof(BezierObject))
                            {
                                BezierObject b = (BezierObject)go;

                                //Start -> BezierObject
                                textWriter.WriteStartElement("BezierObject");

                                //Block -> Start.X
                                textWriter.WriteStartElement("Start.X");
                                textWriter.WriteString(b.Start.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Start.Y
                                textWriter.WriteStartElement("Start.Y");
                                textWriter.WriteString(b.Start.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP1.X
                                textWriter.WriteStartElement("CP1.X");
                                textWriter.WriteString(b.CP1.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP1.Y
                                textWriter.WriteStartElement("CP1.Y");
                                textWriter.WriteString(b.CP1.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP2.X
                                textWriter.WriteStartElement("CP2.X");
                                textWriter.WriteString(b.CP2.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP2.Y
                                textWriter.WriteStartElement("CP2.Y");
                                textWriter.WriteString(b.CP2.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.X
                                textWriter.WriteStartElement("Stop.X");
                                textWriter.WriteString(b.Stop.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.Y
                                textWriter.WriteStartElement("Stop.Y");
                                textWriter.WriteString(b.Stop.Y.ToString());
                                textWriter.WriteEndElement();

                                //End -> BezierObject
                                textWriter.WriteEndElement();
                            }
                        }

                        //End -> CreationObject
                        textWriter.WriteEndElement();
                    }

                    //End -> CreationObject list
                    textWriter.WriteEndElement();

                    //Start -> InsertPoints
                    textWriter.WriteStartElement("InsertPoints");

                    foreach (InsertPoint ip in v.GettInsertPoints())
                    {
                        //Start -> Point
                        textWriter.WriteStartElement("Point");

                        //Start -> Parameters
                        textWriter.WriteStartElement("Parameters");

                        foreach (Parameter p in ip.Parameters.GetValues())
                        {
                            //Start -> Parameter
                            textWriter.WriteStartElement("Parameter");

                            //Block -> Name
                            textWriter.WriteStartElement("Name");
                            textWriter.WriteString(p.Name);
                            textWriter.WriteEndElement();

                            //Block -> Category
                            textWriter.WriteStartElement("Category");
                            textWriter.WriteString(p.Category);
                            textWriter.WriteEndElement();

                            //Block -> Summary
                            textWriter.WriteStartElement("Summary");
                            textWriter.WriteString(p.Summary);
                            textWriter.WriteEndElement();

                            //Block -> Object
                            textWriter.WriteStartElement("Object");
                            textWriter.WriteString(p.Object.ToString());
                            textWriter.WriteEndElement();

                            //End -> Parameter
                            textWriter.WriteEndElement();
                        }

                        //End -> Parameters
                        textWriter.WriteEndElement();

                        //Start -> TrajectoryStart
                        textWriter.WriteStartElement("TrajectoryStart");
                        //Block -> X
                        textWriter.WriteStartElement("X");
                        textWriter.WriteString(ip.TrajectoryStart.X.ToString());
                        textWriter.WriteEndElement();
                        //Block -> Y
                        textWriter.WriteStartElement("Y");
                        textWriter.WriteString(ip.TrajectoryStart.Y.ToString());
                        textWriter.WriteEndElement();
                        //End -> TrajectoryStart
                        textWriter.WriteEndElement();

                        //Start -> TrajectorySpline
                        textWriter.WriteStartElement("TrajectorySpline");

                        foreach (GeometryObject go in ip.GetTrajectorySpline().GetTrajectory())
                        {
                            if (go.GetType() == typeof(LineObject))
                            {
                                LineObject l = (LineObject)go;

                                //Start -> LineObject
                                textWriter.WriteStartElement("LineObject");

                                //Block -> Start.X
                                textWriter.WriteStartElement("Start.X");
                                textWriter.WriteString(l.Start.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Start.Y
                                textWriter.WriteStartElement("Start.Y");
                                textWriter.WriteString(l.Start.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.X
                                textWriter.WriteStartElement("Stop.X");
                                textWriter.WriteString(l.Stop.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.Y
                                textWriter.WriteStartElement("Stop.Y");
                                textWriter.WriteString(l.Stop.Y.ToString());
                                textWriter.WriteEndElement();

                                //End -> LineObject
                                textWriter.WriteEndElement();
                            }
                            else if (go.GetType() == typeof(BezierObject))
                            {
                                BezierObject b = (BezierObject)go;

                                //Start -> BezierObject
                                textWriter.WriteStartElement("BezierObject");

                                //Block -> Start.X
                                textWriter.WriteStartElement("Start.X");
                                textWriter.WriteString(b.Start.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Start.Y
                                textWriter.WriteStartElement("Start.Y");
                                textWriter.WriteString(b.Start.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP1.X
                                textWriter.WriteStartElement("CP1.X");
                                textWriter.WriteString(b.CP1.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP1.Y
                                textWriter.WriteStartElement("CP1.Y");
                                textWriter.WriteString(b.CP1.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP2.X
                                textWriter.WriteStartElement("CP2.X");
                                textWriter.WriteString(b.CP2.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> CP2.Y
                                textWriter.WriteStartElement("CP2.Y");
                                textWriter.WriteString(b.CP2.Y.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.X
                                textWriter.WriteStartElement("Stop.X");
                                textWriter.WriteString(b.Stop.X.ToString());
                                textWriter.WriteEndElement();

                                //Block -> Stop.Y
                                textWriter.WriteStartElement("Stop.Y");
                                textWriter.WriteString(b.Stop.Y.ToString());
                                textWriter.WriteEndElement();

                                //End -> BezierObject
                                textWriter.WriteEndElement();
                            }
                        }

                        //End -> TrajectorySpline
                        textWriter.WriteEndElement();

                        //End -> Point
                        textWriter.WriteEndElement();
                    }

                    //End -> InsertPoints
                    textWriter.WriteEndElement();                    

                    //End -> Volume
                    textWriter.WriteEndElement();
                }

                //End -> VolumeCollection
                textWriter.WriteEndElement();

                //End -> Particle
                textWriter.WriteEndElement();
            }


            //End -> ParticleCollection
            textWriter.WriteEndElement();            

            //End of XML Document
            textWriter.WriteEndDocument();
            textWriter.Close();
        }

        public static List<GenericParticle> XMLToParticles(string path)
        {
            List<GenericParticle> gps = new List<GenericParticle>();

            XmlTextReader textReader = new XmlTextReader(path);
            List<Volume> volumes = null;

            Volume v = null;
            Parameter p = null;
            Parameters pev = null;
            Events evts = null;
            CreationObject cro = null;
            LineObject l = null;
            BezierObject b = null;
            string lastParentElement = "", lastString = "", lastType = "";

            ParamTypeParticle ptp = null;
            ScriptTypeParticle stp = null;
            InsertPoint ip = null;

            while (textReader.Read())
            {
                if (textReader.IsStartElement())
                {
                    if (textReader.Name == "Volume")
                    {
                        v = new Volume();
                        lastParentElement = "Volume";
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "Volume")
                    {
                        v.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Parameters" && lastParentElement == "Volume")
                    {
                        pev = new Parameters();
                        lastParentElement = "VolumeParameters";
                    }
                    else if (textReader.Name == "Parameter" && lastParentElement == "VolumeParameters")
                    {
                        p = new Parameter();
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "VolumeParameters")
                    {
                        p.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Category" && lastParentElement == "VolumeParameters")
                    {
                        p.Category = textReader.ReadString();
                    }
                    else if (textReader.Name == "Summary" && lastParentElement == "VolumeParameters")
                    {
                        p.Summary = textReader.ReadString();
                    }
                    else if (textReader.Name == "Object" && lastParentElement == "VolumeParameters")
                    {
                        switch (p.Name)
                        {
                            case "Use front rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use back rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use thickness rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use border rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Use shadow rainbow": p.Object = Convert.ToBoolean(textReader.ReadString()); break;
                            case "Anchor X": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Anchor Y": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            default: p.Object = textReader.ReadString(); break;
                        }
                        pev.Add(p.Name, p);
                        v.Parameters = pev;
                    }
                    else if (textReader.Name == "Events")
                    {
                        evts = new Events();
                        lastParentElement = "Events";
                    }
                    else if (textReader.Name == "Event")
                    {
                        pev = new Parameters();
                        lastParentElement = "Event";
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "Event")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Parameters" && lastParentElement == "Event")
                    {
                        lastParentElement = "EventParameters";
                    }
                    else if (textReader.Name == "Parameter" && lastParentElement == "EventParameters")
                    {
                        p = new Parameter();
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "EventParameters")
                    {
                        p.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Category" && lastParentElement == "EventParameters")
                    {
                        p.Category = textReader.ReadString();
                    }
                    else if (textReader.Name == "Summary" && lastParentElement == "EventParameters")
                    {
                        p.Summary = textReader.ReadString();
                    }
                    else if (textReader.Name == "Object" && lastParentElement == "EventParameters")
                    {
                        switch (p.Name)
                        {
                            case "Position X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Position Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Position Z": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Scale X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Scale Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Angle Z": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Quake Z": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Thickness": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Border": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Shadow": p.Object = Convert.ToInt32(textReader.ReadString()); break;
                            case "Front color": p.Object = Color.Red; break;
                            case "Back color": p.Object = Color.Red; break;
                            case "Thickness color": p.Object = Color.Red; break;
                            case "Border color": p.Object = Color.Red; break;
                            case "Shadow color": p.Object = Color.Red; break;
                            default: p.Object = textReader.ReadString(); break;
                        }
                        pev.Add(p.Name, p);
                    }
                    else if (textReader.Name == "CreationObjects")
                    {
                        lastParentElement = "CreationObjects";
                    }
                    else if (textReader.Name == "CreationObject")
                    {
                        cro = new CreationObject();
                        lastParentElement = "CreationObject";
                    }
                    else if (textReader.Name == "LineObject")
                    {
                        l = new LineObject();
                        lastParentElement = "LineObject";
                    }
                    else if (textReader.Name == "Start.X" && lastParentElement == "LineObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Start.Y" && lastParentElement == "LineObject")
                    {
                        l.Start = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "Stop.X" && lastParentElement == "LineObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Stop.Y" && lastParentElement == "LineObject")
                    {
                        l.Stop = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                        if (ip != null)
                        {
                            ip.GetTrajectorySpline().AddGeometry(l);
                        }
                        else
                        {
                            cro.Array.Add(l);
                        }
                    }
                    else if (textReader.Name == "BezierObject")
                    {
                        b = new BezierObject();
                        lastParentElement = "BezierObject";
                    }
                    else if (textReader.Name == "Start.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Start.Y" && lastParentElement == "BezierObject")
                    {
                        b.Start = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "CP1.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "CP1.Y" && lastParentElement == "BezierObject")
                    {
                        b.CP1 = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "CP2.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "CP2.Y" && lastParentElement == "BezierObject")
                    {
                        b.CP2 = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                    }
                    else if (textReader.Name == "Stop.X" && lastParentElement == "BezierObject")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Stop.Y" && lastParentElement == "BezierObject")
                    {
                        b.Stop = new Point(Convert.ToInt32(lastString), Convert.ToInt32(textReader.ReadString()));
                        if (ip != null)
                        {
                            ip.GetTrajectorySpline().AddGeometry(b);
                        }
                        else
                        {
                            cro.Array.Add(b);
                        }
                    }
                    else if (textReader.Name == "VolumeCollection")
                    {
                        volumes = new List<Volume>();
                    }
                    else if (textReader.Name == "Particle")
                    {
                        lastParentElement = "Particle";
                    }
                    else if (textReader.Name == "Type" && lastParentElement == "Particle")
                    {
                        lastType = textReader.ReadString();
                        if (lastType == "ParamTypeParticle")
                        {
                            ptp = new ParamTypeParticle();
                        }
                        else if (lastType == "ScriptTypeParticle")
                        {
                            stp = new ScriptTypeParticle();
                        }
                    }
                    else if (textReader.Name == "Point")
                    {
                        ip = new InsertPoint();
                        pev = new Parameters();
                        lastParentElement = "PointParameters";
                    }
                    else if (textReader.Name == "Parameter" && lastParentElement == "PointParameters")
                    {
                        p = new Parameter();
                    }
                    else if (textReader.Name == "Name" && lastParentElement == "PointParameters")
                    {
                        p.Name = textReader.ReadString();
                    }
                    else if (textReader.Name == "Category" && lastParentElement == "PointParameters")
                    {
                        p.Category = textReader.ReadString();
                    }
                    else if (textReader.Name == "Summary" && lastParentElement == "PointParameters")
                    {
                        p.Summary = textReader.ReadString();
                    }
                    else if (textReader.Name == "Object" && lastParentElement == "PointParameters")
                    {
                        switch (p.Name)
                        {
                            case "Position X": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Position Y": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            case "Color": p.Object = Color.Chartreuse; break;
                            case "Size": p.Object = (float)Convert.ToDouble(textReader.ReadString()); break;
                            default: p.Object = textReader.ReadString(); break;
                        }
                        pev.Add(p.Name, p);
                    }
                    else if (textReader.Name == "TrajectoryStart")
                    {
                        lastParentElement = "TrajectoryStart";
                    }
                    else if (textReader.Name == "X" && lastParentElement == "TrajectoryStart")
                    {
                        lastString = textReader.ReadString();
                    }
                    else if (textReader.Name == "Y" && lastParentElement == "TrajectoryStart")
                    {
                        double x = Convert.ToDouble(lastString);
                        double y = Convert.ToDouble(textReader.ReadString());
                        ip.TrajectoryStart = new JPoint(x, y);
                    }
                }
                else if (textReader.Name == "Parameters" && lastParentElement == "EventParameters")
                {
                    evts.Add(lastString, pev);
                    lastParentElement = "Event";
                }
                else if (textReader.Name == "Events")
                {
                    v.Events = evts;
                }
                else if (textReader.Name == "LineObject" | textReader.Name == "BezierObject")
                {
                    lastParentElement = "CreationObject";
                }
                else if (textReader.Name == "CreationObject")
                {
                    v.Objects.Add(cro);
                }
                else if (textReader.Name == "Volume")
                {
                    if (lastType == "ParamTypeParticle")
                    {
                        ptp.ImportVolume(v);
                    }
                    else if (lastType == "ScriptTypeParticle")
                    {
                        
                    }
                }
                else if (textReader.Name == "VolumeCollection")
                {
                    volumes = null;
                }
                else if (textReader.Name == "Point")
                {
                    v.AddtInsertPoint(ip);
                    ip = null;
                }
                else if (textReader.Name == "Parameters" && lastParentElement == "PointParameters")
                {
                    ip.Parameters = pev;
                    lastParentElement = "Point";
                }
                else if (textReader.Name == "TrajectoryStart")
                {
                    lastParentElement = "Point";
                }
                else if (textReader.Name == "Particle")
                {
                    if (lastType == "ParamTypeParticle")
                    {
                        gps.Add(ptp);
                        ptp = null;
                    }
                    else if (lastType == "ScriptTypeParticle")
                    {
                        gps.Add(stp);
                        stp = null;
                    }
                }

            }

            return gps;
        }

        public static List<string> GetRawLinesFromAss(string path)
        {
            List<string> rawLines = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("Dialogue") | line.StartsWith("Comment"))
                        {
                            rawLines.Add(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return rawLines;
        }

        public static Color GetColorFrom(string c)
        {
            if (c.Contains("["))
            {

            }
        }

    }
}
