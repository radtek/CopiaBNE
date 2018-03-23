using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.Chat.Core.Interface;

namespace BNE.Chat.Core.Security
{
    public class EmptySecurity : IClientSimpleSecurity
    {
        protected EmptySecurity()
        {

        }
        public static readonly EmptySecurity Default = new EmptySecurity();

        private static readonly IEnumerable<Func<System.Web.HttpContext, bool>> DefaultPermissions = Enumerable.Empty<Func<System.Web.HttpContext, bool>>();

        public bool Evaluate(System.Web.HttpContext context)
        {
            return true;
        }
        public IEnumerable<Func<System.Web.HttpContext, bool>> Permissions
        {
            get { return DefaultPermissions; }
        }
    }
}
