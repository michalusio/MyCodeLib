using System.Collections.Generic;
using Useful.Plotting.Transformations;

namespace Useful.Plotting
{
    public class Transform2DList
    {
        public List<ITransformation2D> Transforms;

        public Transform2DList(List<ITransformation2D> l)
        {
            Transforms = l;
        }
    }
}