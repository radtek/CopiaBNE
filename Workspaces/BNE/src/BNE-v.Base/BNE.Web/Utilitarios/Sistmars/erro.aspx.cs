using System;
using System.Configuration;
using System.Xml;

namespace SistMars
{
	/// <summary>
	/// Summary description for modelo.
	/// </summary>
	public class erro : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtErro;
		protected System.Web.UI.HtmlControls.HtmlInputButton cmdFechar;
		protected System.Web.UI.WebControls.Button cmdVoltar;
		protected System.Web.UI.HtmlControls.HtmlForm frmErro;

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
			this.cmdVoltar.Click += new System.EventHandler(this.cmdVoltar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			CarregarErros();
			if (!string.IsNullOrEmpty(Request.Params["pagina"]))
			{
				if(string.IsNullOrEmpty(Request.Params["pagina"]))
				{
					cmdFechar.Visible = true;
					cmdVoltar.Visible = false;
				}
				else
				{
					ViewState["strRetorno"] = Request.Params["pagina"];
					cmdFechar.Visible = false;
					cmdVoltar.Visible = true;
            
				}
			}
			else
			{
//				ViewState["strRetorno"] = Request.ServerVariables["HTTP_REFERER"];
				cmdFechar.Visible = true;
				cmdVoltar.Visible = false;
			}
		}

		public static string AbrePopupErro(string msgErro, string tipoErro, bool subDiretorio)
		{
			string mensagemErro;
			msgErro = msgErro.Replace("'", "");
			msgErro = msgErro.Replace("\r", " ");
			msgErro = msgErro.Replace("\n", " ");

			if(subDiretorio)
				mensagemErro = "../erro.aspx?tipo=" + tipoErro + "&erro=" + msgErro.Substring(0, msgErro.Length < 175 ? msgErro.Length : 175);
			else
				mensagemErro = "erro.aspx?tipo=" + tipoErro + "&erro=" + msgErro.Substring(0, msgErro.Length < 175 ? msgErro.Length : 175);
			//
			//mensagemErro = "<script>javascript:window.open('" + mensagemErro + "', 'errevent','height=250,width=450,scrollbars=yes');</script>";
			mensagemErro = "<script>javascript:window.showModalDialog('" + mensagemErro + "', 'errevent','dialogHeight: 270px; dialogwidth: 450px; scroll: no; help: no; status: no');</script>";
			return mensagemErro;
		}

		public static string AbrePopupErro(string msgErro, string tipoErro, bool subDiretorio, int Eventos)
		{
			string mensagemErro;
			msgErro = msgErro.Replace("'", "");
			msgErro = msgErro.Replace("\r", " ");
			msgErro = msgErro.Replace("\n", " ");

			if(subDiretorio)
				mensagemErro = "../erro.aspx?tipo=" + tipoErro + "&erro=" + msgErro.Substring(0, msgErro.Length < 175 ? msgErro.Length : 175);
			else
				mensagemErro = "erro.aspx?tipo=" + tipoErro + "&erro=" + msgErro.Substring(0, msgErro.Length < 175 ? msgErro.Length : 175);
			//
			mensagemErro = "<script>javascript:window.showModalDialog('" + mensagemErro + "', 'eventos" + Eventos.ToString() + "','dialogHeight: 270px; dialogwidth: 450px; scroll: no; help: no; status: no');</script>";
			return mensagemErro;
		}


		public static string AbrePopupErro(string msgErro, string tipoErro, bool subDiretorio, string optionalerro, string pagina)
		{
			string mensagemErro;
			msgErro = msgErro.Replace("'", "");
		
			string optErro;
			if(optionalerro != null)
			{
				optErro = optionalerro.Replace("'", "");
				optErro = optErro.Replace("\n", "!ENTER!");
			}
			else
				optErro = "";

			if(subDiretorio)
				mensagemErro = "../erro.aspx?tipo=" + tipoErro + "&erro=" + msgErro.Substring(0, msgErro.Length < 175 ? msgErro.Length : 175) + "&opterro=" + optErro.Substring(0, optErro.Length < 175 ? optErro.Length : 175) + "&pagina=" + pagina;
			else
				mensagemErro = "erro.aspx?tipo=" + tipoErro + "&erro=" + msgErro.Substring(0, msgErro.Length < 175 ? msgErro.Length : 175) + "&opterro=" + optErro.Substring(0, optErro.Length < 175 ? optErro.Length : 175) + "&pagina=" + pagina;
			
			mensagemErro = "<script>javascript:window.showModalDialog('" + mensagemErro + "', 'errevent','dialogHeight: 270px; dialogwidth: 450px; scroll: no; help: no; status: no');</script>";
			return mensagemErro;
		}

		private void CarregarErros()
		{
			try
			{
				if (!string.IsNullOrEmpty(Request.Params["tipo"]))
				{
					
					XmlDocument doc = new XmlDocument();
					//doc.Load(MapPath("../Includes/Errors.xml"));
					doc.Load(MapPath("Errors.xml"));

					System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(",");
					string sNode = Request.Params["tipo"];
					string Erro = Request.Params["erro"];
					string[] Erros = r.Split(Erro);
					XmlNode docNode = doc.DocumentElement.SelectSingleNode(sNode);
					foreach(string s in Erros)
					{
						//txtErro.Text += docNode.SelectSingleNode("Err" + s).InnerText + "\r";
						txtErro.Text += docNode.SelectSingleNode(s).InnerText + "\r";
					}
					try
					{
						string sOptionalErro = Request.Params["opterro"];
						txtErro.Text += sOptionalErro + "\r";
					}
					catch
					{
					}
				}
				else
					txtErro.Text = Request.Params["erro"];

			}
			catch(Exception e)
			{
				txtErro.Text = e.ToString();
			}
		}

//
//		private void cmdFechar_Click(object sender, System.EventArgs e)
//		{
//			//Response.Redirect ((string) ViewState["strRetorno"]);
//			Response.Write("<script>window.close()</script>");		
//		}

		private void cmdVoltar_Click(object sender, System.EventArgs e)
		{
			//Response.Redirect((string) ViewState["strRetorno"]);

			string url = ConfigurationManager.AppSettings["UrlPrincipal"].ToString();
			url += "/" + (string) ViewState["strRetorno"];
			Response.Redirect(url);
		}
	}
}
