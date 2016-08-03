using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public class Scale3D : INvertibleTransformation3D
  {
    public float X;
    public float Y;
    public float Z;

    public Scale3D(float x, float y, float z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    public void Transform(ref PPoint3D point, List<PPoint3D> allPoints)
    {
      point.X *= X;
      point.Y *= Y;
      point.Z *= Z;
    }

    public void Invert(ref PPoint3D point, List<PPoint3D> allPoints)
    {
      point.X /= X;
      point.Y /= Y;
      point.Z /= Z;
    }
  }
}
