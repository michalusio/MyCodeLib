using System;

namespace Useful.DataStructures
{
  public class HeapNode<T>
  {
    public readonly T Object;
    public readonly float Priority;

    public HeapNode(T obj, float prio)
    {
      Object = obj;
      Priority = prio;
    }

    public static bool operator >(HeapNode<T> a, HeapNode<T> b)
    {
      return  a.Priority > (double) b.Priority;
    }

    public static bool operator <(HeapNode<T> a, HeapNode<T> b)
    {
      return  a.Priority < (double) b.Priority;
    }

    public static bool operator ==(HeapNode<T> a, HeapNode<T> b)
    {
      if (b != null && a != null)
        return Math.Abs(a.Priority - b.Priority) < 2.80259692864963E-45;
      return false;
    }

    public static bool operator !=(HeapNode<T> a, HeapNode<T> b)
    {
      return !(a == b);
    }

    public override bool Equals(object obj)
    {
      if (obj != null)
        return GetHashCode() == obj.GetHashCode();
      return false;
    }

    public override int GetHashCode()
    {
      return Object.GetHashCode() << 8 + Priority.GetHashCode();
    }

    public override string ToString()
    {
      return "HeapNode(" + Object + "|" + Priority.ToString("0.00") + ")";
    }
  }
}
