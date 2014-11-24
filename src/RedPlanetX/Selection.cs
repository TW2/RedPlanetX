using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    class Selection
    {
        private List<GeometryObject> selectedObjects = new List<GeometryObject>();
        private List<GeometryObject> originalList = new List<GeometryObject>();

        public Selection()
        {

        }

        public void SetOriginalList(List<GeometryObject> originalList)
        {
            this.originalList = originalList;
        }

        public void AddSelection(GeometryObject sel)
        {
            selectedObjects.Add(sel);
        }

        public void AddSelection(List<GeometryObject> sel)
        {
            selectedObjects.AddRange(sel);
        }

        public void ClearSelection()
        {
            selectedObjects.Clear();
        }

        //===================================================================================
        // Analysis and process
        //===================================================================================

        private JPoint GetStartPoint()
        {
            int size = originalList.Count;
            JPoint S = null;

            if (originalList[size - 1].GetType() == typeof(LineObject))
            {
                LineObject l = (LineObject)originalList[size - 1];
                S = new JPoint(l.Stop.X, l.Stop.Y);
            }
            else if (originalList[size - 1].GetType() == typeof(BezierObject))
            {
                BezierObject b = (BezierObject)originalList[size - 1];
                S = new JPoint(b.Stop.X, b.Stop.Y);
            }

            return S;
        }

        private GeometryObject Translate(JPoint X, GeometryObject obj)
        {
            GeometryObject go = null;

            if (obj.GetType() == typeof(LineObject))
            {
                LineObject l = (LineObject)obj;

                int distanceX, distanceY;
                distanceX = (int)X.X - l.Start.X;
                distanceY = (int)X.Y - l.Start.Y;

                LineObject lNEW = new LineObject();
                lNEW.Start = new Point((int)X.X, (int)X.Y);
                lNEW.Stop = new Point(l.Stop.X + distanceX, l.Stop.Y + distanceY);

                go = lNEW;
            }
            else if (obj.GetType() == typeof(BezierObject))
            {
                BezierObject b = (BezierObject)obj;

                int distanceX, distanceY;
                distanceX = (int)X.X - b.Start.X;
                distanceY = (int)X.Y - b.Start.Y;

                BezierObject bNEW = new BezierObject();
                bNEW.Start = new Point((int)X.X, (int)X.Y);
                bNEW.CP1 = new Point(b.CP1.X + distanceX, b.CP1.Y + distanceY);
                bNEW.CP2 = new Point(b.CP2.X + distanceX, b.CP2.Y + distanceY);
                bNEW.Stop = new Point(b.Stop.X + distanceX, b.Stop.Y + distanceY);

                go = bNEW;
            }

            return go;
        }

        //-----------------------------------------------------------------------------------
        // COPY 
        //-----------------------------------------------------------------------------------

        public void Operation_Copy()
        {
            foreach (GeometryObject go in selectedObjects)
            {
                //Get last start point
                JPoint start = GetStartPoint();

                //Translate GeometryObject and add them to originalList
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    GeometryObject _go = Translate(start, l);
                    originalList.Add(_go);
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    GeometryObject _go = Translate(start, b);
                    originalList.Add(_go);
                }
            }
        }
    }
}
