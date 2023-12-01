using _4_asynchronous_programming_othertopics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class AsyncOtherTopicTests
    {
        [TestMethod]
        public async Task _1_IProgress_Tests()
        {
            _1_IProgress taskrun = new _1_IProgress();
            await taskrun.DoWork();
        }

        [TestMethod]
        public void _2_TaskCompletionSource_DoWorkBasic_Tests()
        {
            _2_TaskCompletionSource taskrun = new _2_TaskCompletionSource();
            var task = taskrun.DoWorkBasic();
            Console.WriteLine("DoWorkBasic started");
            Console.WriteLine(task.Result);
        }

        [TestMethod]
        public void _2_TaskCompletionSource_DoWork_Tests()
        {
            _2_TaskCompletionSource taskrun = new _2_TaskCompletionSource();
            taskrun.DoWork();
            Console.WriteLine("DoWork started");
        }

        [TestMethod]
        public async Task _3_AttachDetachOperations_DoWork_Tests()
        {
            _3_AttachDetachOperations taskrun = new _3_AttachDetachOperations();
            await taskrun.DoWork();
        }

        [TestMethod]
        public void _3_AttachDetachOperations_DoWork2_Tests()
        {
            _3_AttachDetachOperations taskrun = new _3_AttachDetachOperations();
            taskrun.DoWork2();

            //output =>
            //task starting.
            //Attached child starting.
            //Attached child completing.
            //all completed.
        }

        [TestMethod]
        public async Task _4_AsyncStreams_Tests()
        {
            _4_AsyncStreams taskrun = new _4_AsyncStreams();
            await taskrun.DoWork();
        }

        [TestMethod]
        public void _5_Interlocked_Cancellation_Tests()
        {
            _5_Interlocked_Cancellation taskrun = new _5_Interlocked_Cancellation();
            taskrun.DoWork();
        }
    }
}
