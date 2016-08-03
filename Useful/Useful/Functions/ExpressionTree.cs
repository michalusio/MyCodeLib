using System;
using System.Collections.Generic;
using System.Globalization;

namespace Useful.Functions
{
  public static class ExpressionTree
  {
    public static TreeNode ParseToTree(string formula)
    {
      Onp onp = new Onp();
      Queue<string> stringQueue = onp.Parse(formula);
      List<TreeNode> treeNodeList = new List<TreeNode>();
      foreach (string s in stringQueue)
      {
        if (s.Length == 1 && onp.Figures.ContainsKey(s[0]))
        {
          TreeNode treeNode = new TreeNode(s);
          switch (onp.FigureArgs[s[0]])
          {
            case 1:
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              break;
            case 2:
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Reverse();
              break;
            case 3:
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Reverse();
              break;
          }
          treeNodeList.Add(treeNode);
        }
        else
          treeNodeList.Add(new TreeNode(s));
      }
      return treeNodeList[treeNodeList.Count - 1];
    }

    public static TreeNode ParseToTree(string formula, Onp o)
    {
      Queue<string> stringQueue = o.Parse(formula);
      List<TreeNode> treeNodeList = new List<TreeNode>();
      foreach (string s in stringQueue)
      {
        if (s.Length == 1 && o.Figures.ContainsKey(s[0]))
        {
          TreeNode treeNode = new TreeNode(s);
          switch (o.FigureArgs[s[0]])
          {
            case 1:
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              break;
            case 2:
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Reverse();
              break;
            case 3:
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Add(treeNodeList[treeNodeList.Count - 1]);
              treeNodeList.RemoveAt(treeNodeList.Count - 1);
              treeNode.Leafs.Reverse();
              break;
          }
          treeNodeList.Add(treeNode);
        }
        else
          treeNodeList.Add(new TreeNode(s));
      }
      return treeNodeList[treeNodeList.Count - 1];
    }

    public static double SolveTree(TreeNode q, Dictionary<char, double> x)
    {
      if (q.Element.Length == 1 && x.ContainsKey(q.Element[0]))
        return x[q.Element[0]];
      double result;
      if (double.TryParse(q.Element, NumberStyles.Any, MMath.Nfi, out result))
        return result;
      char ch = q.Element[0];
      if (ch <= 37U)
      {
        if (ch != 35)
        {
          if (ch == 37)
            result = MMath.Mod(SolveTree(q.Leafs[0], x), SolveTree(q.Leafs[1], x));
        }
        else
          result = Math.Abs(SolveTree(q.Leafs[0], x) - SolveTree(q.Leafs[1], x)) > 2.80259692864963E-45 ? 1.0 : 0.0;
      }
      else
      {
        switch (ch)
        {
          case '*':
            result = SolveTree(q.Leafs[0], x) * SolveTree(q.Leafs[1], x);
            break;
          case '+':
            result = SolveTree(q.Leafs[0], x) + SolveTree(q.Leafs[1], x);
            break;
          case '-':
            result = SolveTree(q.Leafs[0], x) - SolveTree(q.Leafs[1], x);
            break;
          case '/':
            result = SolveTree(q.Leafs[0], x) / SolveTree(q.Leafs[1], x);
            break;
          case '<':
            result = SolveTree(q.Leafs[0], x) < SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case '=':
            result = Math.Abs(SolveTree(q.Leafs[0], x) - SolveTree(q.Leafs[1], x)) < 2.80259692864963E-45 ? 1.0 : 0.0;
            break;
          case '>':
            result = SolveTree(q.Leafs[0], x) > SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case 'A':
            result = Math.Abs(SolveTree(q.Leafs[0], x));
            break;
          case 'C':
            result = Math.Cos(SolveTree(q.Leafs[0], x));
            break;
          case 'E':
            result = Math.Ceiling(SolveTree(q.Leafs[0], x));
            break;
          case 'F':
            result = Math.Floor(SolveTree(q.Leafs[0], x));
            break;
          case 'G':
            result = Math.Sign(SolveTree(q.Leafs[0], x));
            break;
          case 'I':
            result = SolveTree(q.Leafs[0], x) > 0.0 ? SolveTree(q.Leafs[1], x) : SolveTree(q.Leafs[2], x);
            break;
          case 'L':
            result = Math.Log(SolveTree(q.Leafs[0], x));
            break;
          case 'N':
            result = -SolveTree(q.Leafs[0], x);
            break;
          case 'O':
            result = 1.0 / Math.Tan(SolveTree(q.Leafs[0], x));
            break;
          case 'R':
            result = Math.Round(SolveTree(q.Leafs[0], x));
            break;
          case 'S':
            result = Math.Sin(SolveTree(q.Leafs[0], x));
            break;
          case 'T':
            result = Math.Tan(SolveTree(q.Leafs[0], x));
            break;
          case 'U':
            result = Math.Sqrt(SolveTree(q.Leafs[0], x));
            break;
          case '[':
            result = SolveTree(q.Leafs[0], x) <= SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case ']':
            result = SolveTree(q.Leafs[0], x) >= SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case '^':
            result = Math.Pow(SolveTree(q.Leafs[0], x), SolveTree(q.Leafs[1], x));
            break;
        }
      }
      return result;
    }

