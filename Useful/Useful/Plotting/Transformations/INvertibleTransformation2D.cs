using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public interface INvertibleTransformation2D : ITransformation2D
  {
    void Invert(ref PPoint2D point, List<PPoint2D> allPoints);
  }
}
