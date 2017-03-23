using System;
using System.Collections.Generic;
using Useful.Other;

namespace Useful.Plotting.Transformations
{
    public class PolarCoords2D : INvertibleTransformation2D, ITransformation2D
    {
        public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num = point.Y * (float) Math.Cos(point.X);
            point.Y = point.Y * (float) Math.Sin(point.X);
            point.X = num;
        }

        public void Invert(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num = (float) Math.Sqrt(point.X * (double) point.X + point.Y * (double) point.Y);
            point.X = (float) Extensions.Mod(Math.Atan2(point.Y, point.X), 2.0 * Math.PI);
            point.Y = num;
        }
    }
}