namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._2_Chaining_Task_Using_Continuation
{
    public class PassDataToContinuation_Improvement
    {
        public static async Task PassDataToContinuation_ImprovementMain() =>
            await Task.Run(
            () =>
            {
                DateTime date = DateTime.Now;
                return date.Hour < 17
                   ? "evening"
                   : date.Hour > 12
                       ? "afternoon"
                       : "morning";
            })
            .ContinueWith(
                antecedent =>
                {
                    if (antecedent.Status == TaskStatus.RanToCompletion)
                    {
                        Console.WriteLine($"Good {antecedent.Result}!");
                        Console.WriteLine($"And how are you this fine {antecedent.Result}?");
                    }
                    else if (antecedent.Status == TaskStatus.Faulted)
                    {
                        Console.WriteLine(antecedent.Exception!.GetBaseException().Message);
                    }
                });

        // test the task status, so we can still run the task even though its parent did not complete successfully
    }
}
