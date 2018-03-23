using System;
using System.Configuration;

namespace SistMars.manut
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtUsuario;
		protected System.Web.UI.WebControls.TextBox txtSenha;
		protected System.Web.UI.WebControls.Button cmdOK;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			Session["loginManut"] = null; //logoff
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
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			if(txtUsuario.Text != ConfigurationManager.AppSettings["userManut"].ToString()
                || new util().EncriptarSenha(txtSenha.Text) != ConfigurationManager.AppSettings["senhaManut"].ToString())
			{
				Response.Write(erro.AbrePopupErro("Err1", "Erros", true));
				return;
			}
			else
			{
				Session.Add("loginManut", "1");
				Response.Redirect("manut.aspx");
			}
		}
	}
}
