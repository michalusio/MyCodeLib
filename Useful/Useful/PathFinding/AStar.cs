using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Useful.DataStructures;
using Useful.Other;

namespace Useful.PathFinding
{
    /// <summary>
    ///     Class containing PathFinding algorithms
    /// </summary>
    /// <typeparam name="T">Node type implementing MainNode abstract class</typeparam>
    public static class Pathing<T> where T : MainNode
    {
        private static double _at;
        private static double _dt;

        /// <summary>
        ///     Time in milliseconds using A* algorithm.
        /// </summary>
        public static double AStarFindingTime => _at;

        /// <summary>
        ///     Time in milliseconds using Dijkstra's algorithm.
        /// </summary>
        public static double DijkstraFindingTime => _dt;

        /// <summary>
        ///     Resets algorithm times.
        /// </summary>
        public static void ResetTimes()
        {
            _at = 0;
            _dt = 0;
        }

        /// <summary>
        ///     Performs path search using Dijkstra's algorithm for a given predicate.
        ///     <para>Returns list of nodes from ending to starting node.</para>
        ///     <para>Returns null if there is no path to goal.</para>
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="targeting">Predicate distinguishing end nodes from the rest</param>
        /// <param name="limiter">Maximum length of path to search for</param>
        public static List<MainNode> Dijkstra(MainNode start, Predicate<MainNode> targeting, int limiter = int.MaxValue)
        {
            if (start == null) return null;
            Stopwatch s = Stopwatch.StartNew();
            var open = new Heap<MainNode> {MinHeap = true};
            var closed = new HashSet<MainNode>();
            var gs = new Dictionary<MainNode, float>();
            var cameFrom = new Dictionary<MainNode, MainNode>();
            open.Add(start, 0.0f);
            while (open.Count > 0)
            {
                MainNode current = open.PopFirst().Object;
                if (targeting.Invoke(current))
                {
                    open.Clear();
                    closed.Clear();
                    var t = ToList(current, cameFrom);
                    s.Stop();
                    Add(ref _dt, s.Elapsed.TotalMilliseconds);
                    return t;
                }
                closed.Add(current);
                if (gs.GetValueOrDefault(current, 0f) > limiter) continue;
                foreach (MainNode neighbor in current.GetNeighbors())
                {
                    var num = gs.GetValueOrDefault(current, 0f) + current.Distance(neighbor);
                    if ((closed.Contains(neighbor) ||
                         open.Contains(neighbor)) && num >= gs.GetValueOrDefault(neighbor, 0f)) continue;
                    cameFrom[neighbor] = current;
                    gs[neighbor] = num;
                    if (!open.Contains(neighbor)) open.Add(neighbor, gs.GetValueOrDefault(neighbor, 0f));
                }
            }
            open.Clear();
            closed.Clear();
            s.Stop();
            Add(ref _dt, s.Elapsed.TotalMilliseconds);
            return null;
        }

        /// <summary>
        ///     Performs path search using A* algorithm for a given start and goal.
        ///     <para>Returns list of nodes from ending to starting node.</para>
        ///     <para>Returns null if there is no path to goal.</para>
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="goal">Ending node</param>
        /// <param name="limiter">Maximum length of path to search for</param>
        public static List<MainNode> AStar(MainNode start, MainNode goal, int limiter = int.MaxValue)
        {
            if (goal == null || start == null) return null;
            Stopwatch s = Stopwatch.StartNew();
            var open = new Heap<MainNode> {MinHeap = true};
            var closed = new HashSet<MainNode>();
            var gs = new Dictionary<MainNode, float>();
            var fs = new Dictionary<MainNode, float>();
            var cameFrom = new Dictionary<MainNode, MainNode>();
            open.Add(start, 0.0f);
            while (open.Count > 0)
            {
                MainNode current = open.PopFirst().Object;
                if (current.NodeEqual(goal))
                {
                    open.Clear();
                    closed.Clear();
                    var t = ToList(current, cameFrom);
                    s.Stop();
                    Add(ref _at, s.Elapsed.TotalMilliseconds);
                    return t;
                }
                closed.Add(current);
                if (gs.GetValueOrDefault(current, 0f) > limiter) continue;
                foreach (MainNode neighbor in current.GetNeighbors())
                {
                    var num = gs.GetValueOrDefault(current, 0f) + current.Distance(neighbor);
                    if ((closed.Contains(neighbor) ||
                         open.Contains(neighbor)) && num >= gs.GetValueOrDefault(neighbor, 0f)) continue;
                    cameFrom[(T) neighbor] = current;
                    gs[(T) neighbor] = num;
                    fs[(T) neighbor] = num + neighbor.Heuristic(goal);
                    if (!open.Contains(neighbor))
                        open.Add(neighbor, fs[neighbor]);
                }
            }
            open.Clear();
            closed.Clear();
            s.Stop();
            Add(ref _at, s.Elapsed.TotalMilliseconds);
            return null;
        }

