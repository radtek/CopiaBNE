namespace BNE.BLL.Enumeradores
{
    public enum ConteudoHTML
    {
        ParametroURL = 1,
        ConteudoEmailPadrao = 2,

        ConteudoEmailConfirmacaoPagamentoVip = 90, //Está sendo usado pelo JAVA liberador de boletos
        ConteudoEmailConfirmacaoPagamentoCIA = 81, //Está sendo usado pelo JAVA liberador de boletos

        /* SMS */
        ConteudoSMSEnvioSemanal = 33,
        ConteudoSMSEnvioSemanalNaoCandidatou = 110,
        ConteudoSMSChameFacil = 74,
        MensagemSMSVisualizacaoCurriculoParaVIP = 108,
        MensagemSMSVisualizacaoCurriculoParaNaoVIP = 109,
        MensagemSMSVisualizacaoCurriculoParaNaoVIPPR = 112,
        MensagemTwitterVagaEmMassa = 68,
        /* FIM : SMS */


        /* Novas cartas - Referenciar carta email e carta sms */
        ConteudoPadraoEmailNovo = 120,
        ConteudoEmailNovoRastreadorVaga = 121,
        ConteudoSMSNovoRastreadorVaga = 122,
        /* FIM : Novas cartas */

        /*Sala Selecionador*/
        ConteudoTelaBoxVagaSalaSelecionadorInicial = 40,
        ConteudoTelaBoxMeuPlanoSalaSelecionadorInicial = 41,
        ConteudoTelaBoxRastreadorSalaSelecionadorInicial = 42,
        ConteudoTelaBoxR1SalaSelecionadorInicial = 44,
        /*Sala Selecionador*/
        /*Sala VIP*/
        ConteudoTelaBoxQuemMeViuSalaVip = 48,
        ConteudoTelaBoxEscolherEmpresaSalaVip = 49,
        ConteudoTelaBoxJaEnvieiSalaVip = 50,
        ConteudoTelaBoxMeuPlanoSalaVip = 51,
        ConteudoTelaBoxPesquisaVagaSalaVip = 105,
        /*Sala VIP*/
        /*Forma Pagamento*/
        //ConteudoTelaFormaPagamentoCandidatoSejaClienteVip = 52,
        //ConteudoTelaFormaPagamentoCandidatoCandidatar = 53,
        //ConteudoTelaFormaPagamentoCandidatoVantagemExclusiva = 55,
        //ConteudoTelaFormaPagamentoEmpresaAcessoIlimitado = 56,
        //ConteudoTelaFormaPagamentoEmpresaDivulgacaoMassa = 57,
        //ConteudoTelaFormaPagamentoCandidatoDadosEmpresa = 54,
        /*Forma Pagamento*/

        ConteudoTelaProposta = 69,

        MensagemPadraoHomeSTC = 72,

        ModalDegustacaoDuasOuMais = 113,
        ModalDegustacaoUma = 114,
        ModalDegustacaoAcabou = 115,
        ModalDegustacaoVoceGanhou = 116,
        ModalDegustacaoVoceGanhouTextoAdicional = 117,

        ContratoCia = 123,

        /* Compartilhamento de vaga */
        MensagemCompartilhamentoVaga = 131,

        ConteudoEmailEnvioBoletoVencimento = 132,

        MensagemSMSCvPerfilVaga = 135,
        MensagemEmailCvPerfilVaga = 136,
        SMSBoasVindasUm = 137,
        SMSBoasVindasDois = 138,
        EdicaoCadastroVaga = 139,
        VagasCadastrasSemCandidaturas = 140,
        MensagemCancelamentoNF = 141,
        ModeloSMSCampanhaMassa1 = 142,
        ModeloSMSCampanhaMassa2 = 143,
        ModeloSMSCampanhaMassa3 = 144
    }
}