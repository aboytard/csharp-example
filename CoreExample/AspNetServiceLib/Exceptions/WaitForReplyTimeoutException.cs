namespace AspNetServiceLib.Exceptions
{
    public class WaitForReplyTimeoutException : Exception
    {
        public WaitForReplyTimeoutException()
            : base("Timeout while waiting for request reply.")
        { }
    }
}
