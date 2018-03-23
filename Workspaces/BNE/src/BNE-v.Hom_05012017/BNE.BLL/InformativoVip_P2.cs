//-- Data: 13/09/2016 15:35
//-- Autor: Marty Sroka

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class InformativoVip // Tabela: BNE_Informativo_Vip
	{
        #region Consultas

        #region SpSelectInformativoPorTipoEOrdem
        private const string SpSelectInformativoPorTipoEOrdem = @"
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

        #endregion

        #region Metodos

        #region RetornarInformativoEnvioCVPorTipo
        public static InformativoVip RetornarInformativoEnvioCVPorTipo(int idCurriculo, int idTipoInformativo)
        {
            InformativoVip informativo = new InformativoVip();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo });
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Tipo_Informativo_Vip", SqlDbType = SqlDbType.Int, Size = 4, Value = idTipoInformativo });

            var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectInformativoPorTipoEOrdem, parms);

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

        #endregion
	}
}