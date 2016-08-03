using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public class Translation2D : INvertibleTransformation2D
  {
    public float X;
    public float Y;

    public Translation2D(float x, float y)
    {
      X = x;
      Y = y;
    }

    public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
    {
      point.X += X;
      point.Y += Y;
    }

    public void Invert(ref PPoint2D point, List<PPoint2D> allPoints)
    {
      point.X -= X;
      point.Y -= Y;
    }
  }
}
