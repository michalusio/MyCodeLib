using System;
using System.Collections.Generic;

namespace Useful.PathFinding
{
  public class Manhattan2DNode : MainNode,IAreaNode
  {
    public static Manhattan2DNode[,] Map;
    public byte Wall;
    public static bool CanDiagonal;

    public byte Id { get; set; }

    public short X { get; set; }

    public short Y { get; set; }

    public Manhattan2DNode()
    {
    }

    public Manhattan2DNode(short x, short y,Manhattan2DNode[,] tab)
    {
          X = x;
          Y = y;
          Map = tab;
    }

    public override string ToString()
    {
      return "M2DN(" +  X + "|" +  Y + ")";
    }

    internal override IEnumerable<MainNode> GetNeighbors()
    {
      List<Manhattan2DNode> manhattan2DnodeList = new List<Manhattan2DNode>();
      if (InBounds(X + 1, Y) && Map[X + 1,Y].Wall!=0xFF)
        manhattan2DnodeList.Add(Map[X + 1,Y]);
      if (InBounds(X - 1, Y) && Map[X - 1,Y].Wall != 0xFF)
        manhattan2DnodeList.Add(Map[X - 1,Y]);
      if (InBounds(X, Y + 1) && Map[X,Y + 1].Wall != 0xFF)
        manhattan2DnodeList.Add(Map[X,Y + 1]);
      if (InBounds(X, Y - 1) && Map[X,Y - 1].Wall != 0xFF)
        manhattan2DnodeList.Add(Map[X,Y - 1]);
      if (CanDiagonal)
      {
        if (InBounds(X + 1, Y + 1) && Map[X + 1,Y + 1].Wall != 0xFF)
          manhattan2DnodeList.Add(Map[X + 1,Y + 1]);
        if (InBounds(X - 1, Y - 1) && Map[X - 1,Y - 1].Wall != 0xFF)
          manhattan2DnodeList.Add(Map[X - 1,Y - 1]);
        if (InBounds(X - 1, Y + 1) && Map[X - 1,Y + 1].Wall != 0xFF)
          manhattan2DnodeList.Add(Map[X - 1,Y + 1]);
        if (InBounds(X + 1, Y - 1) && Map[X + 1,Y - 1].Wall != 0xFF)
          manhattan2DnodeList.Add(Map[X + 1,Y - 1]);
      }
      return manhattan2DnodeList;
    }

    private bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Map.GetLength(0) && y < Map.GetLength(1);
    }

      public override bool NodeEqual(MainNode b)
    {
        Manhattan2DNode manhattan2Dnode = (Manhattan2DNode) b;
        return manhattan2Dnode.X == X && manhattan2Dnode.Y == Y;
    }

      public override float Distance(MainNode b)
    {
      return (10 + ((Manhattan2DNode)b).Wall)*0.1f;
    }

    public override float Heuristic(MainNode goal)
    {
      Manhattan2DNode manhattan2Dnode = (Manhattan2DNode) goal;
      return Math.Abs(manhattan2Dnode.X - X) + Math.Abs(manhattan2Dnode.Y - Y);
    }
  }
}
