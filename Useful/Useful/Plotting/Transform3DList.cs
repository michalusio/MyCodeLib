using System.Collections.Generic;
using Useful.Plotting.Transformations;

namespace Useful.Plotting
{
    public class Transform3DList
    {
        public List<ITransformation3D> Transforms;

        public Transform3DList(List<ITransformation3D> l)
        {
            Transforms = l;
        }
    }
}