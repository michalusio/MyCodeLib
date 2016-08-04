using System.Collections.Generic;

namespace Useful.PathFinding
{
    /// <summary>
    /// Node used by A* and Dijkstra's algorithms for PathFinding.
    /// </summary>
  public abstract class MainNode
  {
    internal float G { get; set; }

    internal float F { get; set; }

    internal MainNode CameFrom { get; set; }

    internal abstract IEnumerable<MainNode> GetNeighbors();
    
    /// <summary>
    /// Checks if two nodes are equal.
    /// </summary>
    /// <param name="b">Second node to check</param>
    public abstract bool NodeEqual(MainNode b);
    
    /// <summary>
    /// Returns distance between two nodes using given metric.
    /// </summary>
    /// <param name="b">Second node to measure distance to</param>
    public abstract float Distance(MainNode b);
    
    /// <summary>
    /// Approximates distance between two nodes using given metric.
    /// </summary>
    /// <param name="goal">Second node to measure distance to</param>
    public abstract float Heuristic(MainNode goal);
  }
}
