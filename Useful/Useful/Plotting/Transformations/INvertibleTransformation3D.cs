using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public interface INvertibleTransformation3D : ITransformation3D
  {
    void Invert(ref PPoint3D point, List<PPoint3D> allPoints);
  }
}
