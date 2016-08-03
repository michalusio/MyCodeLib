using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public interface ITransformation2D
  {
    void Transform(ref PPoint2D point, List<PPoint2D> allPoints);
  }
}
