//-- Data: 24/08/2017 12:28
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class SistemaFolhaPgtoPesquisa // Tabela: BNE_Sistema_Folha_Pgto_Pesquisa
	{

        #region [Consultas]

        #region [spRespondeu]
        private const string spRespondeu = @"select flg_resposta from BNE.BNE_Sistema_Folha_Pgto_Pesquisa pp with(nolock)
                                    join bne.tab_Usuario_Filial_perfil ufp with(nolock) on pp.Idf_Usuario_Filial_Perfil = ufp.idf_usuario_filial_perfil
                                    where ufp.Idf_Filial = @Idf_Filial ";

        #endregion

        #endregion


        #region [Respondeu]
        public static bool JaRespondeu(int idFilial)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter {ParameterName="@Idf_Filial", SqlDbType = SqlDbType.Int , Value = idFilial }
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spRespondeu, parametros))
            {
                if (dr.Read())
                    return true;
            }

            return false;
        }
        #endregion
    }
}