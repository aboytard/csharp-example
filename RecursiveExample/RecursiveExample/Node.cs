namespace RecursiveExample
{
    public class Node
    {
        public Node RightLeaf { get; set; }
        public Node LeftLeaf { get; set; }

        public int Value { get; set; }

        public bool IsValueInTree(Node root, int value) 
        {
            if(root == null)
                return false;
            if (root.Value == value)
                return true;
            var leftTreeContainValue = IsValueInTree(root.LeftLeaf, value);
            var rightTreeContainsValue = IsValueInTree(root.RightLeaf, value);
            return leftTreeContainValue || rightTreeContainsValue;
        }
    }
}
