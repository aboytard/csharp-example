namespace RecursiveExample
{
    internal static class MockTree
    {
        public static Node GetMockTree()
        {
            return new Node()
            {
                Value = 1,
                RightLeaf = new Node()
                {
                    Value = 2,
                    RightLeaf = new Node()
                    {
                        Value = 3,
                        RightLeaf = new Node()
                        {
                            Value = 4,
                            RightLeaf = new Node()
                            {
                                Value = 5
                            },
                            LeftLeaf = new Node()
                            {
                                Value= 6,
                            },
                        },
                        LeftLeaf = new Node()
                        {
                            Value = 7,
                        }
                    }
                },
                LeftLeaf = new Node()
                {
                    Value = 8,
                    RightLeaf = new Node()
                    {
                        Value = 9,
                        RightLeaf = new Node()
                        {
                            Value = 10
                        },
                        LeftLeaf = new Node()
                        {
                            Value = 11,
                        },
                    },
                    LeftLeaf = new Node()
                    {
                        Value = 12,
                    }
                }
            };
        }
    }
}
