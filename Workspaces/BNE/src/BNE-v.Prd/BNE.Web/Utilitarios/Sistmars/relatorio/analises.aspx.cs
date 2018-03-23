using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Sistmars.BLL;

namespace SistMars.relatorio
{
    /// <summary>
    /// Summary description for analises.
    /// </summary>
    public class analises : System.Web.UI.Page
    {
        #region Controles
        protected System.Web.UI.WebControls.TextBox txtPesquisa;
        protected System.Web.UI.WebControls.Button cmdPesquisar;
        protected System.Web.UI.WebControls.DataGrid dtgPesquisa;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvPesquisa;
        protected System.Web.UI.WebControls.Label lblNenhum;
        protected System.Web.UI.WebControls.RadioButtonList optPesquisa;
        #endregion

        #region Eventos

        #region PageLoad

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
            this.cmdPesquisar.Click += new System.EventHandler(this.cmdPesquisar_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region cmdPesquisar

        private void cmdPesquisar_Click(object sender, System.EventArgs e)
        {
            Pesquisar();
        }
        #endregion

        #endregion

        #region Metodos

        #region Pesquisar
        /// <summary>
        /// Executa a pesquisa de acordo com a opção selecionada
        /// </summary>
        private void Pesquisar()
        {          
            List<SqlParameter> parms = new List<SqlParameter>();

            switch (optPesquisa.SelectedItem.Value)
            {     
    
                case "Nome": parms.Add(new SqlParameter("@PES_Nome",SqlDbType.VarChar,100));
                    break;
                case "CPF": parms.Add(new SqlParameter("@PES_CPF",SqlDbType.Char,11));
                    break;
                case "Email":parms.Add(new SqlParameter("@PES_Email",SqlDbType.VarChar,50));
                    break;
            }

            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_PesquisarAnalise", parms).Tables[0];

            if (dt.Rows.Count == 0)
            {
                lblNenhum.Visible = true;
                dtgPesquisa.Visible = false;
            }
            else
            {
                lblNenhum.Visible = false;
                dtgPesquisa.Visible = true;

                dtgPesquisa.DataSource = dt;
                dtgPesquisa.DataBind();
            }

            dt.Dispose();
        }

        #endregion

        #endregion
    }
}
