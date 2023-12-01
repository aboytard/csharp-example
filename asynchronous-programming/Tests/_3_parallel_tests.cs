using _3_parallel_for_foreach_invoke;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ParallelForForeachInvokeTests
    {
        [TestMethod]
        public void _1_Parallel_For_Tests()
        {
            _1_Parallel_For taskrun = new _1_Parallel_For();
            taskrun.DoWork();

            //parallel for output: NonSequential
            //1 - 4
            //2 - 4
            //3 - 4
            //4 - 4
            //5 - 4
            //6 - 4
            //7 - 4
            //8 - 4
            //9 - 4
            //166 - 10
            //10 - 4
            //167 - 10
            //11 - 4
            //168 - 10
            //12 - 4
            //169 - 10
            //13 - 4
            //170 - 10
        }

        [TestMethod]
        public void _1_Parallel_For_Options_Tests()
        {
            //Thread IDs : 4,10,11
            _1_Parallel_For taskrun = new _1_Parallel_For();
            taskrun.DoWorkWithOptions();
        }

        [TestMethod]
        public void _1_Parallel_For_BreakStop_Tests()
        {
            _1_Parallel_For taskrun = new _1_Parallel_For();
            taskrun.DoWorkWithBreakOrStop();
        }

        [TestMethod]
        public void _2_Parallel_Foreach_Tests()
        {
            _2_Parallel_Foreach taskrun = new _2_Parallel_Foreach();
            taskrun.DoWork();
        }

        [TestMethod]
        public void _2_Parallel_Foreach_Bag_Tests()
        {
            _2_Parallel_Foreach taskrun = new _2_Parallel_Foreach();
            taskrun.DoWork2();
        }

        [TestMethod]
        public void _3_Parallel_Invoke_Tests()
        {
            _3_Parallel_Invoke taskrun = new _3_Parallel_Invoke();
            //taskrun.DoWork();
            taskrun.DoWork2();
        }

        [TestMethod]
        public void _4_Parallel_SharedVariables_Tests()
        {
            _4_Parallel_SharedVariables taskrun = new _4_Parallel_SharedVariables();
            taskrun.DoWork();
        }
    }
}
