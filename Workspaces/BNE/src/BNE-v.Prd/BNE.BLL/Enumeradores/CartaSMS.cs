namespace BNE.BLL.Enumeradores
{
    public enum CartaSMS : int
    {
        BoasVindasVIP = 1,
        ValidacaoCelularEnvioCodigo = 2,
        SMSAviso_SaldoZerou = 3,
        SMSAviso_SaldoDisponivelMod1 = 4,
        SMSAviso_SaldoDisponivelMod2 = 5,
        SMSAviso_SaldoDisponivelMod3 = 6,     
        AlertaExperienciaProfissional = 7,
        AlertaExperienciaProfissional2dias = 8,
        AlertaExperienciaProfissionalFraca2dias = 9,
        SMSAlerta_CvsNaoVistos = 10,
        //PesquisaCurriculoEmpresa = 11,
        CampanhaVagaRetornoParaLigacao = 12,
        //CampanhaVagaRetornoSMS = 13,
        CampanhaSucessoParaCliente = 14,
        EmailNull = 15,
        /// <summary>
        /// Notificações VIP Média Salarial
        /// </summary>
        MediaSalarial = 16,

        VipVoceNaFrente = 17,
        VipAparicaoPesquisa = 18,
        VipExtrato = 19,
        VipNovasEmpresas = 20,

        /// <summary>
        /// Notificações VIP SMS Presidente
        /// </summary>
        SmsPresidente = 21
    }
}
