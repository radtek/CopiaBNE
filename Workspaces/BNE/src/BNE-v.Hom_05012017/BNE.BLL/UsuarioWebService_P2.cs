//-- Data: 22/03/2016 10:49
//-- Autor: Francisco Ribas

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class UsuarioWebService // Tabela: BNE_Usuario_WebService
	{
        public const string SP_PESQUISA_USUARIO = "SELECT COUNT(*) FROM BNE.BNE_Usuario_WebService WHERE Des_Usuario = @Des_Usuario AND Sen_Usuario = @Sen_Usuario;";

        public static bool VerificarUsuario(String usuario, String senha)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Des_Usuario", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter("@Sen_Usuario", SqlDbType.VarChar, 20));
            parms[0].Value = usuario; parms[1].Value = senha;

            return (bool)DataAccessLayer.ExecuteScalar(CommandType.Text, SP_PESQUISA_USUARIO, parms);
        }
	}
}