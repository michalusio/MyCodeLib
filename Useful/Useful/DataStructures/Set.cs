namespace Useful.DataStructures
{
  public class Set<T>
  {
    public Set<T> Parent;
    public T Element;
    public int Rank;

    public static Set<T> MakeSet(T element)
    {
        Set<T> set = new Set<T>
        {
            Element = element,
            Rank = 0
        };
        set.Parent = set;
      return set;
    }

    public void Union(Set<T> y)
    {
      Set<T> set1 = Find(this);
      Set<T> set2 = Find(y);
      if (set1 == set2)
        return;
      if (set1.Rank < set2.Rank)
        set1.Parent = set2;
      else if (set1.Rank > set2.Rank)
      {
        set2.Parent = set1;
      }
      else
      {
        set2.Parent = set1;
        set1.Rank = set1.Rank + 1;
      }
    }

    public Set<T> Find(Set<T> x)
    {
      if (x.Parent != x)
        x.Parent = Find(x.Parent);
      return x.Parent;
    }
  }
}
