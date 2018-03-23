using System;
using System.Threading.Tasks;
using System.Web;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Hubs;
using BNE.Chat.Core.Interface;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using System.Web.SessionState;

namespace BNE.Web.Code
{
    public abstract class BNEChatConsumerBasic : BNEChatConsumerBase
    {
        private readonly IClientSimpleSecurity _consumerSecurity;
        protected BNEChatConsumerBasic(IChatListener manager, IClientSimpleSecurity consumerSecurity)
            : base(manager)
        {
            if (consumerSecurity == null)
                throw new NullReferenceException("consumerSecurity");
            _consumerSecurity = consumerSecurity;
        }

        public virtual IClientSimpleSecurity ConsumerSecurity
        {
            get { return _consumerSecurity; }
        }

        protected virtual int GetUniqueIdentifier(HttpSessionStateBase session)
        {
            var usuarioFilialPerfil = session[typeof(SessionVariable<int>) + Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()];

            return ResolveUniqueIdentier(usuarioFilialPerfil);
        }

        private static int ResolveUniqueIdentier(object usuarioFilialPerfil)
        {
            if (usuarioFilialPerfil == null)
                return 0;

            if (usuarioFilialPerfil is int)
            {
                return (int)usuarioFilialPerfil;
            }

            int filiaPerfilId;
            if (Int32.TryParse(usuarioFilialPerfil.ToString(), out filiaPerfilId))
            {
                return filiaPerfilId;
            }

            return 0;
        }

        protected virtual int GetUniqueIdentifierUnsafe()
        {
            var usuarioFilialPerfil = HttpContext.Current.Session[typeof(SessionVariable<int>) + Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()];

            return ResolveUniqueIdentier(usuarioFilialPerfil);
        }

        protected override bool IsValidToUse(object sender, EventArgs eventArgs)
        {
            var res = ConsumerSecurity.Evaluate(HttpContext.Current);

            if (!res)
            {
                var ret = eventArgs as ChatResultEventArgs;
                if (ret != null)
                {
                    var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                    tcs.SetResult(new SignalRGenericResult<int>(401)); // Unauthorized
                    ret.TaskValueResult = tcs.Task;
                }
            }
            return res;
        }
    }
}