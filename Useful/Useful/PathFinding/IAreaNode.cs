namespace Useful.PathFinding
{
    /// <summary>
    /// Class used by area division algorithms.
    /// </summary>
  public interface IAreaNode
  {
    /// <summary>
    /// Id used to divide field into areas
    /// </summary>
    byte Id { get; set; }
    
    /// <summary>
    /// X coordinate on the field
    /// </summary>
    short X { get; set; }
    
    /// <summary>
    /// Y coordinate on the field
    /// </summary>
    short Y { get; set; }
  }
}
