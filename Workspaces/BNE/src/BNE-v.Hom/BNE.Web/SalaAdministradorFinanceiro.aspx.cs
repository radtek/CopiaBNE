using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;

namespace BNE.Web
{
    public partial class SalaAdministradorFinanceiro : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera as Permissoes
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucPlanoFidelidade.PlanoSelecionado += ucPlanoFidelidade_PlanoSelecionado;
            ucDetalhesPlano.Voltar += ucDetalhesPlano_Voltar;

        }

      
        void ucDetalhesPlano_Voltar()
        {
            pnDownloadBoletoRegistrado.Visible = false;
            pnlPlanoFidelidade.Visible = true;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = false;
            pnlLiberacaoBoleto.Visible = false;
            pnlPagamentosCielo.Visible = false;
            ucPlanoFidelidade.LimparCampos();
            ucPlanoFidelidade.CarregarGrid();
            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            
        }

        void ucPlanoFidelidade_PlanoSelecionado(int idPlanoAdquirido)
        {
            ucDetalhesPlano.IdPlanoAdquirido = idPlanoAdquirido;
            ucDetalhesPlano.InicializarModal();

            pnDownloadBoletoRegistrado.Visible = false;
            pnlPlanoFidelidade.Visible = false;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = true;
            pnlLiberacaoBoleto.Visible = false;
            pnlPagamentosCielo.Visible = false;
            
            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            
        }

        public void Inicializar()
        {
            //Ajustando permissões de acesso.
            AjustarPermissoes();

            AjustarTituloTela("Financeiro");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Financeiro");
            pnlPlanoFidelidade.Visible = true;
        }

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, BLL.Enumeradores.CategoriaPermissao.Administrador);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.Administrador.AcessarTelaSalaAdministradorFinanceiro))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(Configuracao.UrlAvisoAcessoNegado);
        }
        #endregion

        protected void btlPlano_Click(object sender, EventArgs e)
        {
            pnlPlanoFidelidade.Visible = true;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = false;
            pnlLiberacaoBoleto.Visible = false;
            pnlPagamentosCielo.Visible = false;
            pnDownloadBoletoRegistrado.Visible = false;
            plnConsultarNotas.Visible = false;
            plnPagamentosPagarMe.Visible = false;

            ucPlanoFidelidade.EsconderPanelPJ(true);
            ucPlanoFidelidade.EsconderPanelPF(true);
            ucPlanoFidelidade.LimparGrid();
            ucPlanoFidelidade.LimparCamposPF();
            ucPlanoFidelidade.LimparCamposPJ();
            ucPlanoFidelidade.LimparCampoPesquisa();
            ucPlanoFidelidade.LimparCampos();
            ucPlanoFidelidade.CarregarComboTipoPlano();
            

            
            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            upDownloadBoletoRegistrado.Update();
            upConsultarNotas.Update();
            upPagamentosPagarMe.Update();
          
        }

        protected void btlTipoPlano_Click(object sender, EventArgs e)
        {
            ucTipoPlano.Inicializar();

            
            pnlPlanoFidelidade.Visible = false;
            pnlTipoPlano.Visible = true;
            pnlDetalhesPlano.Visible = false;
            pnlLiberacaoBoleto.Visible = false;
            pnlPagamentosCielo.Visible = false;
            pnDownloadBoletoRegistrado.Visible = false;
            plnConsultarNotas.Visible = false;
            plnPagamentosPagarMe.Visible = false;

            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            upDownloadBoletoRegistrado.Update();
            upConsultarNotas.Update();
            upPagamentosPagarMe.Update();

        }

        protected void btlRetornoBoleto_Click(object sender, EventArgs e)
        {
            
            pnlLiberacaoBoleto.Visible = true;
            pnlPlanoFidelidade.Visible = false;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = false;
            pnlPagamentosCielo.Visible = false;
            pnDownloadBoletoRegistrado.Visible = false;
            plnConsultarNotas.Visible = false;
            plnPagamentosPagarMe.Visible = false;

            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            upDownloadBoletoRegistrado.Update();
            upConsultarNotas.Update();
            upPagamentosPagarMe.Update();
        }

        protected void lkbPagamentosCielo_Click(object sender, EventArgs e)
        {
            
            pnlPagamentosCielo.Visible = true;
            pnlLiberacaoBoleto.Visible = false;
            pnlPlanoFidelidade.Visible = false;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = false;
            pnDownloadBoletoRegistrado.Visible = false;
            plnConsultarNotas.Visible = false;
            plnPagamentosPagarMe.Visible = false;

            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            upDownloadBoletoRegistrado.Update();
            upConsultarNotas.Update();
            upPagamentosPagarMe.Update();
        }

        protected void LkdConsultarNotas_Click(object sender, EventArgs e)
        {
            pnlPagamentosCielo.Visible = false;
            pnlLiberacaoBoleto.Visible = false;
            pnlPlanoFidelidade.Visible = false;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = false;
            pnDownloadBoletoRegistrado.Visible = false;
            plnConsultarNotas.Visible = true;
            plnPagamentosPagarMe.Visible = false;

            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            upDownloadBoletoRegistrado.Update();
            upConsultarNotas.Update();
            upPagamentosPagarMe.Update();
        }


        protected void lkbDownloadBolReg_Click(object sender, EventArgs e)
        {
            pnlPlanoFidelidade.Visible = false;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = false;
            pnlLiberacaoBoleto.Visible = false;
            pnlPagamentosCielo.Visible = false;
            pnDownloadBoletoRegistrado.Visible = true;
            plnConsultarNotas.Visible = false;
            plnPagamentosPagarMe.Visible = false;

            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            upDownloadBoletoRegistrado.Update();
            upConsultarNotas.Update();
            upPagamentosPagarMe.Update();

        }

        protected void lkbPagarMe_Click(object sender, EventArgs e)
        {
            pnlPagamentosCielo.Visible = false;
            pnlLiberacaoBoleto.Visible = false;
            pnlPlanoFidelidade.Visible = false;
            pnlTipoPlano.Visible = false;
            pnlDetalhesPlano.Visible = false;
            pnDownloadBoletoRegistrado.Visible = false;
            plnConsultarNotas.Visible = false;
            plnPagamentosPagarMe.Visible = true;

            upPlanoFidelidade.Update();
            upTipoPlano.Update();
            upDetalhesPlano.Update();
            upLiberacaoBoleto.Update();
            upPagamentosCielo.Update();
            upDownloadBoletoRegistrado.Update();
            upConsultarNotas.Update();
            upPagamentosPagarMe.Update();
        }
    }
}