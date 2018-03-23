namespace BNE.Chat.Core.Interface
{
    public interface ISignalRGenericResult<out T> : ISignalRGenericResult
    {
        T Result { get; }
    }

    public interface ISignalRGenericResult
    {
        object Value { get; set; }
    }
}