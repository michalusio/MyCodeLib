using System;
using System.Collections.Generic;
using System.Drawing;
using Useful.Plotting.Transformations;

namespace Useful.Plotting
{
    public class PPoint2D
    {
        public Color Color = Color.Black;
        public bool Visible = true;
        public float X;
        public float Y;

        public PPoint2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public PPoint2D(float x, float y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        public PPoint2D(float x, float y, Color color, bool vis)
        {
            X = x;
            Y = y;
            Color = color;
            Visible = vis;
        }

        public static Plot2D operator +(PPoint2D a, PPoint2D b)
        {
            Plot2D plot2D = new Plot2D();
            PPoint2D p1 = a;
            plot2D.AddPoint(p1);
            PPoint2D p2 = b;
            plot2D.AddPoint(p2);
            return plot2D;
        }

        public static PPoint2D operator *(PPoint2D a, ITransformation2D b)
        {
            var allPoints = new List<PPoint2D> {a};
            b.Transform(ref a, allPoints);
            return a;
        }

        public static PPoint2D operator /(PPoint2D a, ITransformation2D b)
        {
            var allPoints = new List<PPoint2D> {a};
            INvertibleTransformation2D transformation2D = b as INvertibleTransformation2D;
            if (transformation2D == null)
                throw new InvalidCastException("Transformation not invertible!");
            transformation2D.Invert(ref a, allPoints);
            return a;
        }

        public float SquareLen()
        {
            return (float) (X * (double) X + Y * (double) Y);
        }
    }
}