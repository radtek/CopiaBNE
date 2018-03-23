//-- Data: 21/06/2011 16:23
//-- Autor: Vinicius Maciel

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class AuditorPublicador // Tabela: BNE_Auditor_Publicador
    {

        private const string SPLISTARPORAUDITOR = @"SELECT  PF.Idf_Pessoa_Fisica ,
                                                            PF.Nme_Pessoa ,
                                                            A.Idf_Publicador
                                                    FROM    BNE.TAB_Pessoa_Fisica PF
                                                            JOIN BNE.TAB_Usuario_Filial_Perfil UFP ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                                                            JOIN BNE.BNE_Auditor_Publicador A ON UFP.Idf_Usuario_Filial_Perfil = A.Idf_Publicador
                                                    WHERE   A.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil AND A.Flg_Inativo = 0";

        #region Metodos

        #region ListarPublicadorPorAuditor

        public static IDataReader ListarPublicadorPorAuditor(int idUsuarioFilialPerfilAuditor)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialPerfilAuditor;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTARPORAUDITOR, parms);
        }

        #endregion

        #endregion
    }
}