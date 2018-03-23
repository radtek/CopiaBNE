//-- Data: 28/02/2013 11:38
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class TransacaoResposta // Tabela: BNE_Transacao_Resposta
    {

        #region SalvarResposta
        public static void SalvarResposta(Transacao objTransacao, bool aprovada, string descricaoResultado, string codigoAutorizacao, string descricaoTransacao, string cartaoMascarado, decimal? numeroSequencial, string comprovanteAdministradora, string nacionalidadeEmissor)
        {
            try
            {
                var objTransacaoResposta = new TransacaoResposta
                {
                    Transacao = objTransacao,
                    FlagTransacaoAprovada = aprovada,
                    DescricaoResultadoSolicitacaoAprovacao = descricaoResultado,
                    DescricaoCodigoAutorizacao = codigoAutorizacao,
                    DescricaoTransacao = descricaoTransacao,
                    DescricaoCartaoMascarado = cartaoMascarado,
                    NumeroSequencialUnico = numeroSequencial,
                    DescricaoComprovanteAdministradora = comprovanteAdministradora,
                    DescricaoNacionalidadeEmissor = nacionalidadeEmissor
                };

                objTransacaoResposta.Save();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

	}
}