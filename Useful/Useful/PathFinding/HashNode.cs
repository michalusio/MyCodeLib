using System;
using System.Collections.Generic;

namespace Useful.PathFinding
{
  public class HashNode : MainNode
  {
    public HashSet<HashNode> Connected = new HashSet<HashNode>();

    public HashNode()
    {
    }

    public HashNode(byte id, short x, short y)
    {
      Id = id;
      X = x;
      Y = y;
    }

    public HashNode(byte id, HashSet<HashNode> tab)
    {
      Id = id;
      Connected = tab;
    }

    public override string ToString()
    {
      return "HN(" + Id + "|" + X + "|" + Y + ")";
    }

    public override IEnumerable<MainNode> GetNeighbors()
    {
      return Connected;
    }

    public override bool NodeEqual(MainNode b)
    {
      return ((HashNode) b).Id == Id;
    }

    public override float Distance(MainNode b)
    {
      HashNode hashNode = (HashNode) b;
      return Math.Abs(hashNode.X - X) + Math.Abs(hashNode.Y - Y);
    }

    public override float Heuristic(MainNode goal)
    {
      HashNode hashNode = (HashNode) goal;
      return Math.Abs(hashNode.X - X) + Math.Abs(hashNode.Y - Y);
    }
  }
}
