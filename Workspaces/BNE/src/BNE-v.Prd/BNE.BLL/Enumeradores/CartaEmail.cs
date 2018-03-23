using System;
using BNE.BLL.Enumeradores.CustomAttribute;

namespace BNE.BLL.Enumeradores
{
    public enum CartaEmail
    {
        ConteudoPadraoBNE = 0,
        AgradecimentoCandidatura = 1,
        BoasVindasSine = 2,
        CadastroCurriculo = 3,
        //ContasTwitter = 4 , /* Retirado a pedido do Tortato em 26/02 */
        AtualizarCurriculo = 5,
        CadastroVaga = 7,
        CartilhaVIP = 8,
        ConfirmacaoPagamentoCIA = 9,
        ConfirmacaoPagamentoVIP = 10,
        ConfirmacaoPublicacao = 11,
        EmpresaLiberada = 12,
        IndiqueBNE = 13,
        RenovacaoCIA = 14,
        RenovacaoVIP = 15,

        RastreadorCurriculo = 17,
        RastreadorCurriculoCurriculoEncontrado = 83,

        CadastroCurriculoSTC = 18,
        FalePresidente = 19,
        FaleConosco = 20,
        AbandonoCompra = 21,
        VagaEmMassa = 22,
        PecaAjuda = 23,
        ForaFaixa = 24,

        CandidaturaEnvioTodosCurriculos = 25,
        CandidaturaEnvioTodosCurriculosLinkCurriculo = 89 ,

        EmailParaFiliais = 27,
        ConteudoEmailAtualizarEmpresa = 29,
        ConteudoEmailCurriculoInativado = 30,
        ConteudoEmailTerminoVipAntes = 31,
        ConteudoEmailPacoteVisualizacaoAntes = 32,
        DesistenciaCadastroEmpresa = 33,
        CadastroCurriculoSTCPadrao = 34,
        ConteudoPadraoEmpresa = 35,
        DesistenciaCadastroCurriculo = 36,
        ConteudoPadraoVIPUniversitario = 37,
        CadastroCurriculoVIPUniversitarioPadrao = 38,
        EnvioR1ParaVagasPoucasCandidaturas = 39,
        SolicitacaoR1 = 40,
        ConteudoPadraoBNERodape = 41,
        CompartilhamentoVagaPorEmail = 42,
        ConteudoBoletoVencimento = 43,
        ConteudoWebEstagiosIntegracaoAnuncioVaga = 45,
        ConteudoWebEstagiosIntegracaoCadastroEmpresa = 46,
        SolicitarContatoR1 = 47,
        ConteudoWebEstagiosIntegracaoQueroContratar = 48,
        ConteudoWebEstagiosIntegracaoPesquisaPorEstagiario = 49,
        SolicitarContatoCIA = 50,
        /// <summary>
        /// Carta a ser enviada ao responsável no financeiro para o tratamento de disputas.
        /// </summary>
        PagSeguro_AvisoDeDisputa = 51,
        ValidacaoEmail = 52,
        VendaPlanoVendedorAbaixoMinimo = 53,
        MensagemEmailCvPerfilVaga = 54,
        AlertaEmailExperienciaProfissional = 55,
        AlertaEmailExperienciaProfissionalFraca = 56,
        CandidatosPlanoLiberadoEmailSMSVaga = 57,
        DadosCandidatosPlanoLiberadoEmailSMSVaga = 58,
        ConteudoEmailAvisoCvsNaoVistos = 59,        
        EnviarEmailAlertaMensalFilial = 60,
        BoletosParaPagamento = 61,
        MensagemCiaNF = 62,
        MensagemVipNF = 63,
        BloqueioNaoPagamentoCartaoDeCredito = 64,
        BoasVindasNovamente = 65,
        ApresentacaoTopoEmailcomCVAnexoparaEmpresa = 67,
        /// <summary>
        /// Carta que é enviada para o vendedor responsável por uma empresa, quando a empresa está sem plano e tentando visualizar um currículo 
        /// </summary>
        EmpresaSemPlanoTentandoVisualizarCurriculo = 68,
        ExtratoPlano = 69,
        VendaPesquisaSalarial = 70,
        CampanhaSemBase = 71,
        CampanhaAtintiuTetoDisparo = 72,
        CampanhaInsucessoMais5Inscritos = 73,
        CampanhaInsucessoMenos5Inscritos = 74,
        CampanhaSucessoAtingiuQuantidadeRetorno = 75,
        EnviarEmailConfirmacaoCandidatura = 76,
        OportunidadeEmpresa = 79,
        OportunidadeEmpresaCorpoCandidato = 80,
        PagamentoRejeitadoViaDebitoHSBC = 81,
        EnviarEmailConfirmacaoCandidaturaVAGA = 82,
        NotificacaoCampanhaCriada = 85,
        indicaoAmigoBH = 86,
        CancelamentoRecorrenciaCIA = 87,
        CarrinhoAbandonadoVagaPremium = 88,
        SMSSemRetorno = 91,
        AlertaPoucosCvsNaPesquisa = 92,
        BoletoValorDiferente = 93,
        CancelamentoRecorrenciaVIP = 94,
        RenovacaoPlanoMensal200 = 95,

