using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.UserControls.Modais;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PessoaFisicaFoto = BNE.Web.Handlers.PessoaFisicaFoto;

namespace BNE.Web.UserControls
{
    public partial class VisualizacaoCurriculo : BaseUserControl
    {

        #region Propriedades

        #region IdCurriculoVisualizacaoCurriculo - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int IdCurriculoVisualizacaoCurriculo
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region PalavraChavePesquisa
        /// <summary>
        /// Usado para transporta para a session e usar na visualização do cv aberto
        /// </summary>
        public string PalavraChavePesquisa
        {
            get
            {
                if (ViewState[Chave.Temporaria.PalavraChavePesquisaRapida.ToString()] != null)
                    return ViewState[Chave.Temporaria.PalavraChavePesquisaRapida.ToString()].ToString();
                return string.Empty;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PalavraChavePesquisaRapida.ToString(), value);
            }
        }
        #endregion

        #region IdVagaSession - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdVaga
        {
            get { return base.IdVaga.HasValue ? base.IdVaga.Value : (int?)null; }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucModalLogin.Logar += ucModalLogin_Logar;
        }
        #endregion

        #region btnVerDados_Click
        protected void btnVerDados_Click(object sender, EventArgs e)
        {
            try
            {
                VerDados();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ucModalLogin_Logar
        void ucModalLogin_Logar(string urlDestino)
        {
            VerDados();
        }
        #endregion

        #region rptExperienciaProfissional_ItemCreated
        protected void rptExperienciaProfissional_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            Literal lt = (Literal)e.Item.FindControl("ltExperiencia");
            var lblTempoExperiencia = (Label)e.Item.FindControl("lblTempoExperiencia");

            if (drv != null)
            {
                if (MostrarNomeCandidatoEEmpresaNaExperienciaProfissional())
                {
                    lt.Text = string.Format("<div><span>{0}: ", TratarTexto(drv["Raz_Social"].ToString()));
                    lt.Text += string.Format(" de {0} - até {1} - ", Convert.ToDateTime(drv["Dta_Admissao"].ToString()).ToString("dd/MM/yyyy"), (drv["Dta_Demissao"] != DBNull.Value ? Convert.ToDateTime(drv["Dta_Demissao"].ToString()).ToString("dd/MM/yyyy") : " emprego atual"));
                    lt.Text += string.Format("{0}</span></div>", Helper.CalcularTempoEmprego(drv["Dta_Admissao"].ToString(), (drv["Dta_Demissao"] != DBNull.Value ? drv["Dta_Demissao"].ToString() : DateTime.Now.ToString())));
                }
                else
                    lblTempoExperiencia.Text = string.Format(" - {0}", Helper.CalcularTempoEmprego(drv["Dta_Admissao"].ToString(), (drv["Dta_Demissao"] != DBNull.Value ? drv["Dta_Demissao"].ToString() : DateTime.Now.ToString())));
            }

            //Exibe o último salário somente na última empresa
            if (e.Item.ItemIndex > 0)
            {
                var c = e.Item.FindControl("trLinhaUltimoSalario");
                if (c != null)
                    c.Visible = false;
            }
        }
        #endregion

        #region btnQueroContratarWebEstagios_OnClick
        protected void btnQueroContratarWebEstagios_OnClick(object sender, EventArgs e)
        {
            if (btnVerDados.Visible)
            {
                this.ExibirMensagem("É necessário acessar todos os dados do currículo para enviar uma contratação", TipoMensagem.Aviso);
                return;
            }

            this.ucModalQueroContratarEstagiario.Inicializar();
            this.ucModalQueroContratarEstagiario.MostrarModal(IdCurriculoVisualizacaoCurriculo);
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar(int idCurriculo)
        {
            IdCurriculoVisualizacaoCurriculo = idCurriculo;

            if (!this.IdFilial.HasValue)
                pnlQueroContratarWebEstagios.Visible = false;

            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue) //Se for administrador
                CarregarDados(true);
            else if (base.STC.Value && base.IdOrigem.HasValue) //Se for STC
            {
                bool mostrarDadosCompletos =
                    BLL.CurriculoOrigem.ExisteCurriculoNaOrigem(new Curriculo(IdCurriculoVisualizacaoCurriculo), new Origem(base.IdOrigem.Value));
                CarregarDados(mostrarDadosCompletos);
            }
            else
            {
                var objCurriculo = new Curriculo(IdCurriculoVisualizacaoCurriculo);

                if (base.IdPessoaFisicaLogada.HasValue)
                {
                    if (base.IdFilial.HasValue)
                    {
                        var objFilial = new Filial(base.IdFilial.Value);

                        CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objCurriculo, false, PageHelper.RecuperarIP());
                        CarregarDados(false);

                        if (!objFilial.EmpresaBloqueada())
                        {
                            CurriculoQuemMeViu.SalvarQuemMeViu(objFilial, objCurriculo);
                            if (objFilial.PossuiPlanoElegivel1Clique())
                                VerDados();
                        }
                    }
                    else if (base.IdCurriculo.HasValue)
                    {
                        CarregarDados(IdCurriculoVisualizacaoCurriculo.Equals(base.IdCurriculo.Value));
                    }
                    else
                    {
                        CarregarDados(false);
                    }
                }
                else
                {
                    CarregarDados(false);
                }
            }
        }
        #endregion

