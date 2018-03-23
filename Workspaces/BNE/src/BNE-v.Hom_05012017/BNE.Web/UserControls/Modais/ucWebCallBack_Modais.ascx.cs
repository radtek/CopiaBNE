using System;
using System.IO;
using System.Net;
using BNE.BLL;
using BNE.BLL.Assincronos;
using BNE.BLL.Common;
using BNE.BLL.Custom.Email;
using BNE.EL;
using BNE.Web.Code;
using Newtonsoft.Json;
using Helper = BNE.BLL.Custom.Helper;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucWebCallBack_Modais : BaseUserControl
    {
        private bool _deveGravarLogCrm = true;

        public bool DeveGravarLogCrm
        {
            get { return _deveGravarLogCrm; }
            set { _deveGravarLogCrm = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Inicializar();
            }
        }

        protected internal void SetModalComercialId(string id)
        {
            var control = this.FindControl("myModalComercial");
            if (control != null)
            {
                control.ID = id;
            }
        }

        protected internal void SetModalMensagemId(string id)
        {
            var control = this.FindControl("myModalMensagem");
            if (control != null)
            {
                control.ID = id;
            }
        }

        public void Inicializar()
        {
            LimparCampos();
            PreencherCampos();
        }

        public RetornoStatus RetornarStatus(string fila)
        {
            //URL para verificar status da Fila (fila=ID_OU_PILOTO_FILA)
            var linkNCall = string.Format("http://{0}/ncall/servicos.php?magic=1333&acao=statusFila&fila={1}",
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IPNexcoreOperadoraCelular), fila);

            RetornoStatus objRetornoStatus;
            //Get the request
            var request = WebRequest.Create(linkNCall);
            using (var response = request.GetResponse())
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var retorno = sr.ReadToEnd();
                objRetornoStatus = JsonConvert.DeserializeObject<RetornoStatus>(retorno);
            }
            return objRetornoStatus;
        }

        protected void btnReceberLigacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTelefone.Fone.Trim() == string.Empty || txtTelefone.DDD.Trim() == string.Empty)
                    return;

                //magic - não alterar
                //origem - ramal ou piloto de fila que irá atender a chamada do cliente.
                //numero - telefone com DDD + número (10 ou 11 dígitos) que o cliente irá digitar no site
                var linkNCall = string.Format("http://{0}/ncall/servicos/click2call.php?magic=1333&origem={1}&numero={2}&nome={3}",
                    Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IPNexcoreOperadoraCelular),
                    Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.PilotoDeFila_Cia),
                    txtTelefone.DDD + txtTelefone.Fone,
                    string.Format("{0}%20{1} ", txtNome.Text.Trim(), Helper.FormatarTelefone(txtTelefone.DDD.Trim(), txtTelefone.Fone.Trim())).Replace(" ", "%20"));

                //Get the request
                WebRequest.Create(linkNCall).GetResponse();

                divJanelaLigacao.Visible = false;
                divAguardarContato.Visible = true;

                //gravarLogCRM
                if (DeveGravarLogCrm)
                {
                    GravarLogCRM();
                }

                LimparCampos();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }

        public void GravarLogCRM()
        {
            Filial objFilial = null;

            if (IdFilial != null)
                objFilial = Filial.LoadObject(Convert.ToInt32(IdFilial.Value));

            var msg = string.Format("Usuário {0} solicitou ligação quando estava na tela de compra, opção planos ilimitados em {1}.", txtNome.Text.Trim(), DateTime.Now.ToString("dd/MM/yyyy"));
            MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(msg, objFilial, null, null);
        }

        protected void btnEnviarMensagem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarCamposMensagem())
                    return;

                string assuntoEmail;
                var enderecoEmail = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailContatoR1);
                var corpoEmail = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.SolicitarContatoCIA, out assuntoEmail);

                var parametrosMensagem = new
                {
                    DataEnvio = DateTime.Today.ToShortDateString(),
                    Nome = txtNomeClienteMsg.Value + " - Telefone: " + "(" + txtTelefoneClienteMsg.DDD.Trim() + ") " + txtTelefoneClienteMsg.Fone.Trim(),
                    Mensagem = txtMensagemCliente.Value
                };

                var assunto = FormatObject.ToString(parametrosMensagem, assuntoEmail);
                var mensagem = FormatObject.ToString(parametrosMensagem, corpoEmail);
                var para = enderecoEmail;
                var de = txtEmailClienteMsg.Value;

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(assunto, mensagem, BLL.Enumeradores.CartaEmail.SolicitarContatoCIA, de, para);

                divJanelaMensagem.Visible = false;
                divAguardarContatoMsg.Visible = true;

                LimparCampos();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }

        protected bool VerificarCamposMensagem()
        {
            try
            {
                if (txtNomeClienteMsg.Value.Trim() == "")
                    return false;
                if (txtTelefoneClienteMsg.DDD.Trim() == "")
                    return false;
                if (txtTelefoneClienteMsg.Fone.Trim() == "")
                    return false;
                if (txtEmailClienteMsg.Value.Trim() == "")
                    return false;
                if (txtMensagemCliente.Value.Trim() == "")
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
                return false;
            }
        }

        /// <summary>
        ///     Se tem usuário de perfil Empresa logado, preenche os campos
        /// </summary>
        protected void PreencherCampos()
        {
            try
            {
                if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    UsuarioFilial objUsuarioFilial;
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                    {
                        txtTelefone.DDD = objUsuarioFilial.NumeroDDDComercial;
                        txtTelefone.Fone = objUsuarioFilial.NumeroComercial;
                        txtTelefoneClienteMsg.DDD = objUsuarioFilial.NumeroDDDComercial;
                        txtTelefoneClienteMsg.Fone = objUsuarioFilial.NumeroComercial;
                        txtEmailClienteMsg.Value = objUsuarioFilial.EmailComercial;
                    }

                    var objPessoaFisica = new PessoaFisica(IdPessoaFisicaLogada.Value);
                    txtNomeClienteMsg.Value = objPessoaFisica.PrimeiroNome;
                    txtNome.Text = objPessoaFisica.NomePessoa;
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }

        protected void LimparCampos()
        {
            try
            {
                txtTelefone.DDD = string.Empty;
                txtTelefone.Fone = string.Empty;

                txtNomeClienteMsg.Value = string.Empty;
                txtTelefoneClienteMsg.DDD = string.Empty;
                txtTelefoneClienteMsg.Fone = string.Empty;
                txtEmailClienteMsg.Value = string.Empty;
                txtMensagemCliente.Value = string.Empty;
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }

        public class RetornoStatus
        {
            public int ret { get; set; }
            public int idfila { get; set; }
            public string nomefila { get; set; }
            public int deslogado { get; set; }
            public int disponivel { get; set; }
            public int ocupado { get; set; }
        }
    }
}