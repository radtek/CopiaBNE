using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Email;

namespace SistMars.pessoa
{
    /// <summary>
    /// Summary description for resultado.
    /// </summary>
    public class resultado : System.Web.UI.Page
    {

        #region Controles
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.Button btnEnviarEmail;
        protected System.Web.UI.WebControls.Label lblNome;
        protected System.Web.UI.WebControls.Label lblDataNascimento;
        protected System.Web.UI.WebControls.Label lblPersonalidade;
        protected System.Web.UI.WebControls.Label lblCaracteristica;
        protected System.Web.UI.WebControls.Label lblEstagioAtual;
        protected System.Web.UI.WebControls.Label lblPessoal;
        protected System.Web.UI.WebControls.ImageButton imgContinuar;
        protected System.Web.UI.HtmlControls.HtmlTableRow trCaracteristica;
        protected System.Web.UI.WebControls.TextBox txtCodAnalise;
        protected System.Web.UI.HtmlControls.HtmlForm frmResultado;
        #endregion

        #region Eventos

        #region Page_Load

        private void Page_Load(object sender, System.EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    //carrega os resultados da avaliação
                    if (Request["ANA_CodigoAnalise"] != null)
                    {
                        //caso tenha sido passado codigo da analise (pelo menut/relat, por exemplo)
                        imgContinuar.Visible = false;
                        CarregarDadosAvaliacao("p_ListarAvaliacaoCodigo ", Request["ANA_CodigoAnalise"].Trim());
                    }
                    else if (Request["Enquete"] != null)
                    {
                        //caso tenha sido passado por parametro o codigo da pessoa (quando retorna da enquete)
                        imgContinuar.Visible = false;
                        CarregarDadosAvaliacao("p_ListarAvaliacaoPessoa ", Request.Cookies["PES_CodigoPessoa"].Value);
                    }
                    else
                    {
                        //caso esteja vindo parar aqui logo depois de realizar o teste..
                        //tem que analisar as respostas e gravar no banco antes de carregar os dados
                        AnalisarResultado();
                        CarregarDadosAvaliacao("p_ListarAvaliacaoPessoa", Request.Cookies["PES_CodigoPessoa"].Value);
                    }
                }
                catch (Exception ex)
                {
                    string body = "Erro ao carregar a página de resultado.<br><br>";
                    body += "<b>Request:</b>" + Request.Form.ToString() + "<br><br>";
                    body += "Descrição do erro:<br>" + ex.Message;
                    body += "<br><br><b>Sistema Operacional:</b> " + Request.Browser.Platform +
                            "<br><b>Browser:</b> " + Request.Browser.Type +
                            "<b> versão </b>" + Request.Browser.Version +
                            "<br><b>Suporte a javascript:</b> " + Request.Browser.EcmaScriptVersion +
                            "<b> versão </b>" + Request.Browser.EcmaScriptVersion;

                    new util().EnviarEmailErro(body);

                    Response.Write(erro.AbrePopupErro("Err4", "Erros", true));
                    return;
                }

            }

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
            this.btnEnviarEmail.Click += new System.EventHandler(this.btnEnviarEmail_Click);
            this.imgContinuar.Click += new System.Web.UI.ImageClickEventHandler(this.imgContinuar_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region imgContinuar

        private void imgContinuar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("enquete.aspx");
        }

        #endregion

        #region btnEnviarEmail

        private void btnEnviarEmail_Click(object sender, System.EventArgs e)
        {
            EnviaEmail(int.Parse(txtCodAnalise.Text), txtEmail.Text);
        }

        #endregion

        #endregion

        #region Metodos

