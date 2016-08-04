using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Useful.Other;

namespace Useful.Functions
{
    /// <summary>
    /// Class used to parse and solve string formulas
    /// </summary>
  public class Onp
  {
    internal readonly Dictionary<char, int> Figures = new Dictionary<char, int>();
    internal readonly Dictionary<char, int> FigureArgs = new Dictionary<char, int>();
    public readonly Dictionary<string, string> FigureDictionary = new Dictionary<string, string>();

    public Onp()
    {
      Figures.Add('I', 1);
      FigureArgs.Add('I', 3);
      FigureDictionary.Add("if(", "I(");
      Figures.Add(')', 2);
      Figures.Add(',', 2);
      Figures.Add('>', 2);
      FigureArgs.Add('>', 2);
      Figures.Add('<', 2);
      FigureArgs.Add('<', 2);
      Figures.Add(']', 2);
      FigureArgs.Add(']', 2);
      FigureDictionary.Add(">=", "]");
      Figures.Add('[', 2);
      FigureArgs.Add('[', 2);
      FigureDictionary.Add("<=", "[");
      Figures.Add('#', 2);
      FigureArgs.Add('#', 2);
      FigureDictionary.Add("!=", "#");
      Figures.Add('=', 2);
      FigureArgs.Add('=', 2);
      FigureDictionary.Add("==", "=");
      Figures.Add('+', 3);
      FigureArgs.Add('+', 2);
      Figures.Add('-', 3);
      FigureArgs.Add('-', 2);
      Figures.Add('*', 4);
      FigureArgs.Add('*', 2);
      Figures.Add('N', 4);
      FigureArgs.Add('N', 1);
      Figures.Add('/', 4);
      FigureArgs.Add('/', 2);
      Figures.Add('%', 4);
      FigureArgs.Add('%', 2);
      Figures.Add('^', 5);
      FigureArgs.Add('^', 2);
      Figures.Add('A', 6);
      FigureArgs.Add('A', 1);
      FigureDictionary.Add("abs(", "A(");
      Figures.Add('G', 6);
      FigureArgs.Add('G', 1);
      FigureDictionary.Add("sgn(", "G(");
      Figures.Add('O', 6);
      FigureArgs.Add('O', 1);
      FigureDictionary.Add("ctg(", "O(");
      Figures.Add('F', 6);
      FigureArgs.Add('F', 1);
      FigureDictionary.Add("floor(", "F(");
      Figures.Add('E', 6);
      FigureArgs.Add('E', 1);
      FigureDictionary.Add("ceil(", "E(");
      Figures.Add('R', 6);
      FigureArgs.Add('R', 1);
      FigureDictionary.Add("round(", "R(");
      Figures.Add('U', 6);
      FigureArgs.Add('U', 1);
      FigureDictionary.Add("sqrt(", "U(");
      Figures.Add('S', 6);
      FigureArgs.Add('S', 1);
      FigureDictionary.Add("sin(", "S(");
      Figures.Add('C', 6);
      FigureArgs.Add('C', 1);
      FigureDictionary.Add("cos(", "C(");
      Figures.Add('T', 6);
      FigureArgs.Add('T', 1);
      FigureDictionary.Add("tan(", "T(");
      Figures.Add('L', 6);
      FigureArgs.Add('L', 1);
      FigureDictionary.Add("ln(", "L(");
      Figures.Add('(', 7);
    }
    
    /// <summary>
    /// Parses formula to queue of operations.
    /// </summary>
    /// <param name="formula">Formula to parse</param>
    public Queue<string> Parse(string formula)
    {
      StringBuilder stringBuilder = new StringBuilder(formula.ToLowerInvariant());
      foreach (KeyValuePair<string, string> figure in FigureDictionary)
        stringBuilder = stringBuilder.Replace(figure.Key, figure.Value);
      if ( stringBuilder[0] == 45)
        stringBuilder[0] = 'N';
      for (int index = 1; index < stringBuilder.Length; ++index)
      {
        if (stringBuilder[index] == 45 && Figures.ContainsKey(stringBuilder[index - 1]) && stringBuilder[index - 1] != 41)
          stringBuilder[index] = 'N';
      }
      formula = stringBuilder.ToString();
      Queue<string> stringQueue = new Queue<string>();
      Stack<char> source = new Stack<char>();
      string str = "";
      foreach (char ch in formula)
      {
        if (Figures.ContainsKey(ch))
        {
          if (str.Length > 0)
          {
            stringQueue.Enqueue(str);
            str = "";
          }
          while (source.Any())
          {
            char key = source.Peek();
            if (ch == 41 && key == 40)
            {
                source.Pop();
                break;
            }
              if (ch != 78 && (key != 73 || ch != 73))
            {
              int num1;
              Figures.TryGetValue(key, out num1);
              int num2;
              Figures.TryGetValue(ch, out num2);
              if (num1 >= num2 && key != 40)
              {
                  stringQueue.Enqueue(key.ToString());
                  source.Pop();
              }
              else
                break;
            }
            else
              break;
          }
          if (ch != 41 && ch != 44)
            source.Push(ch);
        }
        else if (char.IsDigit(ch) || ch == 46)
        {
          str += ch.ToString();
        }
        else
        {
          if (str.Length > 0)
          {
            stringQueue.Enqueue(str);
            str = "";
          }
          if (ch != 32)
            stringQueue.Enqueue(ch.ToString());
        }
      }
      if (str.Length > 0)
        stringQueue.Enqueue(str);
      while (source.Any())
        stringQueue.Enqueue(source.Pop().ToString());
      return stringQueue;
    }
    
