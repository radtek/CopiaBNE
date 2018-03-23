using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Sistmars.BLL;

namespace SistMars.manut
{
    /// <summary>
    /// Summary description for caracteristica.
    /// </summary>
    public class caracteristica : System.Web.UI.Page
    {

        #region Controles
        protected System.Web.UI.WebControls.TextBox txtColunaLinha;
        protected System.Web.UI.WebControls.Button cmdBuscar;
        protected System.Web.UI.WebControls.RegularExpressionValidator revColunaLinha;
        protected System.Web.UI.WebControls.Button cmdGravar;
        protected System.Web.UI.WebControls.TextBox txtTexto;
        protected System.Web.UI.WebControls.TextBox txtDescricao;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvColunaLinha;
        #endregion

        #region Eventos

        #region Page_Load
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
            this.txtColunaLinha.TextChanged += new System.EventHandler(this.txtColunaLinha_TextChanged);
            this.cmdBuscar.Click += new System.EventHandler(this.cmdBuscar_Click);
            this.cmdGravar.Click += new System.EventHandler(this.cmdGravar_Click);
            this.ID = "frmCaracteristica";
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region txtColunaLinha

        private void txtColunaLinha_TextChanged(object sender, System.EventArgs e)
        {
            if (txtColunaLinha.Text.Length == 2)
                Pesquisar();
        }

        #endregion

        #region cmdBuscar

        private void cmdBuscar_Click(object sender, System.EventArgs e)
        {
            Pesquisar();
        }
        #endregion

        #region cmdGravar

        private void cmdGravar_Click(object sender, System.EventArgs e)
        {
            GravarDados();
        }

        #endregion

        #endregion

        #region Metodos

        #region Pesquisar
        /// <summary>
        /// Carrega o texto
        /// </summary>
        private void Pesquisar()
        {
            //seleciona código e descricao da caracteristica do banco
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CAR_ColunaLinha", SqlDbType.Char, 3));
            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_RetornarCaracteristica", parms).Tables[0];

            if (dt.Rows.Count > 0)
            {
                txtDescricao.Text = dt.Rows[0]["CAR_Descricao"].ToString();
                txtTexto.Text = dt.Rows[0]["CAR_Texto"].ToString();
                ViewState["CAR_CodigoCaracteristica"] = dt.Rows[0]["CAR_CodigoCaracteristica"].ToString();
                HabilitarControles(true);
            }
            else
            {
                txtDescricao.Text = "-| não disponível |-";
                txtTexto.Text = "-| não disponível |-";
                ViewState["CAR_CodigoCaracteristica"] = "";
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
            txtTexto.Enabled = Habilitar;
            cmdGravar.Enabled = Habilitar;
        }

        #endregion

        #region GravarDados
        /// <summary>
        /// Grava os dados no banco
        /// </summary>
        private void GravarDados()
        {
           

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CAR_CodigoCaracteristica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@CAR_Texto", SqlDbType.Text, 100));
            parms[0].Value = ViewState["CAR_CodigoCaracteristica"].ToString();
            parms[1].Value = txtTexto.Text;
            try
            {
                DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_AtualizarCaracteristica",parms);
            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar gravar Caracteristica!<br>";
                body += "Descrição do erro:<br>" + ex.Message;
                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err2", "Erros", true));
            }            
        }
        #endregion

        #endregion

    }
}
