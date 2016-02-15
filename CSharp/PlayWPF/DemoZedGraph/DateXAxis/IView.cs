using System;

namespace DemoZedGraph.DateXAxis
{
    interface IView
    {
        void Init();
        void DrawNext(DateTime x, double y);
    }
}
