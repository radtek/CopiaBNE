using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipNaoSeCandidatouVaga : BasePage
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

        #region btnVerVagas_Click
        protected void btnVerVagas_Click(object sender, EventArgs e)
        {
            var objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);

            var objPesquisaVaga = BLL.PesquisaVaga.RecuperarDadosPesquisaVagaCandidato(objPessoaFisica, new Curriculo(base.IdCurriculo.Value), new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value), Common.Helper.RecuperarIP());
            var url = string.Concat("http://", Helper.RecuperarURLVagas(), "/resultado-pesquisa-avancada-de-vagas/", objPesquisaVaga.IdPesquisaVaga);
            Redirect(url);
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UrlOrigem))
                Redirect(UrlOrigem);
            else
                Redirect("Default.aspx");
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            AjustarTituloTela("Já Enviei");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "JaEnviei");
            PropriedadeAjustarTopoUsuarioCandidato(true);
        }
        #endregion

        #endregion
    }
}