    /// <summary>
    /// Solves given formula with given variables.
    /// </summary>
    /// <param name="formula">Queue to solve</param>
    /// <param name="x">Variable dictionary</param>
    public double Solve(Queue<string> formula, Dictionary<char, double> x)
    {
      Stack<double> source = new Stack<double>();
      foreach (string str in formula)
      {
        if (str.Length == 1)
        {
          char ch = str[0];
          if (x.ContainsKey(ch))
          {
            double num;
            x.TryGetValue(ch, out num);
            source.Push(num);
          }
          else if (Figures.ContainsKey(ch))
          {
            if (ch <= 37U)
            {
              if (ch != 35)
              {
                if (ch == 37)
                {
                  double b = source.Pop();
                  source.Push(MMath.Mod(source.Pop(), b));
                }
              }
              else
              {
                double num1 = source.Pop();
                double num2 = source.Pop();
                source.Push(Math.Abs(num2 - num1) > 2.80259692864963E-45 ? 1.0 : 0.0);
              }
            }
            else
            {
              switch (ch)
              {
                case '*':
                  source.Push(source.Pop() * source.Pop());
                  continue;
                case '+':
                  source.Push(source.Pop() + source.Pop());
                  continue;
                case '-':
                  double num3 = source.Pop();
                  source.Push(source.Pop() - num3);
                  continue;
                case '/':
                  double num4 = source.Pop();
                  source.Push(source.Pop() / num4);
                  continue;
                case '<':
                  double num5 = source.Pop();
                  double num6 = source.Pop();
                  source.Push(num6 < num5 ? 1.0 : 0.0);
                  continue;
                case '=':
                  double num7 = source.Pop();
                  double num8 = source.Pop();
                  source.Push(Math.Abs(num8 - num7) < 2.80259692864963E-45 ? 1.0 : 0.0);
                  continue;
                case '>':
                  double num9 = source.Pop();
                  double num10 = source.Pop();
                  source.Push(num10 > num9 ? 1.0 : 0.0);
                  continue;
                case 'A':
                  source.Push(Math.Abs(source.Pop()));
                  continue;
                case 'C':
                  source.Push(Math.Cos(source.Pop()));
                  continue;
                case 'E':
                  source.Push(Math.Ceiling(source.Pop()));
                  continue;
                case 'F':
                  source.Push(Math.Floor(source.Pop()));
                  continue;
                case 'G':
                  source.Push(Math.Sign(source.Pop()));
                  continue;
                case 'I':
                  double num11 = source.Pop();
                  double num12 = source.Pop();
                  source.Push(source.Pop() > 0.0 ? num12 : num11);
                  continue;
                case 'L':
                  source.Push(Math.Log(source.Pop()));
                  continue;
                case 'N':
                  source.Push(-source.Pop());
                  continue;
                case 'O':
                  source.Push(1.0 / Math.Tan(source.Pop()));
                  continue;
                case 'R':
                  source.Push(Math.Round(source.Pop()));
                  continue;
                case 'S':
                  source.Push(Math.Sin(source.Pop()));
                  continue;
                case 'T':
                  source.Push(Math.Tan(source.Pop()));
                  continue;
                case 'U':
                  source.Push(Math.Sqrt(source.Pop()));
                  continue;
                case '[':
                  double num13 = source.Pop();
                  double num14 = source.Pop();
                  source.Push(num14 <= num13 ? 1.0 : 0.0);
                  continue;
                case ']':
                  double num15 = source.Pop();
                  double num16 = source.Pop();
                  source.Push(num16 >= num15 ? 1.0 : 0.0);
                  continue;
                case '^':
                  double y = source.Pop();
                  source.Push(Math.Pow(source.Pop(), y));
                  continue;
                default:
                  continue;
              }
            }
          }
          else
          {
            if (!char.IsDigit(ch))
              throw new Exception("Illegal character!");
            source.Push(double.Parse(ch.ToString()));
          }
        }
        else
          source.Push(double.Parse(str.Replace(".", ",")));
      }
      if (source.Count() != 1)
        throw new Exception("Wrong number of operations!");
      return source.Pop();
    }

