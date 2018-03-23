using System;
using System.Web;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Custom;

namespace BNE.Web.Code
{
    public class PageHelper : Page
    {

        #region ValidarUsuarioLogado

        /// <summary>
        /// Valida se a sessão atual é a última sessão válida
        /// </summary>
        /// <param name="idPessoaFisica">Id da pessoa fisica</param>
        /// <param name="idSessao">Identificador da sessao</param>
        public static bool ValidarUsuarioLogado(int idPessoaFisica, string idSessao)
        {
            string ultimaSessao = new PessoaFisica(idPessoaFisica).RecuperarSessaoAtual();
            return ultimaSessao.Equals(idSessao);
        }

        #endregion

        #region VerificarLoginEmpresa

        /// <summary>
        /// Valida se a sessão atual é a última sessão válida
        /// </summary>
        /// <param name="idPessoaFisica">Id da pessoa fisica</param>
        /// <param name="idFilial">Id da Filial</param>
        public static bool VerificarLoginEmpresa(int idPessoaFisica, int idFilial)
        {
            return Usuario.UsuarioPodeLogarFilial(new Filial(idFilial), new PessoaFisica(idPessoaFisica));
        }

        #endregion

        #region ValidarSenhaUsuario

        /// <summary>
        /// Valida se a senha atual é a a senha do usuário
        /// </summary>
        /// <param name="idPessoaFisica">Id da pessoa fisica</param>
        /// <param name="senhaUsuario">Identificador da sessao</param>
        public static bool ValidarSenhaUsuario(int idPessoaFisica, string senhaUsuario)
        {
            Usuario objUsuario;
            if (Usuario.CarregarPorPessoaFisica(idPessoaFisica, out objUsuario))
            {
                return objUsuario.SenhaUsuario.Equals(senhaUsuario);
            }
            return false;
        }

        #endregion

        #region AtualizarSessaoUsuario

        /// <summary>
        /// Atualiza o identificador da sessao do usuario após autenticação
        /// </summary>
        /// <param name="idPessoaFisica">Id da pessoa fisica</param>
        /// <param name="idSessao">Identificador da sessao</param>
        [Obsolete("Utilizar BNEAutenticacao (forms) e BNELoginProcess (sessão)")]
        public static void AtualizarSessaoUsuario(int idPessoaFisica, string idSessao, Filial objFilial = null)
        {
            Usuario objUsuario;
            if (Usuario.CarregarPorPessoaFisica(idPessoaFisica, out objUsuario))
            {
                objUsuario.DescricaoSessionID = idSessao;
                if (objFilial != null)
                    objUsuario.UltimaFilialLogada = objFilial;
            }
            else
            {
                objUsuario = new Usuario
                {
                    PessoaFisica = new PessoaFisica(idPessoaFisica),
                    DescricaoSessionID = idSessao,
                    SenhaUsuario = new Guid().ToString().Substring(0, 8),
                    UltimaFilialLogada =  objFilial
                };
            }
            objUsuario.Save();
        }

        #endregion

        #region AtualizarDataInteracaoUsuario

        /// <summary>
        /// Atualiza o identificador da sessao do usuario após autenticação
        /// </summary>
        /// <param name="idPessoaFisica">Id da pessoa fisica</param>
        public static void AtualizarDataInteracaoUsuario(int idPessoaFisica)
        {
            var objPessoaFisica = new PessoaFisica(idPessoaFisica);
            objPessoaFisica.AtualizarDataInteracaoUsuario();
        }

        #endregion

        #region RecuperarIP

        public static string RecuperarIP()
        {
            var httpContext = HttpContext.Current;

            string ip = string.Empty;

            if (httpContext != null)
            {
                ip = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip))
                    ip = httpContext.Request.ServerVariables["Remote_Host"];
            }

            return ip;
        }

        #endregion

    }
}