//-- Data: 31/07/2013 15:07
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class CurriculoVisualizacaoHistorico // Tabela: BNE_Curriculo_Visualizacao_Historico
    {

        #region Consultas

        #region Spquantidadevisualizacaodadoscompletosdecurriculovip
        private const string Spquantidadevisualizacaodadoscompletosdecurriculovip = @"
        SELECT  COUNT(DISTINCT(Idf_Curriculo))
        FROM    BNE_Curriculo_Visualizacao_Historico CVH
        WHERE   CVH.Idf_Filial = @Idf_Filial
		        AND CVH.Flg_Visualizacao_Completa = 1
		        AND CVH.Flg_Curriculo_VIP = 1
		        AND CVH.Dta_Visualizacao BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 1, CAST(GETDATE() AS DATE))
        ";
        #endregion

        #endregion

        #region SalvarHistoricoVisualizacao
	    /// <summary>
	    /// M�todo respons�vel por salvar o historico de visualizacao de uma filial a um curr�culo
	    /// </summary>
	    /// <param name="objFilial">Filial que est� acessando o CV</param>
	    /// <param name="objUsuarioFilialPerfil">Usu�rio filial perfil que visualizou o curr�culo</param>
	    /// <param name="objCurriculo">Curr�culo que est� sendo visualizado</param>
	    /// <param name="flagDadosCompletos">Representa se � visualiza��o dos dados completos</param>
	    /// <param name="descricaoIP">IP</param>
        /// <param name="objPesquisaCurriculo">Se vem proveniente de uma pesquisa de curr�culo, ent�o salva a pesquisa de curr�culo na resposta</param>
        public static void SalvarHistoricoVisualizacao(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo, bool flagDadosCompletos, string descricaoIP, PesquisaCurriculo objPesquisaCurriculo = null, Vaga objVaga = null, RastreadorCurriculo objRastreadorCurriculo = null, bool visualizacaoBaseGratis = false)
        {
            var objCurriculoVisualizacaoHistorico = new CurriculoVisualizacaoHistorico
            {
                DataVisualizacao = DateTime.Now,
                Filial = objFilial,
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Curriculo = objCurriculo,
                PesquisaCurriculo = objPesquisaCurriculo,
                Vaga = objVaga,
                RastreadorCurriculo = objRastreadorCurriculo,
                FlagCurriculoVIP = objCurriculo.VIP(),
                FlagVisualizacaoCompleta = flagDadosCompletos,
                FlagBaseGratis = visualizacaoBaseGratis,
                DescricaoIP = descricaoIP
            };
            objCurriculoVisualizacaoHistorico.Save();
        }
        public static void SalvarHistoricoVisualizacao(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo, bool VIP, bool flagDadosCompletos, string descricaoIP, PesquisaCurriculo objPesquisaCurriculo = null, Vaga objVaga = null, RastreadorCurriculo objRastreadorCurriculo = null)
        {
            var objCurriculoVisualizacaoHistorico = new CurriculoVisualizacaoHistorico
            {
                DataVisualizacao = DateTime.Now,
                Filial = objFilial,
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Curriculo = objCurriculo,
                PesquisaCurriculo = objPesquisaCurriculo,
                Vaga = objVaga,
                RastreadorCurriculo = objRastreadorCurriculo,
                FlagCurriculoVIP = VIP,
                FlagVisualizacaoCompleta = flagDadosCompletos,
                DescricaoIP = descricaoIP
            };
            objCurriculoVisualizacaoHistorico.Save();
        }
        public static void SalvarHistoricoVisualizacao(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo, bool flagDadosCompletos, string descricaoIP, string mensagemErro)
        {
            var objCurriculoVisualizacaoHistorico = new CurriculoVisualizacaoHistorico
            {
                DataVisualizacao = DateTime.Now,
                Filial = objFilial,
                Curriculo = objCurriculo,
                FlagCurriculoVIP = objCurriculo.VIP(),
                FlagVisualizacaoCompleta = flagDadosCompletos,
                DescricaoIP = descricaoIP,
                DescricaoMensagemErro = mensagemErro
            };
            objCurriculoVisualizacaoHistorico.Save();
        }

        #endregion

        #region RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP
        /// <summary>
        /// Recupera a quantidade de visualiza��o de dados de curr�culos vip �nicos por dia. Usado no fluxo de empresas chupa vip
        /// </summary>
        /// <param name="objFilial"></param>
        /// <returns></returns>
        public static int RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(Filial objFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadevisualizacaodadoscompletosdecurriculovip, parms));
        }
        public static int RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(Filial objFilial, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spquantidadevisualizacaodadoscompletosdecurriculovip, parms));
        }
        #endregion

    }
}