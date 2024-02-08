namespace AspNetServiceLib.Exceptions
{
    public class UnspecificDescriptorsException : Exception
    {
        public UnspecificDescriptorsException(string actionText)
            : base($"Unspecific service interface descriptors not allowed for {actionText}.")
        {
        }
    }
}
