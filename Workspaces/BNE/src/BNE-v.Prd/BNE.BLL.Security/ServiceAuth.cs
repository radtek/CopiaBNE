using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Web.Services.Protocols;
using System.Reflection;

namespace BNE.BLL.Security
{
    public class ServiceAuth
    {

        public static void AcessoAutorizado(System.Web.Services.WebService ws)
        {
            PropertyInfo objProp = ws.GetType().GetProperties().FirstOrDefault(p => p.Name == "CustomSoapHeader");
            Type tpClass = objProp.PropertyType;

            object objServiceAuthHeader = objProp.GetValue(ws, null);

            PropertyInfo objPropCodHash = tpClass.GetProperties().FirstOrDefault(p => p.Name == "CodHash");
            if (objPropCodHash != null)
            {
                var codHashProp = objPropCodHash.GetValue(objServiceAuthHeader, null);
                if (codHashProp != null && !String.IsNullOrEmpty(codHashProp.ToString()))
                {
                    if (AcessoAutorizado(codHashProp.ToString()) == false)
                        throw new System.Security.SecurityException();
                    else
                        return;
                }
            }

            PropertyInfo objPropUsuario = tpClass.GetProperties().FirstOrDefault(p => p.Name.ToLower() == "usuario");
            PropertyInfo objPropSenha = tpClass.GetProperties().FirstOrDefault(p => p.Name.ToLower() == "senha");

            if (objPropUsuario != null && objPropSenha != null)
            {
                var usuarioProp = objPropUsuario.GetValue(objServiceAuthHeader, null);
                var senhaProp = objPropSenha.GetValue(objServiceAuthHeader, null);

                if (usuarioProp != null && !String.IsNullOrEmpty(usuarioProp.ToString()) &&
                    senhaProp != null && !String.IsNullOrEmpty(senhaProp.ToString()))
                {

                    if (AcessoAutorizado(usuarioProp.ToString(), senhaProp.ToString()) == false)
                        throw new System.Security.SecurityException("Usuário e senha inválidos");
                    else
                        return;
                }
            }

            throw new System.Security.SecurityException("Falha na autenticacao: CodHash ou Usuário/Senha não informados.");
        }

        #region AcessoAutorizado
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="codigoHash"></param>
        /// <returns></returns>
        public static bool AcessoAutorizado(string usuario, string senha)
        {
            try
            {
                return BLL.Security.PasswordValidator.VerificarUsuario(usuario, senha);
            }
            catch
            {
                throw new Exception();
            }
        }
        #endregion

        #region AcessoAutorizado
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigoHash"></param>
        /// <returns></returns>
        private static bool AcessoAutorizado(string codigoHash)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Cod_Hash", SqlDbType.VarChar, 50));
                parms[0].Value = codigoHash;

                int val = (int)DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "login.LOG_SP_Validar_Acesso_Webservice", parms);

                return val > 0;
            }
            catch
            {
                throw new Exception();
            }
        }
        #endregion

        #region GerarHashAcessoWS


        public static void GerarHashAcessoWS(System.Web.Services.Protocols.SoapHttpClientProtocol ws)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            //parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            //parms[0].Value = Num_CPF;

            string hs = DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "login.LOG_SP_Criar_Acesso_Webservice", parms).ToString();
            PropertyInfo objProp = ws.GetType().GetProperties().First(p => p.Name == "ServiceAuthHeaderValue");
            Type tpClass = objProp.PropertyType;
            PropertyInfo objPropCodHash = tpClass.GetProperties().First(p => p.Name == "CodHash");
            object objServiceAuthHeader = Activator.CreateInstance(tpClass);

            objPropCodHash.SetValue(objServiceAuthHeader, hs, null);
            objProp.SetValue(ws, objServiceAuthHeader, null);
        }

        #endregion


    }
}



