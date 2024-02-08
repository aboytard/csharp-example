namespace AspNetServiceLib.DataStructures
{
    public struct WorkflowRecordingScope
    {
        public ServiceCall[] ServiceCalls { get; }

        public WorkflowRecordingScope(params ServiceCall[] serviceCalls)
        {
            ServiceCalls = serviceCalls;
        }
    }
}
