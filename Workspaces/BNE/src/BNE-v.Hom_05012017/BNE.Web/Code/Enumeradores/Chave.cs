namespace BNE.Web.Code.Enumeradores
{
    internal static class Chave
    {
        internal enum Temporaria
        {
            Variavel1,
            Variavel2,
            Variavel3,
            Variavel4,
            Variavel5,
            Variavel6,
            Variavel7,
            Variavel8,
            Variavel9,
            Variavel10,
            Variavel11,
            Variavel12,
            Variavel13,
            Variavel14,
            Variavel15,
            Variavel16,
            Variavel17,
            Variavel18,
            Variavel19,
            /* i-Phone */
            Variavel20,
            Variavel21,
            Variavel22,
            Variavel23,
            Variavel24,
            Variavel25,
            PageIndex,
            PageIndexCampanha,
            IdFuncaoPesquisaRapida,
            IdCidadePesquisaRapida,
            PalavraChavePesquisaRapida,
            Permissoes,
            MensagemPermissao,

            /* Pagamento */
            IdPlano,
            IdPlanoNormal,
            IdPlanoExtendido,
            IdPlanoPromocional,
            IdPlanoRecorrenteVip,
            //IdPlanoSemestral,
            //IdConteudoHTML,
            //CodigoVaga,

            VendaPlanoCIA_PlanoPrePagoIdentificador,
            VendaPlanoCIA_PlanoTrimestralIdentificador,
            VendaPlanoCIA_PlanoAnualIdentificador,
            VendaPlanoCIA_PlanoCustomizadoIdentificador,

            PesquisaPadrao,

            VendaPlanoCIA_PlanoBasicoIdentificador,
            VendaPlanoCIA_PlanoIndicadoIdentificador,
            VendaPlanoCIA_PlanoPlusIdentificador, 
            VendaPlanoCIA_PlanoCemIdentificador,

            Variavel43,
            Variavel44,
            Variavel45,
            Variavel46,
            Variavel47,
            PesquisaIdiomas,
            FiltroPesquisa,
            ViewStateObject_ResultadoPesquisaCurriculo,
        }

        internal enum Redirecionamento
        {
            MensagemPermissaoRedirecionamento
        }

        internal enum Permanente
        {
            IdCurriculo,
            IdVaga,
            IdFilial,
            IdPessoaFisicaLogada,
            IdUsuarioLogado,

            IdUsuarioFilialPerfilLogadoEmpresa,
            IdUsuarioFilialPerfilLogadoCandidato,
            IdUsuarioFilialPerfilLogadoUsuarioInterno,
            IdPerfil,

            STC,
            Theme,
            TipoBuscaMaster,
            IdOrigem,
            //IdCurriculoVisualizacao,
            /* Mater - Utilizado pela busca na master page */
            FuncaoMaster,
            CidadeMaster,
            PalavraChaveMaster,
            /* FIM: Mater */

            UrlDestinoPagamento,
            UrlDestino,

            PagamentoIdentificadorPlano,
            PagamentoIdentificadorPagamento,
            PagamentoIdentificadorPlanoAdquirido,
            PagamentoValorPago,
            PagamentoUrlRetorno,
            PagamentoIdCodigoDesconto ,
            PagamentoAdicionalValorTotal,
            PagamentoAdicionalQuantidade,
            PagamentoFormaPagamento,
            PrazoBoleto,
            ValorBasePlano,

            MostrarModalDegustacaoCandidatura,

            SesssaoIniciadaControle,

            PalavraChavePesquisa,
            IsSTCMaster,

            PagamentoJustificativaAbaixoMinimo,

            PesquisaCurriculo
        }

    }
}