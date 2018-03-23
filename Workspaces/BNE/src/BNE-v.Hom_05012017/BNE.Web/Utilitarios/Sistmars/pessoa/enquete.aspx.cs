using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Sistmars.BLL;

namespace SistMars.pessoa
{
    /// <summary>
    /// Summary description for enquete.
    /// </summary>
    public class frmEnquete : System.Web.UI.Page
    {

        #region Controles
        protected System.Web.UI.WebControls.TextBox txtObservacao;
        protected System.Web.UI.WebControls.RadioButtonList optCorresponde;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
        protected System.Web.UI.WebControls.ImageButton imgContinuar;
        #endregion

        #region Eventos

        #region Page_Load

        private void Page_Load(object sender, System.EventArgs e)
        {
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
            this.imgContinuar.Click += new System.Web.UI.ImageClickEventHandler(this.imgContinuar_Click);
            this.ID = "frmEnquete";
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region imgContinuar

        private void imgContinuar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //Grava os dados da enquete
            GravarDados();

            //Retorna para a tela que tem os resultados da analise da pessoa
            Response.Redirect("resultado.aspx?Enquete=1");
        }

        #endregion

        #endregion

        #region Metodos

        #region GravarDados
        /// <summary>
        /// Grava os dados no banco
        /// </summary>
        private void GravarDados()
        {



            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PES_CodigoPessoa", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@ENQ_Corresponde", SqlDbType.TinyInt, 20));
            parms.Add(new SqlParameter("@ENQ_Observacao", SqlDbType.Text, 120));

            parms[0].Value = Request.Cookies["PES_CodigoPessoa"].Value;
            parms[1].Value = optCorresponde.SelectedItem.Value;
            parms[2].Value = txtObservacao.Text;

            try
            {
                DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_CadastrarEnquete", parms);
            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar gravar Enquete!<br>";
                body += "Descrição do erro:<br>" + ex.Message;
                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err2", "Erros", true));
            }
        }
        #endregion

        #endregion
    }
}
