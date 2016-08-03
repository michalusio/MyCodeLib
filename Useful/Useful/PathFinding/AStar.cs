using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Useful.DataStructures;
using Useful.Other;

namespace Useful.PathFinding
{
    public static class Pathing<T> where T : MainNode
    {
        private static readonly Heap<T> Open = new Heap<T> {MinHeap = true};
        private static readonly HashSet<T> Closed = new HashSet<T>();
        private static double _at,_dt;
        public static double AStarFindingTime => _at;
        public static double DijkstraFindingTime => _dt;

        public static void ResetTimes()
        {
            _at = 0;
            _dt = 0;
        }

        public static List<T> Dijkstra(T start, Predicate<T> targeting,int limiter)
        {
            Stopwatch s=Stopwatch.StartNew();
            Open.Add(start, 0.0f);
            while (Open.Count > 0)
            {
                var current = Open.PopFirst().Object;
                if (targeting.Invoke(current))
                {
                    Open.Clear();
                    Closed.Clear();
                    var t=ToList(current);
                    s.Stop();
                    MMath.Add(ref _dt, s.Elapsed.TotalMilliseconds);
                    return t;
                }
                Closed.Add(current);
                if (current.G > limiter) continue;
                foreach (var neighbor in current.GetNeighbors())
                {
                    var num = current.G + current.Distance(neighbor);
                    if ((Closed.Contains((T) neighbor) ||
                         Open.Contains((T) neighbor)) && num >= neighbor.G) continue;
                    neighbor.CameFrom = current;
                    neighbor.G = num;
                    if (!Open.Contains((T) neighbor)) Open.Add((T) neighbor, neighbor.G);
                }
            }
            Open.Clear();
            Closed.Clear();
            s.Stop();
            MMath.Add(ref _dt, s.Elapsed.TotalMilliseconds);
            return null;
        }

        public static List<T> AStar(T start, T goal)
        {
            Stopwatch s = Stopwatch.StartNew();
            Open.Add(start, 0.0f);
            while (Open.Count > 0)
            {
                var current = Open.PopFirst().Object;
                if (current.NodeEqual(goal))
                {
                    Open.Clear();
                    Closed.Clear();
                    var t = ToList(current);
                    s.Stop();
                    MMath.Add(ref _at, s.Elapsed.TotalMilliseconds);
                    return t;
                }
                Closed.Add(current);
                foreach (var neighbor in current.GetNeighbors())
                {
                    var num = current.G + current.Distance(neighbor);
                    if ((Closed.Contains((T) neighbor) ||
                         Open.Contains((T) neighbor)) && num >= neighbor.G) continue;
                    neighbor.CameFrom = current;
                    neighbor.G = num;
                    neighbor.F = neighbor.G + neighbor.Heuristic(goal);
                    if (!Open.Contains((T) neighbor))
                        Open.Add((T) neighbor, neighbor.F);
                }
            }
            Open.Clear();
            Closed.Clear();
            s.Stop();
            MMath.Add(ref _at, s.Elapsed.TotalMilliseconds);
            return null;
        }

        public static List<T> MultiAStar(T start, List<T> goals)
        {
            Stopwatch s = Stopwatch.StartNew();
            Open.Add(start, 0.0f);
            while (Open.Count > 0)
            {
                var current = Open.PopFirst().Object;
                if (goals.Exists(t => current.NodeEqual(t)))
                {
                    Open.Clear();
                    Closed.Clear();
                    var t = ToList(current);
                    s.Stop();
                    MMath.Add(ref _at, s.Elapsed.TotalMilliseconds);
                    return t;
                }
                Closed.Add(current);
                foreach (var neighbor in current.GetNeighbors())
                {
                    var num = current.G + current.Distance(neighbor);
                    if ((Closed.Contains((T) neighbor) ||
                         Open.Contains((T) neighbor)) && num >= neighbor.G) continue;
                    neighbor.CameFrom = current;
                    neighbor.G = num;
                    neighbor.F = neighbor.G + goals.Min(t => neighbor.Heuristic(t));
                    if (!Open.Contains((T) neighbor))
                        Open.Add((T) neighbor, neighbor.F);
                }
            }
            Open.Clear();
            Closed.Clear();
            s.Stop();
            MMath.Add(ref _at, s.Elapsed.TotalMilliseconds);
            return null;
        }

        private static List<T> ToList(T current)
        {
            var objList = new List<T>();
            while (current != null)
            {
                objList.Add(current);
                var obj = current;
                current = (T) current.CameFrom;
                obj.CameFrom = null;
            }
            return objList;
        }
    }
}
