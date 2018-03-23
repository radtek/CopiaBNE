using System;

namespace SistMars.manut
{
	/// <summary>
	/// Summary description for encripta.
	/// </summary>
	public class encripta : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Button cmdEncriptar;
		protected System.Web.UI.WebControls.TextBox TextBox2;
	
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
			this.cmdEncriptar.Click += new System.EventHandler(this.cmdEncriptar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void cmdEncriptar_Click(object sender, System.EventArgs e)
		{
			TextBox2.Text = new util().EncriptarSenha(TextBox1.Text);
		}
	}
}
