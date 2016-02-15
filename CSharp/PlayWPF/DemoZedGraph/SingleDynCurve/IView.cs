using System;
using System.Collections.Generic;

namespace DemoZedGraph.SingleDynCurve
{
    interface IView
    {
        void Initialize(DateTime min, DateTime max, double majorStep,IEnumerable<string> names);
        void Draw(IEnumerable<TimePoint> points, bool updateScale, DateTime newMin, DateTime newMax);
    }
}
