using TaskParallelLibrary.Data_Parallelism;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._1_Async_Task_Programming;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._2_Chaining_Task_Using_Continuation;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._3_Attached_Detached_Child_Task;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._4_Task_Cancellation_Example;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._5_ExceptionHandling;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._6_ParallelInvokeExample;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._7_Task_Return_Value_Example;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._8_PreComputed_Task_Example;
using TaskParallelLibrary.Task_Based_Asynchronous_Programming._9_TreeDataStructure_Example;

namespace TaskParallelLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Data_Parallelism_Example
            Data_Parallelism_Example();
            // Task_Based Example:
            #endregion
            #region TaskBasedAsynchronousProgrammingExample
            //TaskBasedAsynchronousProgrammingExample();
            //Chaining_Example();
            //Attached_Detached_Child_Example();
            //Cancellation_Example();
            //ExceptionExample();
            //ParallelInvokeExample();
            //TaskReturnValue();
            //PreComputed();
            //TreeDataStructureExample();
            #endregion


            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region data parallelism
        private static void Data_Parallelism_Example()
        {
            //DirectorySize();
            MultiplyMatrices.MultiplyMatricesMain(new string[] { });
            //PrimeExample.PrimeExampleMain();
            //RetrieveStateExample.RetrieveStateExampleMain();
            //PartitionVariableExample.PartitionVariableExampleMain();
            //CancelParallelLoopExample.CancelParallelLoopMain();
            //HandleExceptionInParallelLoopExample.HandleExceptionInParallelLoopExampleMain();
            //SpeedUpSmallLoopBodiesExample.SpeedUpSmallLoopBodiesExampleMain();
            IterateFileWithParallelClassExample.IterateFileWithParallelClassExampleMain();

        }

        private static void DirectorySize()
        {
            //  example full path
            var path = "C:\\Users\\a00542157\\source\\repos\\TaskParallelLibrary\\TaskParallelLibrary";
            DirectorySizeExample.DirectorySizeExampleMain(new string[] { path });
        }
        #endregion

        #region Task_based_Asynchronous_Programming
        private static void TaskBasedAsynchronousProgrammingExample()
        {
            //LambdaExample.LambdaExampleMain_Start();
            //LambdaExample.LambdaExampleMain_Run();

            // Factory
            //AsyncState.AsyncStateMain(); // this one without factory --> show it is also possible that way
            //AsyncState.DoComputationMain();
            //AsyncState.AsyncStateMain_Issue();
            //AsyncState.AsyncStateMain_Issue_Solution();
            //AsyncState.AsyncStateMain_Issue_Solution_Variant();

            // CreateOptions
            //Task_CreateOptions.Task_CreateOptionsMain();

            // ThreadCulture
            //ThreadCultureExample.ThreadCultureExampleMain();
            //Task_CreateContinuationExample.Task_CreateContinuation_1();
            //Task_CreateContinuationExample.Task_CreateContinuation_2();

            // Detached Child
            //Task_CreatingDetachedChild.Task_CreatingDetachedChildMain();
            Task_ChildTask.Task_ChildTaskMain();
        }

        private static void Chaining_Example()
        {
            //PassDataToContinuation_Example.PassDataToContinuation_ExampleMain();
            //PassDataToContinuation_Improvement.PassDataToContinuation_ImprovementMain();
            //CancelContinuationExample.CancelContinuationExampleMain();
            //CancelContinuation_TokenNotProvided.CancelContinuation_WithoutTokenMain();

            // State to Continuation
            //AssociateState_To_Continuation_Example.AssociateState_To_Continuation_ExampleMain();

            // Return Task
            //Continuation_ReturningTaskType.Continuation_ReturningTaskTypeMain();

            // Exception
            Handle_ExceptionThrown.Handle_ExceptionThrownMain_1();
        }

        private static void Attached_Detached_Child_Example()
        {
            //Detached
            //DetachedChild_Example.DetachedChild_ExampleMain();
            //DetachedChild_Example.DetachedChild_ExampleMain_WithTResult();

            // Attached
            //AttachedChild_Example.AttachedChild_ExampleMain();

            ParentDenyingChildAttachingAccess_Example.ParentDenyingChildAttachingAccess_ExampleMain();
        }

        private static void Cancellation_Example()
        {
            //Task_CancellationExample.Task_CancellationExampleMain();
            Cancel_Task_And_ChildrenExample.Cancel_Task_And_ChildrenExampleMain();
        }

        private static void ExceptionExample()
        {
            //AggregatedException_Example1.Aggregated_Example1();
            //AggregatedException_Example1.Aggregated_Example2();

            // nested
            //Flatten_NestedException_Example.Flatten_NestedException_Example1();
                //directory
            Flatten_NestedException_Example.Directory_task_Exception_exampleExecuteTasks();
        }

        private static void ParallelInvokeExample()
        {
            ParallelInvoke.ParallelInvokeMain();
        }

        private static void TaskReturnValue()
        {
            TaskReturnValueExample.TaskReturnValueExampleMain();
        }

        private static void PreComputed() 
        {
            PreComputedExample.PreComputedExampleMain();
        }

        private static void TreeDataStructureExample()
        {
            TreeDataStructure.TreeDataStructureMain();
        }
        #endregion


    }
}