        /// <summary>
        ///     Performs path search using A* algorithm for a given start and list of goals.
        ///     <para>Returns list of nodes from best ending node to starting node.</para>
        ///     <para>Returns null if there is no path to any of the goals.</para>
        /// </summary>
        /// <param name="start">Starting node</param>
        /// <param name="goals">List of ending nodes</param>
        /// <param name="limiter">Maximum length of path to search for</param>
        public static List<MainNode> MultiAStar(MainNode start, List<T> goals, int limiter = int.MaxValue)
        {
            if (goals.All(g => g == null) || start == null) return null;
            Stopwatch s = Stopwatch.StartNew();
            var open = new Heap<MainNode> {MinHeap = true};
            var closed = new HashSet<MainNode>();
            var gs = new Dictionary<MainNode, float>();
            var fs = new Dictionary<MainNode, float>();
            var cameFrom = new Dictionary<MainNode, MainNode>();
            open.Add(start, 0.0f);
            while (open.Count > 0)
            {
                MainNode current = open.PopFirst().Object;
                if (goals.Exists(t => current.NodeEqual(t)))
                {
                    open.Clear();
                    closed.Clear();
                    var t = ToList(current, cameFrom);
                    s.Stop();
                    Add(ref _at, s.Elapsed.TotalMilliseconds);
                    return t;
                }
                closed.Add(current);
                if (gs.GetValueOrDefault(current, 0f) > limiter) continue;
                foreach (MainNode neighbor in current.GetNeighbors())
                {
                    var num = gs.GetValueOrDefault(current, 0f) + current.Distance(neighbor);
                    if ((closed.Contains(neighbor) ||
                         open.Contains(neighbor)) && num >= gs.GetValueOrDefault(neighbor, 0f)) continue;
                    cameFrom[neighbor] = current;
                    gs[neighbor] = num;
                    fs[neighbor] = gs[neighbor] + goals.Min(t => neighbor.Heuristic(t));
                    if (!open.Contains(neighbor))
                        open.Add(neighbor, fs[neighbor]);
                }
            }
            open.Clear();
            closed.Clear();
            s.Stop();
            Add(ref _at, s.Elapsed.TotalMilliseconds);
            return null;
        }

        private static List<MainNode> ToList(MainNode current, IDictionary<MainNode, MainNode> cameFrom)
        {
            var objList = new List<MainNode>();
            while (current != null)
            {
                objList.Add(current);
                current = cameFrom.GetValueOrDefault(current, null);
            }
            return objList;
        }

        /// <summary>
        ///     Thread-safely adds a value to a double variable.
        /// </summary>
        /// <param name="variable">Variable to add to</param>
        /// <param name="value">Value to add</param>
        private static void Add(ref double variable, double value)
        {
            double newCurrentValue = 0;
            while (true)
            {
                var currentValue = newCurrentValue;
                var newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref variable, newValue, currentValue);
                if (Math.Abs(newCurrentValue - currentValue) < 0.000001)
                    return;
            }
        }
    }
}