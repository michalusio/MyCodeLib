using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
    public class SphereCoords2D : INvertibleTransformation2D
    {
        public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num = point.SquareLen();
            point.X /= num;
            point.Y /= num;
        }

        public void Invert(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            Transform(ref point, allPoints);
        }
    }
}