using System.Collections.Generic;

namespace BNE.Log.Base
{
    public interface IReadWriteDbLog
    {
        void WriteList(IEnumerable<BaseMessage> ls);
    }
}
