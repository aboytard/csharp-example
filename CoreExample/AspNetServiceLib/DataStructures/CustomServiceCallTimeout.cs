namespace AspNetServiceLib.DataStructures
{
    public class CustomServiceCallTimeout
    {
        public TimeSpan? DefaultServiceCallTimeout { get; internal set; }

        public Dictionary<string, TimeSpan> ServiceCallTimeouts { get; internal set; } = new();
    }
}
