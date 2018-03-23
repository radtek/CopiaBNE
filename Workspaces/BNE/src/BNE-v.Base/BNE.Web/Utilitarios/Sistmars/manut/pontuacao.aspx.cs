using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Sistmars.BLL;

namespace SistMars.manut
{
    /// <summary>
    /// Summary description for pontuacao.
    /// </summary>
    public class pontuacao : System.Web.UI.Page
    {
        #region Controles
        protected System.Web.UI.WebControls.TextBox txtBaixaMin;
        protected System.Web.UI.WebControls.TextBox txtBaixaMax;
        protected System.Web.UI.WebControls.TextBox txtMediaMax;
        protected System.Web.UI.WebControls.TextBox txtMediaMin;
        protected System.Web.UI.WebControls.TextBox txtAltaMax;
        protected System.Web.UI.WebControls.TextBox txtAltaMin;
        protected System.Web.UI.WebControls.Button cmdGravar;
        #endregion

        #region Eventos

        #region Page_Load

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
                CarregarDados();
        }

        #endregion

        #region cmdGrava

        private void cmdGravar_Click(object sender, System.EventArgs e)
        {
            GravarDados();
            Response.Write(erro.AbrePopupErro("Msg1", "Sucesso", true));
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
            this.cmdGravar.Click += new System.EventHandler(this.cmdGravar_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #endregion

        #region Metodos

        #region CarregarDados
        /// <summary>
        /// Carrega os dados na tela
        /// </summary>
        private void CarregarDados()
        {
            //seleciona código e descricao da caracteristica do banco
            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_RetornarPontuacao", null).Tables[0]; // ("p_RetornarPontuacao");

            txtBaixaMin.Text = dt.Rows[0]["PON_Minimo"].ToString();
            txtBaixaMax.Text = dt.Rows[0]["PON_Maximo"].ToString();
            txtMediaMin.Text = dt.Rows[1]["PON_Minimo"].ToString();
            txtMediaMax.Text = dt.Rows[1]["PON_Maximo"].ToString();
            txtAltaMin.Text = dt.Rows[2]["PON_Minimo"].ToString();
            txtAltaMax.Text = dt.Rows[2]["PON_Maximo"].ToString();

            dt.Dispose();
        }
        #endregion

        #region GravarDados
        /// <summary>
        /// Grava os dados no banco
        /// </summary>
        private void GravarDados()
        {

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PON_BaixaMin", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PON_BaixaMax", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PON_MediaMin", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PON_MediaMax", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PON_AltaMin", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PON_AltaMax", SqlDbType.Int, 4));

            parms[0].Value = txtBaixaMin.Text;
            parms[1].Value = txtBaixaMax.Text;
            parms[2].Value = txtMediaMin.Text;
            parms[3].Value = txtMediaMax.Text;
            parms[4].Value = txtAltaMin.Text;
            parms[5].Value = txtAltaMax.Text;
            try
            {
                DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_AtualizarPontuacao", parms);
            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar gravar a Pontuação!<br>";
                body += "Descrição do erro:<br>" + ex.Message;
                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err2", "Erros", true));
            }           
        }
        #endregion

        #endregion
    }
}
