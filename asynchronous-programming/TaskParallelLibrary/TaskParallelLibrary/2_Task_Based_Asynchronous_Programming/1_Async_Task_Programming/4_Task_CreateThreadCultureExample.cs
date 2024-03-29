﻿using System.Globalization;

namespace TaskParallelLibrary.Task_Based_Asynchronous_Programming._1_Async_Task_Programming
{
    public class ThreadCultureExample
    {
        public static void ThreadCultureExampleMain()
        {
            decimal[] values = { 163025412.32m, 18905365.59m };
            string formatString = "C2";
            Func<String> formatDelegate = () => {
                string output = String.Format("Formatting using the {0} culture on thread {1}.\n",
                                                                                CultureInfo.CurrentCulture.Name,
                                                                                Thread.CurrentThread.ManagedThreadId);
                foreach (var value in values)
                    output += String.Format("{0}   ", value.ToString(formatString));

                output += Environment.NewLine;
                return output;
            };

            Console.WriteLine("The example is running on thread {0}",
                              Thread.CurrentThread.ManagedThreadId);
            // Make the current culture different from the system culture.
            Console.WriteLine("The current culture is {0}",
                              CultureInfo.CurrentCulture.Name);
            if (CultureInfo.CurrentCulture.Name == "fr-FR")
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            else
                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

            Console.WriteLine("Changed the current culture to {0}.\n",
                              CultureInfo.CurrentCulture.Name);

            // Execute the delegate synchronously.
            Console.WriteLine("Executing the delegate synchronously:");
            Console.WriteLine(formatDelegate());

            // Call an async delegate to format the values using one format string.
            Console.WriteLine("Executing a task asynchronously:");
            var t1 = Task.Run(formatDelegate);
            Console.WriteLine(t1.Result);

            Console.WriteLine("Executing a task synchronously:");
            var t2 = new Task<String>(formatDelegate);
            t2.RunSynchronously();
            Console.WriteLine(t2.Result);
        }
    }
}