    public static double SolveTree(TreeNode q, double x)
    {
      if (q.Element.Length == 1 && q.Element[0] == 120)
        return x;
      double result;
      if (double.TryParse(q.Element, NumberStyles.Any, MMath.Nfi, out result))
        return result;
      char ch = q.Element[0];
      if (ch <= 37U)
      {
        if (ch != 35)
        {
          if (ch == 37)
            result = MMath.Mod(SolveTree(q.Leafs[0], x), SolveTree(q.Leafs[1], x));
        }
        else
          result = Math.Abs(SolveTree(q.Leafs[0], x) - SolveTree(q.Leafs[1], x)) > 2.80259692864963E-45 ? 1.0 : 0.0;
      }
      else
      {
        switch (ch)
        {
          case '*':
            result = SolveTree(q.Leafs[0], x) * SolveTree(q.Leafs[1], x);
            break;
          case '+':
            result = SolveTree(q.Leafs[0], x) + SolveTree(q.Leafs[1], x);
            break;
          case '-':
            result = SolveTree(q.Leafs[0], x) - SolveTree(q.Leafs[1], x);
            break;
          case '/':
            result = SolveTree(q.Leafs[0], x) / SolveTree(q.Leafs[1], x);
            break;
          case '<':
            result = SolveTree(q.Leafs[0], x) < SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case '=':
            result = Math.Abs(SolveTree(q.Leafs[0], x) - SolveTree(q.Leafs[1], x)) < 2.80259692864963E-45 ? 1.0 : 0.0;
            break;
          case '>':
            result = SolveTree(q.Leafs[0], x) > SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case 'A':
            result = Math.Abs(SolveTree(q.Leafs[0], x));
            break;
          case 'C':
            result = Math.Cos(SolveTree(q.Leafs[0], x));
            break;
          case 'E':
            result = Math.Ceiling(SolveTree(q.Leafs[0], x));
            break;
          case 'F':
            result = Math.Floor(SolveTree(q.Leafs[0], x));
            break;
          case 'G':
            result = Math.Sign(SolveTree(q.Leafs[0], x));
            break;
          case 'I':
            result = SolveTree(q.Leafs[0], x) > 0.0 ? SolveTree(q.Leafs[1], x) : SolveTree(q.Leafs[2], x);
            break;
          case 'L':
            result = Math.Log(SolveTree(q.Leafs[0], x));
            break;
          case 'N':
            result = -SolveTree(q.Leafs[0], x);
            break;
          case 'O':
            result = 1.0 / Math.Tan(SolveTree(q.Leafs[0], x));
            break;
          case 'R':
            result = Math.Round(SolveTree(q.Leafs[0], x));
            break;
          case 'S':
            result = Math.Sin(SolveTree(q.Leafs[0], x));
            break;
          case 'T':
            result = Math.Tan(SolveTree(q.Leafs[0], x));
            break;
          case 'U':
            result = Math.Sqrt(SolveTree(q.Leafs[0], x));
            break;
          case '[':
            result = SolveTree(q.Leafs[0], x) <= SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case ']':
            result = SolveTree(q.Leafs[0], x) >= SolveTree(q.Leafs[1], x) ? 1.0 : 0.0;
            break;
          case '^':
            result = Math.Pow(SolveTree(q.Leafs[0], x), SolveTree(q.Leafs[1], x));
            break;
        }
      }
      return result;
    }

