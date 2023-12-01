namespace TaskParallelLibrary._0_Asynchronous_Programming_Pattern.Event_Based_Asynchronous_Pattern
{
    internal class FirstExample
    {
        // Synchronous methods.  
        //public int Method1(string param);
        //public void Method2(double param);

        //// Asynchronous methods.  
        //public void Method1Async(string param);
        //public void Method1Async(string param, object userState);
        //public event Method1CompletedEventHandler Method1Completed;

        //public void Method2Async(double param);
        //public void Method2Async(double param, object userState);
        //public event Method2CompletedEventHandler Method2Completed;

        //public void CancelAsync(object userState);

        //public bool IsBusy { get; }
    }

    internal class SecondExample
    {
        // Good design  
        //private void Form1_MethodNameCompleted(object sender, xxxCompletedEventArgs e)
        //{
        //    DemoType result = e.Result;
        //}

        //// Bad design  
        //private void Form1_MethodNameCompleted(object sender, MethodNameCompletedEventArgs e)
        //{
        //    DemoType result = (DemoType)(e.Result);
        //}
    }
}
