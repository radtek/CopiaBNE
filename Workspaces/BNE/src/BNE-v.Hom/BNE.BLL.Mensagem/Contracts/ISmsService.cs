using BNE.BLL.Mensagem.DTO;
using System.Collections.Generic;

namespace BNE.BLL.Mensagem.Contracts
{
    public interface ISmsService
    {
         void Enviar(List<DestinatarioSMS> destinatarios, string identificadorRemetente, bool agendar = false, int? idMesagemCampanha = null);
         void Enviar(DestinatarioSMS destinatarios, string identificadorRemetente, bool agendar = false, int? idMesagemCampanha = null);
         CampanhaTanqueMensagemDTO GetTemplate(Enumeradores.CampanhaTanque campanha);

    }
}
