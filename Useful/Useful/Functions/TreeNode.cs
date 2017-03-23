using System.Collections.Generic;

namespace Useful.Functions
{
    /// <summary>
    ///     Class used to represent mathematical operations.
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        ///     Leaf nodes connected with given element.
        /// </summary>
        public readonly List<TreeNode> Leafs = new List<TreeNode>();

        /// <summary>
        ///     Element connecting this node.
        /// </summary>
        public string Element;

        public TreeNode(string s)
        {
            Element = s;
        }

        public override string ToString()
        {
            return Element;
        }
    }
}