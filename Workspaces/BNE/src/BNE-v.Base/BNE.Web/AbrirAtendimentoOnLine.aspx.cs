using System;
using System.Globalization;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Security;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class AbrirAtendimentoOnLine : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["TIPO"] != null)
                RedirecionarAtendimento(Convert.ToInt32(Request.QueryString["TIPO"]));
            else
                Redirect("~/Default.aspx");
        }

        private void RedirecionarAtendimento(int idTipoAtendimento)
        {
            string url;

            if (idTipoAtendimento.Equals(3)) //Não logado
            {
                wsAutenticacaoSOSRH.wsAutenticacao aut = null;
                try
                {
                    aut = new wsAutenticacaoSOSRH.wsAutenticacao();

                    ServiceAuth.GerarHashAcessoWS(aut);

                    if (aut.RequisitarUrlCodigoSistema((int)Sistema.Nome.BNE, out url))
                        Redirect(url);
                }
                catch (Exception ex)
                {
                    if (!(ex is ThreadAbortException))
                        EL.GerenciadorException.GravarExcecao(ex, "Falha ao abrir atendimento");
                }
                finally
                {
                    if (aut != null)
                        aut.Dispose();
                }
            }
            else
            {
                //Setando valor padrão 
                string nomeEmpresa = String.Empty;

                //Recuperando e-mail
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                
                string email = String.IsNullOrEmpty(objPessoaFisica.EmailPessoa) ? String.Empty : objPessoaFisica.EmailPessoa;

                //Verificando se o atendimento é para empresa.
                if (idTipoAtendimento.Equals(2))
                {
                    Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
                    nomeEmpresa = objFilial.RazaoSocial;
                }

                wsAutenticacaoSOSRH.wsAutenticacao aut = null;
                try
                {
                    aut = new wsAutenticacaoSOSRH.wsAutenticacao();

                    ServiceAuth.GerarHashAcessoWS(aut);

                    if (aut.RequisitarUrl((int)Sistema.Nome.BNE, email, objPessoaFisica.NomePessoa, objPessoaFisica.CPF.ToString(CultureInfo.CurrentCulture), nomeEmpresa, out url))
                        Redirect(url);
                }
                catch (Exception ex)
                {
                    if (!(ex is ThreadAbortException))
                        EL.GerenciadorException.GravarExcecao(ex, "Falha ao abrir atendimento");
                }
                finally
                {
                    if (aut != null)
                        aut.Dispose();
                }

            }
        }
    }
}
