using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
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
                evts.Sort(new EventComparer());
            }       
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return evts.ToList<Event>();
        }

        public List<Event> ToReverseList()
        {
            try
            {
                evts.Sort(new EventComparer());
                evts.Reverse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return evts.ToList<Event>();
        }

    }

    public class EventComparer : IComparer<Event>
    {
        public int Compare(Event x, Event y)
        {
            int frame_x = Convert.ToInt32(x.Name);
            int frame_y = Convert.ToInt32(y.Name);
            if (frame_x > frame_y)
            {
                return 1;
            }
            else if (frame_x < frame_y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class Event
    {
        public string Name { get; set; }
        public Parameters Parameters { get; set; }
    }
}
