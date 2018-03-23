using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Enumeradores
{
    public enum SituacaoCurriculo
    {
        Publicado = 1,
        AguardandoPublicacao = 2,
        ComCritica = 3,
        AguardandoRevisaoVIP = 4,
        Bloqueado = 6,
        Cancelado = 7,
        Invisivel = 8,
        RevisadoVIP = 9,
        Auditado = 10
    }
}
