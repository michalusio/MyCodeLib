using System;

namespace Useful.Plotting.Projections
{
    public class CurvilinearProj
    {
        public Plot2D Project(Plot3D p)
        {
            Plot2D plot2D = new Plot2D {LinesH = p.LinesH, LinesV = p.LinesV, Size = p.Size};
            foreach (PPoint3D point in p.GetPoints())
            {
                var num = (float) Math.Sqrt(point.SquareLen());
                plot2D.AddPoint(new PPoint2D(point.X / num, point.Y / num, point.Color, point.Visible));
            }
            return plot2D;
        }
    }
}