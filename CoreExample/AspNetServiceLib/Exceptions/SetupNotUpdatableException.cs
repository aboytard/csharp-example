namespace AspNetServiceLib.Exceptions
{
    public class SetupNotUpdatableException : Exception
    {
        public SetupNotUpdatableException() : base("Service interface setup not updatable right now.")
        { }
    }
}
