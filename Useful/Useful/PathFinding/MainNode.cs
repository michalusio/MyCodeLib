using System.Collections.Generic;

namespace Useful.PathFinding
{
  public abstract class MainNode:IAreaNode
  {
    internal float G { get; set; }

    internal float F { get; set; }

    public MainNode CameFrom { get; set; }

    public abstract IEnumerable<MainNode> GetNeighbors();

    public abstract bool NodeEqual(MainNode b);

    public abstract float Distance(MainNode b);

    public abstract float Heuristic(MainNode goal);

    public byte Id { get; set; }

    public short X { get; set; }

    public short Y { get; set; }
  }
}
