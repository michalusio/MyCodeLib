using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public class Scale2D : INvertibleTransformation2D
  {
    public float X;
    public float Y;

    public Scale2D(float x, float y)
    {
      X = x;
      Y = y;
    }

    public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
    {
      point.X *= X;
      point.Y *= Y;
    }

    public void Invert(ref PPoint2D point, List<PPoint2D> allPoints)
    {
      point.X /= X;
      point.Y /= Y;
    }
  }
}
