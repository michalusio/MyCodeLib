using System.Collections.Generic;

namespace Useful.Functions
{
  public class TreeNode
  {
    public readonly List<TreeNode> Leafs = new List<TreeNode>();
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
