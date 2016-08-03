using System.Drawing;
using Useful.Other;

namespace Useful.Plotting.Projections
{
  public class StereographicProj
  {
    public Plot3D Project(Plot2D p)
    {
      Plot3D plot3D = new Plot3D { LinesH = p.LinesH, LinesV = p.LinesV, Size = p.Size };
      foreach (PPoint2D point in p.GetPoints())
      {
        float num = point.SquareLen();
        plot3D.AddPoint(new PPoint3D((float) (2.0 * point.X / (num + 1.0)), (float) (2.0 * point.Y / (num + 1.0)), (float) ((num - 1.0) / (num + 1.0)), point.Color, point.Visible));
      }
      return plot3D;
    }

    public Plot2D UnProject(Plot3D p)
    {
      Plot2D plot2D = new Plot2D { LinesH = p.LinesH, LinesV = p.LinesV, Size = p.Size };
      foreach (PPoint3D point in p.GetPoints())
      {
        PPoint3D ppoint3D = point;
        plot2D.AddPoint(new PPoint2D(ppoint3D.X / (1f - ppoint3D.Z), ppoint3D.Y / (1f - ppoint3D.Z), point.Color == Color.Empty ? MMath.HsvToRgb(360.0 * (point.Z - (double) p.MinZ) / (p.MaxZ - (double) p.MinZ), 1.0, 1.0) : point.Color, point.Visible));
      }
      return plot2D;
    }
  }
}
