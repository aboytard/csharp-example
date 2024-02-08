namespace AspNetServiceLib.Attributes
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ServiceInterfaceDataAttribute : Attribute
    {
        public string DataStructureName { get; }

        public ServiceInterfaceDataAttribute(string dataStructureName)
        {
            DataStructureName = dataStructureName;
        }
    }
}
