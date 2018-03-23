using System;

namespace BNE.EL.Interface
{
    public interface ILogger
    {

        string Name { get; set; }
        Guid Error(Exception ex);
        Guid Error(Exception ex, string customMessage);

    }
}
