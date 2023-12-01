using _2_task_parallel_library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class IntroTaskParallelLibraryTests
    {

        [TestMethod]
        public void _1_CreatingAndRunningBasicTasks_Tests()
        {
            _1_CreatingAndRunningBasicTasks taskrun = new _1_CreatingAndRunningBasicTasks();
            taskrun.DoWork1();
            //output is that after dowork1 -> 
            //started
            //completed...
            //8
            //10
            taskrun.DoWork2();
            //output is that after DoWork2 -> 
            //started DoWork
            //10
            //8
            //10
            //completed DoWork...
        }

        [TestMethod]
        public void _2_TaskRun_Tests_GetLines_1()
        {
            _2_TaskRun taskrun = new _2_TaskRun();
            var result = taskrun.GetLines("C:/Test-Sample/Asynchronous/Sample_Lines.txt");
            Console.WriteLine("we have lines");
            result.Wait();
            Assert.AreEqual(189, result.Result.Length);

            //output is that

            //started
            //ended
            //we have lines

            //codes dont wait until the end of task run codes

        }

        [TestMethod]
        public void _2_TaskRun_Tests_GetLines_2()
        {
            _2_TaskRun taskrun = new _2_TaskRun();
            var result = taskrun.GetLines2("C:/Test-Sample/Asynchronous/Sample_Lines.txt");
            result.Wait();
            Console.WriteLine("we have lines");
            Assert.AreEqual(189, result.Result.Length);

            //output is that 

            //started
            //we have lines
            //ended


            //codes will wait until the end of task codes

        }

        [TestMethod]
        public void _2_TaskRun_Tests_ReturnName_2()
        {
            _2_TaskRun taskrun = new _2_TaskRun();
            var result = taskrun.DoWorkWithReturn();
            Console.WriteLine(result);
            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void _3_Task_ActionFunc_Tests()
        {
            _3_Task_ActionFunc taskrun = new _3_Task_ActionFunc();
            taskrun.DoWorkWithAction();
            var result = taskrun.DoWorkWithFunc();
            Console.WriteLine(result);
            //some piece of output ->
            //action: 9
            //action: 10
            //action: 11
            //action: 12
            //action: 13
            //func: 0
            //func: 1
            //func: 2
            //func: 3
            //func: 4
            //func: 5
            //action: 14
            //func: 6
            //action: 15
            //action: 16
            //action: 17
            Assert.AreEqual(1225, result);
        }

        [TestMethod]
        public void _4_Task_ContinueWith_Tests()
        {
            _4_Task_ContinueWith taskrun = new _4_Task_ContinueWith();
            taskrun.DoWork1();
        }

        [TestMethod]
        public void _5_Task_Cancellation_Tests()
        {
            _5_Task_Cancellation taskrun = new _5_Task_Cancellation();

            taskrun.Run("t1", 500000); //will run
            taskrun.Run("t2", 500000); //will be cancelled

            taskrun.Run("t3", 500000); //will run
            taskrun.Run("t4", 500000); //will be cancelled
        }

        [TestMethod]
        public void _6_Task_Handling_Exceptions_Tests()
        {
            _6_Task_Handling_Exceptions taskrun = new _6_Task_Handling_Exceptions();
            taskrun.DoWork();
        }

        [TestMethod]
        public void _7_Task_WaitAll_Tests()
        {
            _7_Task_WaitAll taskrun = new _7_Task_WaitAll();
            taskrun.DoWork_WaitAll();

            //output with WaitAll
            //operations started
            //process 1 :0
            //...
            //process 1 :17
            //process 1 :18
            //process 1 :19
            //process 1 :20
            //process 1 :21
            //process 2 :0
            //process 2 :1
            //process 1 :22
            //process 1 :23
            // .....
            //process 2 :9998
            //process 2 :9999
            //operations completed
        }

        [TestMethod]
        public void _7_Task_WaitAny_Tests()
        {
            _7_Task_WaitAll taskrun = new _7_Task_WaitAll();
            taskrun.DoWork_WaitAny();
        }

        [TestMethod]
        public void _8_Task_ContinueWhenAll_Tests()
        {
            _8_Task_ContinueWhenAll taskrun = new _8_Task_ContinueWhenAll();
            taskrun.DoWork();
        }
    }
}
