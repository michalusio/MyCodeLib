using System.Collections.Generic;
using System.Drawing;

namespace Useful.Plotting
{
    public class Animator
    {
        public int Frame;
        public List<Bitmap> Frames = new List<Bitmap>();

        public static Animator operator +(Animator a, Bitmap b)
        {
            a.Frames.Add(b);
            return a;
        }

        public Bitmap NextFrame()
        {
            Bitmap bitmap = Frames[Frame];
            Frame = Frame + 1;
            if (Frame != Frames.Count)
                return bitmap;
            Frame = 0;
            return bitmap;
        }
    }
}