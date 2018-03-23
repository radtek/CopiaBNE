using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employer.Componentes.UI.Web.Util;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Web.UI;
using System.IO;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Este componente verifica se existe um arquivo de help para a url corrente. Caso tenha disponibilisa o conteúdo em uma modal.
    /// </summary>
    #pragma warning disable 1591
    public class HelpImageButton : ControlModal
    {
        private ImageButton _btnHelp = new ImageButton { ID = "btnHelp" };        
        private Literal _liHtml = new Literal { ID = "liHtml", EnableViewState = false };        

        #region Properties

        #region ImageUrl
        /// <summary>
        /// Url da imagem do botão de help na modal
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return _btnHelp.ImageUrl;
            }
            set
            {
                _btnHelp.ImageUrl = value;
            }
        }
        #endregion

        #region HelpKey
        public String HelpKey
        {
            get
            {
                return Convert.ToString(this.ViewState["HelpKey"]);
            }
            set
            {
                this.ViewState["HelpKey"] = value;
            }
        }
        #endregion

        #region HelpKey
        public String HelpPath
        {
            get
            {
                return Convert.ToString(this.ViewState["HelpPath"]);
            }
            set
            {
                this.ViewState["HelpPath"] = value;
            }
        }
        #endregion

        #endregion

        #region Métodos
        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            this.Controls.Add(_btnHelp);

            base.CreateChildControls();
        }

        /// <inheritdoc/>
        protected override void OnInit(EventArgs e)
        {
            _btnHelp.Click += new ImageClickEventHandler(_btnHelp_Click);

            base.OnInit(e);
        }

        void _btnHelp_Click(object sender, ImageClickEventArgs e)
        {
            this.Show();

            HelpManager objManager = new HelpManager(this.HelpPath);
            var path = Page.Server.MapPath(String.IsNullOrEmpty(this.HelpKey) ? objManager.GetHelp() : objManager.GetHelp(HelpKey));

            if (File.Exists(path))
                _liHtml.Text = File.ReadAllText(path);
            else
                _liHtml.Text = "Tela sem ajuda";
        }

        #region OnPreRender
        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            if (String.IsNullOrEmpty(ImageUrl))
                ImageUrl = Page.ClientScript.GetWebResourceUrl(
                    this.GetType(), "Employer.Componentes.UI.Web.Content.Images.icone_help.png");                                  

            base.OnPreRender(e);
        }
        #endregion

        #endregion

        /// <inheritdoc/>
        protected override void CreateModalContent(System.Web.UI.WebControls.Panel objPanel)
        {
            objPanel.Controls.Add(_liHtml);
        }
    }
}
