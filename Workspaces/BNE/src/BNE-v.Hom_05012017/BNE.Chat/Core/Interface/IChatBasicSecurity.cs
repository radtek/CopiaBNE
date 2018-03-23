using System;

namespace BNE.Chat.Core.Interface
{
    public interface IClientSimpleSecurity
    {
        bool Evaluate(System.Web.HttpContext context);
        System.Collections.Generic.IEnumerable<Func<System.Web.HttpContext, bool>> Permissions { get; }
    }
}
