using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Sistmars.BLL;

namespace SistMars.manut
{
    /// <summary>
    /// Summary description for texto.
    /// </summary>
    public class frmTexto : System.Web.UI.Page
    {
        #region Controles
        protected System.Web.UI.WebControls.TextBox txtPessoal;
        protected System.Web.UI.WebControls.TextBox txtPersonalidade;
        protected System.Web.UI.WebControls.RegularExpressionValidator revDiaMes;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvDiaMes;
        protected System.Web.UI.WebControls.Button cmdBuscar;
        protected System.Web.UI.WebControls.Button cmdGravar1;
        protected System.Web.UI.WebControls.Button cmdGravar2;
        protected System.Web.UI.WebControls.TextBox txtDiaMes;
        #endregion

        #region Métodos

        #region Pesquisar
        /// <summary>
        /// Carrega os textos do dia/mes
        /// </summary>
        private void Pesquisar()
        {
            #region Consulta
            const string SPPESQUISAR = @"SELECT ISNULL(TEX_Pessoal, '') AS TEX_Pessoal, " +
            "ISNULL(TEX_Personalidade, '') AS TEX_Personalidade " +
            "FROM SMA_Texto WHERE TEX_DiaMes = @DiasMes";
            #endregion

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@DiasMes",SqlDbType.Text,20));
            parms[0].Value = txtDiaMes.Text;
            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPPESQUISAR, parms).Tables[0];         

            if (dt.Rows.Count > 0)
            {
                txtPessoal.Text = dt.Rows[0]["TEX_Pessoal"].ToString();
                txtPersonalidade.Text = dt.Rows[0]["TEX_Personalidade"].ToString();
                HabilitarControles(true);
            }
            else
            {
                txtPessoal.Text = "-| não disponível |-";
                txtPersonalidade.Text = "-| não disponível |-";
                HabilitarControles(false);
            }

            dt.Dispose();
        }
        #endregion

        #region HabilitarControles
        /// <summary>
        /// Habilita ou desabilita os controles
        /// </summary>
        /// <param name="Habilitar">true habilita, false desabilita</param>
        private void HabilitarControles(bool Habilitar)
        {
            txtPessoal.Enabled = Habilitar;
            txtPersonalidade.Enabled = Habilitar;
            cmdGravar1.Enabled = Habilitar;
            cmdGravar2.Enabled = Habilitar;
        }
        #endregion

        #region GravarDados
        /// <summary>
        /// Grava os textos no banco
        /// </summary>
        private void GravarDados()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@TEX_DiaMes", SqlDbType.Char, 3));
            parms.Add(new SqlParameter("@TEX_Personalidade", SqlDbType.Text, 30));
            parms.Add(new SqlParameter("@TEX_Pessoal", SqlDbType.VarChar, 120));

            parms[0].Value = txtDiaMes.Text;
            parms[1].Value = txtPersonalidade.Text;
            parms[2].Value = txtPessoal.Text;

            try
            {
                DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "p_AtualizarTexto", parms);
            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar gravar Texto!<br>";
                body += "Descrição do erro:<br>" + ex.Message;
                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err2", "Erros", true));
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
                HabilitarControles(false);
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
            this.txtDiaMes.TextChanged += new System.EventHandler(this.txtDiaMes_TextChanged);
            this.cmdBuscar.Click += new System.EventHandler(this.cmdBuscar_Click);
            this.cmdGravar1.Click += new System.EventHandler(this.cmdGravar1_Click);
            this.cmdGravar2.Click += new System.EventHandler(this.cmdGravar2_Click);
            this.ID = "frmTexto";
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion      

        #region cmdOK
        protected void cmdOK_Click(object sender, System.EventArgs e)
        {
            Pesquisar();
        }
        #endregion

        #region cmdBuscar
        protected void cmdBuscar_Click(object sender, System.EventArgs e)
        {
            Pesquisar();
        }
        #endregion

        #region cmdGravar1

        private void cmdGravar1_Click(object sender, System.EventArgs e)
        {
            GravarDados();
        }

        #endregion

        #region cmdGravar2
        private void cmdGravar2_Click(object sender, System.EventArgs e)
        {
            GravarDados();
        }
        #endregion

        #region txtDiaMes
        private void txtDiaMes_TextChanged(object sender, System.EventArgs e)
        {
            if (txtDiaMes.Text.Length == 4)
                Pesquisar();
        }
        #endregion

        #endregion
    }
}
