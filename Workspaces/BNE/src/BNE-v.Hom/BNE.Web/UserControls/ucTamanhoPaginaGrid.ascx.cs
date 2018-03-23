using System;

namespace BNE.Web.UserControls
{
    public partial class ucTamanhoPaginaGrid : System.Web.UI.UserControl
    {
        #region Delegate
        /// <summary>
        /// Evento disparado quando é alterado um item no dropdown de 
        /// tamanho de páginas.
        /// </summary>
        public event EventHandler MudarTamanhoPagina;
        #endregion

        #region Propriedades
        #region TamanhoPagina
        /// <summary>
        /// Propriedade que retorna o tamanho de páginas selecionado no dropdown.
        /// </summary>
        public int TamanhoPagina
        {
            get
            {
                return Convert.ToInt32(ddlTamanhoPagina.SelectedValue);
            }
        }
        #endregion

        #region HabilitarDropDownList
        /// <summary>
        /// Configura a propriedade Enabled do componente DropDownList.
        /// </summary>
        public bool HabilitarDropDownList
        {
            get
            {
                return ddlTamanhoPagina.Enabled;
            }
            set
            {
                ddlTamanhoPagina.Enabled = value;
            }
        }
        #endregion

        #region EsconderSelecao
        /// <summary>
        /// Configura a propriedade Visible do componente.
        /// </summary>
        public bool EsconderSelecao
        {
            get
            {
                return !ddlTamanhoPagina.Visible;
            }
            set
            {
                lblTextoParte1.Visible = !value;
                ddlTamanhoPagina.Visible = !value;
                lblTextoParte2.Visible = !value;
            }
        }
        #endregion
        #endregion

        #region Eventos
        #region ddlTamanhoPagina
        /// <summary>
        /// Eveneto disparado quando é alterado o tamanho da página no dropdown.
        /// </summary>
        /// <param name="sender">Componente que gerou o evento.</param>
        /// <param name="e">Parâmetros para o evento.</param>
        protected void ddlTamanhoPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MudarTamanhoPagina != null)
            {
                this.MudarTamanhoPagina(sender, e);
            }
        }
        #endregion
        #endregion
    }
}
