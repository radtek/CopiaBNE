using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code.ViewStateObjects;
using BNE.Web.UserControls.Modais;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BNE.BLL.Common;
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

        #region IdPesquisaCurriculo - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o id da pesquisa de curriculo atual
        /// </summary>
        private int? IdPesquisaCurriculo
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

        #region IdVaga - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdVaga
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

        #region IdRastreadorCurriculo - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdRastreadorCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel5.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel5.ToString());
            }
        }
        #endregion

        #region MostrarNomeCandidatoEEmpresaNaExperienciaProfissional - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o booleano se deve ou não mostrar o nome do candidato ou o nome da empresa na experiencia profissional
        /// </summary>
        private bool MostrarNomeCandidatoEEmpresaNaExperienciaProfissional
        {
            get
            {
                return MostrarNomeCandidatoEEmpresaNaExperienciaProfissional();
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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    if (Request.QueryString["visualizar"] != null && Request.QueryString["visualizar"].Equals("true"))
                        VerDados();
                }
                catch (Exception)
                {
                }
                   
            }
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
                if (MostrarNomeCandidatoEEmpresaNaExperienciaProfissional)
                {
                    lt.Text = string.Format("<div><span>{0}: ", TratarTexto(drv["Raz_Social"].ToString()));
                    lt.Text += string.Format(" de {0} - até {1} - ", Convert.ToDateTime(drv["Dta_Admissao"].ToString()).ToString("dd/MM/yyyy"), (drv["Dta_Demissao"] != DBNull.Value ? Convert.ToDateTime(drv["Dta_Demissao"].ToString()).ToString("dd/MM/yyyy") : " emprego atual"));
                    lt.Text += string.Format("{0}</span></div>", BNE.BLL.Custom.Helper.CalcularTempoEmprego(drv["Dta_Admissao"].ToString(), (drv["Dta_Demissao"] != DBNull.Value ? drv["Dta_Demissao"].ToString() : DateTime.Now.ToString())));
                }
                else
                    lblTempoExperiencia.Text = string.Format(" - {0}", BNE.BLL.Custom.Helper.CalcularTempoEmprego(drv["Dta_Admissao"].ToString(), (drv["Dta_Demissao"] != DBNull.Value ? drv["Dta_Demissao"].ToString() : DateTime.Now.ToString())));
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
        public void Inicializar(int idCurriculo, int? idPesquisaCurriculo = null, int? idVaga = null, int? idRastreadorCurriculo = null)
        {
            IdCurriculoVisualizacaoCurriculo = idCurriculo;
            IdPesquisaCurriculo = idPesquisaCurriculo;
            IdVaga = idVaga;
            IdRastreadorCurriculo = idRastreadorCurriculo;

            if (base.IdFilial.HasValue && CurriculoNaoVisivelFilial.CurriculoVisivelParaEmpresa(idCurriculo, base.IdFilial.Value))
                Response.RedirectToRoute("SalaSelecionador");
            if (!this.IdFilial.HasValue)
                pnlQueroContratarWebEstagios.Visible = false;

            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue) //Se for administrador
                CarregarDados(true);
            else if (base.STC.Value && base.IdOrigem.HasValue) //Se for STC
            {
                bool mostrarDadosCompletos = BLL.CurriculoOrigem.ExisteCurriculoNaOrigem(new Curriculo(IdCurriculoVisualizacaoCurriculo), new Origem(base.IdOrigem.Value));
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

                        #region PesquisaCV
                        BLL.PesquisaCurriculo objPesquisaCurriculo = null;
                        if (IdPesquisaCurriculo.HasValue)
                            objPesquisaCurriculo = new BLL.PesquisaCurriculo(IdPesquisaCurriculo.Value);
                        #endregion

                        #region Vaga
                        Vaga objVaga = null;
                        if (IdVaga.HasValue)
                            objVaga = new Vaga(IdVaga.Value);
                        #endregion

                        #region RastreadorCurriculo
                        RastreadorCurriculo objRastreadorCurriculo = null;
                        if (IdRastreadorCurriculo.HasValue)
                            objRastreadorCurriculo = new RastreadorCurriculo(IdRastreadorCurriculo.Value);
                        #endregion

                        CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value), objCurriculo, false, PageHelper.RecuperarIP(), objPesquisaCurriculo, objVaga, objRastreadorCurriculo);
                        CarregarDados(false);

                        if (!objFilial.EmpresaBloqueada())
                        {
                            CurriculoQuemMeViu.SalvarQuemMeViuSite(objFilial, objCurriculo, base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
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
            BLL.PesquisaCurriculo objPesquisaCurriculo = null;
            //carregar pesquisa para pegar a(s) palavra(s) chave(s) pesquisadas
            if(IdPesquisaCurriculo.HasValue)
                objPesquisaCurriculo = BLL.PesquisaCurriculo.LoadObject(IdPesquisaCurriculo.Value);

            //Se não for do Solr ou foi do Solr e não carregou o currículo
            var objCurriculoDTO = Curriculo.CarregarCurriculoSolr(IdCurriculoVisualizacaoCurriculo, objPesquisaCurriculo != null ? objPesquisaCurriculo.DescricaoPalavraChave : "") ?? Curriculo.CarregarCurriculoDTO(IdCurriculoVisualizacaoCurriculo, Curriculo.DadosCurriculo.Tudo);

            if (objCurriculoDTO != null && !objCurriculoDTO.Inativo)
            {
                lblCodigoCurriculo.Text = objCurriculoDTO.IdCurriculo.ToString(CultureInfo.InvariantCulture);
                lblAtualizacaoCV.Text = objCurriculoDTO.DataAtualizacaoCurriculo.ToShortDateString();

                CarregarNomeCurriculo(objCurriculoDTO.VIP, objCurriculoDTO.NomeCompleto, objCurriculoDTO.PrimeiroNome);
                CarregarResumo(objCurriculoDTO);
                CarregarFoto(objCurriculoDTO.NumeroCPF, mostrarDadosCompletos);

                CarregarDadosPessoais(objCurriculoDTO, mostrarDadosCompletos);
                CarregarFuncoesPretendidasPretensoes(objCurriculoDTO);
                CarregarEscolaridade(objCurriculoDTO);

                CarregarExperienciaProfissional(objCurriculoDTO, objPesquisaCurriculo);

                CarregarObservacoes(objCurriculoDTO);

                AjustarMetaTags(objCurriculoDTO);
            }
            else if (objCurriculoDTO != null && objCurriculoDTO.Inativo)
            {
                //Gerar uma pesquisa de Curriculo antes de redirecionar
                string funcaoPretendida = objCurriculoDTO.FuncoesPretendidas.FirstOrDefault().NomeFuncaoPretendida;
                string cidade = objCurriculoDTO.NomeCidade;
                var url = ConfigurationManager.AppSettings["urlSite"];
                string rota = Rota.BuscarPorRouteName(BNE.BLL.Enumeradores.RouteCollection.PesquisaCurriculo.ToString()).DescricaoURL.ToString();

               // BLL.PesquisaCurriculo objPesquisaCurriculo;
                BLL.PesquisaCurriculo.RecuperarDadosPesquisaCurriculo(funcaoPretendida, cidade, null, null, null, objCurriculoDTO.IdCurriculo, out objPesquisaCurriculo);

                //Gravar o resutlado da pesquisa na session
                //A pagina de pesquiva vai buscar esse resultado
                TipoBusca.Value = TipoBuscaMaster.Curriculo;
                Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo }));

                //redirecionar para pesquisa de CV da Cidade e função do CV em questão.
                Response.Status = "301 Moved Permanently";
                Response.Redirect(url + "/" + rota, true);
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
        private DataTable RecuperarExperienciaProfissionalFromDTO(BLL.DTO.Curriculo objCurriculo, BNE.BLL.PesquisaCurriculo objPesquisaCurriculo)
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
                string[] termos = null;
                string funcao = experiencias.Funcao;
                string atividades = experiencias.DescricaoAtividade;


                if (objCurriculo.highlighting != null)
                {
                    foreach (var item in objCurriculo.highlighting)
                    {
                        if (objPesquisaCurriculo.DescricaoPalavraChave != null)
                        {
                            termos = objPesquisaCurriculo.DescricaoPalavraChave.IndexOf(',') != -1 ? objPesquisaCurriculo.DescricaoPalavraChave.Split(',') : objPesquisaCurriculo.DescricaoPalavraChave.Split(' ');
                            

                            foreach (var termo in termos)
                            {
                                if (experiencias.Funcao.ToLower().Contains(termo))
                                    funcao = item.Value.Des_Funcao_Exercida != null ? item.Value.Des_Funcao_Exercida[0].Replace("\\", "") : experiencias.Funcao;
                                
                                if (experiencias.DescricaoAtividade.ToLower().Contains(termo))
                                    atividades = item.Value.Des_Atividade != null ? item.Value.Des_Atividade[0].Replace("\\", "") : experiencias.DescricaoAtividade;
                            }
                        }
                    }
                }

                dr["Raz_Social"] = experiencias.RazaoSocial;
                dr["Dta_Admissao"] = experiencias.DataAdmissao;
                dr["Dta_Demissao"] = experiencias.DataDemissao;
                dr["Des_Area_BNE"] = experiencias.AreaBNE;
                dr["Des_Funcao"] = funcao;
                dr["Des_Atividade"] = atividades;
                dr["Vlr_Ultimo_Salario"] = experiencias.VlrSalario;

                datatable.Rows.Add(dr);
            }

            return datatable;
        }
        #endregion

        #region CarregarNomeCurriculo
        private void CarregarNomeCurriculo(bool flagVIP, string nomeCompleto, string primeiroNome)
        {
            if (MostrarNomeCandidatoEEmpresaNaExperienciaProfissional)
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
                    var objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

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
                            possuiCampanhaEnvioMassaVaga = LiberacaoVisualizacao(objCurriculo, new Vaga(IdVaga.Value));

                        if (objFilial.PossuiPlanoAtivo() //Se a empresa possui plano ativo
                            || autorizacaoPelaWebEstagios
                            || possuiCampanhaEnvioMassaVaga) // Se tem o parâmetro especifico da webestagios) 
                        {
                            btiDownload.Visible = true;
                            if (objCurriculo.VIP() || possuiCampanhaEnvioMassaVaga)
                            {
                                VerDadosCompleto(objCurriculo, objFilial, objUsuarioFilialPerfil);
                            }
                            else
                            {
                                if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                    VerDadosCompleto(objCurriculo, objFilial, objUsuarioFilialPerfil);
                                else
                                    ExibirMensagem("O limite de visualizações foi atingido, para mais informações ligue 0800 41 2400.", TipoMensagem.Aviso);
                            }
                        }
                        else
                        {
                            if (objCurriculo.VIP())
                            {
                                if (objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                    VerDadosCompleto(objCurriculo, objFilial, objUsuarioFilialPerfil);
                                else
                                {
                                    ModalVendaChupaVIP.Inicializar(TipoChuvaVIP.Visualizacao);
                                    objFilial.NotificiarTentantivaVisualizacao(new PessoaFisica(base.IdPessoaFisicaLogada.Value), objCurriculo);
                                }
                            }
                            else if (IdVaga.HasValue && base.IdPessoaFisicaLogada.HasValue)
                            {
                                //O IdVaga.HasValue verifica se o curriculo aberto é de origem da lista de candidatos
                                //da vaga da empresa logacda.
                                #region Campanha 'Ver Currículos' para empresas sem plano
                                Vaga vga = Vaga.LoadObject(IdVaga.Value);
                                CampanhaVisualizacaoCandidatos campanha = new CampanhaVisualizacaoCandidatos(objFilial);
                                if (campanha.PodeVisualizar(objCurriculo, vga))
                                    VerDadosCompleto(objCurriculo, objFilial, objUsuarioFilialPerfil);
                                else
                                {
                                    ucModalVendaCIA.Inicializar(objFilial.IdFilial,
                                        CarregarNomePessoaLogada(),
                                        CalcularPorcentagemDescontoPlano(objFilial), objFilial.Vendedor().NomeVendedor);
                                }
                                objFilial.NotificiarTentantivaVisualizacao(new PessoaFisica(base.IdPessoaFisicaLogada.Value), objCurriculo);
                                #endregion
                            }
                            else
                            {
                                if (objCurriculo.RecuperarDataAtualizacao() < DateTime.Today.AddYears(-1)) //Deixar uma filial sem plano, ver currículo não vip desatualizado.
                                {
                                    VerDadosCompleto(objCurriculo, objFilial, objUsuarioFilialPerfil, true);
                                }
                                else
                                {
                                    ucModalVendaCIA.Inicializar(objFilial.IdFilial, CarregarNomePessoaLogada(), CalcularPorcentagemDescontoPlano(objFilial), objFilial.Vendedor().NomeVendedor);
                                }
                            }
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
                ucModalLogin.InicializarEmpresa();
                ucModalLogin.Mostrar();
            }
        }
        #endregion

        #region [ CarregarNomePessoaLogada ]
        public string CarregarNomePessoaLogada()
        {
            PessoaFisica pessoaLogada = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value,null);
            return pessoaLogada != null ? pessoaLogada.NomeCompleto.Split(' ')[0] : "";
        }
        #endregion

        #region [ CalcularPorcentagemDescontoPlano ]
        private BLL.Enumeradores.Parametro CalcularPorcentagemDescontoPlano(Filial filial)
        {
            DateTime dataHoje = DateTime.Now;
            PlanoAdquirido planoEmpresa = PlanoAdquirido.CarregarUltimoPlanoPessoaJuridica(filial.IdFilial, null);
            List<PlanoAdquirido> listaUltimoPlanoUsado = PlanoAdquirido.CarregarUltimosPlanosEncerrados(filial, 5);

            if(listaUltimoPlanoUsado.Count == 0){
                if (planoEmpresa == null || planoEmpresa.PlanoSituacao.IdPlanoSituacao == (int)BLL.Enumeradores.PlanoSituacao.AguardandoLiberacao 
                    || planoEmpresa.PlanoSituacao.IdPlanoSituacao == (int)BLL.Enumeradores.PlanoSituacao.Cancelado)
                {
                    return  BLL.Enumeradores.Parametro.DescontoExperimenteAgora;
                }
            }
            
            double tempoPlano = (dataHoje - planoEmpresa.DataFimPlano).TotalDays;

            if(tempoPlano > Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.DiasDataFimUltimoPlanoParaDesconto)))
                return BLL.Enumeradores.Parametro.DescontoTempoPlanoMaiorQueParametro;
            else
                return BLL.Enumeradores.Parametro.DescontoTempoPlanoMenorQueParametro;
        }
        #endregion

        #region LiberacaoVisualizacao
        private bool LiberacaoVisualizacao(Curriculo objCurriculo, Vaga objVaga)
        {
            if (PlanoAdquiridoDetalhesCurriculo.RecebeuCampanhaVagaPerfil(objCurriculo, objVaga)) //Se recebeu campanha de divulgação em massa
                return true;

            if (BLL.CampanhaRecrutamento.CurriculoRecebeuCampanha(objCurriculo, objVaga)) //Se recebeu campanha de recrutamento
                return true;

            var objPlanoAdquirido = objVaga.PossuiPlanoPublicacaoImediata();
            if (objPlanoAdquirido != null) //Se a vaga possui plano vigente de publicação imediata
            {
                return CurriculoVisualizacao.VisualizacaoDeCurriculo(objPlanoAdquirido, objCurriculo);
            }

            return false;
        }
        #endregion

        #region VerDadosCompleto
        private void VerDadosCompleto(Curriculo objCurriculo, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, bool visualizacaoBaseGratis = false)
        {
            #region PesquisaCV
            BLL.PesquisaCurriculo objPesquisaCurriculo = null;
            if (IdPesquisaCurriculo.HasValue)
                objPesquisaCurriculo = BLL.PesquisaCurriculo.LoadObject(IdPesquisaCurriculo.Value);
            #endregion

            #region Vaga
            Vaga objVaga = null;
            if (IdVaga.HasValue)
                objVaga = new Vaga(IdVaga.Value);
            #endregion

            #region RastreadorCurriculo
            RastreadorCurriculo objRastreadorCurriculo = null;
            if (IdRastreadorCurriculo.HasValue)
                objRastreadorCurriculo = new RastreadorCurriculo(IdRastreadorCurriculo.Value);
            #endregion

            CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, true, Common.Helper.RecuperarIP(), objPesquisaCurriculo, objVaga, objRastreadorCurriculo, visualizacaoBaseGratis);

            //TODO: Melhorar o carragamento de partes do currículo, para evitar bater no solr.
            //Se não encontra no solr, busca no sql
            var objCurriculoDTO = Curriculo.CarregarCurriculoSolr(IdCurriculoVisualizacaoCurriculo, objPesquisaCurriculo != null ? objPesquisaCurriculo.DescricaoPalavraChave : "") ?? Curriculo.CarregarCurriculoDTO(IdCurriculoVisualizacaoCurriculo, Curriculo.DadosCurriculo.Tudo);

            CarregarDadosPessoais(objCurriculoDTO, true);
            CarregarFoto(objCurriculoDTO.NumeroCPF, true);

            CarregarExperienciaProfissional(objCurriculoDTO, objPesquisaCurriculo);
            CarregarObservacoes(objCurriculoDTO);

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
                    existeFoto = PessoaFisicaFoto.ExisteFoto(numeroCPF);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }

                rbiThumbnailVisualizacao.ImageUrl = existeFoto ? RetornarUrlFoto(numeroCPF) : "/img/img_sem_foto.gif";
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
        private void CarregarDadosPessoais(BLL.DTO.Curriculo objCurriculo, bool mostrarDadosCompletos)
        {
            if (mostrarDadosCompletos)
            {
                pnlDadosPessoais.Visible = litDataNascimentoValor.Visible = litDataNascimento.Visible = tdIdade.Visible = true;
                pnlVerDados.Visible = btnVerDados.Visible = false;

                litNomeValor.Text = objCurriculo.NomeCompleto;

                divNomeCandidato.Visible = true;

                litSexoValor.Text = TratarTexto(objCurriculo.Sexo);
                if (!string.IsNullOrWhiteSpace(objCurriculo.EstadoCivil))
                    litEstadoCivilValor.Text = TratarTexto(objCurriculo.EstadoCivil);
                else
                    litEstadoCivil.Visible = false;

                litDataNascimentoValor.Text = string.Format("{0} - {1} Anos", objCurriculo.DataNascimento.ToShortDateString(), BNE.BLL.Custom.Helper.CalcularIdade(objCurriculo.DataNascimento));
                litCpfValor.Text = BNE.BLL.Custom.Helper.FormatarCPF(objCurriculo.NumeroCPF);

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
                pnlDadosPessoais.Visible = litDataNascimentoValor.Visible = litDataNascimento.Visible = tdIdade.Visible = false;
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

            if (MostrarNomeCandidatoEEmpresaNaExperienciaProfissional)
                listCvResumo.Add(objCurriculo.PrimeiroNome);

            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            if (url.Contains("visualizacao"))
            {

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
            }
            else
            {
                litPretensaoSalarial.Visible = false;
                litPretensaoSalarialValor.Visible = false;
                listCvResumo = listCvResumo.Concat(new List<string>
                {
                    objCurriculo.Sexo, 
                    objCurriculo.EstadoCivil, 
                    objCurriculo.UltimaFormacaoAbreviada,        
                    objCurriculo.Bairro, 
                    objCurriculo.NomeCidade, 
                    objCurriculo.NomeFuncaoPretendida, 
                    objCurriculo.CategoriaHabilitacao
                }).ToList();
            }



            litCVResumo.Text = string.Join(" | ", listCvResumo.Where(c => !string.IsNullOrEmpty(c)).ToArray());

            if (objCurriculo.Deficiencia != null && objCurriculo.Deficiencia != "Nenhuma" && objCurriculo.Deficiencia.Any())
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
            {
                litFuncaoPretendidaValor.Text = string.Join("<br />", objCurriculo.FuncoesPretendidas.Select(fp => TratarTexto(fp.NomeFuncaoPretendida)));
            }
            else
                litFuncaoPretendida.Visible = false;

            if (objCurriculo.ValorPretensaoSalarial.HasValue)
                litPretensaoSalarialValor.Text = "R$ " + Convert.ToDecimal(objCurriculo.ValorPretensaoSalarial).ToString("N2");
            else
                litPretensaoSalarial.Visible = false;

            if (objCurriculo.AceitaEstagio)
            {
                litFuncaoPretendidaValor.Text += "<br />Aceita estágio";
                litFuncaoPretendida.Visible = true;
            }
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
        private void CarregarExperienciaProfissional(BNE.BLL.DTO.Curriculo objCurriculo, BNE.BLL.PesquisaCurriculo objPesquisaCurriculo)
        {
            var datatable = RecuperarExperienciaProfissionalFromDTO(objCurriculo, objPesquisaCurriculo);

            if (datatable.Rows.Count > 0)
                UIHelper.CarregarRepeater(rptExperienciaProfissional, datatable);
            else
                pnlDadosExperienciaProfissional.Visible = false;

            upExperienciaProfissional.Update();
        }
        #endregion

        #region CarregarObservacoes
        private void CarregarObservacoes(BLL.DTO.Curriculo objCurriculo)
        {
            bool usuarioLogado = (base.IdFilial.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue) || base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue;

            pnlObservacoesApenasLogado.Visible = !usuarioLogado;
            pnlObservacoes.Visible = usuarioLogado;

            #region HorarioDisponivel

            var horario = string.Join(" ", objCurriculo.DisponibilidadesTrabalho.Select(dt => dt.Descricao));

            if (!string.IsNullOrEmpty(horario))
                litHorarioDisponivelValor.Text = horario;
            else
                trLinhaHorarioDisponivel.Visible = false;

            #endregion

            #region Observacoes
            var observacaoCurriculo = objCurriculo.Observacao;
            var funcoesPretendidas = objCurriculo.FuncoesPretendidas.Where(f => f.QuantidadeExperiencia > 0).ToList();
            if (funcoesPretendidas.Any())
            {
                if (!string.IsNullOrWhiteSpace(observacaoCurriculo))
                    observacaoCurriculo += "<br>";

                observacaoCurriculo += "Minhas experiências: <br>";
                foreach (var objFuncaoPretendida in funcoesPretendidas)
                {
                    observacaoCurriculo += (new FuncaoPretendida { QuantidadeExperiencia = objFuncaoPretendida.QuantidadeExperiencia, DescricaoFuncaoPretendida = objFuncaoPretendida.NomeFuncaoPretendida }).ToString();
                }
            }

            if (!string.IsNullOrWhiteSpace(observacaoCurriculo))
                litObservacoes.Text = TratarTexto(observacaoCurriculo.ReplaceBreaks());
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
                litOutrosConhecimentosValor.Text = TratarTexto(objCurriculo.OutrosConhecimentos.ReplaceBreaks());
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

            upObservacoes.Update();
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
            if (string.IsNullOrWhiteSpace(PalavraChavePesquisa) || String.IsNullOrWhiteSpace(texto))
                return texto;

            var palavraChave = PalavraChavePesquisa.Split(',');
            foreach (var palavra in palavraChave)
            {
                texto = texto.HighlightText(palavra, "highlight");
            }
            return texto;
        }
        #endregion

        #region AjustarMetaTags
        private void AjustarMetaTags(BLL.DTO.Curriculo objCurriculo)
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

        protected void btiDownload_Click(object sender, EventArgs e)
        {

        }

    }

    #region Enumerador: Formato
    public enum Formato
    {
        BNE,
        Loja
    }
    #endregion

}