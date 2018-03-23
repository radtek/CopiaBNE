using System;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaVip
{
    public partial class QuemMeViu : BaseUserControl
    {

        #region Propriedades


        #region UrlOrigem - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
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

        #region Delegates

        public delegate void DelegateVerEmpresa(int idFilial, string nomeEmpresa);
        public event DelegateVerEmpresa EventVerDadosEmpresa;
        public event DelegateVerEmpresa EventVerVagasEmpresa;

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region VerificarTelaMagica
        private void VerificarTelaMagica()
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(base.IdPessoaFisicaLogada.Value, out objCurriculo) && !objCurriculo.VIP())
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.QuemMeViuSemPlano.ToString(), null));
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect(!string.IsNullOrEmpty(UrlOrigem) ? UrlOrigem : "Default.aspx");
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            // verifica se o usuario é vip, se nao for mostra tela magica
            if (base.IdPessoaFisicaLogada.HasValue)
                VerificarTelaMagica();

            //Carregando a quantidade de itens a ser mostrado em tela
            hdfIdc.Value = base.IdCurriculo.Value.ToString();
            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.ToString();
        }
        #endregion

        #endregion

    }
}