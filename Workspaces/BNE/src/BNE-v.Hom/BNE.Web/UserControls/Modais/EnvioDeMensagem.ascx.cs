using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.EnvioMensagens;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BNE.BLL.Common;
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
                    return;
                }

                if (txtSMS.Text.Length <= 5 && AvisoMensagemCurta && ckbSMS.Checked)
                {
                    base.ExibirMensagem(MensagemAviso._103903, TipoMensagem.Aviso);
                    AvisoMensagemCurta = false;
                    return;
                }
                if (cbAgendar.Checked)
                {
                    try 
	                {	        
		                var Horario = TimeSpan.Parse(txtHoraEnvio.Text);
                        TimeSpan HoraMinima = TimeSpan.Parse("08:00");
                        TimeSpan HoraMaxima = TimeSpan.Parse("20:00");
                        var data = Convert.ToDateTime(string.Format("{0} {1}", txtAgendamento.Text, txtHoraEnvio.Text));
                    if(data < DateTime.Now){
                        base.ExibirMensagem("Agendamento a Data futura",TipoMensagem.Aviso);
                        return;
                    }
                    else if (Horario < HoraMinima || Horario > HoraMaxima)
                    {
                        base.ExibirMensagem(MensagemAviso._505707, TipoMensagem.Aviso);
                        return;
                    }
	                }
	                catch (Exception)
	                {
		                    base.ExibirMensagem("Preencher corretamente a data e hora do agendamento",TipoMensagem.Aviso);
                            return;
	                }
                   
                }
                string mensagemObservacoes;
                bool retornoEnvioMensagem;
                retornoEnvioMensagem = EnviarMensagem(out mensagemObservacoes);

                if (!retornoEnvioMensagem)
                {
                    FecharModal();
                    return;
                }

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
                    {
                        //Tratar o número de SMS enviado. Tratar quando existirem observações.
                        if (string.IsNullOrEmpty(mensagemObservacoes))
                            EnviarConfirmacao("Confirmação", "Mensagem encaminhada com sucesso para o(s) candidato(s);", false);
                        else
                            EnviarConfirmacao("Confirmação", mensagemObservacoes, false);
                    }
                    else
                        EnviarConfirmacao("Confirmação", "Mensagem encaminhada com sucesso!", false);
                }

                FecharModal();
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

            if (CelularSelecionador.VerificaCelularEstaLiberadoParaTanque(base.IdUsuarioFilialPerfilLogadoEmpresa.Value))
                litSaldoSMS.Text = string.Format("<span class=\"badge\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Você tem disponível {0} SMS.\"><span>Você tem disponível {0} SMS.</span></span>", CelularSelecionador.RecuperarCotaDisponivel(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, base.IdFilial.Value));
            else
                litSaldoSMS.Text = string.Format("<span class=\"badge\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Você tem disponível {0} SMS.\"><span>Você tem disponível {0} SMS.</span></span>", new Filial(base.IdFilial.Value).SaldoSMS());

            mpeEnvioMensagem.Show();
            upMensagemSMS.Update();
            upMensagemEmail.Update();
            upSaldoSMS.Update();
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
            txtSMS.Text = txtAgendamento.Text = txtHoraEnvio.Text = String.Empty;

            txtEmail.Text = String.Empty;
            ckbSMS.Checked = true;
            ckbEmail.Checked = true;

        }
        #endregion

        #region EnviarMensagem
        /// <summary>
        /// Método responsável por enviar mensagem
        /// </summary>
        public bool EnviarMensagem(out string mensagemObservacoes)
        {

            var objFilial = new Filial(base.IdFilial.Value);
            var objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
            DateTime? dtaAgendamento = null;
            try
            {
                if (cbAgendar.Checked && ckbSMS.Checked)
                    dtaAgendamento = Convert.ToDateTime(String.Format("{0} {1}", txtAgendamento.Text, txtHoraEnvio.Text));

            }
            catch (Exception ex )
            {
                base.ExibirMensagem("Data Inválida", TipoMensagem.Aviso);
            }
          
            //Se apenas um CV foi selecionado, realiza o envio na hora. Caso mais de um tenha sido selecionado, envia pelo processo assincrono
            if (Curriculos.Count > 1)
            {
                EnvioMensagens.CriarFilaProcessamentoMensagens(objUsuarioFilialPerfil, objFilial, Curriculos, txtSMS.Text + " www.bne.com.br", txtEmail.Text + " www.bne.com.br", ckbSMS.Checked, ckbEmail.Checked, dtaAgendamento);
                mensagemObservacoes = "Campanha de envio de mensagens disparada. Verifique o processamento no seu dashboard.";
                return true;
            }
            else
            {

                bool retornoEnvioMensagem;

                //Criar Objeto da Campanha
                CampanhaMensagem objCampanhaMensagem = new CampanhaMensagem();
                objCampanhaMensagem.DataDisparo = DateTime.Now;
                objCampanhaMensagem.FlagEnviaEmail = ckbEmail.Checked;
                objCampanhaMensagem.FlagEnviaSMS = ckbSMS.Checked;

                if (ckbEmail.Checked)
                    objCampanhaMensagem.DescricaomensagemEmail = txtEmail.Text + " www.bne.com.br";
                if (ckbSMS.Checked)
                    objCampanhaMensagem.DescricaomensagemSMS = txtSMS.Text + " www.bne.com.br";

                objCampanhaMensagem.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objCampanhaMensagem.Save();

                //Cria registro do curriculo para envio
                CampanhaMensagemEnvios objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
                objCampanhaMensagemEnvios.Curriculo = new Curriculo(Curriculos[0]);
                objCampanhaMensagemEnvios.CampanhaMensagem = objCampanhaMensagem;
                objCampanhaMensagemEnvios.DataAgendamento = dtaAgendamento;
                objCampanhaMensagemEnvios.Save();

                retornoEnvioMensagem = EnvioMensagens.EnviarMensagemCV(objCampanhaMensagem, new List<CampanhaMensagemEnvios> { objCampanhaMensagemEnvios }, out mensagemObservacoes);

                if (!retornoEnvioMensagem)
                    ExibirMensagem(mensagemObservacoes, TipoMensagem.Erro);

                return retornoEnvioMensagem;

            }
        }
        #endregion

        #endregion

    }

}