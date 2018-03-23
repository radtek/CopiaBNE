using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using BNE.BLL.Integracoes.Facebook;
using BNE.Sistmars.BLL;

namespace SistMars
{
    /// <summary>
    /// Summary description for dadosPessoais.
    /// </summary>
    public class dadosPessoais : System.Web.UI.Page
    {
        #region Controles
        protected System.Web.UI.WebControls.TextBox txtCPF;
        protected System.Web.UI.WebControls.TextBox txtNome;
        protected System.Web.UI.WebControls.TextBox txtCidade;
        protected System.Web.UI.WebControls.TextBox txtCEP;
        protected System.Web.UI.WebControls.TextBox txtTelefone;
        protected System.Web.UI.WebControls.TextBox txtTelefoneDDD;
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.TextBox txtDataNascimento;
        protected System.Web.UI.WebControls.ImageButton imgContinuar;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvCPF;
        protected System.Web.UI.WebControls.RegularExpressionValidator revCPF;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvNome;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvCidade;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvCEP;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvTelefone;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvEmail;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvDDD;
        protected System.Web.UI.WebControls.RequiredFieldValidator rfvDataNascimento;
        protected System.Web.UI.WebControls.RegularExpressionValidator revCEP;
        protected System.Web.UI.WebControls.RegularExpressionValidator revDDD;
        protected System.Web.UI.WebControls.RegularExpressionValidator revTelefone;
        protected System.Web.UI.WebControls.RegularExpressionValidator revEmail;
        protected System.Web.UI.WebControls.CompareValidator cvDataNascimento;
        protected System.Web.UI.WebControls.RangeValidator rvEstado;
        protected System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
        protected System.Web.UI.WebControls.Button cmdBuscar;
        protected System.Web.UI.WebControls.DropDownList cboEstado;
        #endregion

        #region Eventos

        #region PageLoad

