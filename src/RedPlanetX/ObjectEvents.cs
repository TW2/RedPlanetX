using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetX
{
    public class ObjectEvents
    {
        public string Name { get; set; }

        public object Object { get; set; }

        public Events Events { get; set; }

        public Parameters Parameters { get; set; }

        public ObjectType Type = ObjectType.HorizontalText;

        public enum ObjectType
        {
            Script, HorizontalText, VerticalText, TextArea, Shape, Drawing, Picture, Video
        }

        //==========================================================

        // Retourne un treenode et enregistre les treenodes crées dans les collections passé en paramètres.
        public TreeNode GetTreeNode(Dictionary<TreeNode, ObjectEvents> objects, Dictionary<TreeNode, Event> events)
        {
            TreeNode main = new TreeNode(this.Name);
            main.Name = this.Name;
            objects.Add(main, this);

            foreach (Event e in Events.ToList())
            {
                TreeNode evt = new TreeNode(e.Name);
                evt.Name = e.Name;
                events.Add(evt, e);
                main.Nodes.Add(evt);
            }

            return main;
        }

        public void ModifyTreeNode(TreeNode main)
        {
            main.Name = this.Name;

            main.Nodes.Clear();

            foreach (Event e in Events.ToList())
            {
                TreeNode evt = new TreeNode(e.Name);
                evt.Name = e.Name;
                main.Nodes.Add(evt);
            }
        }

    }

    public class Event
    {
        public string Name { get; set; }

        public Parameters Parameters { get; set; }
    }

    public class Events
    {
        private List<Event> evts = new List<Event>();

        public void Add(string name, Parameters parameters)
        {
            Event evt = new Event();
            evt.Name = name;
            evt.Parameters = parameters;
            evts.Add(evt);
        }

        public void Remove(string name)
        {
            foreach (Event evt in evts)
            {
                if (evt.Name.Equals(name))
                {
                    evts.Remove(evt);
                }
            }            
        }

        public Event Get(string name)
        {
            foreach (Event evt in evts)
            {
                if (evt.Name.Equals(name))
                {
                    return evt;
                }
            }
            return null;
        }

        public List<Event> ToList()
        {
            try
            {
                evts.Sort();
            }
            catch (Exception e)
            {

            }            
            return evts.ToList<Event>();
        }

        public List<Event> ToReverseList()
        {
            try
            {
                evts.Reverse();
            }
            catch (Exception e)
            {

            }
            return evts.ToList<Event>();
        }
    }

    public class Parameter
    {
        public string Name { get; set; }

        public object Object { get; set; }

        public string Category { get; set; }

        public string Summary { get; set; }
    }

    public class Parameters
    {
        private Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();

        public void Add(string param, Parameter o)
        {
            parameters.Add(param, o);
        }

        public void Modify(string param, Parameter o)
        {
            if(parameters.ContainsKey(param))
            {
                parameters[param] = o;
            }
            
        }

        public void Remove(string param)
        {
            parameters.Remove(param);
        }

        public object Get(string param)
        {
            if (parameters.ContainsKey(param))
            {
                return parameters[param];
            }
            return null;
        }

        public Dictionary<string, Parameter> ToDictionary()
        {
            return parameters;
        }

        public List<Parameter> GetValues()
        {
            List<Parameter> prs = new List<Parameter>();
            prs.AddRange(parameters.Values);
            return prs;
        }
    }
}
