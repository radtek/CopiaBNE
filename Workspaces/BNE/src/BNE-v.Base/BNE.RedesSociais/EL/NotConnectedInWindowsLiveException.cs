using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.RedesSociais.EL
{
    public class NotConnectedInWindowsLiveException:RedeSocialException
    {
        public NotConnectedInWindowsLiveException()
            : base("Não conectado ao windows live messenger")
        {

        }
    }
}
