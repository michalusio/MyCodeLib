using System.Collections.Generic;

namespace Useful.Plotting
{
    internal class DistComparer : IComparer<PPoint3D>
    {
        public float CamX;
        public float CamY;
        public float CamZ;

        public int Compare(PPoint3D x, PPoint3D y)
        {
            var num1 = x.X - CamX;
            var num2 = y.X - CamX;
            var num3 = x.Y - CamY;
            var num4 = y.Y - CamY;
            var num5 = x.Z - CamZ;
            var num6 = y.Z - CamZ;
            return
                -((float) (num1 * (double) num1 + num3 * (double) num3 + num5 * (double) num5)).CompareTo(
                    (float) (num2 * (double) num2 + num4 * (double) num4 + num6 * (double) num6));
        }
    }
}