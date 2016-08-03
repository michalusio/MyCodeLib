using System;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace Useful.Other
{
  public static class MMath
  {
        public static readonly NumberFormatInfo Nfi = new NumberFormatInfo()
        {
          NumberDecimalSeparator = ".",
          NumberDecimalDigits = 5
        };

        public static Random Rnd { get; private set; }

        static MMath()
        {
          Rnd = new Random();
        }

        public static Color HsvToRgb(double h, double s, double v)
        {
          double num1 = Mod(h, 360.0);
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
            double num6 = (num5 = (int) (num1 * (1.0 / 60.0))) - (double) num5;
            double num7 = v * (1.0 - s);
            double num8 = v * (1.0 - s * num6);
            double num9 = v * (1.0 - s * (1.0 - num6));
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
          return Color.FromArgb(Clamp((int) (num4 * byte.MaxValue), 0, byte.MaxValue), Clamp((int) (num3 * byte.MaxValue), 0, byte.MaxValue), Clamp((int) (num2 * byte.MaxValue), 0, byte.MaxValue));
        }

        public static int Clamp(int i, int min, int max)
        {
          if (i < min)
            return min;
          if (i <= max)
            return i;
          return max;
        }

        public static bool IsNumeric(object expression)
        {
          double result;
          return double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);
        }

        public static float Clamp(float i, float min, float max)
        {
          if (i < (double) min)
            return min;
          if (i <= (double) max)
            return i;
          return max;
        }

        public static byte[] SubBytes(int startIndex, byte[] buffer)
        {
          byte[] numArray = new byte[buffer.Length - startIndex];
          Array.Copy(buffer, startIndex, numArray, 0, buffer.Length - startIndex);
          return numArray;
        }

        public static byte[] ExtractStringBytes(int startIndex, byte[] buffer)
        {
          byte[] numArray = null;
          int index = startIndex;
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

        public static double PowerTwo(int a)
        {
          if (a >= 0)
            return 1 << a;
          return 1.0 / (1 << -a);
        }

        public static double Mod(double a, double b)
        {
          return a - b * Math.Floor(a / b);
        }

        public static void SetGaussSeed(int seed)
        {
          Rnd = new Random(seed);
        }

        public static double NextGaussian()
        {
          return Math.Sqrt(-2.0 * Math.Log(Rnd.NextDouble())) * Math.Cos(2.0 * Math.PI * Rnd.NextDouble());
        }

        public static double NextGaussian(double o, double u)
        {
          return NextGaussian() * o + u;
        }

        public static double Add(ref double location1, double value)
        {
            double newCurrentValue = 0;
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (Math.Abs(newCurrentValue - currentValue) < 0.000001)
                    return newValue;
            }
        }

        public static float Add(ref float location1, float value)
        {
            float newCurrentValue = 0;
            while (true)
            {
                float currentValue = newCurrentValue;
                float newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (Math.Abs(newCurrentValue - currentValue) < 0.000001f)
                    return newValue;
            }
        }
    }
}
