namespace Freebox.Data.Modules
{
    /// <summary>
    /// Used to identify types that corresponds to responses sent by the Freebox Server
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Éviter les interfaces vides", Justification = "Used to filter types in generics to make sure types can be properly deserialized")]
    public interface IFreeboxApiResponse
    {
    }
}