    /// <summary>
    /// Solves given formula with given variable.
    /// </summary>
    /// <param name="formula">Queue to solve</param>
    /// <param name="x">Variable</param>
    public double Solve(Queue<string> formula, double x)
    {
      Stack<double> source = new Stack<double>();
      foreach (string str in formula)
      {
        if (str.Length == 1)
        {
          char ch = str[0];
          if (ch == 120)
            source.Push(x);
          else if (Figures.ContainsKey(ch))
          {
              if (ch <= 85U)
            {
              switch (ch)
              {
                case '%':
                  double b = source.Pop();
                  source.Push(MMath.Mod(source.Pop(), b));
                  continue;
                case '*':
                  source.Push(source.Pop() * source.Pop());
                  continue;
                case '+':
                  source.Push(source.Pop() + source.Pop());
                  continue;
                case '-':
                  double num1 = source.Pop();
                  source.Push(source.Pop() - num1);
                  continue;
                case '/':
                  double num2 = source.Pop();
                  source.Push(source.Pop() / num2);
                  continue;
                case '<':
                  double num3 = source.Pop();
                  double num4 = source.Pop();
                  source.Push(num4 < num3 ? 1.0 : 0.0);
                  continue;
                case '>':
                  double num5 = source.Pop();
                  double num6 = source.Pop();
                  source.Push(num6 > num5 ? 1.0 : 0.0);
                  continue;
                case 'C':
                  source.Push(Math.Cos(source.Pop()));
                  continue;
                case 'E':
                  source.Push(Math.Ceiling(source.Pop()));
                  continue;
                case 'F':
                  source.Push(Math.Floor(source.Pop()));
                  continue;
                case 'G':
                  source.Push(Math.Sign(source.Pop()));
                  continue;
                case 'I':
                  double num7 = source.Pop();
                  double num8 = source.Pop();
                  source.Push(source.Pop() > 0.0 ? num8 : num7);
                  continue;
                case 'L':
                  source.Push(Math.Log(source.Pop()));
                  continue;
                case 'N':
                  source.Push(-source.Pop());
                  continue;
                case 'O':
                  source.Push(1.0 / Math.Tan(source.Pop()));
                  continue;
                case 'R':
                  source.Push(Math.Round(source.Pop()));
                  continue;
                case 'S':
                  source.Push(Math.Sin(source.Pop()));
                  continue;
                case 'T':
                  source.Push(Math.Tan(source.Pop()));
                  continue;
                case 'U':
                  source.Push(Math.Sqrt(source.Pop()));
                  continue;
                default:
                  continue;
              }
            }
              switch (ch)
              {
                  case '[':
                      double num9 = source.Pop();
                      double num10 = source.Pop();
                      source.Push(num10 <= num9 ? 1.0 : 0.0);
                      continue;
                  case ']':
                      double num11 = source.Pop();
                      double num12 = source.Pop();
                      source.Push(num12 >= num11 ? 1.0 : 0.0);
                      continue;
                  case '^':
                      double y = source.Pop();
                      source.Push(Math.Pow(source.Pop(), y));
                      continue;
                  case '{':
                      double num13 = source.Pop();
                      double num14 = source.Pop();
                      source.Push(Math.Abs(num14 - num13) > 2.80259692864963E-45 ? 1.0 : 0.0);
                      continue;
                  case '}':
                      double num15 = source.Pop();
                      double num16 = source.Pop();
                      source.Push(Math.Abs(num16 - num15) < 2.80259692864963E-45 ? 1.0 : 0.0);
                      continue;
                  default:
                      continue;
              }
          }
          else
          {
            if (!char.IsDigit(ch))
              throw new Exception("Illegal character!");
            source.Push(double.Parse(ch.ToString()));
          }
        }
        else
          source.Push(double.Parse(str.Replace(".", ",")));
      }
      if (source.Count() != 1)
        throw new Exception("Wrong number of operations!");
      return source.Pop();
    }
  }
}
