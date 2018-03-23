using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.AsyncServices;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using Parametro = BNE.BLL.Parametro;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using BNE.BLL.Custom;
using BNE.StorageManager;
using BNE.StorageManager.Managers;
using System.IO;



namespace BNE.Web.UserControls.Forms.CadastroCurriculo
{
    public partial class DadosComplementares : BaseUserControl
    {

        #region Propriedades

        #region IdPessoaFisica - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID de PessoaFisica
        /// </summary>
        public int? IdPessoaFisica
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

        #region EstadoManutencao
        /// <summary>
        /// Propriedade que armazena e recupera um boolean indicando se o user controls está em estado de manutenção
        /// </summary>
        public bool EstadoManutencao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel5.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }
        #endregion

        #region EnumSituacaoCurriculo
        /// <summary>
        /// Propriedade que armazena e recupera um enum situação curriculo setado pelo usuário administrador no curriculo completo
        /// </summary>
        public Enumeradores.SituacaoCurriculo? EnumSituacaoCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return (Enumeradores.SituacaoCurriculo)(ViewState[Chave.Temporaria.Variavel6.ToString()]);
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

        #endregion

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "DadosComplementares");
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "DadosComplementares");

            Ajax.Utility.RegisterTypeForAjax(typeof(DadosComplementares));
        }
        #endregion

        #region btnAdicionarCidade
        /// <summary>
        /// Adiciona a Cidade na Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdicionarCidade_Click(object sender, EventArgs e)
        {
            try
            {
                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadeDisponivel.Text, out objCidade))
                {
                    //TODO: Performance, armazenar o idcurriculo na viewstate para evitar Load desnecessario
                    Curriculo objCurriculo;
                    Curriculo.CarregarPorPessoaFisica((int)IdPessoaFisica, out objCurriculo);

                    CurriculoDisponibilidadeCidade objCurriculoDisponibilidadeCidade;
                    if (!CurriculoDisponibilidadeCidade.CarregarPorCurriculoCidade(objCurriculo, objCidade, out objCurriculoDisponibilidadeCidade))
                    {
                        objCurriculoDisponibilidadeCidade = new CurriculoDisponibilidadeCidade
                        {
                            Cidade = objCidade,
                            Curriculo = objCurriculo
                        };
                    }
                    objCurriculoDisponibilidadeCidade.FlagInativo = false;

                    objCurriculoDisponibilidadeCidade.Save();
                    CarregarGridCidade();
                }
                else
                    ExibirMensagem(MensagemAviso._100007, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnFinalizarCadastro
        /// <summary>
        /// Evento disparado no click do btnSalvar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnFinalizarCadastro_Click(object sender, EventArgs e)
        {
            try
            {
                if (Salvar())
                {
                    EfetuarLogin();
                    LimparCampos();

                    if (IdPessoaFisica.HasValue)
                        Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoRevisao.ToString(), null));
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnCancelar
        /// <summary>
        /// Evento disparado no click do btnCancelar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        #endregion

        #region gvCidadeRowDeleting
        protected void gvCidadeRowDeleting(object source, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvCidade.DataKeys[e.RowIndex]["Idf_Curriculo_Disponibilidade_Cidade"]);
                CurriculoDisponibilidadeCidade objCurriculoDisponibilidadeCidade = CurriculoDisponibilidadeCidade.LoadObject(id);
                objCurriculoDisponibilidadeCidade.FlagInativo = true;
                objCurriculoDisponibilidadeCidade.Save();
                CarregarGridCidade();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvModeloAno_RowDeleting
        protected void gvModeloAno_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvModeloAno.DataKeys[e.RowIndex]["Idf_Veiculo"]);
                PessoaFisicaVeiculo objPessoaFisicaVeiculo = PessoaFisicaVeiculo.LoadObject(id);
                objPessoaFisicaVeiculo.FlagInativo = true;
                objPessoaFisicaVeiculo.Save();
                CarregarGridVeiculos();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlMiniCurriculo
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlMiniCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("CadastroDadosComplementares");
                if (Page.IsValid)
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                }

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosPessoais
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosPessoais_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("CadastroDadosComplementares");
                if (Page.IsValid)
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                }

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoDados.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlFormacaoCursos_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlFormacaoCursos_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("CadastroDadosComplementares");
                if (Page.IsValid)
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                }

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoFormacao.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlRevisaoDados_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlRevisaoDados_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("CadastroDadosComplementares");
                if (Page.IsValid)
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                }

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoRevisao.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlGestao_Click
        /// <summary>
        /// habilitar aba Gestao
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlGestao_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("CadastroDadosComplementares");
                if (Page.IsValid)
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                }

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect("~/CadastroCurriculoGestao.aspx");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region bntAdicionarVeiculo_Click
        /// <summary>
        /// Adiciona o Veículo na Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bntAdicionarVeiculo_Click(object sender, EventArgs e)
        {
            try
            {
                var objPessoaFisicaVeiculo = new PessoaFisicaVeiculo
                    {
                        PessoaFisica = new PessoaFisica(IdPessoaFisica.Value),
                        AnoVeiculo = Convert.ToInt16(txtAnoVeiculo.Valor),
                        DescricaoModelo = txtModelo.Valor,
                        TipoVeiculo = new TipoVeiculo(Convert.ToInt32(ddlTipoVeiculo.SelectedValue))
                    };
                objPessoaFisicaVeiculo.Save();
                CarregarGridVeiculos();
                LimparCamposVeiculo();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }

        #endregion

        #region auUpload_FileUploaded
        protected void auUpload_FileUploaded(object sender, FileUploadedEventArgs e)
        {
        }
        #endregion

        #region btiExcluirAnexo_Click
        protected void btiExcluirAnexo_Click(object sender, EventArgs e)
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            if (PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
            {
                //apagar do Storage
                string urlArquivo = objPessoaFisica.CPF + "_" + objPessoaFisicaComplemento.NomeAnexo;

                BLL.StorageManager.ApagarArquivo("curriculos", urlArquivo);

                objPessoaFisicaComplemento.NomeAnexo = String.Empty;
                objPessoaFisicaComplemento.ArquivoAnexo = null;
                objPessoaFisicaComplemento.Save();
                pnlAsyncUpload.Visible = true;
                pnlUploadedFile.Visible = false;
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        /// <summary>
        /// Método utilizado para para preenchimento de componentes, funções de foco e navegação
        /// </summary>
        private void Inicializar()
        {
            if (EstadoManutencao)
            {
                pnlBotoes.Visible = false;
                pnlAbas.Visible = false;
                litTitulo.Text = "Dados Complementares";

            }

            //Carregar DropDownList
            UIHelper.CarregarDropDownList(ddlTipoVeiculo, TipoVeiculo.Listar(), "Idf_Tipo_Veiculo", "Des_tipo_Veiculo", new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlHabilitacao, CategoriaHabilitacao.Listar(), "Idf_Categoria_Habilitacao", "Des_Categoria_Habilitacao", new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlRaca, Raca.Listar(), "Idf_Raca", "Des_Raca_BNE", new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlDeficiencia, Deficiencia.Listar());
            ddlDeficiencia.SelectedValue = "0";
            UIHelper.CarregarDropDownList(ddlFilhos, Flag.Listar(), "Idf_Flag", "Des_Flag", new ListItem("Selecione", "-1"));

            btlGestao.Visible = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue;

            LimparCampos();
            PreencherCampos();

            //Foco Inicial
            ddlTipoVeiculo.Focus();
            upAbaDadosComplementares.Update();
            CarregarGridCidade();

            UIHelper.ValidateFocus(btnFinalizarCadastro);
            UIHelper.ValidateFocus(btlDadosPessoais);
            UIHelper.ValidateFocus(btlFormacaoCursos);
            UIHelper.ValidateFocus(btlMiniCurriculo);
        }
        #endregion

        #region PreencherCampos
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCampos()
        {
            if (IdPessoaFisica.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                CarregarGridVeiculos();

                PessoaFisicaComplemento objPessoaFisicaComplemento;
                if (PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
                {
                    //Carrega Habilitacao        
                    if (objPessoaFisicaComplemento.CategoriaHabilitacao != null)
                        ddlHabilitacao.SelectedValue = objPessoaFisicaComplemento.CategoriaHabilitacao.IdCategoriaHabilitacao.ToString(CultureInfo.CurrentCulture);

                    //Carrega Altura e Peso
                    txtAltura.Valor = objPessoaFisicaComplemento.NumeroAltura;
                    txtPeso.Valor = objPessoaFisicaComplemento.NumeroPeso;

                    txtOutrosConhecimentos.Valor = objPessoaFisicaComplemento.DescricaoConhecimento;

                    txtHabilitacao.Valor = objPessoaFisicaComplemento.NumeroHabilitacao;

                    //Carrega Raça
                    if (objPessoaFisicaComplemento.FlagFilhos != null)
                        ddlFilhos.SelectedValue = objPessoaFisicaComplemento.FlagFilhos.Value ? "1" : "0";

                    if (objPessoaFisicaComplemento.FlagViagem.HasValue)
                        ckbDisponibilidadeViagem.Checked = objPessoaFisicaComplemento.FlagViagem.Value;

                    txtComplementoDeficiencia.Text = objPessoaFisicaComplemento.DescricaoComplementoDeficiencia;

                    //checar anexos do CV
                    if (objPessoaFisicaComplemento.NomeAnexo != null)
                    {
                        string objPath = objPessoaFisica.CPF + "_" + objPessoaFisicaComplemento.NomeAnexo;
                        if (BLL.StorageManager.ArquivoExiste("curriculos", objPath))
                        {
                            hlDownload.NavigateUrl = GetRouteUrl("Download", new { tipoArquivo = "cv", infoArquivo = Helper.Criptografa(objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica.ToString()) });
                            hlDownload.Text = objPessoaFisicaComplemento.NomeAnexo;
                            pnlUploadedFile.Visible = true;
                            pnlAsyncUpload.Visible = false;
                        }
                        else
                        {
                            pnlUploadedFile.Visible = false;
                            pnlAsyncUpload.Visible = true;
                        }
                    }
                }

                //Carrega Observação - Outros Conhecimentos
                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo))
                {
                    LimparCheckBoxListDisponibilidade();
                    List<CurriculoDisponibilidade> lstCurriculoDisponibilidade = CurriculoDisponibilidade.Listar(objCurriculo.IdCurriculo);

                    if (lstCurriculoDisponibilidade.Count > 0)
                    {

                        foreach (CurriculoDisponibilidade objCurriculoDisponibilidade in lstCurriculoDisponibilidade)
                        {
                            if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals(Enumeradores.Disponibilidade.Manha.GetHashCode()))
                                ckblDisponibilidade.Items.FindByText("Manhã").Selected = true;
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals(Enumeradores.Disponibilidade.Tarde.GetHashCode()))
                                ckblDisponibilidade.Items.FindByText("Tarde").Selected = true;
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals(Enumeradores.Disponibilidade.Noite.GetHashCode()))
                                ckblDisponibilidade.Items.FindByText("Noite").Selected = true;
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals(Enumeradores.Disponibilidade.Sabado.GetHashCode()))
                                ckblDisponibilidade.Items.FindByText("Sábado").Selected = true;
                            else if (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.Equals(Enumeradores.Disponibilidade.Domingo.GetHashCode()))
                                ckblDisponibilidade.Items.FindByText("Domingo").Selected = true;
                        }
                    }
                    else
                        AjustarDisponibilidadeTrabalhoDefault();

                    txtObservacoes.Valor = objCurriculo.ObservacaoCurriculo;


                }

                //Carrega Raça
                if (objPessoaFisica.Raca != null)
                    ddlRaca.SelectedValue = objPessoaFisica.Raca.IdRaca.ToString(CultureInfo.CurrentCulture);

                //Deficiência
                if (objPessoaFisica.Deficiencia != null)
                    ddlDeficiencia.SelectedValue = objPessoaFisica.Deficiencia.IdDeficiencia.ToString(CultureInfo.CurrentCulture);
            }
        }
        #endregion

        #region LimparCampos
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        private void LimparCampos()
        {
            ddlTipoVeiculo.SelectedValue = "0";
            ddlHabilitacao.SelectedValue = "0";
            txtAnoVeiculo.Valor = String.Empty;
            txtModelo.Valor = String.Empty;
            txtHabilitacao.Valor = String.Empty;
            txtObservacoes.Valor = String.Empty;
            txtOutrosConhecimentos.Valor = String.Empty;
            ddlRaca.SelectedValue = "0";
            txtAltura.Valor = null;
            txtPeso.Valor = null;
            ddlFilhos.SelectedValue = "-1";
            txtComplementoDeficiencia.Text = String.Empty;
            //ckbManha.Checked = ckbTarde.Checked = ckbNoite.Checked = false;
            ckblDisponibilidade.SelectedValue = String.Empty;
            ckbDisponibilidadeViagem.Checked = false;
            txtCidadeDisponivel.Text = String.Empty;
            gvModeloAno.DataSource = null; gvModeloAno.DataBind();
            upAbaDadosComplementares.Update();
        }
        #endregion

        #region LimparCamposVeiculo
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        private void LimparCamposVeiculo()
        {
            txtAnoVeiculo.Valor = String.Empty;
            txtModelo.Valor = String.Empty;
            ddlTipoVeiculo.SelectedIndex = 0;
            upAbaDadosComplementares.Update();
        }
        #endregion

        #region CarregarGridVeiculos
        /// <summary>
        /// Carrega grid Veiculos
        /// </summary>
        private void CarregarGridVeiculos()
        {
            UIHelper.CarregarGridView(gvModeloAno, PessoaFisicaVeiculo.ListarVeiculosDT(IdPessoaFisica.Value));
        }
        #endregion

        #region CarregarGridCidade
        /// <summary>
        /// Carrega grid Cidades Disponiveis Por Curriculo
        /// </summary>
        protected void CarregarGridCidade()
        {
            //TODO: Performance, armazenar o idcurriculo na viewstate para evitar Load desnecessario
            Curriculo objCurriculo;
            Curriculo.CarregarPorPessoaFisica((int)IdPessoaFisica, out objCurriculo);
            UIHelper.CarregarGridView(gvCidade, CurriculoDisponibilidadeCidade.ListarCidadesPorCurriculo(objCurriculo.IdCurriculo));
        }
        #endregion

        #region Salvar
        /// <summary>
        /// Método utilizado para salvar um cadastro
        /// </summary>
        public bool Salvar()
        {
            var lstCurriculoDisponibilidade = new List<CurriculoDisponibilidade>();

            if (IdPessoaFisica.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                PessoaFisicaComplemento objPessoaFisicaComplemento;
                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
                    objPessoaFisicaComplemento = new PessoaFisicaComplemento();

                if (ddlHabilitacao.SelectedValue.Equals("0"))
                    objPessoaFisicaComplemento.CategoriaHabilitacao = null;
                else
                    objPessoaFisicaComplemento.CategoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(ddlHabilitacao.SelectedValue));

                objPessoaFisicaComplemento.NumeroHabilitacao = txtHabilitacao.Valor;
                objPessoaFisicaComplemento.NumeroAltura = txtAltura.Valor;
                objPessoaFisicaComplemento.NumeroPeso = txtPeso.Valor;

                if (!ddlFilhos.SelectedValue.Equals("-1"))
                    objPessoaFisicaComplemento.FlagFilhos = !ddlFilhos.SelectedValue.Equals("0");
                else
                    objPessoaFisicaComplemento.FlagFilhos = null;

                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                {
                    CurriculoDisponibilidade objCurriculoDisponibilidade;

                    if (ckblDisponibilidade.Items.FindByText("Manhã").Selected)
                    {
                        objCurriculoDisponibilidade = new CurriculoDisponibilidade
                            {
                                Curriculo = objCurriculo,
                                Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Manha)
                            };
                        lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
                    }

                    if (ckblDisponibilidade.Items.FindByText("Tarde").Selected)
                    {
                        objCurriculoDisponibilidade = new CurriculoDisponibilidade
                            {
                                Curriculo = objCurriculo,
                                Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Tarde)
                            };
                        lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
                    }

                    if (ckblDisponibilidade.Items.FindByText("Noite").Selected)
                    {
                        objCurriculoDisponibilidade = new CurriculoDisponibilidade
                            {
                                Curriculo = objCurriculo,
                                Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Noite)
                            };
                        lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
                    }

                    if (ckblDisponibilidade.Items.FindByText("Sábado").Selected)
                    {
                        objCurriculoDisponibilidade = new CurriculoDisponibilidade
                            {
                                Curriculo = objCurriculo,
                                Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Sabado)
                            };
                        lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
                    }

                    if (ckblDisponibilidade.Items.FindByText("Domingo").Selected)
                    {
                        objCurriculoDisponibilidade = new CurriculoDisponibilidade
                            {
                                Curriculo = objCurriculo,
                                Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Domingo)
                            };
                        lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
                    }

                    if (ckbDisponibilidadeViagem.Checked)
                    {
                        objCurriculoDisponibilidade = new CurriculoDisponibilidade
                            {
                                Curriculo = objCurriculo,
                                Disponibilidade = new Disponibilidade((int)Enumeradores.Disponibilidade.Viagem)
                            };
                        lstCurriculoDisponibilidade.Add(objCurriculoDisponibilidade);
                    }

                    objCurriculo.ObservacaoCurriculo = txtObservacoes.Valor;
                }

                objPessoaFisicaComplemento.FlagViagem = ckbDisponibilidadeViagem.Checked;

                objPessoaFisicaComplemento.DescricaoConhecimento = txtOutrosConhecimentos.Valor;

                //Raça
                if (ddlRaca.SelectedValue.Equals("0"))
                    objPessoaFisica.Raca = null;
                else
                    objPessoaFisica.Raca = new Raca(Convert.ToInt32(ddlRaca.SelectedValue));

                //Deficiência
                objPessoaFisica.Deficiencia = new Deficiencia(Convert.ToInt32(ddlDeficiencia.SelectedValue));
                objPessoaFisicaComplemento.DescricaoComplementoDeficiencia = txtComplementoDeficiencia.Text;

                if (!auUpload.UploadedFiles.Count.Equals(0))
                {
                    UploadedFile objFile = auUpload.UploadedFiles[auUpload.UploadedFiles.Count - 1]; //Recuperando o último arquivo adicionado
                    var byteArray = new byte[objFile.InputStream.Length];
                    objFile.InputStream.Read(byteArray, 0, (int)objFile.InputStream.Length);


                    GravarAnexo(objPessoaFisica.CPF.ToString(), byteArray, objFile.FileName);

                    objPessoaFisicaComplemento.NomeAnexo = objFile.FileName;
                }

                objCurriculo.SalvarDadosComplementares(objPessoaFisica, objPessoaFisicaComplemento, lstCurriculoDisponibilidade, EnumSituacaoCurriculo);

                #region Publicação Currículo
                if (!objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals(Enumeradores.SituacaoCurriculo.Bloqueado.GetHashCode()))
                {
                    try
                    {
                        var enfileiraPublicacao = Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.EnfileiraPublicacaoAutomaticaCurriculo));
                        if (enfileiraPublicacao)
                        {
                            var parametros = new ParametroExecucaoCollection
                                    {
                                        {"idCurriculo", "Curriculo", objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture), objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture)}
                                    };

                            ProcessoAssincrono.IniciarAtividade(TipoAtividade.PublicacaoCurriculo, parametros);
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }
                #endregion

                return true;
            }

            return false;
        }
        #endregion

        #region GravarAnexo
        /// <summary>
        /// Gravar anexo do Currículo no Storage
        /// </summary>
        /// <param name="anexo"></param>
        public void GravarAnexo(string cpf, byte[] anexo, string nomeAnexo)
        {
            string objPath = cpf + "_" + nomeAnexo;
            BLL.StorageManager.SalvarArquivo("curriculos", objPath, anexo);
        }
        #endregion

        #region EfetuarLogin
        /// <summary>
        /// Responsável por efetuar o login
        /// </summary>
        private void EfetuarLogin()
        {
            if (IdPessoaFisica.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                //Se a data de nascimento for válida.
                base.IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;
            }
        }
        #endregion

        #region AjustarDisponibilidadeTrabalhoDefault
        /// <summary>
        /// Ajusta os checkbox de disponibilidade de trabalho, caso seja um novo curriculo
        /// </summary>
        private void AjustarDisponibilidadeTrabalhoDefault()
        {
            ckblDisponibilidade.Items.FindByText("Manhã").Selected = true;
            ckblDisponibilidade.Items.FindByText("Tarde").Selected = true;
            ckblDisponibilidade.Items.FindByText("Noite").Selected = false;
            ckblDisponibilidade.Items.FindByText("Sábado").Selected = false;
            ckblDisponibilidade.Items.FindByText("Domingo").Selected = false;
        }
        #endregion

        #region LimparCheckBoxListDisponibilidade
        /// <summary>
        /// Limpa os checkbox de disponibilidade
        /// </summary>
        private void LimparCheckBoxListDisponibilidade()
        {
            ckblDisponibilidade.Items.FindByText("Manhã").Selected = false;
            ckblDisponibilidade.Items.FindByText("Tarde").Selected = false;
            ckblDisponibilidade.Items.FindByText("Noite").Selected = false;
            ckblDisponibilidade.Items.FindByText("Sábado").Selected = false;
            ckblDisponibilidade.Items.FindByText("Domingo").Selected = false;
        }
        #endregion

        #endregion

        #region AjaxMethods

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #endregion

    }
}