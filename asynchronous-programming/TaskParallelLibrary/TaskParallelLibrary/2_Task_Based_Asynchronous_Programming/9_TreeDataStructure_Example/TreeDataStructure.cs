namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._9_TreeDataStructure_Example
{
    public class TreeDataStructure
    {
        public static void TreeDataStructureMain()
        {
            Tree<MyClass> tree = new Tree<MyClass>()
            {
                Left = new Tree<MyClass>()
                {
                    Left = new Tree<MyClass>() { },
                    Right = new Tree<MyClass>() { },
                    Data = new MyClass() { Number = 2, Name = "Second Left" }
                },
                Right = new Tree<MyClass>()
                {
                    Left = new Tree<MyClass>() { },
                    Right = new Tree<MyClass>() { },
                    Data = new MyClass() { Number = 3, Name = "Third Right" }
                },
                Data = new MyClass() { Number = 1, Name = "First" }
            };
            // ...populate tree

            // Define the Action to perform on each node.
            // Exception --> need to handle when the Tree is finished
            Action<MyClass> myAction = x => Console.WriteLine("{0} : {1}", x.Name, x.Number);

            // Traverse the tree with parallel tasks.
            DoTree(tree, myAction);
        }

        // By using tasks explicitly.
        public static void DoTree<T>(Tree<T> tree, Action<T> action)
        {
            if (tree == null) return;
            var left = Task.Factory.StartNew(() => DoTree(tree.Left, action));
            var right = Task.Factory.StartNew(() => DoTree(tree.Right, action));
            action(tree.Data);

            try
            {
                Task.WaitAll(left, right);
            }
            catch (AggregateException)
            {
                //handle exceptions here
            }
        }

        // By using Parallel.Invoke
        public static void DoTree2<T>(Tree<T> tree, Action<T> action)
        {
            if (tree == null) return;
            Parallel.Invoke(
                () => DoTree2(tree.Left, action),
                () => DoTree2(tree.Right, action),
                () => action(tree.Data)
            );
        }
    }

    public class MyClass
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
    public class Tree<T>
    {
        public Tree<T> Left;
        public Tree<T> Right;
        public T Data;
    }
}
