using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.NotificacoesVip.Enumeradores
{
    public enum StatusExecucaoNotificacaoVip
    {
        criado = 1,
        executando = 2,
        finalizado_Sucesso = 3,
        finalizado_Erro = 4,
        finalizado_SemDados = 5
    }
}
