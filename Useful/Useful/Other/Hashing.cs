using System;

namespace Useful.Other
{
    /// <summary>
    ///     Class to calculate hash of string using Pearson hashing.
    /// </summary>
    public static class Hashing
    {
        private static readonly byte[] _ = new byte[256];

        /// <summary>
        ///     Permute hashing table to guarantee random hashing.
        /// </summary>
        /// <param name="rnd">Random generator used in permuting</param>
        public static void Permute(Random rnd)
        {
            for (var index = 0; index < 256; ++index)
                _[index] = (byte) index;
            for (var index = 0; index < 256; ++index)
            {
                var num1 = (byte) rnd.Next(256);
                var num2 = _[index];
                _[index] = _[num1];
                _[num1] = num2;
            }
        }

        /// <summary>
        ///     Calculate byte hash of a string.
        /// </summary>
        /// <param name="str">String to hash</param>
        public static byte PearsonB(string str)
        {
            var h = _[str[0]];
            for (var index = 1; index < str.Length; ++index)
                h = _[h ^ str[index]];
            return h;
        }

        /// <summary>
        ///     Calculate 2 byte hash of a string.
        /// </summary>
        /// <param name="str">String to hash</param>
        public static ushort PearsonS(string str)
        {
            var num1 = (byte) str[0];
            var num2 = (byte) ((str[0] + 1) & byte.MaxValue);
            for (var index = 1; index < str.Length; ++index)
            {
                num1 = _[num1 ^ str[index]];
                num2 = _[num2 ^ str[index]];
            }
            return (ushort) ((num2 << 8) + num1);
        }

        /// <summary>
        ///     Calculate 4 byte hash of a string.
        /// </summary>
        /// <param name="str">String to hash</param>
        public static uint PearsonI(string str)
        {
            var num1 = (byte) str[0];
            var num2 = (byte) ((str[0] + 1) & byte.MaxValue);
            var num3 = (byte) ((str[0] + 2) & byte.MaxValue);
            var num4 = (byte) ((str[0] + 3) & byte.MaxValue);
            for (var index = 1; index < str.Length; ++index)
            {
                num1 = _[num1 ^ str[index]];
                num2 = _[num2 ^ str[index]];
                num3 = _[num3 ^ str[index]];
                num4 = _[num4 ^ str[index]];
            }
            return (uint) ((num4 << 24) + (num3 << 16) + (num2 << 8) + num1);
        }

        /// <summary>
        ///     Calculate 8 byte hash of a string.
        /// </summary>
        /// <param name="str">String to hash</param>
        public static ulong PearsonL(string str)
        {
            var num1 = (byte) str[0];
            var num2 = (byte) ((str[0] + 1) & byte.MaxValue);
            var num3 = (byte) ((str[0] + 2) & byte.MaxValue);
            var num4 = (byte) ((str[0] + 3) & byte.MaxValue);
            var num5 = (byte) ((str[0] + 4) & byte.MaxValue);
            var num6 = (byte) ((str[0] + 5) & byte.MaxValue);
            var num7 = (byte) ((str[0] + 6) & byte.MaxValue);
            var num8 = (byte) ((str[0] + 7) & byte.MaxValue);
            for (var index = 1; index < str.Length; ++index)
            {
                num1 = _[num1 ^ str[index]];
                num2 = _[num2 ^ str[index]];
                num3 = _[num3 ^ str[index]];
                num4 = _[num4 ^ str[index]];
                num5 = _[num5 ^ str[index]];
                num6 = _[num6 ^ str[index]];
                num7 = _[num7 ^ str[index]];
                num8 = _[num8 ^ str[index]];
            }
            return
                (ulong)
                ((num8 << 24) + (num7 << 16) + (num6 << 8) + num5 + (num4 << 24) + (num3 << 16) + (num2 << 8) + num1);
        }

        //TODO: PearsonX for hashing string into byte array
    }
}