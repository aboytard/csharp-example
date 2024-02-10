using RecursiveExample;

var tree = MockTree.GetMockTree();

var check3 = tree.IsValueInTree(tree, 3);
Console.WriteLine("3 : " + check3);
var check0 = tree.IsValueInTree(tree, 0);
Console.WriteLine("0 : " + check0);
Console.WriteLine("12 : " + tree.IsValueInTree(tree, 12));
Console.WriteLine("8 : " + tree.IsValueInTree(tree, 8));
Console.WriteLine("17 : " + tree.IsValueInTree(tree, 17));
Console.ReadKey();