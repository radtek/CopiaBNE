using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;

namespace BNE.BLL.Custom
{
    public class ActiveDirectory
    {
        public static bool VerificarUsuario(string usuario, string senha)
        {
            bool resultado = false;
            try
            {
                //string domain = "employer.com.br";
                using (var pc = new PrincipalContext(ContextType.Domain, "Employer", "dc=emloyer,dc=com,dc=br"))
                {
                    resultado = pc.ValidateCredentials(usuario, senha);
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                resultado = false;
            }
            return resultado;
        }
    }
}
