using Useful.Other;

namespace Useful.Functions
{
    /// <summary>
    ///     Class for differentiating equations.
    /// </summary>
    public static class Derivatives
    {
        /// <summary>
        ///     Differentiates equation and returns the result.
        /// </summary>
        /// <param name="q">TreeNode with equation</param>
        /// <param name="x">Variable to differentiate from</param>
        public static TreeNode Differentiate(TreeNode q, char x)
        {
            if (q.Element.Length == 1 && q.Element[0] == x)
                return new TreeNode("1");
            if (Extensions.IsNumeric(q.Element))
                return new TreeNode("0");
            var s = q.Element;
            switch (s)
            {
                case "+":
                    return new TreeNode("+")
                    {
                        Leafs =
                        {
                            Differentiate(q.Leafs[0], x),
                            Differentiate(q.Leafs[1], x)
                        }
                    };
                case "/":
                    TreeNode treeNode1 = new TreeNode("/");
                    TreeNode treeNode2 = new TreeNode("-");
                    treeNode2.Leafs.Add(new TreeNode("*"));
                    treeNode2.Leafs.Add(new TreeNode("*"));
                    treeNode2.Leafs[0].Leafs.Add(Differentiate(q.Leafs[0], x));
                    treeNode2.Leafs[0].Leafs.Add(q.Leafs[1]);
                    treeNode2.Leafs[1].Leafs.Add(Differentiate(q.Leafs[1], x));
                    treeNode2.Leafs[1].Leafs.Add(q.Leafs[0]);
                    treeNode1.Leafs.Add(treeNode2);
                    treeNode1.Leafs.Add(new TreeNode("*"));
                    treeNode1.Leafs[1].Leafs.Add(q.Leafs[1]);
                    treeNode1.Leafs[1].Leafs.Add(q.Leafs[1]);
                    return treeNode1;
                case "-":
                    return new TreeNode("-")
                    {
                        Leafs =
                        {
                            Differentiate(q.Leafs[0], x),
                            Differentiate(q.Leafs[1], x)
                        }
                    };
                case "L":
                    return new TreeNode("/")
                    {
                        Leafs =
                        {
                            Differentiate(q.Leafs[0], x),
                            q.Leafs[0]
                        }
                    };
                case "C":
                {
                    TreeNode treeNode = new TreeNode("*");
                    treeNode.Leafs.Add(new TreeNode("S"));
                    treeNode.Leafs[0].Leafs.Add(q.Leafs[0]);
                    treeNode.Leafs.Add(new TreeNode("*"));
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("-1"));
                    treeNode.Leafs[1].Leafs.Add(Differentiate(q.Leafs[0], x));
                    return treeNode;
                }
                case "*":
                {
                    TreeNode treeNode = new TreeNode("+");
                    treeNode.Leafs.Add(new TreeNode("*"));
                    treeNode.Leafs.Add(new TreeNode("*"));
                    treeNode.Leafs[0].Leafs.Add(Differentiate(q.Leafs[0], x));
                    treeNode.Leafs[0].Leafs.Add(q.Leafs[1]);
                    treeNode.Leafs[1].Leafs.Add(Differentiate(q.Leafs[1], x));
                    treeNode.Leafs[1].Leafs.Add(q.Leafs[0]);
                    return treeNode;
                }
                case "I":
                    return new TreeNode("I")
                    {
                        Leafs =
                        {
                            q.Leafs[0],
                            Differentiate(q.Leafs[1], x),
                            Differentiate(q.Leafs[2], x)
                        }
                    };
                case "N":
                    return new TreeNode("-")
                    {
                        Leafs =
                        {
                            new TreeNode("0"),
                            Differentiate(q.Leafs[0], x)
                        }
                    };
                case "O":
                {
                    TreeNode treeNode = new TreeNode("/");
                    treeNode.Leafs.Add(new TreeNode("*"));
                    treeNode.Leafs[0].Leafs.Add(new TreeNode("-1"));
                    treeNode.Leafs[0].Leafs.Add(Differentiate(q.Leafs[0], x));
                    treeNode.Leafs.Add(new TreeNode("^"));
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("S"));
                    treeNode.Leafs[1].Leafs[0].Leafs.Add(q.Leafs[0]);
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("2"));
                    return treeNode;
                }
                case "T":
                {
                    TreeNode treeNode = new TreeNode("/");
                    treeNode.Leafs.Add(Differentiate(q.Leafs[0], x));
                    treeNode.Leafs.Add(new TreeNode("^"));
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("C"));
                    treeNode.Leafs[1].Leafs[0].Leafs.Add(q.Leafs[0]);
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("2"));
                    return treeNode;
                }
                case "U":
                {
                    TreeNode treeNode = new TreeNode("/");
                    treeNode.Leafs.Add(Differentiate(q.Leafs[0], x));
                    treeNode.Leafs.Add(new TreeNode("*"));
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("2"));
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("U"));
                    treeNode.Leafs[1].Leafs[1].Leafs.Add(q.Leafs[0]);
                    return treeNode;
                }
                case "^":
                {
                    TreeNode treeNode = new TreeNode("*");
                    treeNode.Leafs.Add(new TreeNode("^"));
                    treeNode.Leafs[0].Leafs.Add(q.Leafs[0]);
                    treeNode.Leafs[0].Leafs.Add(q.Leafs[1]);
                    treeNode.Leafs.Add(new TreeNode("+"));
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("*"));
                    treeNode.Leafs[1].Leafs[0].Leafs.Add(Differentiate(q.Leafs[1], x));
                    treeNode.Leafs[1].Leafs[0].Leafs.Add(new TreeNode("L"));
                    treeNode.Leafs[1].Leafs[0].Leafs[1].Leafs.Add(q.Leafs[0]);
                    treeNode.Leafs[1].Leafs.Add(new TreeNode("*"));
                    treeNode.Leafs[1].Leafs[1].Leafs.Add(new TreeNode("/"));
                    treeNode.Leafs[1].Leafs[1].Leafs[0].Leafs.Add(q.Leafs[1]);
                    treeNode.Leafs[1].Leafs[1].Leafs[0].Leafs.Add(q.Leafs[0]);
                    treeNode.Leafs[1].Leafs[1].Leafs.Add(Differentiate(q.Leafs[0], x));
                    return treeNode;
                }
                case "S":
                {
                    TreeNode treeNode = new TreeNode("*");
                    treeNode.Leafs.Add(new TreeNode("C"));
                    treeNode.Leafs[0].Leafs.Add(q.Leafs[0]);
                    treeNode.Leafs.Add(Differentiate(q.Leafs[0], x));
                    return treeNode;
                }
            }
            return new TreeNode("0");
        }
    }
}