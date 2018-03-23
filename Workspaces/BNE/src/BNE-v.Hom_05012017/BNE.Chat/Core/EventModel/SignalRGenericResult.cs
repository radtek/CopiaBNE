using BNE.Chat.Core.Interface;

namespace BNE.Chat.Core.Hubs
{
    public class SignalRGenericResult<T> : ISignalRGenericResult<T>
    {
        public SignalRGenericResult()
        {
            
        }
        public SignalRGenericResult(T obj)
        {
            this._value = obj;
        }
        private object _value;

        public T Result
        {
            get { return (T)_value; }
            set { _value = value; }
        }
        object ISignalRGenericResult.Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}