using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Curriculo = BNE.BLL.Curriculo;
using Filial = BNE.BLL.Filial;

namespace BNE.Web.UserControls.Modais
{
    public partial class EnvioDeMensagem : BaseUserControl
    {

        private const string SeparadorUsuario = "|.|";
        private const string SeparadorMensagem = ";.;";

        #region Propriedades

        #region Curriculos - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o DictionaryCurriculo a key é o IdfCurriruclo e o bool indica se o currículo é vip ou não.
        /// </summary>
        public List<int> Curriculos
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Variavel2.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region AvisoMensagemCurta - Variável 4
        /// <summary>
        /// Propriedade contadora
        /// </summary>         
        private bool AvisoMensagemCurta
        {
            get
            {
                return (bool)ViewState[Chave.Temporaria.Variavel4.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
            }
        }
        #endregion

        #region FilialAcabouVisualizacao - Variavel 6
        public bool FilialAcabouVisualizacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] == null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), false);

                return (bool)ViewState[Chave.Temporaria.Variavel6.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
            }
        }
        #endregion FilialAcabouVisualizacao

        #region FilialAcabouSaldoSMS - Variavel 8
        public bool FilialAcabouSaldoSMS
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] == null)
                    ViewState.Add(Chave.Temporaria.Variavel8.ToString(), false);

                return (bool)ViewState[Chave.Temporaria.Variavel8.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel8.ToString(), value);
            }
        }
        #endregion FilialAcabouSaldoSMS

        #endregion

        #region Eventos

        #region btnEnviarMensagem_Click
        protected void btnEnviarMensagem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSMS.Text.Length.Equals(0) && ckbSMS.Checked)
                {
                    base.ExibirMensagem(MensagemAviso._103904, TipoMensagem.Aviso);
                }
                else if (txtSMS.Text.Length <= 5 && AvisoMensagemCurta && ckbSMS.Checked)
                {
                    base.ExibirMensagem(MensagemAviso._103903, TipoMensagem.Aviso);
                    AvisoMensagemCurta = false;
                }
                else
                {
                    int quantidadeSMSEnviada, quantidadeEmailEnviado;
                    EnviarMensagem(out quantidadeSMSEnviada, out quantidadeEmailEnviado);

                    if (ckbEmail.Checked)
                    {
                        var emailmassa = RecuperarCookieChameFacil("emailmassa");
                        GravarCookieChameFacil("emailmassa", AdicionarInformacoesCookie(emailmassa, txtEmail.Text, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value)));
                    }

                    if (ckbSMS.Checked)
                    {
                        var smsEmMassa = RecuperarCookieChameFacil("smsmassa");
                        GravarCookieChameFacil("smsmassa", AdicionarInformacoesCookie(smsEmMassa, txtSMS.Text, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value)));
                    }

                    // se acabou o saldo de visualizacao, mostra modal de compra CIA
                    if (ckbSMS.Checked && FilialAcabouSaldoSMS)
                        base.ExibirMensagem("O seu limite para envio de SMS foi excedido.", TipoMensagem.Aviso);
                    else if (EnviarConfirmacao != null)
                    {
                        if (ckbSMS.Checked)
                            EnviarConfirmacao("Confirmação", string.Format("Mensagem encaminhada para {0} candidato(s)", quantidadeSMSEnviada), false);
                        else
                            EnviarConfirmacao("Confirmação", "Mensagem encaminhada com sucesso!", false);
                    }

                    FecharModal();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region Delegates
        public delegate void delegateEnviarConfirmacao(String titulo, String mensagem, bool cliqueAqui);
        public event delegateEnviarConfirmacao EnviarConfirmacao;
        #endregion

        #endregion

        #region Métodos

        #region InicializarComponentes
        /// <summary>
        /// Método abre a tela de EnvioMensagem
        /// </summary>
        public void InicializarComponentes()
        {
            AvisoMensagemCurta = true;

            LimparCampos();

            var smsEmMassa = RecuperarCookieChameFacil("smsmassa");
            var emailEmMassa = RecuperarCookieChameFacil("emailmassa");

            var listaMensagemSMS = RecuperarInformacoesCookie(smsEmMassa, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value));
            var listaMensagemEmail = RecuperarInformacoesCookie(emailEmMassa, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value));

            if (listaMensagemSMS.Count > 0)
                txtSMS.Text = listaMensagemSMS[0];

            if (listaMensagemEmail.Count > 0)
                txtEmail.Text = listaMensagemEmail[0];

            AjustarSugestao(listaMensagemSMS);

            mpeEnvioMensagem.Show();
            upMensagemSMS.Update();
            upMensagemEmail.Update();
        }
        #endregion

        #region AjustarSugestao
        private void AjustarSugestao(List<string> historicoMensagem)
        {
            UsuarioFilial objUsuarioFilial;
            if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
            {
                objUsuarioFilial.UsuarioFilialPerfil.CompleteObject();

                var parametros = new
                {
                    telefone = String.Format("({0}) {1}", objUsuarioFilial.NumeroDDDComercial, objUsuarioFilial.NumeroComercial.Trim()),
                    selecionador = objUsuarioFilial.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome
                };

                var listaSugest = new List<string>();
                if (historicoMensagem.Count(hm => !string.IsNullOrWhiteSpace(hm)) == 0 || historicoMensagem.Count(hm => !string.IsNullOrWhiteSpace(hm)) == 1)
                {
                    listaSugest.Add(parametros.ToString(ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ModeloSMSCampanhaMassa1)));
                    listaSugest.Add(parametros.ToString(ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ModeloSMSCampanhaMassa2)));
                    listaSugest.Add(parametros.ToString(ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ModeloSMSCampanhaMassa3)));

                }
                else if (historicoMensagem.Count(hm => !string.IsNullOrWhiteSpace(hm)) == 2)
                {
                    listaSugest.Add(historicoMensagem[1]);
                    listaSugest.Add(parametros.ToString(ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ModeloSMSCampanhaMassa1)));
                    listaSugest.Add(parametros.ToString(ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ModeloSMSCampanhaMassa2)));
                }
                else if (historicoMensagem.Count(hm => !string.IsNullOrWhiteSpace(hm)) == 3)
                {
                    listaSugest.Add(historicoMensagem[1]);
                    listaSugest.Add(historicoMensagem[2]);
                    listaSugest.Add(parametros.ToString(ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ModeloSMSCampanhaMassa1)));
                }
                else
                {
                    listaSugest.Add(historicoMensagem[1]);
                    listaSugest.Add(historicoMensagem[2]);
                    listaSugest.Add(historicoMensagem[3]);
                }

                suggest1.Text = listaSugest[0];
                suggest2.Text = listaSugest[1];
                suggest3.Text = listaSugest[2];
            }
        }
        #endregion

        #region RecuperarInformacoesCookie
        private List<string> RecuperarInformacoesCookie(string valorCookie, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            var listaValores = Regex.Split(valorCookie, SeparadorMensagem).ToList();

            var mensagens = new List<Tuple<int, string>>();
            foreach (var mensagem in listaValores)
            {
                if (mensagem.IndexOf(SeparadorUsuario, StringComparison.Ordinal) > 0)
                {
                    int usuario;
                    if (Int32.TryParse(mensagem.Substring(0, mensagem.IndexOf(SeparadorUsuario, StringComparison.Ordinal)), out usuario))
                    {
                        if (usuario == objUsuarioFilialPerfil.IdUsuarioFilialPerfil) //Se for do mesmo usuário atual
                        {
                            mensagens.Add(new Tuple<int, string>(usuario, mensagem.Substring(mensagem.IndexOf(SeparadorUsuario, StringComparison.Ordinal) + SeparadorUsuario.Length)));
                        }
                    }
                }
            }

            const int limit = 5;

            return mensagens.Distinct(m => m.Item2).Select(n => n.Item2).Take(limit).ToList();
        }
        #endregion

        #region AdicionarInformacoesCookie
        private string AdicionarInformacoesCookie(string informacaoAntiga, string informacaoAdicionar, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            return objUsuarioFilialPerfil.IdUsuarioFilialPerfil + SeparadorUsuario + informacaoAdicionar + SeparadorMensagem + informacaoAntiga;
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeEnvioMensagem.Hide();
        }
        #endregion

        #region LimparCampos
        public void LimparCampos()
        {
            txtSMS.Text = String.Empty;
            txtEmail.Text = String.Empty;
            ckbSMS.Checked = true;
            ckbEmail.Checked = true;
        }
        #endregion

        #region EnviarMensagem
        /// <summary>
        /// Método responsável por enviar mensagem
        /// </summary>
        public void EnviarMensagem(out int quantidadeSMSEnviada, out int quantidadeEmailEnviado)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            var objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

            string mensagemErro;

            if (!MensagemCS.EnviarChameFacil(objUsuarioFilialPerfil, objFilial, Destinatarios(), txtSMS.Text + " www.bne.com.br", txtEmail.Text + " www.bne.com.br", ckbSMS.Checked, ckbEmail.Checked, out mensagemErro, out quantidadeSMSEnviada, out quantidadeEmailEnviado))
                ExibirMensagem(mensagemErro, TipoMensagem.Erro);
        }
        #endregion

        #region ListaDestinatarios
        private List<Curriculo> Destinatarios()
        {
            return Curriculos.Select(curriculo => new Curriculo(curriculo)).ToList();
        }
        #endregion

        #endregion

    }

}