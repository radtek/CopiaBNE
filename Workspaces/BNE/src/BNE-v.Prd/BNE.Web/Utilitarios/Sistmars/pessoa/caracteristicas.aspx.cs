using System;

namespace SistMars.pessoa
{
	/// <summary>
	/// Summary description for caracteristicas.
	/// </summary>
	public class frmCaracteristicas : System.Web.UI.Page
    {
        #region Eventos

        #region Controle
        protected System.Web.UI.WebControls.ImageButton imgContinuar;
        #endregion

        #region PageLoad

        private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
        }

        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
        }

        #endregion

        #endregion

        #region Metodos

        #region InitializeComponent
        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.ID = "frmCaracteristicas";
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

        #endregion
    }
}
