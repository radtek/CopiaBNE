using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Sistmars.BLL;

namespace SistMars.pessoa
{
	/// <summary>
	/// Summary description for resultadoImpressao.
	/// </summary>
	public class frmResultadoImpressao : System.Web.UI.Page
    {

        #region Controles
        protected System.Web.UI.WebControls.Label lblNome;
		protected System.Web.UI.WebControls.Label lblDataNascimento;
		protected System.Web.UI.WebControls.Label lblPersonalidade;
		protected System.Web.UI.WebControls.Label lblCaracteristica;
		protected System.Web.UI.WebControls.Label lblEstagioAtual;
		protected System.Web.UI.WebControls.Label lblPessoal;
		protected System.Web.UI.HtmlControls.HtmlTableRow trCaracteristica;
        protected System.Web.UI.WebControls.Image imgTeste;
        #endregion

        #region Eventos

        #region Page_Load

        private void Page_Load(object sender, System.EventArgs e)
		{
            imgTeste.ImageUrl = "http://www.bne.com.br/Utilitarios/sistmars/images/logosistopo.gif";
            CarregarDadosAvaliacao("p_ListarAvaliacaoCodigo ");// , Request.Cookies["ANA_CodigoAnalise"].Value);
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ID = "frmResultadoImpressao";
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #endregion

        #region Metodos

        #region CarregarDadosAvaliacao
        /// <summary>
		/// carrega os resultados da avaliação
		/// </summary>
		/// <param name="proc">Procedure a ser executada</param>
		private void CarregarDadosAvaliacao(string proc)
		{
			

			DataTable dt;
			try
			{
                List<SqlParameter>parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@ANA_CodigoAnalise",SqlDbType.Int,4));
                parms[0].Value = Request.Cookies["ANA_CodigoAnalise"].Value;



                dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure,proc,parms).Tables[0];
				
				lblNome.Text = dt.Rows[0]["PES_Nome"].ToString();
				lblDataNascimento.Text = dt.Rows[0]["PES_DataNascimento"].ToString();

				lblPersonalidade.Text = dt.Rows[0]["TEX_Personalidade"].ToString().Replace("$nome", "<B>" + dt.Rows[0]["PES_Nome"].ToString() + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;");
				lblEstagioAtual.Text = dt.Rows[0]["PON_Texto"].ToString().Replace("$nome", "<B>" + dt.Rows[0]["PES_Nome"].ToString() + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;");
				lblPessoal.Text = dt.Rows[0]["TEX_Pessoal"].ToString().Replace("$nome", "<B>" + dt.Rows[0]["PES_Nome"].ToString() + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;");
				dt.Dispose();

				//carrega as caracteristicas pessoais, de acordo com o que acertou
				// na tabela da tela anterior (caracteristicas)
				//fica em "Espelho de Sua Personalidade"


                dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_RetornarAnaliseCaracteristica", parms).Tables[0];
				if(dt.Rows.Count == 0)
					trCaracteristica.Visible = false;
				else
				{
					trCaracteristica.Visible = true;
					for(int i=0;i<dt.Rows.Count;i++)
					{
						lblCaracteristica.Text += dt.Rows[i]["CAR_Texto"].ToString().Replace("$nome", "<B>" + lblNome.Text + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;") + "<BR>";
					}
				}
			}
			catch(Exception ex)
			{
				string body = "Erro ao tentar carregar o resultado da avaliação (versão para impressão).<br><br>";
				body += "<b>Código da Pessoa:</b>" + Request.Cookies["PES_CodigoPessoa"].Value + "<br><br>";
				body += "Descrição do erro:<br>" + ex.Message;
				//new util().EnviarEmailErro(body);

				Response.Write(erro.AbrePopupErro("Erro ao tentar carregar o resultado da avaliação.\nO administrador já foi avisado.", "Erro", true));
				return;
			}
        }
        #endregion

        #endregion

    }
}
