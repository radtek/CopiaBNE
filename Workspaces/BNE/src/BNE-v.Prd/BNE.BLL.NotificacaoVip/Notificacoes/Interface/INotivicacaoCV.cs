using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.NotificacoesVip.Notificacoes
{
    internal interface  INotificacaoCV
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCurriculo">ID do Currículo que será notificado</param>
        /// <param name="ciclo">Ciclo de envio (Primeiro envio: ciclo 1 / Oitavo envio: ciclo 8)</param>
        bool Notificar(DTO.Curriculo curriculo, short idNotificacao, int idCiclo);
    }
}
    