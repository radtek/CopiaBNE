using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.Enumeradores
{
    public enum MotivoCancelamento
    {
        //VIP
        JaEstouEmpregado = 1,
        AchoQueOVipNaoFunciona = 2,
        PoucasVagas = 3,
        NaoEntendiComoOsiteFunciona = 4,
        PlanoVIPEstaCaro = 5,
        RecebiMuitoEmails = 6,
        OutrosVIP = 7,

        //CIA
        JaFinalizeiMeuProcessoSeletivo_CandidatoBNE = 8,
        JaFinalizeiMeuProcessoSeletivo_Indicacao = 9,
        JaFinalizeiMeuProcessoSeletivo_Outro = 10,
        NaoConseguiUtilizarOSite = 11,
        NaoObtiveResultadosComAnuncio = 12,
        NaoConseguiContatoComOsCandidatos = 13,
        ValorDaAssinaturaMuitoAlto = 14,
        VouTestarOutrasFerramentas = 15,
        EstaAssinaturaNaoAtendeMinhasNecessidades = 16,
        OutrosCIA = 17


    }
}
