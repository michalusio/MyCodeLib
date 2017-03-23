using System;
using System.Collections.Generic;

namespace Useful.PathFinding
{
    public class Manhattan2DNode : MainNode, IAreaNode
    {
        public static IArray2D Map;
        public static bool CanDiagonal;
        public byte Wall;

        public Manhattan2DNode()
        {
        }

        public Manhattan2DNode(short x, short y, IArray2D tab)
        {
            X = x;
            Y = y;
            Map = tab;
        }

        public byte Id { get; set; }

        public short X { get; set; }

        public short Y { get; set; }

        public override string ToString()
        {
            return "M2DN(" + X + "|" + Y + ")";
        }

        internal override IEnumerable<MainNode> GetNeighbors()
        {
            var manhattan2DnodeList = new List<Manhattan2DNode>();
            if (InBounds(X + 1, Y) && Map[X + 1, Y].Wall != 0xFF)
                manhattan2DnodeList.Add(Map[X + 1, Y]);
            if (InBounds(X - 1, Y) && Map[X - 1, Y].Wall != 0xFF)
                manhattan2DnodeList.Add(Map[X - 1, Y]);
            if (InBounds(X, Y + 1) && Map[X, Y + 1].Wall != 0xFF)
                manhattan2DnodeList.Add(Map[X, Y + 1]);
            if (InBounds(X, Y - 1) && Map[X, Y - 1].Wall != 0xFF)
                manhattan2DnodeList.Add(Map[X, Y - 1]);
            if (CanDiagonal)
            {
                if (InBounds(X + 1, Y + 1) && Map[X + 1, Y + 1].Wall != 0xFF)
                    manhattan2DnodeList.Add(Map[X + 1, Y + 1]);
                if (InBounds(X - 1, Y - 1) && Map[X - 1, Y - 1].Wall != 0xFF)
                    manhattan2DnodeList.Add(Map[X - 1, Y - 1]);
                if (InBounds(X - 1, Y + 1) && Map[X - 1, Y + 1].Wall != 0xFF)
                    manhattan2DnodeList.Add(Map[X - 1, Y + 1]);
                if (InBounds(X + 1, Y - 1) && Map[X + 1, Y - 1].Wall != 0xFF)
                    manhattan2DnodeList.Add(Map[X + 1, Y - 1]);
            }
            return manhattan2DnodeList;
        }

        private static bool InBounds(int x, int y)
        {
            return Map.Bounds || x >= 0 && y >= 0 && x < Map.GetLength(0) && y < Map.GetLength(1);
        }

        public override bool NodeEqual(MainNode b)
        {
            Manhattan2DNode manhattan2Dnode = (Manhattan2DNode) b;
            return manhattan2Dnode.X == X && manhattan2Dnode.Y == Y;
        }

        public override float Distance(MainNode b)
        {
            return (10 + ((Manhattan2DNode) b).Wall) * 0.1f;
        }

        public override float Heuristic(MainNode goal)
        {
            Manhattan2DNode manhattan2Dnode = (Manhattan2DNode) goal;
            return Math.Abs(manhattan2Dnode.X - X) + Math.Abs(manhattan2Dnode.Y - Y);
        }
    }
}