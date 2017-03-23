using System.Collections.Generic;

namespace Useful.PathFinding
{
    /// <summary>
    ///     Node using distinct neighbor lists.
    /// </summary>
    public abstract class HashNode : MainNode
    {
        /// <summary>
        ///     Set of connected nodes.
        /// </summary>
        public HashSet<HashNode> Connected = new HashSet<HashNode>();

        /// <summary>
        ///     ID of a node distinguishing it from the rest.
        /// </summary>
        public short Id;

        internal override IEnumerable<MainNode> GetNeighbors()
        {
            return Connected;
        }

        /// <summary>
        ///     Checks if two nodes are equal.
        /// </summary>
        /// <param name="b">Second node to check</param>
        public override bool NodeEqual(MainNode b)
        {
            return ((HashNode) b).Id == Id;
        }
    }
}