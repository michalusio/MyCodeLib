using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
    public class MobiusTransform2D : INvertibleTransformation2D
    {
        public PPoint2D A;
        public PPoint2D B;
        public PPoint2D C;
        public PPoint2D D;

        public MobiusTransform2D(PPoint2D a, PPoint2D b, PPoint2D c, PPoint2D d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num1 = (float) (A.X * (double) point.X - A.Y * (double) point.Y) + B.X;
            var num2 = (float) (A.Y * (double) point.X + A.X * (double) point.Y) + B.Y;
            var num3 = (float) (C.X * (double) point.X - C.Y * (double) point.Y) + D.X;
            var num4 = (float) (C.X * (double) point.Y + C.Y * (double) point.X) + D.Y;
            var num5 = (float) (1.0 / (num3 * (double) num3 + num4 * (double) num4));
            point.X = (float) (num1 * (double) num3 + num2 * (double) num4) * num5;
            point.Y = (float) (num2 * (double) num3 - num1 * (double) num4) * num5;
        }

        public void Invert(ref PPoint2D point, List<PPoint2D> allPoints)
        {
            var num1 = (float) (D.X * (double) point.X - D.Y * (double) point.Y) - B.X;
            var num2 = (float) (D.Y * (double) point.X + D.X * (double) point.Y) - B.Y;
            var num3 = (float) (-(double) C.X * point.X + C.Y * (double) point.Y) + A.X;
            var num4 = (float) (-(double) C.X * point.Y - C.Y * (double) point.X) + A.Y;
            var num5 = (float) (1.0 / (num3 * (double) num3 + num4 * (double) num4));
            point.X = (float) (num1 * (double) num3 + num2 * (double) num4) * num5;
            point.Y = (float) (num2 * (double) num3 - num1 * (double) num4) * num5;
        }
    }
}