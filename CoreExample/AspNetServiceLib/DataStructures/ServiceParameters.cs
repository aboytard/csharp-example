namespace AspNetServiceLib.DataStructures
{
    public struct ServiceParameters
    {
        public bool UseIdentityOwnedServiceCalls { get; }

        public bool UseSingleActiveConsumerInstance { get; }

        public bool UseWorkflowEngine { get; }

        public ServiceParameters(
            bool useIdentityOwnedServiceCalls,
            bool useSingleActiveConsumerInstance,
            bool useWorkflowEngine)
        {
            if (useWorkflowEngine && !useIdentityOwnedServiceCalls)
            {
                throw new ArgumentException(
                    "Workflow Engine can only be used if identity-owned service calls are used.",
                    nameof(useWorkflowEngine));
            }
            if (useSingleActiveConsumerInstance && !useIdentityOwnedServiceCalls)
            {
                throw new ArgumentException(
                    "Single active consumer instance can only be used if identity-owned service calls are used.",
                    nameof(useSingleActiveConsumerInstance));
            }
            UseIdentityOwnedServiceCalls = useIdentityOwnedServiceCalls;
            UseSingleActiveConsumerInstance = useSingleActiveConsumerInstance;
            UseWorkflowEngine = useWorkflowEngine;
        }
    }
}