        #region CarregarDados
        private void CarregarDados(bool mostrarDadosCompletos)
        {
            LimparCampos();
            PreencherCampos(mostrarDadosCompletos);
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos(bool mostrarDadosCompletos)
        {
            bool buscaSolr, buscaSql;
            buscaSolr = buscaSql = false;
            if (Request.QueryString["source"] != null)
            {
                if (Request.QueryString["source"] == "sql")
                    buscaSql = true;
                if (Request.QueryString["source"] == "solr")
                    buscaSolr = true;
            }

            BLL.DTO.Curriculo objCurriculoDTO = null;
            if ((BLL.PesquisaCurriculo.BuscarSolr || buscaSolr) && !buscaSql)
                objCurriculoDTO = Curriculo.CarregarCurriculoSolr(IdCurriculoVisualizacaoCurriculo);

            if (!BLL.PesquisaCurriculo.BuscarSolr || objCurriculoDTO == null) //Se não for do Solr ou foi do Solr e não carregou o currículo
                objCurriculoDTO = Curriculo.CarregarCurriculoDTO(IdCurriculoVisualizacaoCurriculo, Curriculo.DadosCurriculo.Tudo);

            if (objCurriculoDTO != null)
            {
                lblCodigoCurriculo.Text = objCurriculoDTO.IdCurriculo.ToString(CultureInfo.InvariantCulture);
                lblAtualizacaoCV.Text = objCurriculoDTO.DataAtualizacaoCurriculo.ToShortDateString();

                CarregarNomeCurriculo(objCurriculoDTO.VIP, objCurriculoDTO.NomeCompleto, objCurriculoDTO.PrimeiroNome);
                CarregarResumo(objCurriculoDTO);
                CarregarFoto(objCurriculoDTO.NumeroCPF, mostrarDadosCompletos);

                CarregarDadosPessoais(objCurriculoDTO, mostrarDadosCompletos);
                CarregarFuncoesPretendidasPretensoes(objCurriculoDTO);
                CarregarEscolaridade(objCurriculoDTO);

                CarregarExperienciaProfissional(objCurriculoDTO);

                CarregarObservacoes(objCurriculoDTO);

                AjustarMetaTags(objCurriculoDTO);
            }
            else
                EL.GerenciadorException.GravarExcecao(new Exception(string.Format("Currículo {0} não encontrado!", IdCurriculoVisualizacaoCurriculo)));
        }
        #endregion

        #region RecuperarIdiomasFromDTO
        private DataTable RecuperarIdiomasFromDTO(BLL.DTO.Curriculo objCurriculo)
        {
            var datatable = new DataTable();
            datatable.Columns.Add("Des_Idioma");
            datatable.Columns.Add("Des_Nivel_Idioma");

            foreach (var idiomas in objCurriculo.Idiomas)
            {
                var dr = datatable.NewRow();

                dr["Des_Idioma"] = idiomas.DescricaoIdioma;
                dr["Des_Nivel_Idioma"] = idiomas.NivelIdioma;

                datatable.Rows.Add(dr);
            }

            return datatable;
        }
        #endregion

        #region RecuperarCursosFromDTO
        private DataTable RecuperarCursosFromDTO(BLL.DTO.Curriculo objCurriculo)
        {
            var datatable = new DataTable();
            datatable.Columns.Add("Des_BNE");
            datatable.Columns.Add("Des_Curso");
            datatable.Columns.Add("Sig_Fonte");
            datatable.Columns.Add("Nme_Fonte");
            datatable.Columns.Add("Ano_Conclusao");
            datatable.Columns.Add("Des_Situacao_Formacao");
            datatable.Columns.Add("Num_Periodo");

            foreach (var cursos in objCurriculo.Cursos)
            {
                var dr = datatable.NewRow();

                dr["Des_BNE"] = cursos.DescricaoFormacao;
                dr["Des_Curso"] = cursos.DescricaoCurso;
                dr["Sig_Fonte"] = cursos.SiglaFonte;
                dr["Nme_Fonte"] = cursos.NomeFonte;
                dr["Ano_Conclusao"] = cursos.AnoConclusao;
                dr["Des_Situacao_Formacao"] = cursos.SituacaoFormacao;
                dr["Num_Periodo"] = cursos.Periodo;

                datatable.Rows.Add(dr);
            }

            return datatable;
        }
        #endregion

        #region RecuperarFormacaoFromDTO
        private DataTable RecuperarFormacaoFromDTO(BLL.DTO.Curriculo objCurriculo)
        {
            var datatable = new DataTable();
            datatable.Columns.Add("Des_BNE");
            datatable.Columns.Add("Des_Curso");
            datatable.Columns.Add("Sig_Fonte");
            datatable.Columns.Add("Nme_Fonte");
            datatable.Columns.Add("Ano_Conclusao");
            datatable.Columns.Add("Des_Situacao_Formacao");
            datatable.Columns.Add("Num_Periodo");

            foreach (var formacoes in objCurriculo.Formacoes)
            {
                var dr = datatable.NewRow();

                dr["Des_BNE"] = formacoes.DescricaoFormacao;
                dr["Des_Curso"] = formacoes.DescricaoCurso;
                dr["Sig_Fonte"] = formacoes.SiglaFonte;
                dr["Nme_Fonte"] = formacoes.NomeFonte;
                dr["Ano_Conclusao"] = formacoes.AnoConclusao;
                dr["Des_Situacao_Formacao"] = formacoes.SituacaoFormacao;
                dr["Num_Periodo"] = formacoes.Periodo;

                datatable.Rows.Add(dr);
            }

            return datatable;
        }
        #endregion

        #region RecuperarExperienciaProfissionalFromDTO
        private DataTable RecuperarExperienciaProfissionalFromDTO(BLL.DTO.Curriculo objCurriculo)
        {
            var datatable = new DataTable();
            datatable.Columns.Add("Raz_Social");
            datatable.Columns.Add("Dta_Admissao");
            datatable.Columns.Add("Dta_Demissao");
            datatable.Columns.Add("Des_Area_BNE");
            datatable.Columns.Add("Des_Funcao");
            datatable.Columns.Add("Des_Atividade");
            datatable.Columns.Add("Vlr_Ultimo_Salario");

            foreach (var experiencias in objCurriculo.Experiencias)
            {
                var dr = datatable.NewRow();

                dr["Raz_Social"] = experiencias.RazaoSocial;
                dr["Dta_Admissao"] = experiencias.DataAdmissao;
                dr["Dta_Demissao"] = experiencias.DataDemissao;
                dr["Des_Area_BNE"] = experiencias.AreaBNE;
                dr["Des_Funcao"] = experiencias.Funcao;
                dr["Des_Atividade"] = experiencias.DescricaoAtividade;
                dr["Vlr_Ultimo_Salario"] = experiencias.VlrSalario;

                datatable.Rows.Add(dr);
            }

            return datatable;
        }
        #endregion

        #region CarregarNomeCurriculo
        private void CarregarNomeCurriculo(bool flagVIP, string nomeCompleto, string primeiroNome)
        {
            if (MostrarNomeCandidatoEEmpresaNaExperienciaProfissional())
                litNomeValor.Text = flagVIP ? nomeCompleto : primeiroNome;
            else
                divNomeCandidato.Visible = false;
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            rbiThumbnailVisualizacao.ImageUrl = "/img/img_sem_foto.gif";
            litNomeValor.Text = String.Empty;
            litFuncaoPretendidaValor.Text = String.Empty;
            litPretensaoSalarialValor.Text = String.Empty;
            litIdadeValor.Text = String.Empty;
            litSexoValor.Text = String.Empty;
            litCpfValor.Text = String.Empty;
            litCidadeValor.Text = String.Empty;
            litEnderecoValor.Text = String.Empty;
            litCepValor.Text = String.Empty;
            litTelefoneCelularValor.Text = String.Empty;
            litTelefoneResidencialValor.Text = String.Empty;
            litTelefoneRecadoValor.Text = String.Empty;
            litTelefoneRecadoFalarComValor.Text = String.Empty;
            litCelularRecadoValor.Text = String.Empty;
            litCelularRecadoFalarComValor.Text = String.Empty;
            litHabilitacaoValor.Text = String.Empty;
            imgOperadoraCelular.AlternateText = string.Empty;
            imgOperadoraCelular.ImageUrl = string.Empty;
        }
        #endregion

        #region VerDados
        private void VerDados()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdFilial.HasValue)
                {
                    Filial objFilial = Filial.LoadObject(this.IdFilial.Value);

                    if (!objFilial.EmpresaBloqueada())
                    {
                        if (objFilial.EmpresaEmAuditoria())
                        {
                            int QuantidadeCurriculosVIPVisualizadosPelaEmpresa =
                                 BLL.CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(objFilial);

                            if (objFilial.DataCadastro.DayOfYear == DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 20)
                            {
                                ExibirMensagem("O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.", TipoMensagem.Aviso);
                                return;
                            }
                            else if (objFilial.DataCadastro.DayOfYear < DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 5)
                            {
                                ExibirMensagem("O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.", TipoMensagem.Aviso);
                                return;
                            }
                        }

                        var objCurriculo = new Curriculo(IdCurriculoVisualizacaoCurriculo);
                        var curriculoEstagio = objCurriculo.CurriculoCompativelComEstagio();
                        var autorizacaoPelaWebEstagios = objFilial.AvalWebEstagios() && curriculoEstagio;
                        bool possuiCampanhaEnvioMassaVaga = false;
                        if (IdVaga.HasValue)
                            possuiCampanhaEnvioMassaVaga = PlanoAdquiridoDetalhesCurriculo.RecebeuCampanhaVagaPerfil(objCurriculo, new Vaga((int)IdVaga));

                        if (objFilial.PossuiPlanoAtivo() //Se a empresa possui plano ativo
                            || autorizacaoPelaWebEstagios
                            || possuiCampanhaEnvioMassaVaga) // Se tem o parâmetro especifico da webestagios) 
                        {
                            if (objCurriculo.VIP() || possuiCampanhaEnvioMassaVaga)
                            {
                                VerDadosCompleto(objCurriculo, objFilial);
                            }
                            else
                            {
                                if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                    VerDadosCompleto(objCurriculo, objFilial);
                                else
                                    ExibirMensagem("Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!", TipoMensagem.Aviso);
                            }
                        }
                        else
                        {
                            if (objCurriculo.VIP())
                            {
                                if (objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                    VerDadosCompleto(objCurriculo, objFilial);
                                else
                                    ModalVendaChupaVIP.Inicializar(TipoChuvaVIP.Visualizacao);
                            }
                            else
                                ucModalVendaCIA.Inicializar();
                        }

                        if (!curriculoEstagio)
                        {
                            pnlQueroContratarWebEstagios.Visible = false;
                            upQueroContratar.Update();
                        }
                    }
                    else
                    {
                        pnlQueroContratarWebEstagios.Visible = false;
                        upQueroContratar.Update();
                        ucEmpresaBloqueada.MostrarModal();
                    }
                }
                else
                {
                    pnlQueroContratarWebEstagios.Visible = false;
                    upQueroContratar.Update();
                    base.ExibirMensagem("Somente empresas cadastradas tem acesso a currículos", TipoMensagem.Aviso);
                }
            }
            else
            {
                ucModalLogin.Inicializar();
                ucModalLogin.Mostrar();
            }
        }
        #endregion

        #region VerDadosCompleto
        private void VerDadosCompleto(Curriculo objCurriculo, Filial objFilial)
        {
            CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objCurriculo, true, Common.Helper.RecuperarIP());

            bool buscaSolr, buscaSql;
            buscaSolr = buscaSql = false;
            if (Request.QueryString["source"] != null)
            {
                if (Request.QueryString["source"] == "sql")
                    buscaSql = true;
                if (Request.QueryString["source"] == "solr")
                    buscaSolr = true;
            }

            BLL.DTO.Curriculo objCurriculoDTO = null;
            if ((BLL.PesquisaCurriculo.BuscarSolr || buscaSolr) && !buscaSql)
                objCurriculoDTO = Curriculo.CarregarCurriculoSolr(IdCurriculoVisualizacaoCurriculo);

            if (!BLL.PesquisaCurriculo.BuscarSolr || objCurriculoDTO == null) //Se não for do Solr ou foi do Solr e não carregou o currículo
                objCurriculoDTO = Curriculo.CarregarCurriculoDTO(IdCurriculoVisualizacaoCurriculo, Curriculo.DadosCurriculo.Basico);

            CarregarDadosPessoais(objCurriculoDTO, true);
            CarregarFoto(objCurriculoDTO.NumeroCPF, true);

            if (VerDadosContato != null)
                VerDadosContato();
        }
        #endregion

        #region CarregarFoto
        /// <summary>
        /// Metodo responsável por carregar a foto 
        /// </summary>
        private void CarregarFoto(decimal numeroCPF, bool mostrarDadosCompletos)
        {
            bool existeFoto = false;

            if (mostrarDadosCompletos)
            {
                try
                {
                    existeFoto = Handlers.PessoaFisicaFoto.ExisteFoto(numeroCPF);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }

                if (existeFoto)
                    rbiThumbnailVisualizacao.ImageUrl = RetornarUrlFoto(numeroCPF);
                else
                    rbiThumbnailVisualizacao.ImageUrl = "/img/img_sem_foto.gif";
            }
            else
            {
                rbiThumbnailVisualizacao.ImageUrl = "/img/img_sem_foto.gif";
            }

            upFotoVisualizacaoCurriculo.Update();
        }
        #endregion

        #region CarregarDadosPessoais
        /// <summary>
        /// Dados Pessoais 
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <param name="mostrarDadosCompletos"></param>
        private void CarregarDadosPessoais(BNE.BLL.DTO.Curriculo objCurriculo, bool mostrarDadosCompletos)
        {
            if (mostrarDadosCompletos)
            {
                pnlDadosPessoais.Visible = true;
                pnlVerDados.Visible = btnVerDados.Visible = false;

                litNomeValor.Text = objCurriculo.NomeCompleto;

                divNomeCandidato.Visible = true;

                litSexoValor.Text = TratarTexto(objCurriculo.Sexo);
                if (!string.IsNullOrWhiteSpace(objCurriculo.EstadoCivil))
                    litEstadoCivilValor.Text = TratarTexto(objCurriculo.EstadoCivil);
                else
                    litEstadoCivil.Visible = false;

                litDataNascimentoValor.Text = string.Format("{0} - {1} Anos", objCurriculo.DataNascimento.ToShortDateString(), Helper.CalcularIdade(objCurriculo.DataNascimento));
                litCpfValor.Text = Helper.FormatarCPF(objCurriculo.NumeroCPF);

                #region TelefoneCelular
                if (objCurriculo.NumeroDDDCelular != null && objCurriculo.NumeroCelular != null)
                {
                    litTelefoneCelularValor.Text = UIHelper.FormatarTelefone(objCurriculo.NumeroDDDCelular, objCurriculo.NumeroCelular);

                    if (!string.IsNullOrWhiteSpace(objCurriculo.NomeOperadoraCelular) && !string.IsNullOrWhiteSpace(objCurriculo.URLImagemOperadoraCelular))
                    {
                        imgOperadoraCelular.ImageUrl = objCurriculo.URLImagemOperadoraCelular;
                        imgOperadoraCelular.AlternateText = objCurriculo.NomeOperadoraCelular;
                        imgOperadoraCelular.Visible = true;
                    }
                }
                else
                    litTelefoneCelular.Visible = false;
                #endregion

                #region TelefoneResidencial
                if (!string.IsNullOrWhiteSpace(objCurriculo.NumeroDDDTelefone) && !string.IsNullOrWhiteSpace(objCurriculo.NumeroTelefone))
                    litTelefoneResidencialValor.Text = UIHelper.FormatarTelefone(objCurriculo.NumeroDDDTelefone, objCurriculo.NumeroTelefone);
                else
                    trLinhaTelefoneResidencial.Visible = false;
                #endregion

                #region TelefoneContato,Falar Com
                if (!string.IsNullOrWhiteSpace(objCurriculo.NumeroDDDTelefoneRecado) && !string.IsNullOrWhiteSpace(objCurriculo.NumeroTelefoneRecado))
                    litTelefoneRecadoValor.Text = UIHelper.FormatarTelefone(objCurriculo.NumeroDDDTelefoneRecado, objCurriculo.NumeroTelefoneRecado);
                else
                    litTelefoneRecado.Visible = false;

                if (!string.IsNullOrWhiteSpace(objCurriculo.TelefoneRecadoContato))
                    litTelefoneRecadoFalarComValor.Text = objCurriculo.TelefoneRecadoContato;
                else
                    litTelefoneRecadoFalarCom.Visible = false;
                #endregion

                #region CelularContato,Falar Com
                if (!string.IsNullOrWhiteSpace(objCurriculo.NumeroDDDCelularRecado) && !string.IsNullOrWhiteSpace(objCurriculo.NumeroCelularRecado))
                    litCelularRecadoValor.Text = UIHelper.FormatarTelefone(objCurriculo.NumeroDDDCelularRecado, objCurriculo.NumeroCelularRecado);
                else
                    litCelularRecado.Visible = false;

                if (!string.IsNullOrWhiteSpace(objCurriculo.CelularRecadoContato))
                    litCelularRecadoFalarComValor.Text = objCurriculo.CelularRecadoContato;
                else
                    litCelularRecadoFalarCom.Visible = false;
                #endregion

                #region Endereco,CEP,Cidade
                //Cidade e CEP
                litCidadeValor.Text = string.Format("{0}/{1}", objCurriculo.NomeCidade, objCurriculo.SiglaEstado);
                if (!string.IsNullOrWhiteSpace(objCurriculo.NumeroCEP))
                    litCepValor.Text = objCurriculo.NumeroCEP.Insert(5, "-");
                else
                    litCep.Visible = false;

                //Endereço
                var lstEndereco = new List<string>();
                if (!string.IsNullOrWhiteSpace(objCurriculo.Logradouro))
                    lstEndereco.Add(objCurriculo.Logradouro);
                if (!string.IsNullOrWhiteSpace(objCurriculo.NumeroEndereco))
                    lstEndereco.Add(objCurriculo.NumeroEndereco);
                if (!string.IsNullOrWhiteSpace(objCurriculo.Complemento))
                    lstEndereco.Add(objCurriculo.Complemento);
                if (!string.IsNullOrWhiteSpace(objCurriculo.Bairro))
                    lstEndereco.Add(TratarTexto(objCurriculo.Bairro));

                if (lstEndereco.Count > 0)
                    litEnderecoValor.Text = string.Join(" , ", lstEndereco.Select(e => e.Trim()).ToArray());
                else
                    trLinhaEndereco.Visible = false;
                #endregion

                #region Filhos,Habilitação

                if (objCurriculo.TemFilhos.HasValue)
                    litFilhosValor.Text = objCurriculo.TemFilhos.Equals(true) ? "Sim" : "Não";
                else
                    litFilhos.Visible = false;

                if (!string.IsNullOrWhiteSpace(objCurriculo.CategoriaHabilitacao))
                    litHabilitacaoValor.Text = objCurriculo.CategoriaHabilitacao;
                else
                    litHabilitacao.Visible = false;

                #endregion

            }
            else
            {
                pnlDadosPessoais.Visible = false;
                pnlVerDados.Visible = btnVerDados.Visible = true;
            }
            hfEstagiario.Value = new Curriculo(IdCurriculoVisualizacaoCurriculo).CurriculoCompativelComEstagio().ToString();
            upBotaoVerDados.Update();
            upVerDados.Update();
            upLogoCliente.Update();
            upFotoVisualizacaoCurriculo.Update();
            upNomeCandidato.Update();
        }
        #endregion

        #region CarregarResumo
        private void CarregarResumo(BLL.DTO.Curriculo objCurriculo)
        {
            var listCvResumo = new List<string>();

            if (MostrarNomeCandidatoEEmpresaNaExperienciaProfissional())
                listCvResumo.Add(objCurriculo.PrimeiroNome);

            listCvResumo = listCvResumo.Concat(new List<string>
                {
                    objCurriculo.Sexo, 
                    objCurriculo.EstadoCivil, 
                    objCurriculo.Idade.ToString(), 
                    objCurriculo.UltimaFormacaoAbreviada, 
                    objCurriculo.ValorPretensaoSalarial.HasValue ? objCurriculo.ValorPretensaoSalarial.Value.ToString("N2") : string.Empty, 
                    objCurriculo.Bairro, 
                    objCurriculo.NomeCidade, 
                    objCurriculo.NomeFuncaoPretendida, 
                    objCurriculo.CategoriaHabilitacao
                }).ToList();

            litCVResumo.Text = string.Join(" | ", listCvResumo.Where(c => !string.IsNullOrEmpty(c)).ToArray());

            if (objCurriculo.Deficiencia != null && objCurriculo.Deficiencia.Any())
                pnlPCD.Visible = true;
        }
        #endregion

        #region CarregarFuncoesPretendidasPretensoes
        /// <summary>
        /// Funções Pretendidas e pretensão salarial
        /// </summary>
        /// <param name="objCurriculo">Currículo</param>
        private void CarregarFuncoesPretendidasPretensoes(BLL.DTO.Curriculo objCurriculo)
        {
            if (objCurriculo.FuncoesPretendidas.Count > 0)
                litFuncaoPretendidaValor.Text = string.Join("<br />", objCurriculo.FuncoesPretendidas.Select(fp => fp.NomeFuncaoPretendida));
            else
                litFuncaoPretendida.Visible = false;

            if (objCurriculo.ValorPretensaoSalarial.HasValue)
                litPretensaoSalarialValor.Text = "R$ " + Convert.ToDecimal(objCurriculo.ValorPretensaoSalarial).ToString("N2");
            else
                litPretensaoSalarial.Visible = false;
        }
        #endregion

        #region CarregarEscolaridade
        private void CarregarEscolaridade(BNE.BLL.DTO.Curriculo objCurriculo)
        {
            var dtFormacaoNivel = RecuperarFormacaoFromDTO(objCurriculo);
            var dtIdioma = RecuperarIdiomasFromDTO(objCurriculo);
            var dtFormacaoCursos = RecuperarCursosFromDTO(objCurriculo);

            if (dtIdioma.Rows.Count > 0 || dtFormacaoNivel.Rows.Count > 0 || dtFormacaoCursos.Rows.Count > 0)
            {
                if (dtIdioma.Rows.Count > 0)
                    UIHelper.CarregarRepeater(rptIdiomas, dtIdioma);
                else
                    trLinhaIdiomas.Visible = false;

                if (dtFormacaoNivel.Rows.Count > 0)
                    UIHelper.CarregarRepeater(rptNivel, dtFormacaoNivel);
                else
                    trLinhaEscolaridadeNivel.Visible = false;

                if (dtFormacaoCursos.Rows.Count > 0)
                    UIHelper.CarregarRepeater(rptCursos, dtFormacaoCursos);
                else
                    trLinhaEscolaridadeCursos.Visible = false;
            }
            else
                pnlDadosEscolaridade.Visible = false;
        }
        #endregion

        #region CarregarExperienciaProfissional
        private void CarregarExperienciaProfissional(BNE.BLL.DTO.Curriculo objCurriculo)
        {
            var datatable = RecuperarExperienciaProfissionalFromDTO(objCurriculo);

            if (datatable.Rows.Count > 0)
                UIHelper.CarregarRepeater(rptExperienciaProfissional, datatable);
            else
                pnlDadosExperienciaProfissional.Visible = false;
        }
        #endregion

        #region CarregarObservacoes
        private void CarregarObservacoes(BNE.BLL.DTO.Curriculo objCurriculo)
        {
            #region HorarioDisponivel

            var horario = string.Join(" ", objCurriculo.DisponibilidadesTrabalho.Select(dt => dt.Descricao));

            if (!string.IsNullOrEmpty(horario))
                litHorarioDisponivelValor.Text = horario;
            else
                trLinhaHorarioDisponivel.Visible = false;

            #endregion

            #region Observacoes

            if (!string.IsNullOrWhiteSpace(objCurriculo.Observacao))
                litObservacoes.Text = TratarTexto(objCurriculo.Observacao.ReplaceBreaks());
            else
                trLinhaObservacoes.Visible = false;

            #endregion

            #region VeiculoCurriculo
            if (objCurriculo.Veiculos.Count > 0)
            {
                foreach (var veiculo in objCurriculo.Veiculos)
                    litTipoVeiculoValor.Text += string.Format("{0} - {1} - {2}<br/>", TratarTexto(veiculo.Tipo), veiculo.Modelo, veiculo.Ano);
            }
            else
                trLinhaTipoVeiculo.Visible = false;
            #endregion

            #region Raca/Altura/Peso
            var listCaracteristicasPessoais = new List<string>();
            if (!string.IsNullOrWhiteSpace(objCurriculo.Raca))
            {
                if (!objCurriculo.Raca.Equals("Não Informado"))
                    listCaracteristicasPessoais.Add(objCurriculo.Raca);
            }

            if (objCurriculo.Altura != null && objCurriculo.Altura > 0)
                listCaracteristicasPessoais.Add(Convert.ToDecimal(objCurriculo.Altura).ToString("N2"));

            if (objCurriculo.Peso != null && objCurriculo.Peso > 0)
                listCaracteristicasPessoais.Add(Convert.ToDecimal(objCurriculo.Peso).ToString("N2"));

            #region Conhecimentos

            if (!string.IsNullOrEmpty(objCurriculo.OutrosConhecimentos))
                litOutrosConhecimentosValor.Text = objCurriculo.OutrosConhecimentos.ReplaceBreaks();
            else
                trLinhaOutrosConhecimentos.Visible = false;

            #endregion

            if (listCaracteristicasPessoais.Count > 0)
            {
                string caracteristicasPessoais = string.Empty;
                int i = 0;
                int index = listCaracteristicasPessoais.Count - 1;
                foreach (string caracteristicas in listCaracteristicasPessoais)
                {
                    if (!listCaracteristicasPessoais[i].Equals(listCaracteristicasPessoais[index]))
                        caracteristicasPessoais += caracteristicas + " - ";
                    else
                        caracteristicasPessoais += caracteristicas;

                    i++;
                }

                litCaracteristicasPessoaisValor.Text = caracteristicasPessoais;
            }
            else
                trLinhaCaracteristicasPessoais.Visible = false;
            #endregion

            #region Disponibilidade

            var listaCidadePretendidas = CurriculoDisponibilidadeCidade.ListarCidade(objCurriculo.IdCurriculo);
            if (listaCidadePretendidas.Count > 0)
                litDisponibilidadeMoradiaValor.Text = string.Join(", ", listaCidadePretendidas.ToArray());
            else
                trLinhaDisponibilidadeMoradia.Visible = false;

            if (objCurriculo.DisponibilidadeViajar.HasValue)
                litDisponibilidadeViagensValor.Text = objCurriculo.DisponibilidadeViajar.Equals(true) ? "Sim" : "Não";
            else
                trLinhaDisponibilidadeViagens.Visible = false;

            #endregion

            #region Deficiencia
            if (!string.IsNullOrWhiteSpace(objCurriculo.Deficiencia))
            {
                if (objCurriculo.Deficiencia != "Nenhuma")
                    litTipoDeficiencia.Text = objCurriculo.Deficiencia;
                else
                    trTipoDeficiencia.Visible = false;
            }
            else
                trTipoDeficiencia.Visible = false;

            #endregion
        }
        #endregion

        #region RetornarUrlFoto
        private string RetornarUrlFoto(decimal numeroCPF)
        {
            return UIHelper.RetornarUrlFoto(numeroCPF, PessoaFisicaFoto.OrigemFoto.Local);
        }
        #endregion

        #region TratarTexto
        protected string TratarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(PalavraChavePesquisa))
                return texto;

            return texto.HighlightText(PalavraChavePesquisa, "highlight");
        }
        #endregion

        #region AjustarMetaTags
        private void AjustarMetaTags(BNE.BLL.DTO.Curriculo objCurriculo)
        {
            try
            {
                #region Keywords

                string keywords = objCurriculo.FuncoesPretendidas.Aggregate(string.Empty, (current, funcaoPretendida) => current + (funcaoPretendida.NomeFuncaoPretendida + ","));

                keywords += objCurriculo.NomeCidade + ",";
                keywords += objCurriculo.NomeEstado + ",";

                string atribuicoes = null;

                foreach (var experienciaProfissional in objCurriculo.Experiencias)
                {
                    if (string.IsNullOrEmpty(atribuicoes))
                        atribuicoes = experienciaProfissional.DescricaoAtividade;

                    if (!string.IsNullOrEmpty(experienciaProfissional.RazaoSocial))
                        keywords += experienciaProfissional.RazaoSocial + ",";
                }

                keywords = keywords.Substring(0, keywords.Length - 1);

                if (!string.IsNullOrEmpty(keywords))
                    Page.Header.Controls.AddAt(0, new HtmlMeta { Name = "keywords", Content = keywords });

                #endregion

                #region Description

                #region Primeiro Nome
                string primeiroNomePessoa = objCurriculo.PrimeiroNome;
                #endregion

                #region Idiomas
                string idiomas = objCurriculo.Idiomas.Aggregate<BLL.DTO.Idioma, string>(null, (current, objIdioma) => current + (objIdioma.DescricaoIdioma + ", "));
                #endregion

                #region Pretensão
                string valorPretensao = string.Empty;
                if (objCurriculo.ValorPretensaoSalarial.HasValue)
                    valorPretensao = objCurriculo.ValorPretensaoSalarial.Value.ToString("C");
                #endregion

                string descricao = string.Format("{0}, {6}, {1} anos, {2}{3}com pretensão salarial de {4}. Última experiência: {5}", primeiroNomePessoa, objCurriculo.Idade, string.IsNullOrEmpty(objCurriculo.UltimaFormacaoCompleta) ? string.Empty : objCurriculo.UltimaFormacaoCompleta + ", ", string.IsNullOrEmpty(idiomas) ? string.Empty : idiomas, valorPretensao, atribuicoes, objCurriculo.NomeFuncaoPretendida);

                //Limitando para 255 caracteres
                descricao = descricao.Length > 255 ? descricao.Substring(0, 255) : descricao;

                if (!string.IsNullOrEmpty(descricao))
                    Page.Header.Controls.AddAt(0, new HtmlMeta { Name = "description", Content = descricao });

                #endregion

                if (objCurriculo.Inativo || objCurriculo.Bloqueado)
                {
                    var metaTagNoindex = new HtmlMeta { Name = "robots", Content = "noindex, nofollow" };
                    Page.Header.Controls.AddAt(0, metaTagNoindex);
                }

                Page.Title = SitemapHelper.MontarTituloCurriculo(objCurriculo.NomeFuncaoPretendida, objCurriculo.NomeCidade, objCurriculo.NomeEstado, objCurriculo.SiglaEstado, objCurriculo.IdCurriculo);

            }
            catch (Exception ex)
            {
                string message = "Falha ao ajudar SEO para o currículo " + objCurriculo.IdCurriculo.ToString();
                EL.GerenciadorException.GravarExcecao(ex, message);
            }
        }
        #endregion

        #endregion

        public delegate void VerDadosContatoHandler();
        public event VerDadosContatoHandler VerDadosContato;

    }

    #region Enumerador: Formato
    public enum Formato
    {
        BNE,
        Loja
    }
    #endregion

}