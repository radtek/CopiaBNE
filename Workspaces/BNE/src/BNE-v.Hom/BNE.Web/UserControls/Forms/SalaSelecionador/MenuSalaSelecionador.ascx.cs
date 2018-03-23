using System;
using System.Globalization;
using BNE.BLL;
using BNE.Web.Code.Session;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System.Collections.Generic;
using System.Linq;

namespace BNE.Web.UserControls.Forms.SalaSelecionador
{
    public partial class MenuSalaSelecionador : BaseUserControl
    {

        #region Propriedades

        #region PossuiPlanoAdquirido

        public bool PossuiPlanoAdquirido
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }

        #endregion

        #region IdPlanoAdquirido

        public int IdPlanoAdquirido
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }

        #endregion

        #endregion

        #region Eventos

        #region Delegates 

        public delegate bool DelegateVerificarEmpresaAuditoria();
        public event DelegateVerificarEmpresaAuditoria eventVerificarEmpresaAuditoria;

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }

        #endregion

        #region btlMinhasVagas_Click
        /// <summary>
        /// Evento do botão Minhas Vagas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btlMinhasVagas_Click(object sender, EventArgs e)
        {
            if (eventVerificarEmpresaAuditoria != null)
            {
                if (eventVerificarEmpresaAuditoria())
                {
                    Redirect("SalaSelecionadorVagasAnunciadas.aspx");
                }
            }
        }
        #endregion

        #region btlRastreadorCV_Click
        protected void btlRastreadorCV_Click(object sender, EventArgs e)
        {
            if (eventVerificarEmpresaAuditoria != null)
            {
                if (eventVerificarEmpresaAuditoria())
                {
                    Redirect("SalaSelecionadorRastreadorCV.aspx");
                }
            }
        }
        #endregion

        #region btlMensagensEnviadas_Click
        protected void btlMensagensEnviadas_Click(object sender, EventArgs e)
        {
            if (eventVerificarEmpresaAuditoria != null)
            {
                if (eventVerificarEmpresaAuditoria())
                {
                    Redirect("SalaSelecionadorMensagens.aspx");
                }
            }
        }
        #endregion

        #region btlRequisicaoR1_Click
        protected void btlRequisicaoR1_Click(object sender, EventArgs e)
        {
            if (eventVerificarEmpresaAuditoria != null)
            {
                if (eventVerificarEmpresaAuditoria())
                {
                    Redirect("SolicitacaoR1.aspx");
                }
            }
        }
        #endregion

        #region btlCompraPlanos_Click
        protected void btlCompraPlanos_Click(object sender, EventArgs e)
        {
            if (eventVerificarEmpresaAuditoria != null)
            {
                if (eventVerificarEmpresaAuditoria())
                {
                    if (PossuiPlanoAdquirido)
                    {
                        UserSession.UrlDestino.Value = "SalaSelecionador.aspx";
                        Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPlanoAdquirido);
                        Redirect("SalaSelecionadorPlanoIlimitado.aspx");
                    }
                    else
                    {
                        UserSession.UrlDestino.Value = "SalaSelecionador.aspx";
                        Redirect("SalaSelecionadorPlanoIlimitado.aspx");
                    }
                }
            }
        }
        #endregion

        #region btlMeuCadastro_Click
        protected void btlMeuCadastro_Click(object sender, EventArgs e)
        {
            Redirect("CadastroEmpresaDados.aspx");
        }
        #endregion

        #region btlSalaVip_Click
        protected void btlSalaVip_Click(object sender, EventArgs e)
        {
            Redirect("SalaVip.aspx");
        }
        #endregion

        #region btlFalePresidente_Click
        protected void btlFalePresidente_Click(object sender, EventArgs e)
        {
            Redirect("FalePresidente.aspx");
        }
        #endregion

        #region btlFalePresidente_Click
        protected void btlPesquisaAvancada_OnClick(object sender, EventArgs e)
        {
            if (eventVerificarEmpresaAuditoria != null)
            {
                if (eventVerificarEmpresaAuditoria())
                {
                    Redirect("PesquisaCurriculoAvancada.aspx");
                }
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        private void Inicializar()
        {
            CarregarBoxVagas();
            CarregarBoxMeuPlano();
            CarregarBoxRastreador();
            CarregarBoxMensagens();
            CarregarBoxR1();
        }
        #endregion

        #region CarregarBoxVagas
        public void CarregarBoxVagas()
        {
            #region Vagas

            int quantidadeVagasFilial = Filial.RecuperarQuantidadeVagasAnunciadas(UserSession.IdFilial.Value);

            if (quantidadeVagasFilial > 0)
            {
                lblVagasAnunciadasQuantidade.Text = quantidadeVagasFilial.ToString(CultureInfo.CurrentCulture);

                if (quantidadeVagasFilial.Equals(1))
                    lblVagasAnunciadasMensagem.Text = " Vaga Anunciada";
                else
                    lblVagasAnunciadasMensagem.Text = " Vagas Anunciadas";
            }
            else
                lblVagasAnunciadasMensagem.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxVagaSalaSelecionadorInicial);

            #endregion

            #region Curriculos

            int quantidadeCurriculosCandidatados = VagaCandidato.CandidatoVagaPorFilial(UserSession.IdFilial.Value);

            if (quantidadeCurriculosCandidatados > 0)
            {
                lblCurriculosRecebidosQuantidade.Text = quantidadeCurriculosCandidatados.ToString();

                if (quantidadeCurriculosCandidatados.Equals(1))
                    lblCurriculosRecebidosMensagem.Text = " CV Recebido";
                else
                    lblCurriculosRecebidosMensagem.Text = " novos CV's Recebidos";

            }

            #endregion
        }
        #endregion

        #region CarregarBoxMeuPlano
        public void CarregarBoxMeuPlano()
        {
            //TODO: Implementar isso na classe. Estag foca.
            // Carrega lista de planos liberados ou em aberto do usuario.
            List<PlanoAdquirido> lstPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(null, UserSession.IdFilial.HasValue ? UserSession.IdFilial.Value : (int?)null);

            // Seleciona se existir o plano liberado vigente ou o plano liberado que sera iniciado.
            PlanoAdquirido objPlanoAdquirido = lstPlanoAdquirido.Where(p => p.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado && p.DataFimPlano >= DateTime.Now).OrderBy(p => p.DataInicioPlano).FirstOrDefault();                                                

            if (objPlanoAdquirido != null)
            {
                PossuiPlanoAdquirido = true;
                objPlanoAdquirido.Plano.CompleteObject();

                lblPlanoAcessoValidade.Text = string.Format("Plano {0} válido até {1}", objPlanoAdquirido.Plano.DescricaoPlano, objPlanoAdquirido.DataFimPlano.ToShortDateString());

                IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;

                PlanoQuantidade objPlanoQuantidade;
                if (PlanoQuantidade.CarregarPorPlano(objPlanoAdquirido.IdPlanoAdquirido, out objPlanoQuantidade))
                {
                    if (objPlanoQuantidade.QuantidadeSMSUtilizado.Equals(0))
                        lblQuantidadeSMSUtilizado.Text = "0";
                    else
                        lblQuantidadeSMSUtilizado.Text = objPlanoQuantidade.QuantidadeSMSUtilizado.ToString(CultureInfo.CurrentCulture);

                    lblSMSUtilizadoMensagem.Text = " SMS Utilizados";
                }
            }
            else
            {
                PossuiPlanoAdquirido = false;
                lblPlanoAcessoValidade.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxMeuPlanoSalaSelecionadorInicial);
            }
        }
        #endregion

        #region CarregarBoxRastreador
        public void CarregarBoxRastreador()
        {
            int quantidadeRastreadoresProgramados = Rastreador.QuantidadeRastreadoresProgramadosPorEmpresa(UserSession.IdFilial.Value);

            if (quantidadeRastreadoresProgramados > 0)
            {
                lblQuantidadePerfisProgramados.Text = quantidadeRastreadoresProgramados.ToString();

                if (quantidadeRastreadoresProgramados.Equals(1))
                    lblPerfisProgramadosMensagem.Text = " Perfil Programado";
                else
                    lblPerfisProgramadosMensagem.Text = " Perfis Programados";


                int quantidadeCvsEncontrados = Rastreador.QuantidadeCvsEncontradosRastreador(UserSession.IdFilial.Value);

                if (quantidadeCvsEncontrados > 0)
                {
                    lblQuantidadeCvsRecebidosRastreador.Text = quantidadeCvsEncontrados.ToString();

                    if (quantidadeCvsEncontrados.Equals(1))
                        lblCvsRecebidosRastreadorMensagem.Text = " Cv Recebido";
                    else
                        lblCvsRecebidosRastreadorMensagem.Text = " Novos Cv´s Recebidos";
                }
            }
            else
                lblPerfisProgramadosMensagem.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxRastreadorSalaSelecionadorInicial);
        }
        #endregion

        #region CarregarBoxMensagens
        public void CarregarBoxMensagens()
        {
            int quantidadeMensagensRecibidas = MensagemCS.QuantidadeMensagensRecebidas(UserSession.IdUsuarioFilialPerfilLogadoEmpresa.Value);

            lblMensagemPossuiMensagem.Text = "Você possui ";

            lblQuantidadeMensagens.Text = quantidadeMensagensRecibidas.ToString(CultureInfo.CurrentCulture);

            if (quantidadeMensagensRecibidas.Equals(1))
                lblTextoMensagem.Text = "mensagem recebida";
            else
                lblTextoMensagem.Text = "mensagens recebidas";
        }
        #endregion

        #region CarregarBoxR1
        public void CarregarBoxR1()
        {
            lblSolicitacaoMensagem.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxR1SalaSelecionadorInicial);
        }
        #endregion

        #endregion

    }
}