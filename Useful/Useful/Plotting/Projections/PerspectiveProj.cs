using System;

namespace Useful.Plotting.Projections
{
  public class PerspectiveProj
  {
    private float _ez;

    public float Fov
    {
      get
      {
        return (float) (2.0 * Math.Atan2(1.0, _ez));
      }
      set
      {
        _ez = (float) (1.0 / Math.Tan(value * 0.5));
      }
    }

    public Plot2D Project(Plot3D p)
    {
      Plot2D plot2D = new Plot2D { LinesH = p.LinesH, LinesV = p.LinesV, Size = p.Size };
      foreach (PPoint3D point in p.GetPoints())
        plot2D.AddPoint(new PPoint2D(point.X * _ez / point.Z, point.Y * _ez / point.Z, point.Color, point.Visible));
      return plot2D;
    }
  }
}
