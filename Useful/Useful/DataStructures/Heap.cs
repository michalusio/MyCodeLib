using System;
using System.Collections.Generic;

namespace Useful.DataStructures
{
  public class Heap<T>
  {
    private readonly HashSet<T> _containing = new HashSet<T>();
    private int _c = 1;
    private HeapNode<T>[] _nodes;
    public bool MinHeap;

    public int Count => _c - 1;

      public int Capacity
    {
      get
      {
        return _nodes.Length - 1;
      }
      set
      {
        if (value < Count)
          throw new IndexOutOfRangeException("Data loss while resizing heap!");
        HeapNode<T>[] heapNodeArray = new HeapNode<T>[value + 1];
        _nodes.CopyTo(heapNodeArray, 0);
        _nodes = heapNodeArray;
      }
    }

    public HeapNode<T> this[int i]
    {
      get
      {
        return _nodes[i + 1];
      }
      set
      {
        _nodes[i + 1] = value;
      }
    }

    public Heap()
    {
      _nodes = new HeapNode<T>[11];
    }

    public Heap(int capacity)
    {
      _nodes = new HeapNode<T>[capacity > 10 ? capacity : 11];
    }

    public void Add(T Object, float priority)
    {
      if (Count == Capacity)
      {
        HeapNode<T>[] heapNodeArray = new HeapNode<T>[_nodes.Length << 2];
        _nodes.CopyTo(heapNodeArray, 0);
        _nodes = heapNodeArray;
      }
      int i = _c;
      _nodes[i] = new HeapNode<T>(Object, priority);
      TravelUp(i);
      _containing.Add(Object);
      _c = _c + 1;
    }

    private void TravelUp(int i)
    {
      while (i > 1)
      {
        HeapNode<T> me = _nodes[i];
        HeapNode<T> parent = _nodes[i / 2];
        if (!Comp(me, parent))
          break;
        HeapNode<T> heapNode = me;
        _nodes[i] = parent;
        _nodes[i / 2] = heapNode;
        i /= 2;
      }
    }

    private bool Comp(HeapNode<T> me, HeapNode<T> parent)
    {
        return MinHeap ? me < parent : me > parent;
    }

      private void TravelDown(int i)
    {
      int index;
      for (; i << 1 < _c; i = index)
      {
        index = i << 1;
        if (index < Count && Comp(_nodes[index + 1], _nodes[index]))
          ++index;
        if (!Comp(_nodes[index], _nodes[i]))
          break;
        HeapNode<T> heapNode = _nodes[i];
        _nodes[i] = _nodes[index];
        _nodes[index] = heapNode;
      }
    }

    public HeapNode<T> PopFirst()
    {
      HeapNode<T> heapNode = _nodes[1];
        if (heapNode.Equals(null)) return heapNode;
        _containing.Remove(heapNode.Object);
        _nodes[1] = _nodes[Count];
        _nodes[Count] = null;
        if (Count > 0)
            _c = _c - 1;
        TravelDown(1);
        return heapNode;
    }

    public bool Contains(T neighbor)
    {
      return _containing.Contains(neighbor);
    }

    public void Clear()
    {
      _containing.Clear();
      for (int index = 0; index < _nodes.Length; ++index)
        _nodes[index] = null;
      _c = 1;
    }
  }
}
