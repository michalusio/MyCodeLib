using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
    public interface ITransformation3D
    {
        void Transform(ref PPoint3D point, List<PPoint3D> allPoints);
    }
}