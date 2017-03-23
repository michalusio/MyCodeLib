using System;
using System.Collections.Generic;
using System.Drawing;
using Useful.Plotting.Transformations;

namespace Useful.Plotting
{
    public class PPoint3D
    {
        public Color Color = Color.Black;
        public bool Visible = true;
        public float X;
        public float Y;
        public float Z;

        public PPoint3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public PPoint3D(float x, float y, float z, Color color)
        {
            X = x;
            Y = y;
            Z = z;
            Color = color;
        }

        public PPoint3D(float x, float y, float z, Color color, bool vis)
        {
            X = x;
            Y = y;
            Z = z;
            Color = color;
            Visible = vis;
        }

        public static Plot3D operator +(PPoint3D a, PPoint3D b)
        {
            Plot3D plot3D = new Plot3D();
            PPoint3D p1 = a;
            plot3D.AddPoint(p1);
            PPoint3D p2 = b;
            plot3D.AddPoint(p2);
            return plot3D;
        }

        public static PPoint3D operator *(PPoint3D a, ITransformation3D b)
        {
            var allPoints = new List<PPoint3D> {a};
            b.Transform(ref a, allPoints);
            return a;
        }

        public static PPoint3D operator /(PPoint3D a, ITransformation3D b)
        {
            var allPoints = new List<PPoint3D> {a};
            INvertibleTransformation3D transformation3D = b as INvertibleTransformation3D;
            if (transformation3D == null)
                throw new InvalidCastException("Transformation not invertible!");
            transformation3D.Invert(ref a, allPoints);
            return a;
        }

        public float SquareLen()
        {
            return (float) (X * (double) X + Y * (double) Y + Z * (double) Z);
        }
    }
}