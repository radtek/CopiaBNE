using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System.Data;
using Telerik.Web.UI;
using BNE.BLL;
using BNE.EL;
using Resources;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class Noticias : BaseUserControl
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
                else
                    return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region IdNoticia
        public int? IdNoticia
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel1.ToString()]);
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

        #region EmEdicao
        public bool EmEdicao
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel2.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            try
            {
                Noticia objNoticia = Noticia.LoadObject(IdNoticia.Value);
                objNoticia.FlagInativo = true;
                objNoticia.Save();
                CarregarGrid();
                upPnlNoticias.Update();

                base.ExibirMensagemConfirmacao("Confirmação de Exclusão", "Notícia excluída com sucesso", false);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvNoticias_PageIndexChanged
        protected void gvNoticias_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region gvNoticias_ItemCommand
        protected void gvNoticias_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                int idNoticia = Convert.ToInt32(gvNoticias.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Noticia"]);
                MostrarPainelEdicao(idNoticia);
                EmEdicao = true;
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                int idNoticia = Convert.ToInt32(gvNoticias.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Noticia"]);
                IdNoticia = idNoticia;

                ucConfirmacaoExclusao.Inicializar("Confirmação de Exclusão", "Tem certeza que deseja excluir esta notícia?");
                ucConfirmacaoExclusao.MostrarModal();
            }
        }
        #endregion

        #region btnNovaNoticia_Click
        protected void btnNovaNoticia_Click(object sender, EventArgs e)
        {
            EmEdicao = true;
            MostrarPainelEdicao(null);
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            PageIndex = 1;
            gvNoticias.PageSize = 6;
            CarregarGrid();
            EmEdicao = false;

            pnlNoticias.Visible = true;
            pnlNoticiasEditar.Visible = false;

            upPnlNoticias.Update();
            upPnlNoticiasEditar.Update();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros = 0;
            DateTime? dtaPublicacao = null;
            String titulo = String.Empty;

            if (!String.IsNullOrEmpty(tbxFiltroBusca.Text))
            {
                DateTime dateParsed;
                if (DateTime.TryParse(tbxFiltroBusca.Text, out dateParsed))
                    dtaPublicacao = dateParsed;
                else
                    titulo = tbxFiltroBusca.Text;
            }

            UIHelper.CarregarRadGrid(gvNoticias, Noticia.ListarNoticiasPorDataTitulo(dtaPublicacao, titulo, false, PageIndex, gvNoticias.PageSize, out totalRegistros), totalRegistros);
        }
        #endregion

        #region MostrarPainelEdicao
        private void MostrarPainelEdicao(int? idNoticia)
        {
            ucNoticiasEditar.Inicializar(idNoticia);
            pnlNoticiasEditar.Visible = true;
            pnlNoticias.Visible = false;

            upPnlNoticias.Update();
            upPnlNoticiasEditar.Update();
        }
        #endregion

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        #endregion

    }
}