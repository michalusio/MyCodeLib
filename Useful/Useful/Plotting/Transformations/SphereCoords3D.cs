using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
    public class SphereCoords3D : INvertibleTransformation3D
    {
        public void Transform(ref PPoint3D point, List<PPoint3D> allPoints)
        {
            var num = point.SquareLen();
            point.X /= num;
            point.Y /= num;
            point.Z /= num;
        }

        public void Invert(ref PPoint3D point, List<PPoint3D> allPoints)
        {
            Transform(ref point, allPoints);
        }
    }
}