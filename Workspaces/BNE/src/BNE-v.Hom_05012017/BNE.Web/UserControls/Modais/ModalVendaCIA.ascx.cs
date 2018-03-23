using System;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Assincronos;
using BNE.BLL;
using System.Web.UI;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using System.Text;
using FormatObject = BNE.BLL.Common.FormatObject;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalVendaCIA : BaseUserControl
    {

        private string MensagemDesconto15 = @"Usuário {0} recebeu desconto de {1}% para reativar assinatura no BNE em {2} após não conseguir acessar cv por falta de plano";

        #region btlQueroComprarPlano_Click
        protected void btlQueroComprarPlano_Click(object sender, EventArgs e)
        {
            RegistrarFilialObservacao();

            //setar o id do plano Experimente na session
            Session.Add(BNE.Web.Code.Enumeradores.Chave.Temporaria.IdPlano.ToString(), 599);
            
            string rota = GetRouteUrl(Enumeradores.RouteCollection.EscolhaPlanoCIA.ToString(), null);
            Redirect(rota);
        }
        #endregion

        #region Inicializar
        public void Inicializar(int idFilial, string nomeResponsavel, BLL.Enumeradores.Parametro desconto, string vendedor)
        {
            lblNomeReponsavel.Text = string.Format("<p class=\"tit_modal_vip\">{0},</p>", nomeResponsavel);
            hddIdFilial.Value = idFilial.ToString();
            hddDesconto.Value = Parametro.RecuperaValorParametro(desconto);
            hddNomeCliente.Value = nomeResponsavel;
            lblVendedor.Text = vendedor;
            
            switch (desconto)
            {
                case BLL.Enumeradores.Parametro.DescontoExperimenteAgora:
                    lblDescontoExperimente.Text = Parametro.RecuperaValorParametro(desconto);
                    pnDesconto40.Visible = true;
                    pnDesconto15.Visible = false;
                    pnDesconto3.Visible = false;
                    pnLigueMe.Visible = false;
                    break;
                case BLL.Enumeradores.Parametro.DescontoTempoPlanoMaiorQueParametro:
                    lblDescontoTempoMaior.Text = Parametro.RecuperaValorParametro(desconto);
                    pnDesconto40.Visible = false;
                    pnDesconto15.Visible = true;
                    pnDesconto3.Visible = false;
                    pnLigueMe.Visible = false;
                break;
                default:
                    pnDesconto40.Visible = false;
                    pnDesconto15.Visible = false;
                    pnLigueMe.Visible = false;
                    pnDesconto3.Visible = true;
                    break;
            }

            
            mpeVendaCIA.Show();
            upNomeReponsavel.Update();
        }
        #endregion

        #region btiFechar_Click
        protected void btiLigarAgora_Click(object sender, EventArgs e)
        {
            RegistrarFilialObservacao();
            InicializarLigarAgora();
            LimparCampos();
            lblNomeReponsavel.Visible = false;
            PreencherCampos();
            pnDesconto15.Visible = false;
            pnDesconto3.Visible = false;
            pnLigueMe.Visible = true;
        }
        #endregion

        #region [ Registrar FilialObservacao]
        public void RegistrarFilialObservacao()
        {
            string msg = string.Format(MensagemDesconto15, hddNomeCliente.Value, hddDesconto.Value, DateTime.Now.ToString("dd/MM/yyyy"));
            Filial objFilial = BLL.Filial.LoadObject(Convert.ToInt32(hddIdFilial.Value));
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(base.IdPessoaFisicaLogada.Value, objFilial.IdFilial, out objUsuarioFilialPerfil);
            MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(msg, objFilial, objUsuarioFilialPerfil, null);

            if(hddDesconto.Value == "3")
            {
                string from = BNE.BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, null);
                string to = objFilial.Vendedor().EmailVendedor;
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("O Cliente {0}, ficou travado na tela de Viualização de Currículo por falta de plano.", hddNomeCliente.Value);
                sb.AppendFormat("<p>Nome da Empresa: {0}</p>", objFilial.RazaoSocial);
                sb.AppendFormat("<p>CNPJ: {0}</p>", objFilial.CNPJ);

                string Subject = string.Format("Cliente {0} ficou travado na tela Visualização de Currículo", hddNomeCliente.Value);

                //Enviar Email para o Vendedor
                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(Subject, sb.ToString(), null, from, to);
            }
        }
        #endregion

        #region InicializarLigarAgora
        public void InicializarLigarAgora()
        {
            var objWebCallBack_Dependencia = new ucWebCallBack_Modais();
            var objRetornoStatusCIA = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Cia));

            if (objRetornoStatusCIA != null && objRetornoStatusCIA.disponivel > 0)
            {
                lnkLigueAgora3.Attributes.Add("data-target", "#myModalComercial");
                lnkLigarAgora.Attributes.Add("data-target", "#myModalComercial");
                upMyModalComercial.Visible = true;
                upMyModalMensagem.Visible = false;
            }
            else
            {
                var objRetornoStatusAtendimento = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Atendimento));
                if (objRetornoStatusAtendimento != null && objRetornoStatusAtendimento.disponivel > 0)
                {
                    upMyModalComercial.Visible = true;
                    upMyModalMensagem.Visible = false;
                    lnkLigueAgora3.Attributes.Add("data-target", "#myModalComercial");
                    lnkLigarAgora.Attributes.Add("data-target", "#myModalComercial");
                }
                else
                {
                    upMyModalComercial.Visible = false;
                    upMyModalMensagem.Visible = true;
                    lnkLigueAgora3.Attributes.Add("data-target", "#myModalMensagem");
                    lnkLigarAgora.Attributes.Add("data-target", "#myModalMensagem");
                }
            }
        }
        #endregion

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
                    txtNome.Text = objPessoaFisica.NomePessoa;
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

        public RetornoStatus RetornarStatus(string fila)
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
                string linkNCall = string.Format("http://{0}/ncall/servicos/click2call.php?magic=1333&origem={1}&numero={2}&nome={3}",
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.IPNexcoreOperadoraCelular),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Cia),
                    txtTelefone.DDD + txtTelefone.Fone,
                    String.Format("{0}%20{1} ", txtNome.Text.Trim(), Helper.FormatarTelefone(txtTelefone.DDD.Trim(), txtTelefone.Fone.Trim())).Replace(" ", "%20"));

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

                string assunto = FormatObject.ToString(parametrosMensagem, assuntoEmail);
                string mensagem = FormatObject.ToString(parametrosMensagem, corpoEmail);
                string para = enderecoEmail;
                string de = txtEmailClienteMsg.Value;

                EmailSenderFactory
                       .Create(TipoEnviadorEmail.Fila)
                       .Enviar(assunto, mensagem, Enumeradores.CartaEmail.SolicitarContatoCIA, de, para);

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