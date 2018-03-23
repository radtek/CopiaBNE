using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Assincronos;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using BNE.BLL.Common;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class PlanoFidelidade : BaseUserControl
    {

        #region Propriedades

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());

                return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region IdFilial - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdFilial
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());

            }
        }
        #endregion

        #region IdPessoaFisica - Variavel 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel2.ToString());

            }
        }
        #endregion

        #region IdPlanoAdquirido - Variavel 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdPlanoAdquirido
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel3.ToString());

            }
        }
        #endregion

        #region IdPlanoAdquiridoExclusao - Variavel 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdPlanoAdquiridoExclusao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel4.ToString());

            }
        }
        #endregion

        #region DT - Variavel 5
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected DataTable DT
        {
            get
            {
                return (DataTable)(ViewState[Chave.Temporaria.Variavel5.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }
        #endregion

        #region IdPlano
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        protected int? IdPlano
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlano.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlano.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlano.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlano.ToString(), value);
            }
        }
        #endregion

        #region ValorPlanoSMS - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public decimal? ValorPlanoSMS
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Decimal.Parse(ViewState[Chave.Temporaria.Variavel6.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #region ValorQuantidadeSMS - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera a quantidade de sms
        /// </summary>
        public int? ValorQuantidadeSMS
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel7.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel7.ToString());
            }
        }
        #endregion

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

        public decimal ValorTotalPlano
        {
            get
            {
                return Convert.ToDecimal(ViewState[Chave.Permanente.ValorTotalPlano.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Permanente.ValorTotalPlano.ToString(), value);
            }

        }

        #endregion

        #region Métodos

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros = 0;
            if (IdPessoaFisica.HasValue)
                UIHelper.CarregarRadGrid(gvPlanos, PlanoAdquirido.CarregarTodosPlanosPessoaFisica(IdPessoaFisica.Value, PageIndex, gvPlanos.PageSize, out totalRegistros), totalRegistros);
            else if (IdFilial.HasValue)
                UIHelper.CarregarRadGrid(gvPlanos, PlanoAdquirido.CarregarTodosPlanosPessoaJuridica(IdFilial.Value, PageIndex, gvPlanos.PageSize, out totalRegistros), totalRegistros);
            else
                UIHelper.CarregarRadGrid(gvPlanos, new DataTable(), totalRegistros);

            upPnlPlanos.Update();
        }
        #endregion

        #region CarregarOpcoesModal
        private void CarregarOpcoesModal()
        {
            UIHelper.CarregarRadGrid(gvSelecionarCliente, DT);
            upPnlSelecionarCliente.Update();
            mpeSelecionarCliente.Show();
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            CarregarEAjustarPermissoes();

            UIHelper.CarregarRadComboBox(rcbSituacaoEmpresa, SituacaoFilial.Listar());

            PageIndex = 1;
            gvPlanos.PageSize = 6;
            gvSelecionarCliente.PageSize = 5;

            gvLiberarPlanoAdicional.CurrentPageIndex = 0;
            gvLiberarPlanoAdicional.PageSize = 5;

            CarregarGrid();
            LimparCampos();

            //Ajustar datas
            txtDataEnvioBoleto.DataMinima = DateTime.Today;
            txtDataVencimentoBoleto.DataMinima = DateTime.Today;
            txtDataNFAntecipada.DataMinima = DateTime.Today.AddDays(1);//Pois já pode ter passado o horário de execução do processo.
        }
        #endregion

        #region CarregarEAjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void CarregarEAjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, Enumeradores.CategoriaPermissao.Administrador);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.AcessarTelaSalaAdministradorFinanceiro))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroSalvarPlano))
                {
                    btnSalvar.Enabled = false;
                    btnSalvar.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroAtualizarPlano))
                {
                    btnAtualizarPlano.Enabled = false;
                    btnAtualizarPlano.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroLiberarPlano))
                {
                    btnLiberarPlano.Enabled = false;
                    btnLiberarPlano.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroLiberarCliente))
                {
                    btnLiberarCliente.Enabled = false;
                    btnLiberarCliente.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroVisualizarInformacoes))
                {
                    btnVisualizarInformacoes.Enabled = false;
                    btnVisualizarInformacoes.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.AlterarSituacaoFilial))
                {
                    rcbSituacaoEmpresa.Enabled = false;
                    rcbSituacaoEmpresa.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }
                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.BloqueioPlanoEmpresa))
                {
                    btnBloqueioPlano.Enabled = false;
                    btnBloqueioPlano.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }
                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.DesbloqueioPlanoEmpresa))
                {
                    btnDesbloqueioPlano.Enabled = false;
                    btnDesbloqueioPlano.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

            }
            else
                Redirect(Configuracao.UrlAvisoAcessoNegado);
        }
        #endregion

        #region CarregarComboTipoPlano
        public void CarregarComboTipoPlano()
        {
            UIHelper.CarregarRadComboBox(rcbTipoPlano, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaFisica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));
            upDados.Update();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos(int? idPessoaFisica, int? idFilial)
        {
            PageIndex = 1;
            RadQuantidadeParcelas.Items.Clear();
            pnlAtualizarPlano.Visible = btnAtualizarPlano.Visible = false;
            UIHelper.CarregarRadComboBox(rcbFilialGestora, Filial.ListarFiliaisEmployer());
            rcbFilialGestora.SelectedValue = Parametro.RecuperaValorParametro(Enumeradores.Parametro.FilialGestoraPadraoDoPlano);

            if (idPessoaFisica.HasValue)
            {
                IdPessoaFisica = idPessoaFisica;
                IdFilial = null;

                pnlDadosClientePF.Visible = true;
                pnlDadosClientePJ.Visible = false;
                LimparCamposPF();

                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica.Value);

                litNome.Text = objPessoaFisica.NomePessoa;
                litCPF.Text = objPessoaFisica.NumeroCPF;
                litDataNascimento.Text = objPessoaFisica.DataNascimento.ToShortDateString();
                litEmail.Text = objPessoaFisica.EmailPessoa;

                //Carregando informações da modal de liberação
                lblNomeLiberarCliente.Text = "Nome do Candidato:";
                litNomeLiberarCliente.Text = objPessoaFisica.NomePessoa;
                pnlLiberarCliente.Visible = btnLiberarCliente.Visible = true;

                litTipoPlanoLiberarCliente.Visible = true;
                rcbTipoPlano.Visible = false;

                UIHelper.CarregarRadComboBox(rcbPlano, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaFisica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));

            
            }
            else if (idFilial.HasValue)
            {
                IdPessoaFisica = null;
                IdFilial = idFilial;

                pnlDadosClientePF.Visible = false;
                pnlDadosClientePJ.Visible = true;

                Filial objFilial = Filial.LoadObject(idFilial.Value);

                litRazaoSocial.Text = objFilial.RazaoSocial;
                litNomeFantasia.Text = objFilial.NomeFantasia;

                if (objFilial.NumeroCNPJ.HasValue)
                    litCNPJ.Text = objFilial.NumeroCNPJ.Value.ToString(CultureInfo.CurrentCulture);

                if (objFilial.Endereco != null)
                {
                    objFilial.Endereco.CompleteObject();
                    if (objFilial.Endereco.Cidade != null)
                    {
                        objFilial.Endereco.Cidade.CompleteObject();
                        litCidadePJ.Text = objFilial.Endereco.Cidade.NomeCidade;
                    }
                }

                //Carregando informações da modal de liberação de cliente
                lblNomeLiberarCliente.Text = "Nome da Empresa:";
                litNomeLiberarCliente.Text = objFilial.RazaoSocial;
                litTipoPlanoLiberarCliente.Visible = true;

                lblDataEnvioBoleto.Text = "Data de Envio do Boleto";
                lblDataVencimentoBoleto.Text = "Data de Vencimento do Boleto";
                btnVisualizarInformacoes.Visible = true;

                rcbTipoPlano.Visible = false;
                UIHelper.CarregarRadComboBox(rcbPlano, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaJuridica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));

                rcbSituacaoEmpresa.SelectedValue = objFilial.SituacaoFilial.IdSituacaoFilial.ToString(CultureInfo.CurrentCulture);
                txtQuantidadeUsuario.Valor =
                        Convert.ToString(Convert.ToInt32(
                            Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeLimiteUsuarios)) +
                        (objFilial.QuantidadeUsuarioAdicional.HasValue ? objFilial.QuantidadeUsuarioAdicional.Value : 0));

                upSituacaoEmpresa.Update();
                upQuantidadeUsuario.Update();

                ucObservacaoFilial.IdFilial = objFilial.IdFilial;
                ucObservacaoFilial.Inicializar();
            }

            CarregarGrid();
            HabilitarDesabilitarCampos();

            upPnlPlanoFidelidade.Update();
            upDados.Update();
            upPnlDadosClientePF.Update();
            upPnlDadosClientePJ.Update();
        }
        #endregion

        #region LimparCamposPF
        public void LimparCamposPF()
        {
            litNome.Text = String.Empty;
            litCPF.Text = String.Empty;
            litDataNascimento.Text = String.Empty;
            litEmail.Text = String.Empty;
            upPnlDadosClientePF.Update();
        }
        #endregion

        #region LimparCamposPJ
        public void LimparCamposPJ()
        {
            litRazaoSocial.Text = String.Empty;
            litNomeFantasia.Text = String.Empty;
            litCNPJ.Text = String.Empty;
            litCidadePJ.Text = String.Empty;
            upPnlDadosClientePJ.Update();

            rcbSituacaoEmpresa.SelectedValue = "1";
            txtQuantidadeUsuario.Valor = String.Empty;
            upSituacaoEmpresa.Update();
            upQuantidadeUsuario.Update();
        }
        #endregion

        #region LimparCampoPesquisa
        public void LimparCampoPesquisa()
        {
            //tbxFiltroBusca.Text = string.Empty;
            tbxCPF.Text = string.Empty;
            tbxCNPJ.Text = string.Empty;
            upPesquisaCliente.Update();
        }
        #endregion

        #region PreencherCamposPlano
        private void PreencherCamposPlano(PlanoAdquirido objPlanoAdquirido)
        {
            objPlanoAdquirido.Plano.CompleteObject();

            //Ajustando moldal de liberação de plano.
            litTipoPlanoLiberarCliente.Text = objPlanoAdquirido.Plano.DescricaoPlano;
            btnCriarAdicional.Visible = true;
            btnLiberarPlano.Visible = true;
            btnLiberarCliente.Visible = true;
            chkRegistraBoleto.Checked = objPlanoAdquirido.FlagBoletoRegistrado;
            bool planoRecorrente = objPlanoAdquirido.Plano.FlagRecorrente;

            if (objPlanoAdquirido.Filial != null)
            {

                if (objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.Liberado))
                {
                    PlanoQuantidade objPlanoQuantidade;
                    if (PlanoQuantidade.CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoQuantidade))
                    {
                        lblQuantidadeVisualizacaoAtual.Text = String.Format("Atual: {0}", objPlanoQuantidade.QuantidadeVisualizacao);
                        lblQuantidadeSMSAtual.Text = String.Format("Atual: {0}", objPlanoQuantidade.QuantidadeSMS);
                        lblQuantidadeCampanhaAtual.Text = String.Format("Atual: {0}", objPlanoQuantidade.QuantidadeCampanha);
                        pnlAtualizarPlano.Visible = btnAtualizarPlano.Visible = btnBloqueioPlano.Visible = true;
                        btnLiberarCliente.Visible = false;
                    }
                }
                else if (objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.AguardandoLiberacao))
                {
                    pnlAtualizarPlano.Visible = btnAtualizarPlano.Visible = false;
                    btnLiberarCliente.Visible = true;
                    btnCriarAdicional.Visible = false;
                    btnLiberarPlanoAdicional.Visible = false;
                }
                else if (objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.Bloqueado))
                {
                    pnlAtualizarPlano.Visible = btnAtualizarPlano.Visible = false;
                    btnLiberarCliente.Visible = true;
                    btnCriarAdicional.Visible = false;
                    btnLiberarPlanoAdicional.Visible = false;
                    btnLiberarPlano.Visible = false;

                    btnDesbloqueioPlano.Visible = true;
                }
            }
            else
                btnCriarAdicional.Visible = false; //Se não for um plano de filila não deve criar adicional

            //Verifica a existencia de planos adicionais para o plano adquirido
            AjustarVisibilidadeBotaoLiberarAdicional(objPlanoAdquirido);

            if (objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.Cancelado))
            {
                btnCriarAdicional.Visible = false;
                btnLiberarPlano.Visible = false;
                btnLiberarPlanoAdicional.Visible = false;
            }

            rcbPlano.SelectedValue = objPlanoAdquirido.Plano.IdPlano.ToString(CultureInfo.CurrentCulture);

            ValorTotalPlano = Convert.ToDecimal(objPlanoAdquirido.QtdParcela.HasValue ? objPlanoAdquirido.QtdParcela.Value : objPlanoAdquirido.Plano.QuantidadeParcela) * objPlanoAdquirido.ValorBase;

            CarregaDdlQuantidadeParcelas(objPlanoAdquirido.Plano.QuantidadeParcela, objPlanoAdquirido.Plano.FlagCustomizaParcela);
            decimal valorPlano = CalculaValorParcelado(objPlanoAdquirido.QtdParcela.HasValue ? objPlanoAdquirido.QtdParcela.Value : objPlanoAdquirido.Plano.QuantidadeParcela);
            txtValorParcela.Valor = valorPlano;
            
            
            RadQuantidadeParcelas.SelectedValue = objPlanoAdquirido.QtdParcela.ToString();
            txtDataInicioPlano.ValorDatetime = objPlanoAdquirido.DataInicioPlano;
            txtDataFimPlano.ValorDatetime = objPlanoAdquirido.DataFimPlano;

            //Atualizar data de vencimento Boletos
            Pagamento objPagamento = Pagamento.CarregarPrimeiraPagamentoPlanoAdquiridoPorSituacao(objPlanoAdquirido.IdPlanoAdquirido, Enumeradores.PagamentoSituacao.EmAberto);
            if (objPagamento == null)
            {
                divDataVencimentoBoleto.Visible = false;
                divDataEnvioBoleto.Visible = false;
            }
            else
            {
                divDataVencimentoBoleto.Visible = true;
                divDataEnvioBoleto.Visible = true;
                txtDataVencimentoBoleto.DataMinima = DateTime.Today >= objPagamento.DataVencimento.Value ? objPagamento.DataVencimento.Value : DateTime.Today;
                txtDataVencimentoBoleto.ValorDatetime = objPagamento.DataVencimento.Value;
                lblDataVencimentoBoleto.Text = "Data de Vencimento Boleto em Aberto";
                //Atualizar data de Emissão Boletos
                txtDataEnvioBoleto.DataMinima = DateTime.Today >= objPagamento.DataEmissao.Value ? objPagamento.DataEmissao.Value : DateTime.Today;
                txtDataEnvioBoleto.ValorDatetime = objPagamento.DataEmissao.Value;
                lblDataEnvioBoleto.Text = "";
                lblDataEnvioBoleto.Text = "Data de Envio do Boleto em Aberto";
            }

            //txtValorParcela.Valor = objPlanoAdquirido.ValorBase;
            ckbPagamentoRecorrenteMensal.Checked = objPlanoAdquirido.FlagRecorrente;

           

            if (objPlanoAdquirido.FlgNotaAntecipada)
            {
                PlanoParcela objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquiridoDataAntecipacaoNotaFiscal(objPlanoAdquirido);
                if (objPlanoParcela != null)
                {
                    chkNotaAntecipada.Checked = objPlanoAdquirido.FlgNotaAntecipada;
                    txtDataNFAntecipada.ValorDatetime = objPlanoParcela.DataEmissaoNotaAntecipada.Value;
                    divDataNFAntecipada.Visible = true;
                }
            }

            PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes;
            if (PlanoAdquiridoDetalhes.CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoAdquiridoDetalhes))
            {
                txtEnviarPara.Text = objPlanoAdquiridoDetalhes.EmailEnvioBoleto;
                if (objPlanoAdquiridoDetalhes.FlagNotaFiscal)
                    rbSim.Checked = true;

                txtNomeCompleto.Valor = objPlanoAdquiridoDetalhes.NomeResPlanoAdquirido;
                txtTelefoneComercial.DDD = objPlanoAdquiridoDetalhes.NumeroResDDDTelefone;
                txtTelefoneComercial.Fone = objPlanoAdquiridoDetalhes.NumeroResTelefone;
                txtObservacoes.Valor = objPlanoAdquiridoDetalhes.DescricaoObservacao;
                rcbFilialGestora.SelectedValue = objPlanoAdquiridoDetalhes.FilialGestora.IdFilial.ToString();
            }
            else
            {
                if (IdFilial.HasValue)
                {
                    UsuarioFilialPerfil objUsuarioFilialPerfil;
                    if (UsuarioFilialPerfil.CarregarPorPerfilFilial((int)Enumeradores.Perfil.AcessoEmpresaMaster, IdFilial.Value, out objUsuarioFilialPerfil))
                    {
                        objUsuarioFilialPerfil.PessoaFisica.CompleteObject();
                        txtNomeCompleto.Valor = objUsuarioFilialPerfil.PessoaFisica.NomePessoa;

                        UsuarioFilial objUsuarioFilial;
                        if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                        {
                            txtEnviarPara.Text = objUsuarioFilial.EmailComercial;
                            txtTelefoneComercial.DDD = objUsuarioFilial.NumeroDDDComercial;
                            txtTelefoneComercial.Fone = objUsuarioFilial.NumeroComercial;
                        }
                    }
                }
                rcbFilialGestora.SelectedValue = Parametro.RecuperaValorParametro(Enumeradores.Parametro.FilialGestoraPadraoDoPlano);
            }

            HabilitarDesabilitarCampos(planoRecorrente);

            upPnlBotoes.Update();
            upDados.Update();
            upPnlPlanoFidelidade.Update();
        }
        #endregion

        #region LimparCampos
        public void LimparCampos()
        {
            rcbPlano.ClearSelection();
            rcbFilialGestora.SelectedValue = Parametro.RecuperaValorParametro(Enumeradores.Parametro.FilialGestoraPadraoDoPlano);
            txtDataInicioPlano.ValorDatetime = null;
            txtDataFimPlano.ValorDatetime = null;
            txtDataEnvioBoleto.ValorDatetime = null;
            txtDataVencimentoBoleto.ValorDatetime = null;
            txtEnviarPara.Text = String.Empty;
            rbSim.Checked = true;
            txtNomeCompleto.Valor = String.Empty;
            txtTelefoneComercial.DDD = String.Empty;
            txtTelefoneComercial.Fone = String.Empty;
            txtObservacoes.Valor = String.Empty;
            chkNotaAntecipada.Checked = false;
            txtDataNFAntecipada.ValorDatetime = null;
            divDataNFAntecipada.Visible = false;
            divPlanoPagamentoRecorrente.Visible = false;
            divDataVencimentoBoleto.Visible = true;
            divDataEnvioBoleto.Visible = true;
            RadQuantidadeParcelas.ClearSelection();

            HabilitarDesabilitarCampos();

            btnCriarAdicional.Visible = false;
            btnLiberarPlano.Visible = false;
            btnLiberarPlanoAdicional.Visible = false;
            btnVisualizarInformacoes.Visible = false;

            btnBloqueioPlano.Visible = false;
            btnDesbloqueioPlano.Visible = false;

            upPnlBotoes.Update();
            upPnlPlanoFidelidade.Update();
        }
        #endregion

        #region Salvar
        private PlanoAdquirido Salvar(Plano objPlano)
        {
            PlanoAdquirido objPlanoAdquirido;
            PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes;
            UsuarioFilialPerfil objUsuarioFilialPerfil;

            //Variáveis Clone
            PlanoAdquirido objPlanoAdquiridoOld = null;
            PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhesOld = null;
            UsuarioFilialPerfil objUsuarioFilialPerfilOld = null;

            Filial objFilial = null;

            if (IdFilial.HasValue)
                objFilial = Filial.LoadObject(IdFilial.Value);

            if (IdPlanoAdquirido.HasValue)
            {
                objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value);


                if (objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Cancelado ||
                    objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Encerrado)
                {
                    ExibirMensagemAlerta("Operação não permitida", "Não é possível modificar planos Cancelados ou Encerrados!");
                    return objPlanoAdquirido;
                }

                objPlanoAdquiridoOld = objPlanoAdquirido.Clone() as PlanoAdquirido; //Clonando para gerar LOG

                if (!PlanoAdquiridoDetalhes.CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoAdquiridoDetalhes))
                    objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes();
                else
                    objPlanoAdquiridoDetalhesOld = objPlanoAdquiridoDetalhes.Clone() as PlanoAdquiridoDetalhes;
            }
            else
            {
                objPlanoAdquirido = new PlanoAdquirido();
                objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes();
                if (objFilial != null)
                {
                    //Se plano de empresa e houver plano liberado, coloca o plano como Liberacao Automatica.
                    //Assim o plano sera liberado na data de inicio do plano
                    //Logica colocada neste componente pois o status LiberacaAutomatica so deve ser utilizada na area do administrador.
                    PlanoAdquirido objPlanoLiberado;
                    if (PlanoAdquirido.CarregarPlanoVigentePessoaJuridicaPorSituacao(objFilial, new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado), out objPlanoLiberado))
                        objPlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.LiberacaAutomatica);
                    else
                        objPlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.AguardandoLiberacao);
                }
                else
                {
                    objPlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.AguardandoLiberacao);
                }
                objPlanoAdquirido.QuantidadeSMS = objPlano.QuantidadeSMS;
                objPlanoAdquirido.QuantidadePrazoBoleto = 0;
            }

            decimal valorPlano = objPlano.ValorBase;
            if (txtValorParcela.Valor.HasValue)
                valorPlano = txtValorParcela.Valor.Value;

            objPlanoAdquirido.ValorBase = valorPlano;
            objPlanoAdquirido.QtdParcela = Convert.ToInt32(RadQuantidadeParcelas.SelectedValue);
            objPlanoAdquirido.DataInicioPlano = txtDataInicioPlano.ValorDatetime.Value;
            objPlanoAdquirido.DataFimPlano = txtDataFimPlano.ValorDatetime.Value;
            objPlanoAdquirido.FlagBoletoRegistrado = chkRegistraBoleto.Checked;
            objPlanoAdquirido.FlgNotaAntecipada = chkNotaAntecipada.Checked;
            objPlanoAdquirido.FlagRecorrente = ckbPagamentoRecorrenteMensal.Checked;

            if (!chkNotaAntecipada.Checked) txtDataNFAntecipada.ValorDatetime = null;

            objPlanoAdquiridoDetalhes.FilialGestora = new Filial(Convert.ToInt32(rcbFilialGestora.SelectedValue));



            if (objFilial != null)
            {
                //Detalhes
                objPlanoAdquiridoDetalhes.EmailEnvioBoleto = txtEnviarPara.Text;
                objPlanoAdquiridoDetalhes.FlagNotaFiscal = rbSim.Checked;
                objPlanoAdquiridoDetalhes.NomeResPlanoAdquirido = txtNomeCompleto.Valor;
                objPlanoAdquiridoDetalhes.NumeroResDDDTelefone = txtTelefoneComercial.DDD;
                objPlanoAdquiridoDetalhes.NumeroResTelefone = txtTelefoneComercial.Fone;
                objPlanoAdquiridoDetalhes.DescricaoObservacao = txtObservacoes.Valor;

                UsuarioFilialPerfil.CarregarPorPerfilFilial((int)Enumeradores.Perfil.AcessoEmpresaMaster, IdFilial.Value, out objUsuarioFilialPerfil);

                if (objUsuarioFilialPerfil == null) //Se o usuário está nulo, pega o que está logado.
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
                else
                    objUsuarioFilialPerfilOld = objUsuarioFilialPerfil.Clone() as UsuarioFilialPerfil;

                if (IdPlanoAdquirido.HasValue) //Atualiza apenas plano e plano detalhes
                {
                    objPlanoAdquirido.SalvarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objFilial, objPlano, objPlanoAdquiridoDetalhes,txtDataEnvioBoleto.ValorDatetime,txtDataVencimentoBoleto.ValorDatetime, txtDataNFAntecipada.ValorDatetime, false);
                    if(txtDataVencimentoBoleto.ValorDatetime.HasValue && txtDataEnvioBoleto.ValorDatetime.HasValue)
                        objPlanoAdquirido.AjustarDatasDePagamentos(txtDataVencimentoBoleto.ValorDatetime.Value, txtDataEnvioBoleto.ValorDatetime.Value);
                    ExibirMensagemConfirmacao("Confirmação de Cadastro", "Plano atualizado com sucesso!", false);

                    string descricao = string.Empty;
                    descricao += objPlanoAdquiridoOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPlanoAdquiridoOld, objPlanoAdquirido, objPlanoAdquirido.GetType())):string.Empty;
                    descricao += objPlanoAdquiridoDetalhesOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPlanoAdquiridoDetalhesOld, objPlanoAdquiridoDetalhes, objPlanoAdquiridoDetalhes.GetType())):string.Empty;
                    descricao += objUsuarioFilialPerfilOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objUsuarioFilialPerfilOld, objUsuarioFilialPerfil, objUsuarioFilialPerfil.GetType())):string.Empty;

                    if (!string.IsNullOrEmpty(descricao))
                        MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM("Tela PlanoAdquirido: <br/><br/>"+ descricao, objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);

                }
                else
                {
                    objPlanoAdquirido.SalvarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objFilial, objPlano, objPlanoAdquiridoDetalhes, txtDataEnvioBoleto.ValorDatetime.Value, txtDataVencimentoBoleto.ValorDatetime.Value, txtDataNFAntecipada.ValorDatetime, true);

                    //Ajustando a modal de liberação
                    litTipoPlanoLiberarCliente.Text = objPlanoAdquirido.Plano.DescricaoPlano;
                    CarregarGrid();
                    ExibirMensagemConfirmacao("Confirmação de Cadastro", "Plano cadastrado com sucesso!", false);
                    MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM("Confirmação de Cadastro do Plano", objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
                }

                IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;

            }
            else if (IdPessoaFisica.HasValue)
            {
                objPlanoAdquiridoDetalhes = null;

                if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(new PessoaFisica(IdPessoaFisica.Value), out objUsuarioFilialPerfil))
                {
                    objUsuarioFilialPerfilOld = objUsuarioFilialPerfil.Clone() as UsuarioFilialPerfil;
                    objPlanoAdquirido.SalvarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, null, objPlano, objPlanoAdquiridoDetalhes, txtDataEnvioBoleto.ValorDatetime, txtDataVencimentoBoleto.ValorDatetime, null, true);
                }

                if (IdPlanoAdquirido.HasValue)
                {
                    //Ajustando a modal de liberação
                    litTipoPlanoLiberarCliente.Text = objPlanoAdquirido.Plano.DescricaoPlano;

                    Curriculo objCurriculo;
                    Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo);
                    
                    string descricao = MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPlanoAdquiridoOld, objPlanoAdquirido, objPlanoAdquirido.GetType()),
                            MensagemAssincronoLogCRM.DiffProperties(objUsuarioFilialPerfilOld, objUsuarioFilialPerfil, objUsuarioFilialPerfil.GetType()));

                    if (!string.IsNullOrEmpty(descricao))
                        MensagemAssincronoLogCRM.SalvarModificacoesCurriculoCRM(descricao, objCurriculo, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
                    ExibirMensagemConfirmacao("Alteração de Dados", "Plano alterado com sucesso!", false);  
                }
                else
                {
                    litTipoPlanoLiberarCliente.Text = objPlanoAdquirido.Plano.DescricaoPlano;

                    Curriculo objCurriculo;
                    Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo);
                    MensagemAssincronoLogCRM.SalvarModificacoesCurriculoCRM("Plano cadastrado com sucesso!", objCurriculo, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value));

                    ExibirMensagemConfirmacao("Confirmação de Cadastro", "Plano cadastrado com sucesso!", false);   
                }
                CarregarGrid();
            }
            return objPlanoAdquirido;
        }
        #endregion

        #region CriarAdicional
        private void CriarAdicional(PlanoAdquirido objPlanoAdquirido)
        {
            //Atualiza o valor e a quantidade do plano.
            decimal valorTotal = Convert.ToDecimal(txtCriarPlanoAdicionalValorTotal.Valor);
            int quantidadeSMS = Convert.ToInt32(txtCriarPlanoAdicionalQuantidadeSMS.Valor);

            UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);

            BLL.Pagamento objPagamento;
            AdicionalPlano.CriarPagamentoEPlanoAdicionalSMS(objPlanoAdquirido, valorTotal, quantidadeSMS, objUsuarioFilialPerfil, Enumeradores.TipoPagamento.BoletoBancario, DateTime.Now, txtCriarPlanoAdicionalDataVencimento.ValorDatetime.Value, out objPagamento);
            //Antes aqui gerava o boleto
        }
        #endregion

        #region HabilitarDesabilitarCampos
        private void HabilitarDesabilitarCampos(bool planoRecorrente = false)
        {
            bool mostrarCamposDetalhes = false;
            bool habilitarCampos = !IdPlanoAdquirido.HasValue;

            if (IdFilial.HasValue)
                mostrarCamposDetalhes = true;
            else if (!IdPlanoAdquirido.HasValue && !IdPessoaFisica.HasValue)
                mostrarCamposDetalhes = true;

            rcbPlano.Enabled = habilitarCampos;
            RadQuantidadeParcelas.Enabled = habilitarCampos;
            //rcbFilialGestora.Enabled = habilitarCampos;

            //divDataEnvioBoleto.Visible = habilitarCampos;
            //divDataVencimentoBoleto.Visible = habilitarCampos;
            //divValorParcela.Visible = habilitarCampos;

            //txtDataEnvioBoleto.Obrigatorio = habilitarCampos;
            //txtDataVencimentoBoleto.Obrigatorio = habilitarCampos;

            //txtValorParcela.Obrigatorio = mostrarCamposDetalhes;

            divEnviarPara.Visible = mostrarCamposDetalhes;
            divNomeCompleto.Visible = mostrarCamposDetalhes;
            divEmitirNotaFiscal.Visible = mostrarCamposDetalhes;
            divTelefoneComercial.Visible = mostrarCamposDetalhes;

            //txtEnviarPara.Obrigatorio = mostrarCamposDetalhes;
            txtNomeCompleto.Obrigatorio = mostrarCamposDetalhes;
            txtTelefoneComercial.Obrigatorio = mostrarCamposDetalhes;

            lblObservacoes.Visible = mostrarCamposDetalhes;
            txtObservacoes.Visible = mostrarCamposDetalhes;

            chkRegistraBoleto.Enabled = !planoRecorrente;
            chkNotaAntecipada.Enabled = !planoRecorrente;
            divPlanoPagamentoRecorrente.Visible = planoRecorrente;
        }
        #endregion

        #region EsconderPanelPJ

        public void EsconderPanelPJ(bool esconder)
        {
            pnlDadosClientePJ.Visible = !esconder;
            upPnlDadosClientePJ.Update();
        }

        #endregion

        #region EsconderPanelPF
        public void EsconderPanelPF(bool esconder)
        {
            pnlDadosClientePF.Visible = !esconder;
            upPnlDadosClientePF.Update();
        }
        #endregion

        #region LimparGrid
        public void LimparGrid()
        {
            var dt = new DataTable();
            dt.Columns.Add("Des_Plano");
            dt.Columns.Add("Dta_Inicio_Plano");
            dt.Columns.Add("Dta_Fim_Plano");
            dt.Columns.Add("Des_Plano_Situacao");
            dt.Columns.Add("Qtd_Visualizacao");
            dt.Columns.Add("Idf_Plano");
            dt.Columns.Add("Idf_Plano_Situacao");

            UIHelper.CarregarRadGrid(gvPlanos, new DataTable());
        }
        #endregion

        #region AjustarResultadoBusca
        private void AjustarResultadoBusca(DataTable dt)
        {
            if (dt.Rows.Count.Equals(0))
                ExibirMensagem("Nenhum cliente encontrado com os dados informados", TipoMensagem.Aviso);
            else if (dt.Rows.Count.Equals(1))
            {
                //Se retornou apenas uma linha de resultado, identifica se é pessoa fisica ou juridica
                var pessoaFisica = dt.Rows[0]["Idf_Pessoa_Fisica"];
                var filial = dt.Rows[0]["Idf_Filial"];
                int idPessoaFisica, idFilial;

                LimparCamposPF();
                LimparCamposPJ();
                LimparCampos();

                IdFilial = IdPessoaFisica = IdPlanoAdquirido = null;

                if (Int32.TryParse(pessoaFisica.ToString(), out idPessoaFisica))
                    PreencherCampos(idPessoaFisica, null);

                if (Int32.TryParse(filial.ToString(), out idFilial))
                    PreencherCampos(null, idFilial);
            }
            else
            {
                DT = dt;
                CarregarOpcoesModal();
            }
        }
        #endregion

        #region LiberarAdicionalPlano
        private void LiberarAdicionalPlano(AdicionalPlano objAdicionalPlano)
        {
            try
            {
                objAdicionalPlano.LiberarPlanoAdicional();
                CarregarPlanosAdicionais(objAdicionalPlano.PlanoAdquirido);
                ExibirMensagem(MensagemAviso._100110, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region CarregarPlanosAdicionais
        private void CarregarPlanosAdicionais(PlanoAdquirido objPlanoAdquirido)
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvLiberarPlanoAdicional, AdicionalPlano.ListarAdicionaisPorPlanoAdquirido(objPlanoAdquirido, gvLiberarPlanoAdicional.CurrentPageIndex, gvLiberarPlanoAdicional.PageSize, out totalRegistros), totalRegistros);
            upDadosLiberarPlanoAdicional.Update();
        }
        #endregion

        #region CarregarPlanoSmsEAjustarValoresPadraoPlanoAdicional
        private void CarregarPlanoSmsEAjustarValoresPadraoPlanoAdicional()
        {
            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.ValorSMSAvulso,
                    //Enumeradores.Parametro.CodigoPlanoSMS,
                    Enumeradores.Parametro.QuantidadeSMSMinima
                };

            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            //IdPlano = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.CodigoPlanoSMS]);

            decimal valorSms = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.ValorSMSAvulso]);
            int quantidadeSms = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.QuantidadeSMSMinima]);

            txtCriarPlanoAdicionalQuantidadeSMS.Valor = quantidadeSms.ToString(CultureInfo.CurrentCulture);
            txtCriarPlanoAdicionalValorSMS.Valor = valorSms;

            txtCriarPlanoAdicionalValorTotal.Valor = quantidadeSms * valorSms;
        }
        #endregion

        #region CarregarListaSugestaoTipoAdicional
        private void CarregarListaSugestaoTipoAdicional()
        {
            var dictionary = new Dictionary<string, string>
                {
                    {"1", "SMS"}
                };
            UIHelper.CarregarRadComboBox(rcbCriarPlanoAdicionalTipoAdicional, dictionary);
        }
        #endregion

        #region AjustarVisibilidadeBotaoLiberarAdicional
        private void AjustarVisibilidadeBotaoLiberarAdicional(PlanoAdquirido objPlanoAdquirido)
        {
            btnLiberarPlanoAdicional.Visible = objPlanoAdquirido.ExistePlanoAdicionalAguardandoLiberacao();
            upPnlBotoes.Update();
        }
        #endregion

        #region AjustarVisibilidadeBotaoLiberarAdicional
        public bool AjustarVisibilidadeBotaoLiberarAdicional(int idfAdicionalPlanoSituacao)
        {
            return idfAdicionalPlanoSituacao.Equals((int)Enumeradores.AdicionalPlanoSituacao.AguardandoLiberacao);
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;

            Ajax.Utility.RegisterTypeForAjax(typeof(PlanoFidelidade));
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquiridoExclusao.Value);

                int? idCurriculo = null;

                if (objPlanoAdquirido.Plano != null)
                {
                    objPlanoAdquirido.Plano.CompleteObject();
                    if (objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                    {
                        objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();

                        int idPessoaFisica = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica;
                        Curriculo objCurriculo;
                        if (Curriculo.CarregarPorPessoaFisica(idPessoaFisica, out objCurriculo))
                        {
                            idCurriculo = objCurriculo.IdCurriculo;
                        }
                    }
                }

                objPlanoAdquirido.CancelarPlanoAdquirido(new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null, true, idCurriculo);

                //Se o plano vigente é igual ao plano que está sendo excluido, então possibilita a inclusão de um novo plano.
                if (IdPlanoAdquirido.Equals(IdPlanoAdquiridoExclusao))
                {
                    IdPlanoAdquirido = IdPlanoAdquiridoExclusao = null;
                    LimparCampos();
                    PreencherCampos(IdPessoaFisica, IdFilial);
                }

                CarregarGrid();
                base.ExibirMensagemConfirmacao("Confirmação de Cancelamento", "Plano cancelado com sucesso!", false);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvPlanos_PageIndexChanged
        protected void gvPlanos_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region gvPlanos_ItemCommand
        protected void gvPlanos_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                int idPlanoAdquirido = Convert.ToInt32(gvPlanos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Plano_Adquirido"]);

                if (PlanoSelecionado != null)
                    PlanoSelecionado(idPlanoAdquirido);
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                IdPlanoAdquiridoExclusao = Convert.ToInt32(gvPlanos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Plano_Adquirido"]);

                string nome = String.Empty;

                if (IdFilial.HasValue)
                {
                    Filial objFilial = Filial.LoadObject(IdFilial.Value);
                    nome = "da empresa " + objFilial.RazaoSocial;
                }
                else if (IdPessoaFisica.HasValue)
                {
                    var objPessoaFisica = new PessoaFisica(IdPessoaFisica.Value);
                    nome = "do candidato " + objPessoaFisica.NomePessoa;
                }

                ucConfirmacaoExclusao.Inicializar("Confirmação", String.Format("Tem certeza que deseja excluir o plano {0} ?", nome));
                ucConfirmacaoExclusao.MostrarModal();
            }
            else if (e.CommandName.Equals("EditarPlano"))
            {
                IdPlanoAdquirido = Convert.ToInt32(gvPlanos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Plano_Adquirido"]);
                LimparCampos();
                PreencherCamposPlano(PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value));
            }
            else if (e.CommandName.Equals("GerarPdfBoletos"))
            {
                int idPlanoAdquirido = Convert.ToInt32(gvPlanos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Plano_Adquirido"]);

                List<Pagamento> pagamentos = new List<Pagamento>();
                foreach (var objPlanoParcela in PlanoParcela.RecuperarParcelasPlanoAdquirido(PlanoAdquirido.LoadObject(idPlanoAdquirido)).Where(p => p.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)Enumeradores.PlanoParcelaSituacao.EmAberto).OrderBy(o => o.IdPlanoParcela))
                {
                    var pagamentosParcela = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela); //Recupera todos os pagamentos relacionados com a parcela
                    pagamentos.Add(pagamentosParcela.Select(p => p).FirstOrDefault(p => p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false && p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)); //Recupera o primeeiro pagamento em aberto
                }

                if(pagamentos.Count > 0)
                    hlPDF.NavigateUrl = BoletoBancario.RetornarBoleto(BoletoBancario.CriarBoletosPagarMe(pagamentos), Enumeradores.FormatoBoleto.PDF);

                upDadosBoletos.Update();
                mpeVisualizacaoBoleto.Show();
            }
        }
        #endregion

        #region gvPlano_ItemDataBound
        protected void gvPlano_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var btiEditarPlano = ((ImageButton)e.Item.FindControl("btiEditarPlano"));
                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroEditarPlano))
                {
                    btiEditarPlano.Enabled = false;
                    btiEditarPlano.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                var btiEditarParcelas = ((ImageButton)e.Item.FindControl("btiEditarParcelas"));
                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroVisualizarParcelas))
                {
                    btiEditarParcelas.Enabled = false;
                    btiEditarParcelas.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                var btiExcluir = ((ImageButton)e.Item.FindControl("btiExcluir"));
                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroCancelarPlano))
                {
                    btiExcluir.Enabled = false;
                    btiExcluir.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }
            }
        }
        #endregion

        #region gvSelecionarCliente_ItemCommand
        protected void gvSelecionarCliente_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Selecionar"))
            {
                IdFilial = IdPessoaFisica = IdPlanoAdquirido = null;
                var pessoaFisica = gvSelecionarCliente.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pessoa_Fisica"];
                var filial = gvSelecionarCliente.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"];
                int idPessoaFisica, idFilial;

                LimparCampos();

                if (Int32.TryParse(pessoaFisica.ToString(), out idPessoaFisica))
                    PreencherCampos(idPessoaFisica, null);
                else if (Int32.TryParse(filial.ToString(), out idFilial))
                    PreencherCampos(null, idFilial);

                mpeSelecionarCliente.Hide();
            }
        }
        #endregion

        #region gvSelecionarCliente_PageIndexChanged
        protected void gvSelecionarCliente_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            CarregarOpcoesModal();
        }
        #endregion

        #region gvLiberarPlanoAdicional

        #region gvLiberarPlanoAdicional_ItemCommand
        protected void gvLiberarPlanoAdicional_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Liberar"))
            {
                var adicionalPlano = gvLiberarPlanoAdicional.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Adicional_Plano"];
                int idAdicionalPlano;
                if (Int32.TryParse(adicionalPlano.ToString(), out idAdicionalPlano))
                {
                    AdicionalPlano objAdicionalPlano = AdicionalPlano.LoadObject(idAdicionalPlano);
                    LiberarAdicionalPlano(objAdicionalPlano);
                }
            }
        }
        #endregion

        #region gvLiberarPlanoAdicional_PageIndexChanged
        protected void gvLiberarPlanoAdicional_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvLiberarPlanoAdicional.CurrentPageIndex = e.NewPageIndex;
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value);
            CarregarPlanosAdicionais(objPlanoAdquirido);
        }
        #endregion

        #endregion

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            DataTable dt;
            string cpf = tbxCPF.Text.Trim();
            string cnpj = tbxCNPJ.Text.Trim();
            string boleto = tbxBoleto.Text.Trim();
            string tid = tbxTID.Text.Trim();

            if ((String.IsNullOrEmpty(cpf) && String.IsNullOrEmpty(cnpj) && String.IsNullOrEmpty(boleto) && String.IsNullOrEmpty(tid))
                || (!String.IsNullOrEmpty(cpf) && !String.IsNullOrEmpty(cnpj))
                || (!String.IsNullOrEmpty(cpf) && !String.IsNullOrEmpty(boleto))
                || (!String.IsNullOrEmpty(cpf) && !String.IsNullOrEmpty(tid))
                || (!String.IsNullOrEmpty(cnpj) && !String.IsNullOrEmpty(boleto))
                || (!String.IsNullOrEmpty(tid) && !String.IsNullOrEmpty(boleto))
                || (!String.IsNullOrEmpty(tid) && !String.IsNullOrEmpty(cnpj)))
                ExibirMensagem("Informe um CPF ou um CNPJ ou um número de boleto ou uma TID!", TipoMensagem.Erro);
            else if (!String.IsNullOrEmpty(cpf))
            {
                dt = Busca.PesquisarClientePessoaFisica(cpf);
                AjustarResultadoBusca(dt);
            }
            else if (!String.IsNullOrEmpty(cnpj))
            {
                dt = Busca.PesquisarClientePessoaJuridica(cnpj);
                AjustarResultadoBusca(dt);
            }
            else if (!String.IsNullOrEmpty(boleto))
            {
                dt = Busca.PesquisarClienteBoleto(boleto);
                AjustarResultadoBusca(dt);
            }
            else if (!String.IsNullOrEmpty(tid))
            {
                dt = Busca.PesquisarPagamentoTID(tid);
                AjustarResultadoBusca(dt);
            }
        }
        #endregion

        #region btnCancelar_Click
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            mpeSelecionarCliente.Hide();
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IdFilial.HasValue || IdPessoaFisica.HasValue)
                {
                    Plano objPlano = Plano.LoadObject(Convert.ToInt32(rcbPlano.SelectedValue));

                    //if (!IdPlanoAdquirido.HasValue && objPlano.FlagRecorrente)
                    //{
                    //    ExibirMensagem("Não é possível cadastrar um Plano Recorrente, apenas editar planos existentes.", TipoMensagem.Aviso);
                    //    return;
                    //}

                    PlanoAdquirido objPlanoAdquirido = Salvar(objPlano);
                    CarregarGrid();
                    AjustarVisibilidadeBotaoLiberarAdicional(objPlanoAdquirido);
                    upDados.Update();
                    btnFiltrar_Click(btnFiltrar, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region rcbPlano_SelectedIndexChanged
        protected void rcbPlano_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(rcbPlano.SelectedValue))
            {
                Plano objPlano = Plano.LoadObject(Convert.ToInt32(rcbPlano.SelectedValue));
               
                txtDataInicioPlano.ValorDatetime = DateTime.Today;
                txtDataFimPlano.ValorDatetime = DateTime.Today.AddDays(objPlano.QuantidadeDiasValidade);
                txtValorParcela.Valor = objPlano.ValorBase;
                chkRegistraBoleto.Checked = objPlano.FlagBoletoRegistrado;
                ValorTotalPlano = objPlano.QuantidadeParcela * objPlano.ValorBase;
                CarregaDdlQuantidadeParcelas(objPlano.QuantidadeParcela, objPlano.FlagCustomizaParcela);
                if (objPlano.FlagRecorrente)
                {
                    chkRegistraBoleto.Enabled = false;
                    chkNotaAntecipada.Enabled = false;
                    divPlanoPagamentoRecorrente.Visible = ckbPagamentoRecorrenteMensal.Checked = true;
                }
                else
                {
                    chkRegistraBoleto.Enabled = true;
                    chkNotaAntecipada.Enabled = true;
                    divPlanoPagamentoRecorrente.Visible = ckbPagamentoRecorrenteMensal.Checked = false;
                }
            }
        }
        #endregion

        #region btnAtualizarPlano_Click
        protected void btnAtualizarPlano_Click(object sender, EventArgs e)
        {
            try
            {
                if (IdFilial.HasValue)
                {
                    PlanoAdquirido objPlanoAdquirido;
                    if (PlanoAdquirido.CarregarPlanoVigentePessoaJuridicaPorSituacao(new Filial(IdFilial.Value), new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado), out objPlanoAdquirido))
                    {
                        objPlanoAdquirido.Plano.CompleteObject();

                        PlanoQuantidade objPlanoQuantidade;
                        if (PlanoQuantidade.CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoQuantidade))
                        {
                            var descricaoParametros = new
                            {
                                NomePlano = objPlanoAdquirido.Plano.DescricaoPlano,
                                VisualizacaoAnterior = objPlanoQuantidade.QuantidadeVisualizacao,
                                SMSAnterior = objPlanoQuantidade.QuantidadeSMS,
                                CampanhaAnterior = objPlanoQuantidade.QuantidadeCampanha,
                                VisualizacaoAtual = txtQuantidadeVisualizacao.Valor,
                                SMSAtual = txtQuantidadeSMS.Valor,
                                CampanhaAtual = txtQuantidadeCampanha.Valor
                            };

                            objPlanoQuantidade.QuantidadeVisualizacao = Convert.ToInt32(txtQuantidadeVisualizacao.Valor);
                            objPlanoQuantidade.QuantidadeSMS = Convert.ToInt32(txtQuantidadeSMS.Valor);
                            objPlanoQuantidade.QuantidadeCampanha = Convert.ToInt16(txtQuantidadeCampanha.Valor);
                            objPlanoQuantidade.Save();

                            //Atualizar Cota do Tanque
                            CelularSelecionador.HabilitarDesabilitarUsuarios(new Filial(IdFilial.Value), objPlanoQuantidade.QuantidadeSMS);


                            const string templateDescricao = "Plano {NomePlano} atualizado. Visualizacao: {VisualizacaoAnterior} para {VisualizacaoAtual} / SMS: {SMSAnterior} para {SMSAtual} / Campanha: {CampanhaAnterior} para {CampanhaAtual}";

                            var objFilialObservacao = new FilialObservacao
                            {
                                DescricaoObservacao = descricaoParametros.ToString(templateDescricao),
                                Filial = new Filial((int)IdFilial),
                                FlagSistema = true,
                                UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value)
                            };
                            objFilialObservacao.Save();

                            txtQuantidadeSMS.Valor = string.Empty;
                            txtQuantidadeVisualizacao.Valor = string.Empty;
                            upDados.Update();
                        }
                    }
                }
                base.ExibirMensagemConfirmacao("Confirmação de Liberação", "Plano atualizado com sucesso!", false);
                CarregarGrid();
                mpeLiberarCliente.Hide();
                btnFiltrar_Click(btnFiltrar, new EventArgs());
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnLiberarPlano_Click
        protected void btnLiberarPlano_Click(object sender, EventArgs e)
        {
            mpeLiberarCliente.Show();
        }
        #endregion

        #region btnLiberarPlanoAdicional_Click
        protected void btnLiberarPlanoAdicional_Click(object sender, EventArgs e)
        {
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value);
            CarregarPlanosAdicionais(objPlanoAdquirido);
            mpeLiberarPlanoAdicional.Show();
        }
        #endregion

        #region btnLiberarCliente_Click
        protected void btnLiberarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value);
                PlanoAdquirido objPlanoAdquiridoOld = objPlanoAdquirido.Clone() as PlanoAdquirido;

                if(objPlanoAdquirido.LiberarPlanoAdquirido(new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value)) &&
                    objPlanoAdquirido.Plano.IdPlano == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.WebfopagControle50)))
                {
                    Pagamento objPagamento = null;
                    Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento);

                    if (objPagamento.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto)
                        objPagamento.Liberar(DateTime.Today);

                }

                string descricao = MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPlanoAdquiridoOld, objPlanoAdquirido, objPlanoAdquirido.GetType()));

                if (!string.IsNullOrEmpty(descricao))
                {
                    if (objPlanoAdquirido.ParaPessoaFisica())
                    {
                        Curriculo objCurriculo;
                        Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo);
                        MensagemAssincronoLogCRM.SalvarModificacoesCurriculoCRM(descricao, objCurriculo, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);

                    }
                    else
                    {
                        objPlanoAdquirido.Filial.CompleteObject();
                        MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(descricao, objPlanoAdquirido.Filial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);

                    }
                }

                //Reabilita o botão de criação de adicional, já que o plano foi liberado.
                btnCriarAdicional.Visible = true; upBtnCriarAdicional.Update();
                ExibirMensagemConfirmacao("Confirmação de Liberação", "Plano liberado com sucesso!", false);
                CarregarGrid();
                PreencherCamposPlano(PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value));
                mpeLiberarCliente.Hide();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnCriarAdicional_Click
        protected void btnCriarAdicional_Click(object sender, EventArgs e)
        {
            //Desabilitando a combo de seleção de tipo de adicional
            rcbCriarPlanoAdicionalTipoAdicional.Enabled = false;

            CarregarPlanoSmsEAjustarValoresPadraoPlanoAdicional();
            CarregarListaSugestaoTipoAdicional();

            upCriarPlanoAdicionalDados.Update();

            mpeCriarPlanoAdicional.Show();
        }
        #endregion

        #region btnCriarAdicionalSalvar_Click
        protected void btnCriarAdicionalSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value);
                CriarAdicional(objPlanoAdquirido);
                mpeCriarPlanoAdicional.Hide();
                AjustarVisibilidadeBotaoLiberarAdicional(objPlanoAdquirido);
                base.ExibirMensagem(MensagemAviso._100012, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnCriarAdicionalCancelar_Click
        protected void btnCriarAdicionalCancelar_Click(object sender, EventArgs e)
        {
            mpeCriarPlanoAdicional.Hide();
        }
        #endregion

        #region btnVisualizarInformacoes_Click
        protected void btnVisualizarInformacoes_Click(object sender, EventArgs e)
        {
            ucObservacaoFilial.Inicializar();
            mpeInformacoes.Show();
        }
        #endregion

        #region btnSalvarSituacaoFilial_Click
        protected void btnSalvarSituacaoFilial_Click(object sender, EventArgs e)
        {
            try
            {
                Filial objFilial = Filial.LoadObject(IdFilial.Value);

                //Armazenando os dados da filial antiga
                Filial objFilialAntiga = (Filial)objFilial.Clone();
                objFilialAntiga.SituacaoFilial.CompleteObject();
                SituacaoFilial objSituacaoFilialAntigo = (SituacaoFilial)objFilial.SituacaoFilial.Clone();
                objFilial.SituacaoFilial = SituacaoFilial.LoadObject(Convert.ToInt32(rcbSituacaoEmpresa.SelectedValue));

                //Completando para salvar o mapeamento
                objFilial.Endereco.CompleteObject();
                objFilial.Endereco.Cidade.CompleteObject();

                //Verifica alteração  na quantidadeUsuariosAdicionais
                int quantidadeUsuariosAdicionais = Convert.ToInt32(txtQuantidadeUsuario.Valor) - Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeLimiteUsuarios));
                if (quantidadeUsuariosAdicionais > 0)
                    objFilial.QuantidadeUsuarioAdicional = quantidadeUsuariosAdicionais;
                else
                    objFilial.QuantidadeUsuarioAdicional = null;


                //Usuario logado
                int? idUsuarioFilialPerfilLogado = null;
                if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                    idUsuarioFilialPerfilLogado = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value;

                if (objFilialAntiga != null)
                {
                    #region Filial
                    List<CompareObject.CompareResult> listaAlteracaoFilial = null;
                    if (objFilialAntiga != null && objFilial != null)
                    {
                        listaAlteracaoFilial = CompareObject.CompareList(objFilialAntiga, objFilial, new[] { "ApelidoFilial", "CNAEPrincipal", "NaturezaJuridica", "SituacaoFilial", "Endereco" });

                        List<CompareObject.CompareResult> listaAlteracaoSituacaoFilial = null;
                        if (objSituacaoFilialAntigo != null && objFilial.SituacaoFilial != null)
                            listaAlteracaoSituacaoFilial = CompareObject.CompareList(objSituacaoFilialAntigo, objFilial.SituacaoFilial, new[] { "IdSituacaoFilial", "FlagInativo", "DataCadastro" });

                        if (listaAlteracaoSituacaoFilial != null)
                            listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoSituacaoFilial).ToList();

                        objFilial.SalvarInformacoesEmpresa(idUsuarioFilialPerfilLogado.Value, listaAlteracaoFilial);
                        ExibirMensagemConfirmacao("Confirmação", MensagemAviso._100001, false);
                        mpeInformacoes.Hide();
                    }
                    #endregion
                }

                Task.Factory.StartNew(() => CelularSelecionador.HabilitarDesabilitarUsuarios(objFilial));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnBloqueio_Click
        protected void btnBloqueio_Click(object sender, EventArgs e)
        {
            if (IdPlanoAdquirido.HasValue)
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value);

                if (!(objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Bloqueado))
                    objPlanoAdquirido.BloquearPlano(UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), "Bloqueio por atraso ou falta de pagamento do PlanoAdquirido: " + objPlanoAdquirido.IdPlanoAdquirido);
                else
                    objPlanoAdquirido.DesbloquearPlano(UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), "Desbloqueio manual realizado pelo financeiro do PlanoAdquirido: " + objPlanoAdquirido.IdPlanoAdquirido);
                LimparCampos();
                PreencherCamposPlano(PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value));
                CarregarGrid();
            }
        }
        #endregion

        #region chkNotaAntecipada_CheckedChanged
        protected void chkNotaAntecipada_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNotaAntecipada.Checked)
            {
                divDataNFAntecipada.Visible = true;
                txtDataNFAntecipada.Obrigatorio = true;
            }
            else
            {
                divDataNFAntecipada.Visible = false;
                txtDataNFAntecipada.Obrigatorio = false;
            }
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void delegatePlanoSelecionado(int idPlanoAdquirido);
        public event delegatePlanoSelecionado PlanoSelecionado;
        #endregion

        #region ValidarFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="idContrato"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Funcao objFuncao;
            return Funcao.CarregarPorDescricao(valor, out objFuncao);
        }
        #endregion

        protected void ddlQuantidadeParcela_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox dd = o as RadComboBox;

            decimal valorPlano = CalculaValorParcelado(Convert.ToInt32(dd.SelectedItem.Value));

            txtValorParcela.Valor = valorPlano;
            //txtPlanoCustomizadoPor.Text = valorPlano.ToString("0", formatter);

            //upPlanoCustomizado.Update();
        }


        public void CarregaDdlQuantidadeParcelas(int quantidadeMaximaParcelas, bool customizaParcela)
        {
            RadQuantidadeParcelas.Items.Clear();
            for (int i = 1; i <= quantidadeMaximaParcelas; i++)
            {
                RadQuantidadeParcelas.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            }
            RadQuantidadeParcelas.SelectedValue = quantidadeMaximaParcelas.ToString();

            RadQuantidadeParcelas.Enabled = customizaParcela;
        }

        public decimal CalculaValorParcelado(int quantidadeParcelas)
        {
            decimal valorPlano = ValorTotalPlano / Convert.ToDecimal(quantidadeParcelas);
            return valorPlano;
            
        }

    }
}