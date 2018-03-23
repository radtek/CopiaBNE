using System;

namespace SistMars.pessoa
{
	/// <summary>
	/// Summary description for quemevc.
	/// </summary>
	public class frmQuemevc : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.ImageButton imgContinuar;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.imgContinuar.Click += new System.Web.UI.ImageClickEventHandler(this.imgContinuar_Click);
			this.ID = "frmQuemevc";
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void imgContinuar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect("dadospessoais.aspx");
		}
	}
}
