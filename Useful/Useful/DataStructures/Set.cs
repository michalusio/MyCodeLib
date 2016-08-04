namespace Useful.DataStructures
{
  /// <summary>
  /// Class made for quick discrete sets management
  /// </summary>
  /// <typeparam name="T">Type of stored objects</typeparam>
  public class Set<T>
  {
    private Set<T> _parent;
    /// <summary>
    /// Object contained within this set
    /// </summary>
    public T Element;
    private int _rank;
    
    /// <summary>
    /// Creates set containing a single element
    /// </summary>
    /// <param name="element">Object to make set from</param>
    /// <returns>Set object containing said element</returns>
    public static Set<T> MakeSet(T element)
    {
        Set<T> set = new Set<T>
        {
            Element = element,
            _rank = 0
        };
        set._parent = set;
      return set;
    }
    
    /// <summary>
    /// Connects two sets
    /// </summary>
    /// <param name="y">Second set to connect with</param>
    public void Union(Set<T> y)
    {
      Set<T> set1 = Find(this);
      Set<T> set2 = Find(y);
      if (set1 == set2)
        return;
      if (set1._rank < set2._rank)
        set1._parent = set2;
      else if (set1._rank > set2._rank)
      {
        set2._parent = set1;
      }
      else
      {
        set2._parent = set1;
        set1._rank = set1._rank + 1;
      }
    }
    
    /// <summary>
    /// Finds representator of given set
    /// </summary>
    /// <param name="x">Set to get representator from</param>
    /// <returns>Representator for checking containing</returns>
    public Set<T> Find(Set<T> x)
    {
      if (x._parent != x)
        x._parent = Find(x._parent);
      return x._parent;
    }
  }
}
