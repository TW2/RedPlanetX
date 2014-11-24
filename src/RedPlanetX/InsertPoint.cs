using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public class InsertPoint
    {
        private TrajectorySpline trajectory = new TrajectorySpline();
        public JPoint TrajectoryStart { get; set; }
        public Parameters Parameters { get; set; }

        public InsertPoint()
        {
            Parameters main_prs = new Parameters();

            Parameter x1 = new Parameter();
            x1.Name = "Position X";
            x1.Object = 0f;
            x1.Category = "Position";
            x1.Summary = "The start of the trajectory for this particle.";
            main_prs.Add(x1.Name, x1);

            Parameter y1 = new Parameter();
            y1.Name = "Position Y";
            y1.Object = 0f;
            y1.Category = "Position";
            y1.Summary = "The start of the trajectory for this particle.";
            main_prs.Add(y1.Name, y1);

            Parameter color = new Parameter();
            color.Name = "Color";
            color.Object = Color.Chartreuse;
            color.Category = "Color";
            color.Summary = "A color to recognize this point of insertion.";
            main_prs.Add(color.Name, color);

            Parameter size = new Parameter();
            size.Name = "Size";
            size.Object = 100f;
            size.Category = "Size";
            size.Summary = "Size in pourcentage of the original object.";
            main_prs.Add(size.Name, size);
            
            Parameters = main_prs;

            TrajectoryStart = null;
        }

        public TrajectorySpline GetTrajectorySpline()
        {
            return trajectory;
        }
    }
}
