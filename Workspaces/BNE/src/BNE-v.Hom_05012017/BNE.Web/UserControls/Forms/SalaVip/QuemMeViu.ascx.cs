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

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());

                return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

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

        #region gvQuemMeViu_PageIndexChanged
        protected void gvQuemMeViu_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region gvQuemMeViu_ItemCommand
        protected void gvQuemMeViu_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("VerDadosEmpresa"))
            {
                int idFilial = Convert.ToInt32(gvQuemMeViu.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);

                if (EventVerDadosEmpresa != null)
                    EventVerDadosEmpresa(idFilial, null);
            }
            else if (e.CommandName.Equals("VerVagasEmpresa"))
            {
                int idFilial = Convert.ToInt32(gvQuemMeViu.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                var nomeEmpresa = ((Label)e.Item.FindControl("lblEmpresa")).Text;

                if (EventVerVagasEmpresa != null)
                    EventVerVagasEmpresa(idFilial, nomeEmpresa);
            }
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
            const int quantidadeitens = 50;
            PageIndex = 1;
            gvQuemMeViu.PageSize = quantidadeitens;
            CarregarGrid();

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.ToString();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvQuemMeViu, CurriculoQuemMeViu.RecuperarQuemMeViu(base.IdCurriculo.Value, PageIndex, gvQuemMeViu.PageSize, out totalRegistros), totalRegistros);
            upGvQuemMeViu.Update();
        }
        #endregion

        #endregion

    }
}