    public static TreeNode Optimize(TreeNode q)
    {
      for (int index = 0; index < q.Leafs.Count; ++index)
        q.Leafs[index] = Optimize(q.Leafs[index]);
      if (q.Element.Length > 1)
        return q;
      char ch = q.Element[0];
      if (ch <= 47U)
      {
        switch (ch)
        {
          case '%':
            double result1;
            bool flag1 = double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result1);
            double result2;
            bool flag2 = double.TryParse(q.Leafs[1].Element, NumberStyles.Any, MMath.Nfi, out result2);
            if (flag1 & flag2)
              return new TreeNode(MMath.Mod(result1, result2).ToString(MMath.Nfi));
            if (flag1 && Math.Abs(result1) < 4.94065645841247E-323 || flag2 && Math.Abs(result2 - 1.0) < 4.94065645841247E-323)
              return new TreeNode("0");
            if (flag1 && Math.Abs(result1 - 1.0) < 4.94065645841247E-323)
              return new TreeNode("1");
            break;
          case '*':
            double result3;
            bool flag3 = double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result3);
            double result4;
            bool flag4 = double.TryParse(q.Leafs[1].Element, NumberStyles.Any, MMath.Nfi, out result4);
            if (flag3 & flag4)
              return new TreeNode((result3 * result4).ToString(MMath.Nfi).Replace(',', '.'));
            if (flag3 && Math.Abs(result3) < 4.94065645841247E-323 || flag4 && Math.Abs(result4) < 4.94065645841247E-323)
              return new TreeNode("0");
            if (flag3 && Math.Abs(result3 - 1.0) < 4.94065645841247E-323)
              return q.Leafs[1];
            if (flag4 && Math.Abs(result4 - 1.0) < 4.94065645841247E-323)
              return q.Leafs[0];
            break;
          case '+':
            double result5;
            bool flag5 = double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result5);
            double result6;
            bool flag6 = double.TryParse(q.Leafs[1].Element, NumberStyles.Any, MMath.Nfi, out result6);
            if (flag5 & flag6)
              return new TreeNode((result5 + result6).ToString(MMath.Nfi));
            if (flag5 && Math.Abs(result5) < 4.94065645841247E-323)
              return q.Leafs[1];
            if (flag6)
            {
              if (Math.Abs(result6) < 4.94065645841247E-323)
                return q.Leafs[0];
              if (result6 < 0.0)
              {
                q.Element = "-";
                q.Leafs[1].Element = q.Leafs[1].Element.Remove(0, 1);
              }
            }
            break;
          case '-':
            double result7;
            bool flag7 = double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result7);
            double result8;
            bool flag8 = double.TryParse(q.Leafs[1].Element, NumberStyles.Any, MMath.Nfi, out result8);
            if (flag7 & flag8)
              return new TreeNode((result7 - result8).ToString(MMath.Nfi));
            if (flag7 && Math.Abs(result7) < 4.94065645841247E-323)
              return new TreeNode("N")
              {
                Leafs = {
                  q.Leafs[1]
                }
              };
            if (flag8)
            {
              if (Math.Abs(result8) < 4.94065645841247E-323)
                return q.Leafs[0];
              if (result8 < 0.0)
              {
                q.Element = "+";
                q.Leafs[1].Element = q.Leafs[1].Element.Remove(0, 1);
              }
            }
            break;
          case '/':
            double result9;
            bool flag9 = double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result9);
            double result10;
            bool flag10 = double.TryParse(q.Leafs[1].Element, NumberStyles.Any, MMath.Nfi, out result10);
            if (flag9 & flag10)
              return new TreeNode((result9 / result10).ToString(MMath.Nfi));
            if (flag9 && Math.Abs(result9) < 4.94065645841247E-323)
              return new TreeNode("0");
            if (flag10 && Math.Abs(result10 - 1.0) < 4.94065645841247E-323)
              return q.Leafs[0];
            break;
        }
      }
      else
      {
        switch (ch)
        {
          case 'A':
            double result11;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result11))
              return new TreeNode(Math.Abs(result11).ToString(MMath.Nfi));
            break;
          case 'C':
            double result12;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result12))
              return new TreeNode(Math.Cos(result12).ToString(MMath.Nfi));
            break;
          case 'E':
            double result13;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result13))
              return new TreeNode(Math.Ceiling(result13).ToString(MMath.Nfi));
            break;
          case 'F':
            double result14;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result14))
              return new TreeNode(Math.Floor(result14).ToString(MMath.Nfi));
            break;
          case 'G':
            double result15;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result15))
              return new TreeNode(Math.Sign(result15).ToString(MMath.Nfi));
            break;
          case 'I':
            double result16;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result16))
            {
              if (result16 <= 0.0)
                return q.Leafs[2];
              return q.Leafs[1];
            }
            break;
          case 'L':
            double result17;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result17))
              return new TreeNode(Math.Log(result17).ToString(MMath.Nfi));
            if (q.Leafs[0].Element == "e")
              return new TreeNode("1");
            break;
          case 'N':
            double result18;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result18))
              return new TreeNode((-result18).ToString(MMath.Nfi));
            break;
          case 'O':
            double result19;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result19))
              return new TreeNode((1.0 / Math.Tan(result19)).ToString(MMath.Nfi));
            break;
          case 'R':
            double result20;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result20))
              return new TreeNode(Math.Round(result20).ToString(MMath.Nfi));
            break;
          case 'S':
            double result21;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result21))
              return new TreeNode(Math.Sin(result21).ToString(MMath.Nfi));
            break;
          case 'T':
            double result22;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result22))
              return new TreeNode(Math.Tan(result22).ToString(MMath.Nfi));
            break;
          case 'U':
            double result23;
            if (double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result23))
              return new TreeNode(Math.Sqrt(result23).ToString(MMath.Nfi));
            break;
          case '^':
            if (q.Leafs[0].Element == "e" && q.Leafs[1].Element == "L")
              return q.Leafs[1].Leafs[0];
            if (q.Leafs[0].Element == "^")
            {
              TreeNode q1 = new TreeNode("^");
              q1.Leafs.Add(q.Leafs[0].Leafs[0]);
              q1.Leafs.Add(new TreeNode("*"));
              q1.Leafs[1].Leafs.Add(q.Leafs[0].Leafs[1]);
              q1.Leafs[1].Leafs.Add(q.Leafs[1]);
              return Optimize(q1);
            }
            double result24;
            bool flag11 = double.TryParse(q.Leafs[0].Element, NumberStyles.Any, MMath.Nfi, out result24);
            double result25;
            bool flag12 = double.TryParse(q.Leafs[1].Element, NumberStyles.Any, MMath.Nfi, out result25);
            if (flag11 & flag12)
              return new TreeNode(Math.Pow(result24, result25).ToString(MMath.Nfi));
            if (flag12 && Math.Abs(result25 - 1.0) < 4.94065645841247E-323)
              return q.Leafs[0];
            if (flag12 && Math.Abs(result25) < 4.94065645841247E-323)
              return new TreeNode("1");
            if (flag11 && Math.Abs(result24) < 4.94065645841247E-323)
              return new TreeNode("0");
            break;
        }
      }
      return q;
    }
  }
}
