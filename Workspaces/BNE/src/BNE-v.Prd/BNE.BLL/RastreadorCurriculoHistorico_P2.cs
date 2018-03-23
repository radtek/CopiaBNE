//-- Data: 01/02/2016 14:13
//-- Autor: Gieyson Stelmak

using System;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class RastreadorCurriculoHistorico // Tabela: BNE_Rastreador_Curriculo_Historico
    {

        #region SalvarHistorico
	    /// <summary>
	    /// Salva um histórico de rastreamento de currículos
	    /// </summary>
	    /// <param name="objRastreadorCurriculo"></param>
	    /// <param name="total"></param>
	    /// <param name="novos"></param>
	    /// <param name="tempoProcessamentoSolr"></param>
	    /// <param name="trans"></param>
	    public static void SalvarHistorico(RastreadorCurriculo objRastreadorCurriculo, Int16 total, Int16 novos, TimeSpan tempoProcessamentoSolr, SqlTransaction trans)
        {
	        var objRastreadorCurriculoHistorico = new RastreadorCurriculoHistorico
	        {
                RastreadorCurriculo = objRastreadorCurriculo,
                DataProcessamento = DateTime.Now,
                QuantidadeTotal = total,
                QuantidadeNovos = novos,
                HrsDuracaoProcessamento = tempoProcessamentoSolr
	        };
            objRastreadorCurriculoHistorico.Save(trans);
        }
        #endregion

    }
}