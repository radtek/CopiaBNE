//-- Data: 18/07/2016 15:08
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BNE.BLL
{
	public partial class PlanoMotivoCancelamento // Tabela: BNE_Plano_Motivo_Cancelamento
	{
        #region Consulta

        #region     [spConsultaMotivoCancelamentoUltimoPlano]
        private const string spConsultaMotivoCancelamentoUltimoPlano = @"select  mc.des_motivo_Cancelamento, pmc.des_detalhe_motivo_Cancelamento, pf.nme_pessoa
                                                 from bne.bne_plano_motivo_cancelamento pmc with(nolock)
                                                join bne.bne_motivo_cancelamento mc with(nolock) on pmc.idf_motivo_cancelamento = mc.idf_motivo_cancelamento
                                                left join bne.tab_usuario_filial_perfil ufp with(nolock) on ufp.idf_usuario_filial_perfil = pmc.idf_usuario_Filial_perfil
                                                left join bne.tab_pessoa_fisica pf with(nolock) on ufp.idf_pessoa_Fisica = pf.idf_pessoa_Fisica
                                                where idf_plano_adquirido = @Idf_Plano_Adquirido";
        #endregion



        #endregion

        #region Metodos

        #region Metricas

        public static IDataReader Metricas(int idCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
            };

            parms[0].Value = idCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE.SP_Metricas_CV", parms);

        }
        #endregion



        #region [ConsultaMotivoExclusaoCurriculo]

        public static string ConsultaMotivoCancelamentoUltimoPlano(int IdPlanoAdquirido, out string QuemCancelou)
        {
            StringBuilder stringRetorno = new StringBuilder();
            QuemCancelou = string.Empty;
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Value = IdPlanoAdquirido}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spConsultaMotivoCancelamentoUltimoPlano, parametros))
            {
                while (dr.Read())
                {

                    if (dr["des_detalhe_motivo_Cancelamento"] != DBNull.Value)
                        stringRetorno.Append($" {dr["des_detalhe_motivo_Cancelamento"].ToString()} ");
                    else
                        stringRetorno.Append($" {dr["des_motivo_Cancelamento"].ToString()} ");

                    QuemCancelou = dr["nme_pessoa"].ToString();
                }

            }
            return stringRetorno.ToString();
        }
        #endregion


        #endregion
    }
}