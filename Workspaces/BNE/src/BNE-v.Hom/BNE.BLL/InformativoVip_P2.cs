//-- Data: 13/09/2016 15:35
//-- Autor: Marty Sroka

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class InformativoVip // Tabela: BNE_Informativo_Vip
	{
        #region Consultas

        #region SpSelectInformativoPorTipoEOrdemNaoEnviado
        private const string SpSelectInformativoPorTipoEOrdemNaoEnviado = @"
            SELECT 
                TOP 1 iv.* 
            FROM 
                BNE.BNE_Informativo_Vip iv WITH(NOLOCK) 
            OUTER APPLY (
	                SELECT * FROM BNE.BNE_Informativo_Vip_Curriculo ivc WITH(NOLOCK) 
	                where ivc.Idf_Informativo_Vip = iv.Idf_Informativo_Vip
	                AND ivc.Idf_Curriculo = @Idf_Curriculo
                ) NotificacoesCV
            WHERE 1 = 1 
                AND iv.Flg_Inativo = 0                
                AND iv.Idf_Tipo_Informativo_Vip = @Idf_Tipo_Informativo_Vip
                AND NotificacoesCV.Idf_Informativo_Vip IS null
                
        ";
        #endregion

        #region SpSelectInformativoGenericoParaEnvio
        private const string SpSelectInformativoGenericoParaEnvio = @"
            SELECT 
               TOP 1 iv.* 
            FROM 
                BNE.BNE_Informativo_Vip iv WITH(NOLOCK) 
            OUTER APPLY (
	                SELECT TOP 1 * FROM BNE.BNE_Informativo_Vip_Curriculo ivc WITH(NOLOCK) 
	                where ivc.Idf_Informativo_Vip = iv.Idf_Informativo_Vip
	                AND ivc.Idf_Curriculo = @Idf_Curriculo
		            ORDER BY Dta_Cadastro desc
                ) NotificacoesEnviadasCV
            WHERE 1 = 1 
                AND iv.Flg_Inativo = 0                
                AND iv.Idf_Tipo_Informativo_Vip = @Idf_Tipo_Informativo_Vip
            ORDER BY NotificacoesEnviadasCV.Dta_Cadastro
        ";
        #endregion

        #endregion

        #region Metodos

        #region RetornarInformativoNaoEnviadoCVPorTipo
        /// <summary>
        /// Retorna o primeiro informativo que nunca foi enviado para o VIP
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="idTipoInformativo"></param>
        /// <returns></returns>
        public static InformativoVip RetornarInformativoNaoEnviadoCVPorTipo(int idCurriculo, int idTipoInformativo)
        {
            InformativoVip informativo = new InformativoVip();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo });
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Tipo_Informativo_Vip", SqlDbType = SqlDbType.Int, Size = 4, Value = idTipoInformativo });

            var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectInformativoPorTipoEOrdemNaoEnviado, parms);

            if(SetInstance(dr, informativo))
            {
                return informativo;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region RetornarProximoInformativoCVPorTipo
        /// <summary>
        /// Retorna o informativo que foi enviado a mais tempo (ou nunca foi enviado) para o VIP, criando uma fila de envio
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="idTipoInformativo"></param>
        /// <returns></returns>
        public static InformativoVip RetornarProximoInformativoCVPorTipo(int idCurriculo, int idTipoInformativo)
        {
            InformativoVip informativo = new InformativoVip();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo });
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Tipo_Informativo_Vip", SqlDbType = SqlDbType.Int, Size = 4, Value = idTipoInformativo });

            var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectInformativoGenericoParaEnvio, parms);

            if (SetInstance(dr, informativo))
            {
                return informativo;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #endregion
    }
}