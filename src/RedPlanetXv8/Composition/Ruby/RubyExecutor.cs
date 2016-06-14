using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition.Ruby
{
    public class RubyExecutor : ExecutorBase
    {
        protected override void CreateRuntime()
        {
            scriptRuntime = IronRuby.Ruby.CreateRuntime();
        }

        protected override void CreateEngine()
        {
            scriptEngine = scriptRuntime.GetEngine("rb");
        }
    }
}
