using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Sistmars.BLL;

namespace SistMars.relatorio
{
    /// <summary>
    /// Summary description for enquetes.
    /// </summary>
    public class enquetes : System.Web.UI.Page
    {
        #region Controles
        protected System.Web.UI.WebControls.Label lblTotal;
        protected System.Web.UI.WebControls.Label lblPositivas;
        protected System.Web.UI.WebControls.Label lblParciais;
        protected System.Web.UI.WebControls.Label lblNegativas;
        protected System.Web.UI.WebControls.Button cmdPesquisar;
        protected System.Web.UI.WebControls.TextBox txtDataInicial;
        protected System.Web.UI.WebControls.TextBox txtDataFinal;
        protected System.Web.UI.WebControls.CompareValidator cvDataInicial;
        protected System.Web.UI.WebControls.CompareValidator cvDataFinal;
        protected System.Web.UI.WebControls.Label lblPositivasPerc;
        protected System.Web.UI.WebControls.Label lblParciaisPerc;
        protected System.Web.UI.WebControls.DataGrid dtgDetalhes;
        protected System.Web.UI.HtmlControls.HtmlInputHidden txtPageIndex;
        protected System.Web.UI.WebControls.Label lblNegativasPerc;
        #endregion

        #region Eventos

        #region PageLoad
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LimparPesquisa();

                //caso tenha sido passada alguma data por parametro, ja efetua a pesquisa automaticamente
                //isso ocorre quando volta da tela de resposta da enquete da pessoa, para poder recarregar os dados
                if (Request["dataInicial"] != null)
                    txtDataInicial.Text = Request["dataInicial"].ToString();

                if (Request["dataFinal"] != null)
                    txtDataFinal.Text = Request["dataFinal"].ToString();

                if (Request["dataInicial"] != null || Request["dataFinal"] != null)
                    ListarTotais("R");
            }
        }
        #endregion

        #region dtgDetalhes

        private void dtgDetalhes_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            dtgDetalhes.CurrentPageIndex = e.NewPageIndex;
            txtPageIndex.Value = e.NewPageIndex.ToString(); //guarda no hidden pra usar no javascript
            //dtgDetalhes.DataSource = ViewState["dtLista"];
            dtgDetalhes.DataSource = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_PesquisarEnqueteLista", null);
            dtgDetalhes.DataBind();
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
            this.dtgDetalhes.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dtgDetalhes_PageIndexChanged);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region cmdPesquisar

        private void cmdPesquisar_Click(object sender, System.EventArgs e)
        {
            ListarTotais("P");
        }
        #endregion

        #endregion

        #region Metodos

        #region CalcularPercentuais
        /// <summary>
        /// Coloca os valores percentuais
        /// </summary>
        private void CalcularPercentuais()
        {
            int iTotal = int.Parse(lblTotal.Text);
            float div = 0;

            if (iTotal > 0)
            {
                div = float.Parse(lblPositivas.Text) / float.Parse(iTotal.ToString()) * 100;
                lblPositivasPerc.Text = div.ToString("0.0");
                div = float.Parse(lblParciais.Text) / float.Parse(iTotal.ToString()) * 100;
                lblParciaisPerc.Text = div.ToString("0.0");
                div = float.Parse(lblNegativas.Text) / float.Parse(iTotal.ToString()) * 100;
                lblNegativasPerc.Text = div.ToString("0.0");
            }
        }
        #endregion

        #region ListarTotais
        /// <summary>
        /// Preenche a tabela com os totais de respostas
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void ListarTotais(string pesquisaResposta)
        {
            int iTotal = 0;
            List<SqlParameter> parms = new List<SqlParameter>();

            //cria array de parametros para poder fazer pesquisa indepentende dos campos selecionados
            ArrayList aParam = new ArrayList();
            int iParms = 0;

            //caso tenha preenchido data inicial, cria parametro
            if (!string.IsNullOrEmpty(txtDataInicial.Text))
            {
                parms.Add(new SqlParameter("@DataInicial", SqlDbType.SmallDateTime, 15));
                parms[0].Value = txtDataInicial.Text;
                aParam.Add(parms);
                iParms++;
            }

            //caso tenha preenchido data final, cria parametro
            if (!string.IsNullOrEmpty(txtDataFinal.Text))
            {
                parms.Add(new SqlParameter("@DataFinal", SqlDbType.SmallDateTime, 15));
                parms[1].Value = txtDataFinal.Text;
                aParam.Add(parms);
                iParms++;
            }

            DataTable dtTotal = null;
            DataTable dtLista = null;
            DataSet ds = null;
            try
            {
                dtTotal = new DataTable();
                dtLista = new DataTable();
                ds = new DataSet();

                //caso tenha preenchido alguma das datas, execute procedure com parametros
                SqlParameter[] paramSP = null;
                if (iParms > 0)
                {
                    paramSP = new SqlParameter[iParms];
                    aParam.CopyTo(paramSP);
                    ds = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_PesquisarEnquete", parms);
                    dtTotal = ds.Tables[0];
                    dtLista = ds.Tables[1];
                }
                else //senão, execute sem os parametros
                {
                    dtTotal = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_PesquisarEnquete", null).Tables[0];
                    dtLista = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_PesquisarEnqueteLista", null).Tables[0];
                }

                //limpa os dados de pesquisas anteriores
                LimparPesquisa();

                for (int i = 0; i < dtTotal.Rows.Count; i++) //varre a tabela
                {
                    switch (dtTotal.Rows[i]["ENQ_Corresponde"].ToString())
                    {
                        case "0": lblNegativas.Text = dtTotal.Rows[i]["Total"].ToString(); break;
                        case "1": lblParciais.Text = dtTotal.Rows[i]["Total"].ToString(); break;
                        case "2": lblPositivas.Text = dtTotal.Rows[i]["Total"].ToString(); break;
                    }
                    iTotal += int.Parse(dtTotal.Rows[i]["Total"].ToString());
                }
                lblTotal.Text = iTotal.ToString();

                //popula dataGrid
                dtgDetalhes.DataSource = dtLista;

                //caso tenha vindo da tela de resposta
                if (Request["pageIndex"] != null && pesquisaResposta == "R")
                    dtgDetalhes.CurrentPageIndex = int.Parse(Request["pageIndex"].ToString());
                else
                    dtgDetalhes.CurrentPageIndex = 0;

                txtPageIndex.Value = dtgDetalhes.CurrentPageIndex.ToString();
                dtgDetalhes.DataBind();

                //caso tenha retornado algo na pesquisa
                if (iTotal > 0)
                    CalcularPercentuais();
            }
            finally
            {
                if (ds != null)
                    ((IDisposable)ds).Dispose();

                if (dtTotal != null)
                    ((IDisposable)dtTotal).Dispose();

                if (dtLista != null)
                    ((IDisposable)dtLista).Dispose();
            }
        }
        #endregion

        #region LimparPesquisa
        /// <summary>
        /// Limpa campos da pesquisa
        /// </summary>
        private void LimparPesquisa()
        {
            lblNegativas.Text = "0";
            lblParciais.Text = "0";
            lblPositivas.Text = "0";
        }
        #endregion

        #endregion
    }
}
