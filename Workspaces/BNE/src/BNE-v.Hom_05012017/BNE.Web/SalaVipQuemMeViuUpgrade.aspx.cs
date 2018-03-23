using System;
using BNE.BLL;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using CartaEmail = BNE.BLL.CartaEmail;
using Parametro = BNE.BLL.Parametro;
using TipoMensagem = BNE.Web.Code.Enumeradores.TipoMensagem;

namespace BNE.Web
{
    public partial class SalaVipQuemMeViuUpgrade : BasePage
    {
        #region Propriedades

        #region UrlOrigem - Variável 1
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel1.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #region lbAtualizeAquiPretensaoSalarial_Click
        protected void lbAtualizeAquiPretensaoSalarial_Click(object sender, EventArgs e)
        {
            RedirectMiniCurriculo();
        }
        #endregion

        #region lbAtualizeAquiFuncoes_Click
        protected void lbAtualizeAquiFuncoes_Click(object sender, EventArgs e)
        {
            RedirectMiniCurriculo();
        }
        #endregion

        #region lbAtualizaAquiOutraCidade_Click
        protected void lbAtualizaAquiOutraCidade_Click(object sender, EventArgs e)
        {
            if (IdPessoaFisicaLogada.HasValue)
            {
                Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisicaLogada.Value);
                Redirect(GetRouteUrl(RouteCollection.CadastroCurriculoComplementar.ToString(), null));
            }
            else
            {
                Session.Add(Chave.Temporaria.Variavel2.ToString(), GetRouteUrl(RouteCollection.QuemMeViuVip.ToString(), null));
                Redirect(GetRouteUrl(RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region btnAtualizarCurriculo_Click
        protected void btnAtualizarCurriculo_Click(object sender, EventArgs e)
        {
            RedirectMiniCurriculo();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect(!string.IsNullOrEmpty(UrlOrigem) ? UrlOrigem : "Default.aspx");
        }
        #endregion

        #region btnPecaAjuda_Click
        protected void btnPecaAjuda_Click(object sender, EventArgs e)
        {
            try
            {
                var objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisicaLogada.Value);

                if (objPessoaFisica.EmailPessoa != null)
                {
                    SalvarAjuda(objPessoaFisica);
                    ucModalConfirmacao.PreencherCampos("Confirmação de Envio", "Seus dados foram enviados para a nova auditoria de currículo, em breve entraremos em contato", false);
                    ucModalConfirmacao.MostrarModal();
                    upQuemMeViuUpgrade.Update();
                }
                else
                    ExibirMensagem("Por Favor cadastre o seu email", TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarTituloTela("Quem me Viu?");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "QuemMeViuVirgem");

            PropriedadeAjustarTopoUsuarioCandidato(true);

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;
        }
        #endregion

        #region RedirectMiniCurriculo
        public void RedirectMiniCurriculo()
        {
            if (IdPessoaFisicaLogada.HasValue)
            {
                Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisicaLogada.Value);
                Redirect(GetRouteUrl(RouteCollection.CadastroCurriculoMini.ToString(), null));
            }
            else
            {
                Session.Add(Chave.Temporaria.Variavel2.ToString(), GetRouteUrl(RouteCollection.QuemMeViuVip.ToString(), null));
                Redirect(GetRouteUrl(RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region SalvarAjuda
        public void SalvarAjuda(PessoaFisica objPessoaFisica)
        {
            var objUsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value);

            string assunto;
            var templateMensagem = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.PecaAjuda, out assunto);
            var parametros = new
            {
                NomeCandidato = objPessoaFisica.NomePessoa, objPessoaFisica.NumeroCPF,
                DataNascimento = objPessoaFisica.DataNascimento.ToShortDateString()
            };
            var mensagem = parametros.ToString(templateMensagem);

            EmailSenderFactory
                .Create(TipoEnviadorEmail.Fila)
                .Enviar(new Curriculo(IdCurriculo.Value), objUsuarioFilialPerfil, null, assunto, mensagem, BLL.Enumeradores.CartaEmail.PecaAjuda, objPessoaFisica.EmailPessoa, Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailPecaAjuda));
        }
        #endregion

        #endregion
    }
}