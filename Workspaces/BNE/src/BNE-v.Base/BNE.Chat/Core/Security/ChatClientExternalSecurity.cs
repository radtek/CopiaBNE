using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BNE.Chat.Core.Interface;

namespace BNE.Chat.Core.Security
{
    public class ChatClientExternalSecurity : IClientSimpleSecurity
    {
        private IEnumerable<Func<HttpContext, bool>> _permissions;

        public IEnumerable<Func<HttpContext, bool>> Permissions
        {
            get { return _permissions ?? Enumerable.Empty<Func<HttpContext, bool>>(); }
            set { _permissions = value; }
        }

        public bool Evaluate(HttpContext context)
        {
            if (context == null)
                if (!Permissions.Any())
                    return true;
                else
                    return false;

            foreach (var item in Permissions)
            {
                if (!item(context))
                    return false;
            }
            return true;
        }
    }
}
