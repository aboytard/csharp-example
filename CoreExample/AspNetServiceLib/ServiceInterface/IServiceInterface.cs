using AspNetServiceLib.DataStructures;

namespace AspNetServiceLib.ServiceInterface
{
    public enum InterfaceRoleEnum { Consumed, Provided };

    public interface IServiceInterface : IDisposable
    {
        string InterfaceName { get; }

        ServiceInterfaceDescriptors Descriptors { get; }

        //TODO: Do we really need this property? Maybe remove this later
        InterfaceRoleEnum InterfaceRole { get; }
    }
}
