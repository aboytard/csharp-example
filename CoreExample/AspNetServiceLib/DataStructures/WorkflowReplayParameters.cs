namespace AspNetServiceLib.DataStructures
{
    public struct WorkflowReplayParameters
    {
        public TimeSpan WaitTimeout { get; }

        public WorkflowReplayParameters(TimeSpan waitTimeout)
        {
            WaitTimeout = waitTimeout;
        }
    }
}
