namespace BNE.Common.Enumeradores
{
    public static class Chave
    {

        public enum Permanente
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
            STCUniversitario,
            STCComVIP,
            Theme,
            TipoBuscaMaster,
            IdOrigem,
            /* Mater - Utilizado pela busca na master page */
            FuncaoMaster,
            CidadeMaster,
            PalavraChaveMaster,
            /* FIM: Mater */
            
            UrlDestinoPagamento,
            UrlDestino,

            PagamentoIdentificadorPlano ,
            PagamentoIdentificadorPagamento ,
            PagamentoIdentificadorPlanoAdquirido,
            PagamentoUrlRetorno ,
            PagamentoIdCodigoDesconto ,
            PagamentoAdicionalValorTotal,
            PagamentoAdicionalQuantidade,
            PagamentoFormaPagamento,

            MostrarModalDegustacaoCandidatura,

            SesssaoIniciadaControleEventos,
            AutenticacaoVerificadaNaSessao,
            IsSTCMaster , 
            PrimeiraGratis
        }

    }
}