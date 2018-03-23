using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;

namespace BNE.BLL.Security
{
    public class PasswordValidator
    {
        public const string SP_PESQUISA_USUARIO = "SELECT COUNT(*) FROM BNE.BNE_Usuario_WebService WHERE Des_Usuario = @Des_Usuario AND Sen_Usuario = @Sen_Usuario;";

        public static bool VerificarUsuario(String usuario, String senha)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Des_Usuario", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter("@Sen_Usuario", SqlDbType.VarChar, 20));
            parms[0].Value = usuario; parms[1].Value = senha;

            return Convert.ToBoolean(DataAccessLayer.ExecuteScalar(new ConfigAccessLayerBNE(), CommandType.Text, SP_PESQUISA_USUARIO, parms));
        }

        public bool Validate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                throw new SecurityTokenException("Username and password required");

            return VerificarUsuario(userName, password);
        }
    }
}