        #region AnalisarResultado
        /// <summary>
        /// Analisa o resultado da avaliação e grava os dados no banco
        /// </summary>
        private void AnalisarResultado()
        {
            //deixa no formato pro SQL ('A1','B4','H8')
            string colunasLinhas = "'" + String.Join("'',''", Request["hiddenSelecao"].ToString().Split(',')) + "'";
            string colLin = "";
            string colunasLinhasCertas = "'";

            byte totalPontos = 0, linha = 0;
            util oUtil = new util();

            //cada trimestre da data de nascimento da pessoa, tem duas linhas que contam pontos
            //por exemplo.. se ele nascer no primeiro trimestre do ano, as linhas 1 e 5 valem pontos
            //se nascer no segundo trimestre, cada item selecionado nas linhas 2 e 6 conta um ponto
            //e assim por diante
            linha = oUtil.RetornarTrimestre(DateTime.Parse(Request.Cookies["PES_DataNascimento"].Value));

            //para cada item na linha que o cara acertou, ganha um ponto
            for (byte i = 1; i <= 8; i++)
            {
                //pega coluna e linha ("A1" por exemplo)
                colLin = oUtil.RetornarLetraNumero(i) + linha.ToString();
                if (colunasLinhas.IndexOf(colLin) > 0)
                {
                    //caso tenha achado essa coluna/linha nas respostas da pessoa:
                    colunasLinhasCertas += colLin + "','"; //adiciona na lista de respostas corretas
                    totalPontos++; //soma um ponto de acerto
                }
            }

            linha += 4;
            //para cada item na linha que o cara acertou, ganha um ponto
            for (byte i = 1; i <= 8; i++)
            {
                //pega coluna e linha ("A1" por exemplo)
                colLin = oUtil.RetornarLetraNumero(i) + linha.ToString();
                if (colunasLinhas.IndexOf(colLin) > 0)
                {
                    //caso tenha achado essa coluna/linha nas respostas da pessoa:
                    colunasLinhasCertas += colLin + "','"; //adiciona na lista de respostas corretas
                    totalPontos++; //soma um ponto de acerto
                }
            }

            //tira a ultima virgula que sobra quando coloca as colunas/linhas certas
            if (colunasLinhasCertas != "'")
                colunasLinhasCertas = colunasLinhasCertas.Substring(0, colunasLinhasCertas.Length - 3) + "'";
            else
                colunasLinhasCertas = string.Empty;

            try
            {
                //grava os dados da avaliação no banco
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@PES_CodigoPessoa", SqlDbType.Int, 4));
                parms.Add(new SqlParameter("@TotalPontos", SqlDbType.TinyInt, 20));
                parms.Add(new SqlParameter("@TodasColunasLinhas", SqlDbType.VarChar, 120));

                parms[0].Value = Request.Cookies["PES_CodigoPessoa"].Value;
                parms[1].Value = totalPontos;
                parms[2].Value = colunasLinhasCertas;

                BNE.Sistmars.BLL.DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "p_CadastrarAvaliacao", parms);

            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar cadastrar a avaliação.<br><br>";
                body += "<b>Código da Pessoa:</b>" + Request.Cookies["PES_CodigoPessoa"].Value + "<br><br>";
                body += "Descrição do erro:<br>" + ex.Message;
                body += "<br><br><b>Sistema Operacional:</b> " + Request.Browser.Platform +
                        "<br><b>Browser:</b> " + Request.Browser.Type +
                        "<b> versão </b>" + Request.Browser.Version +
                        "<br><b>Suporte a javascript:</b> " + Request.Browser.EcmaScriptVersion +
                        "<b> versão </b>" + Request.Browser.EcmaScriptVersion;

                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err2", "Erro", true));
                return;
            }
        }

        #endregion

        #region CarregarDadosAvaliacao
        /// <summary>
        /// carrega os resultados da avaliação
        /// </summary>
        /// <param name="proc">Procedure a ser executada</param>
        private void CarregarDadosAvaliacao(string proc, string codPessoa)
        {
            string codigoAnalise = string.Empty;
            DataTable dt;

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                if (Request["ANA_CodigoAnalise"] != null)
                {
                    parms.Add(new SqlParameter("@ANA_CodigoAnalise", SqlDbType.Int, 4));
                    parms[0].Value = codPessoa;
                }
                else
                {
                    parms.Add(new SqlParameter("@PES_CodigoPessoa", SqlDbType.Int, 4));
                    parms[0].Value = codPessoa;
                }

                dt = BNE.Sistmars.BLL.DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, proc, parms).Tables[0];

                lblNome.Text = dt.Rows[0]["PES_Nome"].ToString();
                lblDataNascimento.Text = dt.Rows[0]["PES_DataNascimento"].ToString();

                lblPersonalidade.Text = dt.Rows[0]["TEX_Personalidade"].ToString().Replace("$nome", "<B>" + dt.Rows[0]["PES_Nome"].ToString() + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;");
                lblEstagioAtual.Text = dt.Rows[0]["PON_Texto"].ToString().Replace("$nome", "<B>" + dt.Rows[0]["PES_Nome"].ToString() + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;");
                lblPessoal.Text = dt.Rows[0]["TEX_Pessoal"].ToString().Replace("$nome", "<B>" + dt.Rows[0]["PES_Nome"].ToString() + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;");

                txtCodAnalise.Text = dt.Rows[0]["ANA_CodigoAnalise"].ToString();

                codigoAnalise = dt.Rows[0]["ANA_CodigoAnalise"].ToString();
                Response.Cookies.Add(new System.Web.HttpCookie("ANA_CodigoAnalise", codigoAnalise)); //para usar na "versão p/ impressão"
                dt.Dispose();

                //carrega as caracteristicas pessoais, de acordo com o que acertou
                // na tabela da tela anterior (caracteristicas)
                //fica em "Espelho de Sua Personalidade"

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ANA_CodigoAnalise", SqlDbType.Int, 4));
                parameters[0].Value = codigoAnalise;

                dt = BNE.Sistmars.BLL.DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_RetornarAnaliseCaracteristica", parameters).Tables[0];

                if (dt.Rows.Count == 0)
                    trCaracteristica.Visible = false;
                else
                {
                    trCaracteristica.Visible = true;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lblCaracteristica.Text += dt.Rows[i]["CAR_Texto"].ToString().Replace("$nome", "<B>" + lblNome.Text + "</B>").Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;") + "<BR>";
                    }
                }
            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar carregar o resultado da avaliação.<br><br>";
                body += "<b>Código da Pessoa:</b>" + Request.Cookies["PES_CodigoPessoa"].Value + "<br><br>";
                body += "Descrição do erro:<br>" + ex.Message;
                body += "<br><br><b>Sistema Operacional:</b> " + Request.Browser.Platform +
                        "<br><b>Browser:</b> " + Request.Browser.Type +
                        "<b> versão </b>" + Request.Browser.Version +
                        "<br><b>Suporte a javascript:</b> " + Request.Browser.EcmaScriptVersion +
                        "<b> versão </b>" + Request.Browser.EcmaScriptVersion;
                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err4", "Erro", true));
                return;
            }
        }

        #endregion

        #region Envio de Email
        public void EnviaEmail(int iCodigoAnalise, string emailEnvio)
        {
            //Se não for informado um email pelo usuário exibirá uma mensagem de erro.
            if (!string.IsNullOrEmpty(emailEnvio))
            {
                //string strBodyEmail = "http://www.bne.com.br/Utilitarios/sistmars/pessoa/.aspx?ANA_CodigoAnalise=" + iCodigoAnalise;
                string strBodyEmail = string.Format("http://{0}/Utilitarios/Sistmars/pessoa/resultadomail.aspx?ANA_CodigoAnalise={1}", Request.ServerVariables["HTTP_HOST"], iCodigoAnalise);
                string html;
                WebRequest request = WebRequest.Create(strBodyEmail);
                WebResponse response = request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                html = sr.ReadToEnd();
                var emailMensagem = BNE.BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                try
                {
                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar("Resultado SistMars", html,null, emailMensagem, emailEnvio);

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "a", "<script>alert('E-mail enviado!');</script>", false);
                }
                catch
                {
                    Response.Write("Problemas no envio de e-mail. Contacte o administrador.");
                }
            }
            else
            {
                Response.Write("Problemas no envio de e-mail. Contacte o administrador.");
            }
        }
        #endregion

        #endregion

    }
}
