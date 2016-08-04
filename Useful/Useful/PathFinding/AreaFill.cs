using System.Collections.Generic;

namespace Useful.PathFinding
{
    /// <summary>
    /// Class containing area division algorithm
    /// </summary>
  public class AreaFill
  {
    private readonly List<HashSet<IAreaNode>> _areas = new List<HashSet<IAreaNode>>();
    private readonly Queue<IAreaNode> _q = new Queue<IAreaNode>();
    
    /// <summary>
    /// Section grid by generating areas using FloodFill.
    /// </summary>
    /// <param name="fields">Array forming 2D grid</param>
    public void GenerateAreas(IAreaNode[][] fields)
    {
      _areas.Clear();
      HashSet<IAreaNode> areaNodeSet = new HashSet<IAreaNode>();
      _q.Clear();
      int index = -1;
      foreach (IAreaNode[] field in fields)
      {
        foreach (IAreaNode areaNode1 in field)
        {
            if (areaNodeSet.Contains(areaNode1)) continue;
            ++index;
            _areas.Add(new HashSet<IAreaNode>());
            _q.Clear();
            _q.Enqueue(areaNode1);
            while (_q.Count > 0)
            {
                IAreaNode areaNode2 = _q.Dequeue();
                if (areaNodeSet.Contains(areaNode2)) continue;
                if (areaNode2.X > 0 && fields[areaNode2.X - 1][areaNode2.Y].Id == areaNode1.Id)
                    _q.Enqueue(fields[areaNode2.X - 1][areaNode2.Y]);
                if (areaNode2.Y > 0 && fields[areaNode2.X][areaNode2.Y - 1].Id == areaNode1.Id)
                    _q.Enqueue(fields[areaNode2.X][areaNode2.Y - 1]);
                if (areaNode2.X < fields.Length - 1 && fields[areaNode2.X + 1][areaNode2.Y].Id == areaNode1.Id)
                    _q.Enqueue(fields[areaNode2.X + 1][areaNode2.Y]);
                if (areaNode2.Y < fields[areaNode2.X].Length - 1 && fields[areaNode2.X][areaNode2.Y + 1].Id == areaNode1.Id)
                    _q.Enqueue(fields[areaNode2.X][areaNode2.Y + 1]);
                areaNodeSet.Add(areaNode2);
                _areas[index].Add(areaNode2);
            }
        }
      }
    }
    
    /// <summary>
    /// Checks in what area the node resides.
    /// </summary>
    /// <param name="x">The node to be checked</param>
    public int GetArea(IAreaNode x)
    {
      for (int index = 0; index < _areas.Count; ++index)
      {
        if (_areas[index].Contains(x))
          return index;
      }
      return -1;
    }
  }
}
