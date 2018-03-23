using System;
using System.Web.UI.WebControls;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;
using System.Data;
using BNE.BLL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class EdicaoCV : BaseUserControl
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

        public delegate void DelegateEdicaoCurriculo(int idCurriculo);
        public event DelegateEdicaoCurriculo EventEditarCurriculo;

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucConcessaoVip.Fechar += ucConcessaoVip_Fechar;
            ucExportarCurriculo.Fechar += ucExportarCurriculo_Fechar;
            ucBloquearCandidato.Fechar += ucBloquearCandidato_Fechar;
            ucEditarDadosPessoais.Sucesso += ucEditarDadosPessoais_sucesso;
        }
        #endregion

        #region ucEditarDadosPessoais_sucesso
        void ucEditarDadosPessoais_sucesso()
        {
            ucModalConfirmacao.PreencherCampos("Sucesso", "Sucesso ao editar os dados pessoais", false);
            ucModalConfirmacao.MostrarModal();
            CarregarGrid();
            upGvEdicaoCV.Update();
        }
        #endregion

        #region ucConcessaoVip_Fechar
        void ucConcessaoVip_Fechar(string Mensagem)
        {
            ucConcessaoVip.EsconderModal();
            ucModalConfirmacao.PreencherCampos("Plano VIP", Mensagem, false, "OK");
            ucModalConfirmacao.MostrarModal();
            UIHelper.CarregarRadGrid(gvEdicaoCV, new DataTable(), 0);
            upGvEdicaoCV.Update();
        }
        #endregion

        #region ucExportarCurriculo_Fechar
        void ucExportarCurriculo_Fechar(string Mensagem)
        {
            ucExportarCurriculo.EsconderModal();
            ucModalConfirmacao.PreencherCampos("Confirmação de envio", Mensagem, false, "OK");
            ucModalConfirmacao.MostrarModal();
        }
        #endregion

        #region ucBloquearCandidato_Fechar
        void ucBloquearCandidato_Fechar(string Mensagem)
        {
            ucBloquearCandidato.EsconderModal();
            ucModalConfirmacao.PreencherCampos("Confirmação", Mensagem, false, "OK");
            ucModalConfirmacao.MostrarModal();
            UIHelper.CarregarRadGrid(gvEdicaoCV, new DataTable(), 0);
            upGvEdicaoCV.Update();
        }
        #endregion

        #region gvEdicaoCV_ItemCommand
        protected void gvEdicaoCV_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditarCurriculo"))
            {
                if (EventEditarCurriculo != null)
                {
                    int idCurriculo = Convert.ToInt32(gvEdicaoCV.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

                    EventEditarCurriculo(idCurriculo);
                }
            }
            else if (e.CommandName.Equals("ExportarCurriculo"))
            {
                ucExportarCurriculo.Inicializar(Convert.ToInt32(e.CommandArgument), ((Label)e.Item.FindControl("lblNomeCandidato")).Text);
                ucExportarCurriculo.MostrarModal();
            }
            else if (e.CommandName.Equals("BronquinhaBloquear"))
            {
                ucBloquearCandidato.InicializarBloquear(Convert.ToInt32(e.CommandArgument), null, ((Label)e.Item.FindControl("lblNomeCandidato")).Text);
                ucBloquearCandidato.MostrarModal();
            }
            else if (e.CommandName.Equals("BronquinhaBloqueado"))
            {
                ucBloquearCandidato.InicializarBloqueado(Convert.ToInt32(e.CommandArgument), null, ((Label)e.Item.FindControl("lblNomeCandidato")).Text);
                ucBloquearCandidato.MostrarModal();
            }
            else if (e.CommandName.Equals("EditarDadosPessoais"))
            {
                int idPessoaFisica = Convert.ToInt32(gvEdicaoCV.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pessoa_Fisica"]);
                ucEditarDadosPessoais.IdPessoaFisica = idPessoaFisica;
                ucEditarDadosPessoais.Inicializar();
                ucEditarDadosPessoais.MostrarModal();
            }
            else if (e.CommandName.Equals("QuemMeViu"))
            {
                int idCurriculo = Convert.ToInt32(gvEdicaoCV.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);
                Session.Add(Chave.Temporaria.Variavel2.ToString(), idCurriculo);

                Redirect("SalaAdministradorQuemMeViu.aspx");
            }
        }
        #endregion

        #region gvEdicaoCV_PageIndexChanged
        protected void gvEdicaoCV_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministrador.aspx");
        }
        #endregion

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
            upGvEdicaoCV.Update();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            PageIndex = 1;
            gvEdicaoCV.PageSize = 6;
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvEdicaoCV, Curriculo.CarregarCurriculosPorFiltro(tbxFiltroBusca.Text.Trim(), PageIndex, gvEdicaoCV.PageSize, out totalRegistros), totalRegistros);
        }
        #endregion

        #region RetornarURL
        protected string RetornarURL(string nomeFuncao, string nomeCidade, string siglaEstado, int identificadorCurriculo)
        {
            return SitemapHelper.MontarUrlVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo);
        }
        #endregion

        #endregion

    }
}