        private void Page_Load(object sender, System.EventArgs e)
        {
            Response.Cookies["PES_CodigoPessoa"].Expires = DateTime.Now.AddYears(-30);
            Response.Cookies["PES_DataNascimento"].Expires = DateTime.Now.AddYears(-30);
            Response.Cookies["ANA_CodigoAnalise"].Expires = DateTime.Now.AddYears(-30);
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
            this.cmdBuscar.Click += new System.EventHandler(this.cmdBuscar_Click);
            this.imgContinuar.Click += new System.Web.UI.ImageClickEventHandler(this.imgContinuar_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

        #region imgContinua
        /// <summary>
        /// grava os dados e vai pro próximo passo
        /// </summary>
        private void imgContinuar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string sCPF = txtCPF.Text;
            string erro_cpf = string.Empty;
            Array retorno = null;
            util objUtil = new util();

            //verifica se o cpf digitado é válido
            if (!objUtil.ValidarCPF(sCPF, out erro_cpf))
            {
                Response.Write(erro.AbrePopupErro(erro_cpf, "CPF", true));
                return;
            }

            //tudo ok.. hora de gravar os dados...
            //basta passar tudo pra procedure, que ela se encarrega
            //de atualizar os dados caso ja existam ou
            //de gerar novo registro caso ainda nao exista para o cpf
            try
            {
                retorno = GravarDados();
            }
            catch (Exception ex)
            {
                string body = "Erro ao tentar gravar dados da pessoa.<br><br>";
                body += "<b>Request:</b>" + Request.Form.ToString() + "<br><br>";
                body += "Descrição do erro:<br>" + ex.Message;
                body += "<br><br><b>Sistema Operacional:</b> " + Request.Browser.Platform +
                        "<br><b>Browser:</b> " + Request.Browser.Type +
                        "<b> versão </b>" + Request.Browser.Version +
                        "<br><b>Suporte a javascript:</b> " + Request.Browser.EcmaScriptVersion +
                        "<b> versão </b>" + Request.Browser.EcmaScriptVersion;
                new util().EnviarEmailErro(body);

                Response.Write(erro.AbrePopupErro("Err2", "Erros", true));
                return;
            }

            //Grava no Cookie o Codigo da Pessoa e a data de nascimento
            Response.Cookies.Add(new System.Web.HttpCookie("PES_CodigoPessoa", retorno.GetValue(0).ToString()));
            Response.Cookies.Add(new System.Web.HttpCookie("PES_DataNascimento", retorno.GetValue(1).ToString()));
            Response.Redirect("caracteristicas.aspx");
        }
        #endregion

        #region cmdBuscar
        /// <summary>
        /// busca dados pessoais de acordo com o cpf
        /// </summary>
        private void cmdBuscar_Click(object sender, System.EventArgs e)
        {
            string sCPF = txtCPF.Text;
            string erro_cpf = "";
            util objutil = new util();

            //verifica se o cpf digitado é válido
            if (!objutil.ValidarCPF(sCPF, out erro_cpf))
            {
                Response.Write(erro.AbrePopupErro(erro_cpf, "CPF", true));
                return;
            }

            //tenta achar os dados da pessoa em dois bancos pra tentar carregar...
            CarregarPessoa(sCPF);
        }
        #endregion

        #endregion

        #region Metodos

        #region GravarDados
        /// <summary>
        /// Grava os dados e retorna o id da pessoa
        /// </summary>
        /// <returns>Codigo da pessoa e data de nascimento (seja cadastro ou alteração)</returns>
        /// 
        private Array GravarDados()
        {
            decimal CPF = decimal.Parse(txtCPF.Text);
            string nome = txtNome.Text.ToString();
            string cidade = txtCidade.Text.ToString();
            string estado = cboEstado.SelectedItem.ToString();
            int CEP = Convert.ToInt32(txtCEP.Text);
            string telefone = txtTelefoneDDD.Text + txtTelefone.Text;
            string email = txtEmail.Text.ToString();

            DateTime dataNascimento = new DateTime();
            DateTime dataDummy;
            if (DateTime.TryParse(txtDataNascimento.Text, out dataDummy))
                dataNascimento = dataDummy;
            else
            {
                if (DateTime.TryParseExact(txtDataNascimento.Text, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataDummy))
                    dataNascimento = dataDummy;
            }

            Array array = Pessoa.SalvarDados(CPF, nome, cidade, estado, CEP, telefone, email, dataNascimento);

            //retorna o codigo inserido/alterado
            return array;
        }
        #endregion

        #region CarregarPessoa
        /// <summary>
        /// Busca os dados da pessoa de acordo com o CPF informado.
        /// Primeiro tentar buscar na base do Sistmars, depois no BNE.
        /// </summary>
        /// <param name="sCPF">CPF a se pesquisado</param>
        private void CarregarPessoa(string sCPF)
        {
            //tenta buscar os dados no sistMars

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PES_CPF", SqlDbType.Char, 11));
            parms[0].Value = sCPF;

            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_ListarPessoa", parms).Tables[0];

            if (dt.Rows.Count > 0)
            {
                txtNome.Text = dt.Rows[0]["PES_Nome"].ToString();
                txtCidade.Text = dt.Rows[0]["PES_Cidade"].ToString();
                cboEstado.ClearSelection();
                cboEstado.Items.FindByText(dt.Rows[0]["PES_Estado"].ToString()).Selected = true;
                try
                {
                    txtCEP.Text = dt.Rows[0]["PES_CEP"].ToString();
                }
                catch
                {

                }

                try
                {
                    txtTelefoneDDD.Text = dt.Rows[0]["PES_Telefone"].ToString().Substring(0, 2);
                    txtTelefone.Text = dt.Rows[0]["PES_Telefone"].ToString().Substring(2);
                }
                catch
                {

                }
                txtEmail.Text = dt.Rows[0]["PES_Email"].ToString();
                //txtDataNascimento.Text = dt.Rows[0]["PES_DataNascimento"].ToString();
                dt.Dispose();
            }
            else
            {
                dt.Dispose();
                //já que nao achou no sistmars, vai ver se tem cadastro no BNE   
                List<SqlParameter> parametrosBne = new List<SqlParameter>();
                parametrosBne.Add(new SqlParameter("@CPF", SqlDbType.Decimal, 11));
                parametrosBne[0].Value = txtCPF.Text;
                dt = BNE.BLL.DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BNE_SP_LISTAR_PESSOA_FISICA", parametrosBne).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    txtNome.Text = dt.Rows[0]["Nme_Pessoa"].ToString();
                    txtCidade.Text = dt.Rows[0]["Nme_Cidade"].ToString();
                    cboEstado.ClearSelection();
                    try
                    {
                        cboEstado.Items.FindByText(dt.Rows[0]["Sig_Estado"].ToString()).Selected = true;
                    }
                    catch
                    {

                    }
                    txtCEP.Text = dt.Rows[0]["Num_Cep"].ToString();
                    try
                    {

                        txtTelefoneDDD.Text = dt.Rows[0]["Num_DDD_Celular"].ToString();
                        txtTelefone.Text = dt.Rows[0]["Num_Celular"].ToString();
                    }
                    catch
                    {

                    }
                    try
                    {
                        txtEmail.Text = dt.Rows[0]["Eml_Pessoa"].ToString();
                    }
                    catch
                    {

                    }
                    //txtDataNascimento.Text = dt.Rows[0]["Data_Nascimento"].ToString();
                }
                else
                    LimparCampos();

                dt.Dispose();
            }

            //desce para o final da pagina            
            ClientScript.RegisterStartupScript(this.GetType(), "scroll", "<script language=javascript>window.scrollBy(0, 500);</script>");
        }
        #endregion

        #region LimparCampos
        /// <summary>
        /// Limpa todos os campos da tela
        /// </summary>
        private void LimparCampos()
        {
            txtNome.Text = string.Empty;
            txtCidade.Text = string.Empty;
            cboEstado.ClearSelection();
            txtCEP.Text = string.Empty;
            txtTelefoneDDD.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDataNascimento.Text = string.Empty;
        }
        #endregion

        #endregion
    }
}

