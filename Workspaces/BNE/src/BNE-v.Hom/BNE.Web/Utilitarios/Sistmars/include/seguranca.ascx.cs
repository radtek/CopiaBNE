namespace SistMars.include
{
	using System;

    /// <summary>
	///		Summary description for seguranca.
	/// </summary>
	public abstract class seguranca : System.Web.UI.UserControl
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(Session["loginManut"] == null)
				Response.Redirect("../manut/default.aspx");
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
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
