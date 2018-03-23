using System;
using System.Net.Mail;


namespace SistMars
{
	/// <summary>
	/// Summary description for TesteEmail.
	/// </summary>
	public class TesteEmail : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			enviarEmail();
		}

		public void enviarEmail()
		{

            MailAddress ojbFrom = new MailAddress("\"SistMars\" <joseluiz@bne.com.br>");
            MailAddress objTo = new MailAddress("luigialves@bne.com.br");
            MailMessage objEmail = null;
            
            try 
            {
                objEmail = new MailMessage(ojbFrom,objTo);
			
			
			objEmail.Subject = "Teste";
			objEmail.Body = "Teste";
            objEmail.IsBodyHtml = true;
			objEmail.Priority = MailPriority.High;
			
                SmtpClient objSmtp = new SmtpClient("200.193.137.46");
                objSmtp.Send(objEmail);
			}
			catch
			{
				Response.Write("erro");
			}
			finally
			{
                if (objEmail != null)
                    objEmail.Dispose();
			}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