        [Mailsender("6eca6740-12cc-4cf0-97b8-b960b02fefd0")]
        QuemMeViuSmile = 97,
        [Mailsender("f8ed3b3b-32c7-4ced-8abe-e364d83bebe6")]
        QuemMeViuCheck = 99,
        [Mailsender("f73e75fc-2eda-477b-93d5-34811ab8b1a9")]
        QuemMeViuLupaCvs = 100,
        [Mailsender("4b1ab069-8d49-4361-84a7-91e8a3d24e2a")]
        QuemMeViuLupaLista = 101,
        [Mailsender("4c3b302f-28cd-475e-bbf1-89118b469284")]
        QuemMeViuWantYou = 102,
        [Mailsender("5c558899-621a-45f9-aa3a-2fc729f7bf9d")]
        QuemMeViuBatLuz = 103,

        CIACarrinhoAbandonado = 98,
        ExtratoVip = 104,
        PropagandaSalarioBR = 105,
        InscritosSTC = 106,
        AparicaoPesquisasVip = 107,
        VipVoceNaFrenteDePessoas = 108,
        NovasEmpresasNaCidade = 109,
        /// <summary>
        /// Notificação VIP de Média Salarial
        /// </summary>
        MediaSalarial = 110,
        /// <summary>
        /// Notificação VIP de Email Presidente
        /// </summary>
        EmailPresidente = 111,
        RastreadorCurriculoSemFuncao = 112,
        SolicitarAtualizacaoCV = 113,
        CurriculoAtualizado = 114, 
        NotasAntecipadasComPagamentoEmAberto = 115,
        AlertaVagasVip = 116,


        [Mailsender("f2e2a6db-51ac-4f8f-baa5-8281f36ba3a7")]
        AlertaPoucosCV = 117,



        ConteudoBoletoVencimentoSINE = 118,
        PesquisasRealizadasPelaEmpresa = 119,

        TemplateEmpresa = 120,
        ConfirmacaoCadastroVaga = 121,
        CardCurriculo = 122,
        ConfirmacaoCadastroVagaRecomencadoes = 123,
        ConfirmacaoCadastroVagaLinkParaVaga = 124,
        NotificarClienteVagaComPoucasCandidaturas = 125,
        NotificarInternamenteVagaComPoucasCandidaturas = 126,
        AlertaTentativaCompra = 127,
        AlertaTentativaCompraPF = 128,
        InscritosSTC_Candidatos = 129,
        PoucosCvsParaEmpresa = 130,
        NotaAntecipada = 131,
        BoletosBancariosNovos =  132,
        ConteudoPadraoSA = 133,
        CandEtapaProcessamento = 134,
        CandEtapaAnalise = 135,
        CandEtapaEnvio = 136,
        CandCurriculoVisualizado = 137,
        EmpresaBuscaSeuPerfil = 138

    }
}
