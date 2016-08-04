using System;
using System.Collections.Generic;

namespace Useful.DataStructures
{
  ///<summary>
  /// Class utilizing the min-max heap data structure.
  ///</summary>
  public class Heap<T>
  {
    private readonly HashSet<T> _containing = new HashSet<T>();
    private int _c = 1;
    private HeapNode<T>[] _nodes;
    
    /// <summary>
    /// Set if heap is min-heap or max-heap.
    /// </summary>
    public bool MinHeap;

    /// <summary>
    ///Retrieve how many items are in the heap.
    /// </summary>
    public int Count => _c - 1;

    ///<summary>
    /// Get/Set maximum capacity of base array.
    ///</summary>
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
    
    /// <summary>
    /// Retrieves heap node like in an array.
    /// </summary>
    /// <param name="i">Index of node</param>
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
    
    /// <summary>
    /// Initializes heap with initial capacity of 10 elements.
    /// </summary>
    public Heap()
    {
      _nodes = new HeapNode<T>[11];
    }
    
    /// <summary>
    /// Initializes heap with given initial capacity clamped to 10+ elements.
    /// </summary>
    /// <param name="capacity">Initial capacity</param>
    public Heap(int capacity)
    {
      _nodes = new HeapNode<T>[capacity > 10 ? capacity : 11];
    }
    
    /// <summary>
    /// Add object to heap and situate it in its place.
    /// </summary>
    /// <param name="Object">Added object</param>
    /// <param name="priority">Priority of said object</param>
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
    
    /// <summary>
    /// Retrieves first object in a heap.
    /// If heap is min-heap, then the object has minimum priority.
    /// If heap is max-heap, then the object has maximum priority.
    /// </summary>
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
    
    /// <summary>
    /// Checks if object is contained in heap.
    /// Uses HashSet for quick checking.
    /// </summary>
    /// <param name="neighbor">Object to check</param>
    public bool Contains(T neighbor)
    {
      return _containing.Contains(neighbor);
    }
    
    /// <summary>
    /// Clears the heap.
    /// </summary>
    public void Clear()
    {
      _containing.Clear();
      for (int index = 0; index < _nodes.Length; ++index)
        _nodes[index] = null;
      _c = 1;
    }
  }
}
