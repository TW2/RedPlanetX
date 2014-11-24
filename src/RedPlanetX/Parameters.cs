using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public class Parameters
    {
        private Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();

        public void Add(string param, Parameter o)
        {
            parameters.Add(param, o);
        }

        public void Modify(string param, Parameter o)
        {
            if (parameters.ContainsKey(param))
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

    public class Parameter
    {
        public string Name { get; set; }
        public object Object { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
    }
}
