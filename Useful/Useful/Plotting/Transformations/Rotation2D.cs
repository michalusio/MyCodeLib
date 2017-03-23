using System;
using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
    public class Rotation2D : INvertibleTransformation2D
    {
        private float _a;
        private float _c;
        private float _s;

        public Rotation2D(float a)
        {
            Alpha = a;
        }

        public float Alpha
        {
            get { return _a; }
            set
            {
                _a = value;
                _c = (float) Math.Cos(value);
                _s = (float) Math.Sin(value);
            }
        }

        public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num = (float) (point.X * (double) _c - point.Y * (double) _s);
            point.Y = (float) (point.X * (double) _s + point.Y * (double) _c);
            point.X = num;
        }

        public void Invert(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num = (float) (point.X * (double) _c + point.Y * (double) _s);
            point.Y = (float) (point.Y * (double) _c - point.X * (double) _s);
            point.X = num;
        }
    }
}