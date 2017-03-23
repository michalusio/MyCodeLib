using System;
using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
    public class Normalization2D : ITransformation2D
    {
        public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num = 1f / (float) Math.Sqrt(point.SquareLen());
            point.X *= num;
            point.Y *= num;
        }
    }
}