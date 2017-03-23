using System;

namespace Useful.Other
{
    /// <summary>
    ///     Class for pixel operations.
    /// </summary>
    public class Pixel
    {
        /// <summary>
        ///     B value of pixel.
        /// </summary>
        public int B;

        /// <summary>
        ///     G value of pixel.
        /// </summary>
        public int G;

        /// <summary>
        ///     R value of pixel.
        /// </summary>
        public int R;

        /// <summary>
        ///     Creates pixel instance with given rgb values.
        /// </summary>
        /// <param name="r">R value</param>
        /// <param name="g">G value</param>
        /// <param name="b">B value</param>
        public Pixel(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        ///     Adds rgb values of the given pixel to this instance.
        /// </summary>
        /// <param name="p">Pixel to add from</param>
        public Pixel Add(Pixel p)
        {
            R = R + p.R;
            G = G + p.G;
            B = B + p.B;
            return this;
        }

        /// <summary>
        ///     Divides rgb values by argument.
        /// </summary>
        /// <param name="x">Argument</param>
        public Pixel Div(int x)
        {
            R = R / x;
            G = G / x;
            B = B / x;
            return this;
        }

        public override string ToString()
        {
            var num = Math.Min(R, Math.Min(G, B));
            return "[" + (num < 0 ? R - num : R) + "|" + (num < 0 ? G - num : G) + "|" + (num < 0 ? B - num : B) + "]";
        }

        /// <summary>
        ///     Returns clamped R value.
        /// </summary>
        public byte GetR()
        {
            return (byte) Math.Min(byte.MaxValue, Math.Max(R, 0));
        }

        /// <summary>
        ///     Returns clamped G value.
        /// </summary>
        public byte GetG()
        {
            return (byte) Math.Min(byte.MaxValue, Math.Max(G, 0));
        }

        /// <summary>
        ///     Returns clamped B value.
        /// </summary>
        public byte GetB()
        {
            return (byte) Math.Min(byte.MaxValue, Math.Max(B, 0));
        }
    }
}