using _5_parallel_linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PlinqTests
    {
        [TestMethod]
        public void _1_Plinq_Tests()
        {
            _1_Plinq taskrun = new _1_Plinq();
            taskrun.DoWork();
        }

        [TestMethod]
        public void _1_Plinq_OddNumbers_Tests()
        {
            _1_Plinq taskrun = new _1_Plinq();
            taskrun.DoWork2();
        }

        [TestMethod]
        public void _1_Plinq_EvenNumbers_Tests()
        {
            _1_Plinq taskrun = new _1_Plinq();
            taskrun.DoWork3();
        }
    }
}
