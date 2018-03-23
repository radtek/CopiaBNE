using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.RedesSociais.EL
{
    /// <summary>
    /// Excessão levantada quando ocorrem erros dentro da integração de uma rede social
    /// </summary>
    public class RedeSocialException:Exception
    {
        public RedeSocialException(String message):base(message)
        {
                
        }
    }
}
