using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Custom.IntegrationObjects;
using BNE.Web.Code;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucWebCallBack : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LimparCampos();
                PreencherCampos();
                DefineModalParaExibir();
            }
        }

        protected void DefineModalParaExibir()
        {
            try
            {
                var objRetornoStatusCIA = RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Cia));

                if (objRetornoStatusCIA != null && objRetornoStatusCIA.disponivel > 0)
                    modalzinha.Attributes.Add("data-target", "#myModalComercial");
                else
                {
                    var objRetornoStatusAtendimento = RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Atendimento));
                    if (objRetornoStatusAtendimento != null && objRetornoStatusAtendimento.disponivel > 0)
                        modalzinha.Attributes.Add("data-target", "#myModalComercial");
                    else
                        modalzinha.Attributes.Add("data-target", "#myModalMensagem");
                }

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        private RetornoStatus RetornarStatus(string fila)
        {
            //URL para verificar status da Fila (fila=ID_OU_PILOTO_FILA)
            string linkNCall = string.Format("http://{0}/ncall/servicos.php?magic=1333&acao=statusFila&fila={1}",
                Parametro.RecuperaValorParametro(Enumeradores.Parametro.IPNexcoreOperadoraCelular), fila);

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
                string linkNCall = string.Format("http://{0}/ncall/servicos/click2call.php?magic=1333&origem={1}&numero={2}",
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.IPNexcoreOperadoraCelular),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Cia),
                    txtTelefone.DDD + txtTelefone.Fone);

                //Get the request
                WebRequest.Create(linkNCall).GetResponse();

                divJanelaLigacao.Visible = false;
                divAguardarContato.Visible = true;

                LimparCampos();
            }

            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        protected void btnEnviarMensagem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarCamposMensagem())
                    return;

                string assuntoEmail;
                string enderecoEmail = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailContatoR1);
                string corpoEmail = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.SolicitarContatoCIA, out assuntoEmail);

                var parametrosMensagem = new
                {
                    DataEnvio = DateTime.Today.ToShortDateString(),
                    Nome = txtNomeClienteMsg.Value + " - Telefone: " + "(" + txtTelefoneClienteMsg.DDD.Trim() + ") " + txtTelefoneClienteMsg.Fone.Trim(),
                    Mensagem = txtMensagemCliente.Value
                };

                string assunto = parametrosMensagem.ToString(assuntoEmail);
                string mensagem = parametrosMensagem.ToString(corpoEmail);
                string para = enderecoEmail;
                string de = txtEmailClienteMsg.Value;

                EmailSenderFactory
                       .Create(TipoEnviadorEmail.Fila)
                       .Enviar(assunto, mensagem, de, para);

                divJanelaMensagem.Visible = false;
                divAguardarContatoMsg.Visible = true;

                LimparCampos();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
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
                EL.GerenciadorException.GravarExcecao(ex);
                return false;
            }
        }

        /// <summary>
        /// Se tem usuário de perfil Empresa logado, preenche os campos
        /// </summary>
        protected void PreencherCampos()
        {
            try
            {
                if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    UsuarioFilial objUsuarioFilial;
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                    {
                        txtTelefone.DDD = objUsuarioFilial.NumeroDDDComercial;
                        txtTelefone.Fone = objUsuarioFilial.NumeroComercial;
                        txtTelefoneClienteMsg.DDD = objUsuarioFilial.NumeroDDDComercial;
                        txtTelefoneClienteMsg.Fone = objUsuarioFilial.NumeroComercial;
                        txtEmailClienteMsg.Value = objUsuarioFilial.EmailComercial;
                    }

                    var objPessoaFisica = new PessoaFisica(base.IdPessoaFisicaLogada.Value);
                    txtNomeClienteMsg.Value = objPessoaFisica.PrimeiroNome;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
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
                EL.GerenciadorException.GravarExcecao(ex);
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