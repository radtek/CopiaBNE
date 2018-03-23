using System;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class Vagas : BaseUserControl
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

        #endregion

        #region Delegates

        public delegate void DelegateExcluir(int idVaga);
        public event DelegateExcluir EventExcluir;

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            gvVagas.PageSize = 6;

            if (!IsPostBack)
                CarregarGrid(String.IsNullOrEmpty(tbxFiltroBusca.Text));
        }
        #endregion

        #region gvVagas_PageIndexChanged
        protected void gvVagas_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid(String.IsNullOrEmpty(tbxFiltroBusca.Text));
        }
        #endregion

        #region gvVagas_ItemCommand
        protected void gvVagas_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("VisualizarVaga"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);
                    Redirect(Vaga.MontarUrlVaga(idVaga));
                }

                if (e.CommandName.Equals("EditarVaga"))
                {
                    base.UrlDestino.Value = "SalaAdministradorVagas.aspx";
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);
                    Session.Add(Chave.Temporaria.Variavel2.ToString(), idVaga);
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                }

                if (e.CommandName.Equals("ExcluirVaga"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);
                    if (EventExcluir != null)
                        EventExcluir(idVaga);
                }

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarGrid(String.IsNullOrEmpty(tbxFiltroBusca.Text));
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalaAdministrador.aspx", false);
        }
        #endregion

        #endregion

        #region Metodos
        #region CarregarGrid
        public void CarregarGrid(bool naoAuditadas = false)
        {
            const int totalRegistros = 8;
            UIHelper.CarregarRadGrid(gvVagas, Vaga.ListarVagasSalaAdministrador(tbxFiltroBusca.Text, naoAuditadas), totalRegistros);
            upGvVagas.Update();
        }
        #endregion

        #region ExcluirVaga
        public void ExcluirVaga(int idVaga)
        {
            Vaga.InativarVaga(idVaga);
            CarregarGrid(String.IsNullOrEmpty(tbxFiltroBusca.Text));
            ExibirMensagem(MensagemAviso._24023, TipoMensagem.Aviso);
        }
        #endregion

        #endregion
    }
}

