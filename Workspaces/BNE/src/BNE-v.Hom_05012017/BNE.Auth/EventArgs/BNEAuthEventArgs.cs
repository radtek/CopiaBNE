using System.Web;
using System.Web.SessionState;
using Microsoft.IdentityModel.Claims;

namespace BNE.Auth.EventArgs
{
    public class BNEAuthEventArgs : System.EventArgs
    {
        private readonly HttpContext _context;
        private readonly HttpSessionState _session;
        private HttpContextBase _contextBase;
        private HttpSessionStateBase _sessionBase;
        private ClaimsIdentity _identity;

        public BNEAuthEventArgs()
        {

        }

        public BNEAuthEventArgs(HttpSessionState session)
        {
            this._session = session;
        }

        public BNEAuthEventArgs(HttpSessionStateBase session)
        {
            this._sessionBase = session;
        }

        public BNEAuthEventArgs(HttpContext context)
        {
            this._context = context;
        }

        public BNEAuthEventArgs(HttpContextBase context)
        {
            this._contextBase = context;
        }

        public ClaimsIdentity Identity
        {
            get
            {
                return _identity != null ? _identity :
                                    Context != null && Context.User != null ? Context.User.Identity as ClaimsIdentity : null;
            }
            set
            {
                _identity = value;
            }
        }

        public HttpContextBase Context
        {
            get
            {
                if (_contextBase != null)
                    return _contextBase;

                if (_context == null)
                    return null;

                return _contextBase = new HttpContextWrapper(_context);
            }
            set { _contextBase = value; }
        }

        public HttpSessionStateBase Session
        {
            get
            {
                if (_sessionBase != null)
                    return _sessionBase;

                if (_session != null)
                {
                    return _sessionBase = new HttpSessionStateWrapper(_session);
                }

                return Context.Session;
            }
            set
            {
                _sessionBase = value;
            }
        }

    }
}
