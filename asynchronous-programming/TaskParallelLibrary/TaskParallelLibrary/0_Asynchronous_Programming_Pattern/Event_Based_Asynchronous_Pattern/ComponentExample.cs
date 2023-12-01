using System.ComponentModel;

namespace TaskParallelLibrary._0_Asynchronous_Programming_Pattern.Event_Based_Asynchronous_Pattern
{
    // this is to be used with winForm afterwards
    internal class PrimeNumberCalculator : Component
    {
        public delegate void ProgressChangedEventHandler(
            ProgressChangedEventArgs e);

        public delegate void CalculatePrimeCompletedEventHandler(
            object sender,
            CalculatePrimeCompletedEventArgs e);

        public event ProgressChangedEventHandler ProgressChanged;
        public event CalculatePrimeCompletedEventHandler CalculatePrimeCompleted;

        public class CalculatePrimeCompletedEventArgs :
            AsyncCompletedEventArgs
        {
            private int numberToTestValue = 0;
            private int firstDivisorValue = 1;
            private bool isPrimeValue;

            public CalculatePrimeCompletedEventArgs(
                int numberToTest,
                int firstDivisor,
                bool isPrime,
                Exception e,
                bool canceled,
                object state) : base(e, canceled, state)
            {
                this.numberToTestValue = numberToTest;
                this.firstDivisorValue = firstDivisor;
                this.isPrimeValue = isPrime;
            }

            public int NumberToTest
            {
                get
                {
                    // Raise an exception if the operation failed or
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return the
                    // property value.
                    return numberToTestValue;
                }
            }

            public int FirstDivisor
            {
                get
                {
                    // Raise an exception if the operation failed or
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return the
                    // property value.
                    return firstDivisorValue;
                }
            }

            public bool IsPrime
            {
                get
                {
                    // Raise an exception if the operation failed or
                    // was canceled.
                    RaiseExceptionIfNecessary();

                    // If the operation was successful, return the
                    // property value.
                    return isPrimeValue;
                }
            }
        }
    }
}
