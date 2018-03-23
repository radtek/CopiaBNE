using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;
using BNE.BLL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
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

        #region IdCurriculo - Variavel 2
        /// <summary>
        /// Propriedade que armazena e recupera o Id do Currículo
        /// </summary>
        public int IdCurriculo
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel2.ToString()] = value;
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

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region gvQuemMeViu_PageIndexChanged
        protected void gvQuemMeViu_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
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
            UIHelper.CarregarRadGrid(gvQuemMeViu, CurriculoQuemMeViu.RecuperarQuemMeViuAdministrador(IdCurriculo, PageIndex, gvQuemMeViu.PageSize, out totalRegistros), totalRegistros);
            upGvQuemMeViu.Update();
        }
        #endregion

        #endregion

    }
}