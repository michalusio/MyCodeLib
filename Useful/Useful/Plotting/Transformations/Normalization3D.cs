using System;
using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public class Normalization3D : ITransformation3D
  {
    public void Transform(ref PPoint3D point, List<PPoint3D> allPoints)
    {
      float num = 1f / (float) Math.Sqrt(point.SquareLen());
      point.X *= num;
      point.Y *= num;
      point.Z *= num;
    }
  }
}
