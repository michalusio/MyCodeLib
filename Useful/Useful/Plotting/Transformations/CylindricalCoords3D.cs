using System;
using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public class CylindricalCoords3D : INvertibleTransformation3D
  {
    public void Transform(ref PPoint3D point, List<PPoint3D> allPoints)
    {
      float num = point.Y * (float) Math.Cos(point.X);
      point.Y = point.Y * (float) Math.Sin(point.X);
      point.X = num;
    }

    public void Invert(ref PPoint3D point, List<PPoint3D> allPoints)
    {
      float num = (float) Math.Sqrt(point.X * (double) point.X + point.Y * (double) point.Y);
      point.X = (float) MMath.Mod(Math.Atan2(point.Y, point.X), 2.0 * Math.PI);
      point.Y = num;
    }
  }
}
