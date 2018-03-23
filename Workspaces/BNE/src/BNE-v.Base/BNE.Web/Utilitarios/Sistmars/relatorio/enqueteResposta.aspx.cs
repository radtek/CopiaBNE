using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Email;

namespace SistMars.relatorio
{
    /// <summary>
    /// Summary description for enqueteResposta.
    /// </summary>
    public class enqueteResposta : System.Web.UI.Page
    {
        #region Controles
        protected System.Web.UI.WebControls.Label lblNome;
        protected System.Web.UI.WebControls.Image imgAvaliacao;
        protected System.Web.UI.WebControls.Label lblEmail;
        protected System.Web.UI.WebControls.Label lblTelefone;
        protected System.Web.UI.WebControls.Label lblDataEnquete;
        protected System.Web.UI.WebControls.TextBox txtObservacao;
        protected System.Web.UI.WebControls.Label lblExplicacao;
        protected System.Web.UI.WebControls.Label lblRespondida;
        protected System.Web.UI.WebControls.TextBox txtResposta;
        protected System.Web.UI.WebControls.Button cmdResponder;
        #endregion

        #region Eventos

        #region Page_Load

        private void Page_Load(object sender, System.EventArgs e)
        {
            ViewState.Add("ENQ_CodigoEnquete", Request["ENQ_CodigoEnquete"].ToString()); //codigo da enquete a ser respondida
            ViewState.Add("dataInicial", Request["dataInicial"].ToString()); //data inicial usada na pesquisa
            ViewState.Add("dataFinal", Request["dataFinal"].ToString()); //data final usada na pesquisa
            ViewState.Add("pageIndex", Request["pageIndex"].ToString()); //pagina atual do datagrid na pagina de pesquisa

            CarregarRegistro();
        }
        #endregion

        #region cmdResponder

        private void cmdResponder_Click(object sender, System.EventArgs e)
        {
            //verifica se digitou a resposta
            if (string.IsNullOrEmpty(Request["txtResposta"]) || string.IsNullOrEmpty(Request["txtResposta"]))
            {
                Response.Write(erro.AbrePopupErro("Err5", "Erros", true));
                return;
            }

            //cadastra a resposta no banco

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ENQ_CodigoEnquete", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@RES_Resposta", SqlDbType.VarChar, 2000));
            BNE.Sistmars.BLL.DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "p_CadastrarEnqueteResposta", parms);
            try
            {
                //envia resposta à pessoa por email
                EnviarEmailRespostaPessoa();
            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar enviar email de resposta de enquete.<br><br>";
                body += "Descrição do erro:<br>" + ex.Message;
                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err3", "Erros", true));
                return;
            }

            //atualiza parent (pagina de pesquisa) e essa pagina
            string script = "<script language=javascript>";
            script += "window.opener.navigate('enquetes.aspx";
            script += "?dataInicial=" + ViewState["dataInicial"].ToString();
            script += "&dataFinal=" + ViewState["dataFinal"].ToString();
            script += "&pageIndex=" + ViewState["pageIndex"].ToString() + "');\n";
            script += "window.close();\n";
            script += "</script>";            
            ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", script, true);

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
            this.cmdResponder.Click += new System.EventHandler(this.cmdResponder_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #endregion

        #region Metodos

        #region CarregarRegistro
        /// <summary>
        /// Carrega os dados de um registro de enquete, usando o ViewState["registroAtual"]
        /// </summary>
        private void CarregarRegistro()
        {
            //pega dados da enquete
            DataTable dt = BNE.Sistmars.BLL.DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_PesquisarEnqueteDetalhes", null).Tables[0];
            DataRow dr = dt.Rows[0];
            //carrega os dados
            lblNome.Text = dr["PES_Nome"].ToString();
            lblEmail.Text = dr["PES_Email"].ToString();
            lblTelefone.Text = dr["PES_Telefone"].ToString();
            txtObservacao.Text = dr["ENQ_Observacao"].ToString();
            lblDataEnquete.Text = dr["ENQ_DataCadastro"].ToString();
            imgAvaliacao.ImageUrl = "../images/avaliacao" + dr["ENQ_Corresponde"].ToString() + ".jpg";
            if (dr["RES_CodigoEnqueteResposta"].ToString() != "0")
            {
                //Ja foi respondida
                lblRespondida.Visible = true;
                txtResposta.ReadOnly = true;
                cmdResponder.Enabled = false;
                lblExplicacao.Visible = false;

                txtResposta.Text = dr["RES_Resposta"].ToString();
                lblRespondida.Text = "essa enquete já foi respondida em " + dr["RES_DataCadastro"].ToString();
            }
            else
            {
                //Ainda nao foi respondida
                lblRespondida.Visible = false;
                txtResposta.ReadOnly = false;
                cmdResponder.Enabled = true;
                lblExplicacao.Visible = true;

                txtResposta.Text = string.Empty;
                lblRespondida.Text = string.Empty;
            }
        }
        #endregion

        #region EnviarEmailRespostaPessoa
        /// <summary>
        /// Envia email para a pessoa com a resposta da enquete
        /// </summary>
        private void EnviarEmailRespostaPessoa()
        {
            StreamReader sr = null;
            string strBody;
            try
            {
                sr = new StreamReader(Server.MapPath("/sistmars/email/resposta.htm"));
                strBody = sr.ReadToEnd();
                strBody = strBody.Replace("#resposta#", Request["txtResposta"].ToString().Replace("\n", "<BR>"));
            

                if (string.IsNullOrEmpty(Request["txtObservacao"]) || string.IsNullOrEmpty(Request["txtObservacao"]))
                {
                    strBody = strBody.Replace("#comentario#", "");
                }
                else
                {
                    string obs = "<br><br><hr>De: " + lblNome.Text + "<br>";
                    obs += "Data: " + lblDataEnquete.Text + "<br><br>";
                    obs += Request["txtObservacao"].ToString();

                    strBody = strBody.Replace("#comentario#", obs);
                }
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }

            string from = BNE.BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, null);
            string to = lblEmail.Text;
            string message = strBody;
            string Subject = "Resposta de enquete SistMars";

            try
            {
                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Smtp)
                    .Enviar(Subject, message, from, to);

                ClientScript.RegisterClientScriptBlock(this.GetType(), "a", "<script>alert('E-mail enviado!');</script>", false);
            }
            catch
            {                
                ClientScript.RegisterClientScriptBlock(this.GetType(), "a", "<script>alert('Erro ao enviar o e-mail!');</script>", false);
            }
        }

        #endregion

        #endregion
    }
}
