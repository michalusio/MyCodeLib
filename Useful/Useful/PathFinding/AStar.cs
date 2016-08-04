using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Useful.DataStructures;
using Useful.Other;

namespace Useful.PathFinding
{
    /// <summary>
    /// Class containing PathFinding algorithms
    /// </summary>
    /// <typeparam name="T">Node type implementing MainNode abstract class</typeparam>
    public static class Pathing<T> where T : MainNode
    {
        private static readonly Heap<T> Open = new Heap<T> {MinHeap = true};
        private static readonly HashSet<T> Closed = new HashSet<T>();
        private static double _at,_dt;
        /// <summary>
        /// Time in milliseconds using A* algorithm.
        /// </summary>
        public static double AStarFindingTime => _at;
        /// <summary>
        /// Time in milliseconds using Dijkstra's algorithm.
        /// </summary>
        public static double DijkstraFindingTime => _dt;
        /// <summary>
        /// Resets algorithm times.
        /// </summary>
        public static void ResetTimes()
        {
            _at = 0;
            _dt = 0;
        }
        /// <summary>
        /// Performs path search using Dijkstra's algorithm for a given predicate.
        /// <para>Returns list of nodes from ending to starting node.</para>
        /// <para>Returns null if there is not path to goal.</para>
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="targeting">Predicate distinguishing end nodes from the rest</param>
        /// <param name="limiter">Maximum length of path to search for</param>
        public static List<T> Dijkstra(T start, Predicate<T> targeting,int limiter=int.MaxValue)
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
        /// <summary>
        /// Performs path search using A* algorithm for a given start and goal.
        /// <para>Returns list of nodes from ending to starting node.</para>
        /// <para>Returns null if there is no path to goal.</para>
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="goal">Ending node</param>
        /// <param name="limiter">Maximum length of path to search for</param>
        public static List<T> AStar(T start, T goal,int limiter=int.MaxValue)
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
                if (current.G > limiter) continue;
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
        /// <summary>
        /// Performs path search using A* algorithm for a given start and list of goals.
        /// <para>Returns list of nodes from best ending node to starting node.</para>
        /// <para>Returns null if there is no path to any of the goals.</para>
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="goals">List of ending nodes</param>
        /// <param name="limiter">Maximum length of path to search for</param>
        public static List<T> MultiAStar(T start, List<T> goals,int limiter=int.MaxValue)
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
                if (current.G > limiter) continue;
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
