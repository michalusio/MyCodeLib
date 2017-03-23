using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Useful.Other
{
    /// <summary>
    ///     Class containing useful functions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Number format formatting with dots and 5 decimal places.
        /// </summary>
        public static readonly NumberFormatInfo Nfi = new NumberFormatInfo
        {
            NumberDecimalSeparator = ".",
            NumberDecimalDigits = 5
        };

        static Extensions()
        {
            Rnd = new Random();
        }

        /// <summary>
        ///     Ready random generator.
        /// </summary>
        public static Random Rnd { get; private set; }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static T MaxBy<T, R>(this IEnumerable<T> en, Func<T, R> evaluate) where R : IComparable<R>
        {
            return en.Select(t => new KeyValuePair<T, R>(t, evaluate(t)))
                .Aggregate((max, next) => next.Value.CompareTo(max.Value) > 0 ? next : max).Key;
        }

        public static T MinBy<T, R>(this IEnumerable<T> en, Func<T, R> evaluate) where R : IComparable<R>
        {
            return en.Select(t => new KeyValuePair<T, R>(t, evaluate(t)))
                .Aggregate((max, next) => next.Value.CompareTo(max.Value) < 0 ? next : max).Key;
        }

        /// <summary>
        ///     Convert hsv color to rgb color.
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="v">Value</param>
        public static Color HsvToRgb(double h, double s, double v)
        {
            var num1 = Mod(h, 360.0);
            double num2;
            double num3;
            double num4;
            if (v <= 0.0)
            {
                double num5;
                num2 = num5 = 0.0;
                num3 = num5;
                num4 = num5;
            }
            else if (s <= 0.0)
            {
                double num5;
                num2 = num5 = v;
                num3 = num5;
                num4 = num5;
            }
            else
            {
                int num5;
                var num6 = (num5 = (int) (num1 * (1.0 / 60.0))) - (double) num5;
                var num7 = v * (1.0 - s);
                var num8 = v * (1.0 - s * num6);
                var num9 = v * (1.0 - s * (1.0 - num6));
                switch (num5)
                {
                    case -1:
                        num4 = v;
                        num3 = num7;
                        num2 = num8;
                        break;
                    case 0:
                        num4 = v;
                        num3 = num9;
                        num2 = num7;
                        break;
                    case 1:
                        num4 = num8;
                        num3 = v;
                        num2 = num7;
                        break;
                    case 2:
                        num4 = num7;
                        num3 = v;
                        num2 = num9;
                        break;
                    case 3:
                        num4 = num7;
                        num3 = num8;
                        num2 = v;
                        break;
                    case 4:
                        num4 = num9;
                        num3 = num7;
                        num2 = v;
                        break;
                    case 5:
                        num4 = v;
                        num3 = num7;
                        num2 = num8;
                        break;
                    case 6:
                        num4 = v;
                        num3 = num9;
                        num2 = num7;
                        break;
                    default:
                        double num10;
                        num2 = num10 = v;
                        num3 = num10;
                        num4 = num10;
                        break;
                }
            }
            return Color.FromArgb(Clamp((int) (num4 * byte.MaxValue), 0, byte.MaxValue),
                Clamp((int) (num3 * byte.MaxValue), 0, byte.MaxValue),
                Clamp((int) (num2 * byte.MaxValue), 0, byte.MaxValue));
        }

        /// <summary>
        ///     Clamps an int to a range.
        /// </summary>
        /// <param name="i">Argument to clamp</param>
        /// <param name="min">Minimum of range</param>
        /// <param name="max">Maximum of range</param>
        public static int Clamp(int i, int min, int max)
        {
            return i < min ? min : (i <= max ? i : max);
        }

        /// <summary>
        ///     Quickly check if object is a numeric.
        /// </summary>
        /// <param name="expression">Object to check</param>
        public static bool IsNumeric(object expression)
        {
            double result;
            return double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo,
                out result);
        }

        /// <summary>
        ///     Clamps a float to a range.
        /// </summary>
        /// <param name="f">Argument to clamp</param>
        /// <param name="min">Minimum of range</param>
        /// <param name="max">Maximum of range</param>
        public static float Clamp(float f, float min, float max)
        {
            return f < (double) min ? min : (f <= (double) max ? f : max);
        }

        /// <summary>
        ///     Cuts off some starting bytes of an array.
        /// </summary>
        /// <param name="startIndex">Index to start new array from</param>
        /// <param name="buffer">Array to cut from</param>
        public static byte[] SubBytes(int startIndex, byte[] buffer)
        {
            var numArray = new byte[buffer.Length - startIndex];
            Array.Copy(buffer, startIndex, numArray, 0, buffer.Length - startIndex);
            return numArray;
        }

        /// <summary>
        ///     Extract two-byte-char null-terminater string from a given byte array.
        /// </summary>
        /// <param name="startIndex">Index of start of a string</param>
        /// <param name="buffer">Array to extract from</param>
        public static byte[] ExtractStringBytes(int startIndex, byte[] buffer)
        {
            byte[] numArray = null;
            var index = startIndex;
            while (index < buffer.Length)
            {
                if (buffer[index] == 0 && buffer[index + 1] == 0)
                {
                    numArray = new byte[index - startIndex];
                    Array.Copy(buffer, startIndex, numArray, 0, index - startIndex);
                    break;
                }
                index += 2;
            }
            return numArray;
        }

        /// <summary>
        ///     Calculates power of two, allowing negative indexes.
        /// </summary>
        /// <param name="a">2's power</param>
        public static double PowerTwo(int a)
        {
            if (a >= 0)
                return 1 << a;
            return 1.0 / (1 << -a);
        }

        /// <summary>
        ///     Calculate modulo of two real numbers.
        /// </summary>
        /// <param name="a">Dividend</param>
        /// <param name="b">Divisor</param>
        public static double Mod(double a, double b)
        {
            return a - b * Math.Floor(a / b);
        }

        /// <summary>
        ///     Sets new random seed.
        /// </summary>
        /// <param name="seed">New seed to use</param>
        public static void SetGaussSeed(int seed)
        {
            Rnd = new Random(seed);
        }

        /// <summary>
        ///     Returns normally-distributed random number with average of 0 and deviation of 1.
        /// </summary>
        public static double NextGaussian()
        {
            return Math.Sqrt(-2.0 * Math.Log(Rnd.NextDouble())) * Math.Cos(2.0 * Math.PI * Rnd.NextDouble());
        }

        /// <summary>
        ///     Returns normally-distributed random number with given average and deviation.
        /// </summary>
        /// <param name="o">Standard deviation</param>
        /// <param name="u">Average</param>
        public static double NextGaussian(double o, double u)
        {
            return NextGaussian() * o + u;
        }

        /// <summary>
        ///     Thread-safely adds a value to a double variable.
        /// </summary>
        /// <param name="variable">Variable to add to</param>
        /// <param name="value">Value to add</param>
        public static double Add(ref double variable, double value)
        {
            double newCurrentValue = 0;
            while (true)
            {
                var currentValue = newCurrentValue;
                var newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref variable, newValue, currentValue);
                if (Math.Abs(newCurrentValue - currentValue) < 0.000001)
                    return newValue;
            }
        }

        /// <summary>
        ///     Thread-safely adds a value to a float variable.
        /// </summary>
        /// <param name="variable">Variable to add to</param>
        /// <param name="value">Value to add</param>
        public static float Add(ref float variable, float value)
        {
            float newCurrentValue = 0;
            while (true)
            {
                var currentValue = newCurrentValue;
                var newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref variable, newValue, currentValue);
                if (Math.Abs(newCurrentValue - currentValue) < 0.000001f)
                    return newValue;
            }
        }
    }
}