using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.vaga
{
    public partial class Vaga : BasePage
    {

        #region Propriedades

        #region IdVagaVisualizacao - RouteValues
        /// <summary>
        /// Propriedade que armazena e recupera o IdVagaSession
        /// </summary>
        private int? IdVagaVisualizacao
        {
            get
            {
                if (RouteData.Values.Count > 0)
                {
                    if (RouteData.Values["Identificador"] != null)
                        return Convert.ToInt32(RouteData.Values["Identificador"]);
                }
                return null;
            }

        }
        #endregion

        #region QuantidadeCandidaturas - Variável 15
        /// <summary>
        /// Propriedade que armazena e recupera a QuantidadeCandidaturas
        /// </summary>
        protected int? QuantidadeCandidaturas
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel15.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel15.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel15.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel15.ToString());
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Principal)Page.Master;

            if (master != null)
            {
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;
                master.FecharModalLogin += master_FecharModalLogin;
            }

            if (!IsPostBack)
            {
                pnlJaEnviei.Visible = false;
                Inicializar();
            }

            ucModalQuestionarioVagas.Salvar += ucModalQuestionarioVagas_Salvar;
            ucModalLogin.Logar += ucModalLogin_Logar;

            ucModalConfirmacaoExclusao.Confirmar += ucModalConfirmacaoExclusaoConfirmar;
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "Vaga");

        }
        #endregion

        #region btiQueroMeCandidatar_Click
        protected void btiQueroMeCandidatar_Click(object sender, EventArgs e)
        {
            try
            {
                if (base.IdCurriculo.HasValue)
                    ValidarCandidato();
                else
                {
                    ucModalLogin.Inicializar();
                    ucModalLogin.Mostrar();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex, MensagemAviso._300041);
            }
        }
        #endregion

        #region ucModalQuestionarioVagas_Salvar
        void ucModalQuestionarioVagas_Salvar(object sender, UserControls.Modais.VagaRespostaEventArgs e)
        {
            if (base.IdCurriculo.HasValue)
            {
                BLL.Vaga objVaga = BLL.Vaga.LoadObject(e.IdVaga);
                Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
                CandidatarVaga(objVaga, objCurriculo, e);
            }
        }
        #endregion

        #region ucModalLogin_Logar
        void ucModalLogin_Logar(string urlDestino)
        {
            if (base.IdCurriculo.HasValue)
                ValidarCandidato();
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }

        protected void master_LoginEfetuadoSucesso()
        {
            //Indica se é para remover a candidatura ou não
            bool remover = false;

            #region Tratamento originado do click no email

            ///C - Confirmar Candidatura
            ///R - Remover Candidatura
            ///Default - Visualiza a Vaga
            string acao = Request.QueryString["acao"];

            if (!string.IsNullOrEmpty(acao))
                if (acao.Equals("R"))
                    remover = true;

            #endregion

            if (base.IdCurriculo.HasValue)
                ValidarCandidato(remover);
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }
        #endregion

        #region ucModalLogin_Fechar
        void master_FecharModalLogin()
        {
            if (btiQueroMeCandidatar.Visible && pnlJaEnviei.Visible)
                pnlJaEnviei.Visible = false;
        }
        #endregion

        #region ucModalConfirmacaoExclusaoConfirmar

        protected void ucModalConfirmacaoExclusaoConfirmar()
        {
            Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
            BLL.Vaga objVaga = BLL.Vaga.LoadObject(IdVagaVisualizacao.Value);

            RemoverCandidaturaVaga(objVaga, objCurriculo);

            pnlJaEnviei.Visible = false;
            btiQueroMeCandidatar.Visible = true;
            upControles.Update();
        }

        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            if (IdVagaVisualizacao.HasValue)
            {
                PreencherCampos();

                if (base.IdCurriculo.HasValue)
                    VagaVisualizada.SalvarVisualizacaoVaga(new BLL.Vaga(IdVagaVisualizacao.Value), new Curriculo(base.IdCurriculo.Value));
                else
                    VagaVisualizada.SalvarVisualizacaoVaga(new BLL.Vaga(IdVagaVisualizacao.Value));
            }
        }
        #endregion

        #region PreecherCampos
        private void PreencherCampos()
        {
            BLL.Vaga objVaga = BLL.Vaga.LoadObject(IdVagaVisualizacao.Value);
            objVaga.Funcao.CompleteObject();
            objVaga.Cidade.CompleteObject();

            litTituloVaga.Text = objVaga.Funcao.DescricaoFuncao;

            if (objVaga.QuantidadeVaga != null)
            {
                lblQuantidadeVagas.Text = String.Format("({0} {1})", objVaga.QuantidadeVaga,
                                          objVaga.QuantidadeVaga.Value.Equals(1) ? " vaga" : " vagas");
            }

            string salario = "R$ A combinar";
            if (objVaga.ValorSalarioDe.HasValue && objVaga.ValorSalarioPara.HasValue)
                salario = string.Format("{0} a {1}", objVaga.ValorSalarioDe.Value.ToString("C"), objVaga.ValorSalarioPara.Value.ToString("C"));

            litResumoDadosVaga.Text = String.Format("{0} - {1}/{2}", salario, objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.SiglaEstado);
            //Atribuições
            pnlAtribuicoes.Visible = !String.IsNullOrEmpty(objVaga.DescricaoAtribuicoes);
            litTextoAtribuicoes.Text = objVaga.DescricaoAtribuicoes;
            //Benefícios
            pnlBeneficios.Visible = !String.IsNullOrEmpty(objVaga.DescricaoBeneficio);
            litTextoBeneficios.Text = objVaga.DescricaoBeneficio;
            //Requisitos
            //pnlRequisitos.Visible = objVaga.Escolaridade != null || !String.IsNullOrEmpty(objVaga.DescricaoRequisito);
            string escolaridade = string.Empty;

            if (objVaga.Escolaridade != null)
            {
                objVaga.Escolaridade.CompleteObject();
                escolaridade = objVaga.Escolaridade.DescricaoBNE;
            }

            litTextoRequisitos.Text = string.Format("{0} {1}", escolaridade,
                                                    objVaga.DescricaoRequisito);

            //Deficiência
            pnlDeficiencia.Visible = false;
            if (objVaga.Deficiencia != null)
            {
                objVaga.Deficiencia.CompleteObject();
                pnlDeficiencia.Visible = !objVaga.Deficiencia.IdDeficiencia.Equals(0);
                litTextoDeficienciaVaga.Text = objVaga.Deficiencia.DescricaoDeficiencia;
            }

            //Disponibilidade para Trabalho
            pnlDisponibilidadeTrabalho.Visible = false;
            List<VagaDisponibilidade> vagaDisponibilidade = VagaDisponibilidade.ListarDisponibilidadesPorVaga(objVaga);
            if (vagaDisponibilidade.Count > 0)
            {
                pnlDisponibilidadeTrabalho.Visible = true;
                foreach (var objVagadisponibilidade in vagaDisponibilidade)
                {
                    objVagadisponibilidade.Disponibilidade.CompleteObject();
                    litTextoDisponibilidadeTrabalho.Text += string.Format("{0} ", objVagadisponibilidade.Disponibilidade.DescricaoDisponibilidade);
                }
            }

            //Tipo de Vínculo
            pnlTipoVinculoTrabalho.Visible = false;
            List<VagaTipoVinculo> vagaTipoVinculo = VagaTipoVinculo.ListarTipoVinculoPorVaga(objVaga.IdVaga);
            if (vagaTipoVinculo.Count > 0)
            {
                pnlTipoVinculoTrabalho.Visible = true;
                foreach (var objVagaTipoVinculo in vagaTipoVinculo)
                {
                    objVagaTipoVinculo.TipoVinculo.CompleteObject();
                    litTextoTipoVinculoTrabalho.Text += string.Format("{0} ", objVagaTipoVinculo.TipoVinculo.DescricaoTipoVinculo);
                }
            }

            litTextoCodigoVaga.Text = objVaga.CodigoVaga;
            if (objVaga.DataAbertura != null) litDataPublicacao.Text = objVaga.DataAbertura.Value.ToShortDateString();

            PreencherCamposEmpresa(objVaga.Filial);
            AjustarCssPainelVaga(objVaga.FlagBNERecomenda);
            AjustarMetaTags(objVaga);
            AjustarBotaoCandidatarJaEnviei();

            Page.Title = SitemapHelper.MontarTituloVaga(objVaga.Funcao.DescricaoFuncao, objVaga.Funcao.AreaBNE.DescricaoAreaBNE, objVaga.QuantidadeVaga ?? (short)1, objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.NomeEstado, objVaga.Cidade.Estado.SiglaEstado, objVaga.IdVaga);
        }
        #endregion

        #region PreencherCamposEmpresa
        private void PreencherCamposEmpresa(Filial objFilial)
        {
            objFilial.CompleteObject();
            objFilial.Endereco.CompleteObject();
            objFilial.Endereco.Cidade.CompleteObject();

            lblNumeroFuncionarioValor.Text = objFilial.QuantidadeFuncionarios.ToString(CultureInfo.CurrentCulture);
            lblCidadeValor.Text = UIHelper.FormatarCidade(objFilial.Endereco.Cidade.NomeCidade, objFilial.Endereco.Cidade.Estado.SiglaEstado);
            lblBairroValor.Text = objFilial.Endereco.DescricaoBairro;
            lblDataCadastroValor.Text = objFilial.DataCadastro.ToShortDateString();
            lblCurriculosVisualizadosValor.Text = objFilial.RecuperarQuantidadeCurriculosVisualizados().ToString(CultureInfo.CurrentCulture);
            lblVagasDivulgadasValor.Text = objFilial.RecuperarQuantidadeVagasDivuldadas().ToString(CultureInfo.CurrentCulture);
        }
        #endregion

        #region AjustarMetaTags
        private void AjustarMetaTags(BLL.Vaga objVaga)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { Name = "keywords", Content = objVaga.RetornarKeywords() });
            Page.Header.Controls.AddAt(0, new HtmlMeta { Name = "description", Content = objVaga.RetornarDescription() });

            if (objVaga.FlagInativo || objVaga.FlagVagaArquivada)
            {
                var metaTagNoindex = new HtmlMeta { Name = "robots", Content = "noindex" };
                Page.Header.Controls.AddAt(0, metaTagNoindex);
            }
        }
        #endregion

        #region AjustarCssPainelVaga
        /// <summary>
        /// Ajusta o css do painel vaga
        /// </summary>
        /// <param name="flgBneRecomenda"></param>
        /// <returns></returns>
        private void AjustarCssPainelVaga(bool flgBneRecomenda)
        {
            if (flgBneRecomenda && !base.STC.Value)
                pnlVaga.CssClass = "painel_vaga bne_recomenda";
            else
                pnlVaga.CssClass = "painel_vaga";
        }
        #endregion

        #region ValidarCandidato
        private void ValidarCandidato(bool RemoverCandidatura = false)
        {
            Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
            BLL.Vaga objVaga = BLL.Vaga.LoadObject(IdVagaVisualizacao.Value);

            if (!RemoverCandidatura)
            {
                if (AjustaPerguntaExistente(objVaga, objCurriculo))
                    CandidatarVaga(objVaga, objCurriculo, null);
            }
            else
            {
                ucModalConfirmacaoExclusao.Inicializar("Remover a Candidatura", "<br/>Deseja realmente remover a candidatura para esta vaga?");
                ucModalConfirmacaoExclusao.MostrarModal();
            }
        }
        #endregion

        #region CandidatarVaga
        private void CandidatarVaga(BLL.Vaga objVaga, Curriculo objCurriculo, UserControls.Modais.VagaRespostaEventArgs e)
        {
            var listVagaPergunta = new List<VagaCandidatoPergunta>();

            if (e != null)
            {
                VagaCandidatoPergunta objVagaCandidatoPergunta;
                if (e.IdPergunta1.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                                                   {
                                                       VagaPergunta = new VagaPergunta(e.IdPergunta1.Value),
                                                       FlagResposta = e.FlagRespostaPergunta1.Value
                                                   };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }
                if (e.IdPergunta2.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                                                   {
                                                       VagaPergunta = new VagaPergunta(e.IdPergunta2.Value),
                                                       FlagResposta = e.FlagRespostaPergunta2.Value
                                                   };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }
                if (e.IdPergunta3.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                                                   {
                                                       VagaPergunta = new VagaPergunta(e.IdPergunta3.Value),
                                                       FlagResposta = e.FlagRespostaPergunta3.Value
                                                   };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }
                if (e.IdPergunta4.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                                                   {
                                                       VagaPergunta = new VagaPergunta(e.IdPergunta4.Value),
                                                       FlagResposta = e.FlagRespostaPergunta4.Value
                                                   };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }

                ucModalQuestionarioVagas.FecharModal();
            }

            //Se a empresa possui Minha Empresa Oferece Cursos é usada a origem da empresa como principal e não mais da vaga.
            Origem objOrigem = null;
            OrigemFilial objOrigemFilial = null;
            if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()))
                objOrigem = new Origem(base.IdOrigem.Value);

            int? quantidadeCandidaturas;
            VagaCandidato.Candidatar(objCurriculo, objVaga, objOrigem, listVagaPergunta,
                Common.Helper.RecuperarIP(), base.STC.Value, (objOrigemFilial != null && !objOrigemFilial.Filial.PossuiSTCUniversitario() && !objOrigemFilial.Filial.PossuiSTCLanhouse()), false, Enumeradores.OrigemCandidatura.Site, out quantidadeCandidaturas);

            QuantidadeCandidaturas = quantidadeCandidaturas;
            if (QuantidadeCandidaturas == null)
            {
                MostrarConfirmacao(objCurriculo, objVaga);
                EsconderBotaoCandidatar();
            }
            else if (QuantidadeCandidaturas >= 0)
            {
                MostrarDegustacao(quantidadeCandidaturas - 1);
                if (quantidadeCandidaturas - 1 >= 0)
                    EsconderBotaoCandidatar();
            }
        }
        #endregion

        #region RemoverCandidaturaVaga
        private void RemoverCandidaturaVaga(BLL.Vaga objVaga, Curriculo objCurriculo)
        {
            VagaCandidato objVagaCandidato = null;
            if (VagaCandidato.CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
            {
                if (objVagaCandidato != null)
                    VagaCandidato.Delete(objVagaCandidato.IdVagaCandidato);
            }
        }
        #endregion

        #region MostrarConfirmacao
        private void MostrarConfirmacao(Curriculo objCurriculo, BLL.Vaga objVaga)
        {
            string protocolo = BLL.Vaga.RetornarProtocolo(objCurriculo.IdCurriculo, objVaga.CodigoVaga);

            ucModalConfirmacaoEnvioCurriculo.Inicializar(objCurriculo.PessoaFisica.PrimeiroNome, protocolo);
            ucModalConfirmacaoEnvioCurriculo.MostrarModal();
        }
        #endregion

        #region MostrarDegustacao
        private void MostrarDegustacao(int? quantidadeCandidaturasRestantes)
        {
            ucModalDegustacaoCandidatura.Inicializar(false, false, true, quantidadeCandidaturasRestantes);
        }
        #endregion

        #region AjustarBotaoCandidatarJaEnviei
        private void AjustarBotaoCandidatarJaEnviei()
        {
            #region Tratamento originado do click no email

            ///C - Confirmar Candidatura
            ///R - Remover Candidatura
            ///Default - Visualiza a Vaga
            string acao = Request.QueryString["acao"];

            #endregion

            switch (acao)
            {
                #region Candidatar ou Confirmar

                case "C":
                    try
                    {
                        if (base.IdCurriculo.HasValue)
                        {
                            if (VagaCandidato.CurriculoJaCandidatouVaga(new Curriculo(base.IdCurriculo.Value), new BLL.Vaga(IdVagaVisualizacao.Value)))
                                MostrarConfirmacao(new Curriculo(base.IdCurriculo.Value), new BLL.Vaga(IdVagaVisualizacao.Value));
                            else
                                ValidarCandidato();

                            pnlJaEnviei.Visible = true;
                            btiQueroMeCandidatar.Visible = false;

                            upControles.Update();
                        }
                        else
                        {
                            base.ExibirLogin();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExibirMensagemErro(ex, MensagemAviso._300041);
                    }

                    break;
                #endregion

                #region Remover Candidatura
                case "R":
                    try
                    {
                        if (base.IdCurriculo.HasValue)
                        {
                            ValidarCandidato(true);
                        }
                        else
                        {
                            base.ExibirLogin();
                        }

                    }
                    catch (Exception ex)
                    {
                        ExibirMensagemErro(ex, MensagemAviso._300041);
                    }
                    break;
                #endregion

                #region Padrão ou Visualizar
                default:
                    if (base.IdCurriculo.HasValue)
                    {
                        if (VagaCandidato.CurriculoJaCandidatouVaga(new Curriculo(base.IdCurriculo.Value), new BLL.Vaga(IdVagaVisualizacao.Value)))
                        {
                            pnlJaEnviei.Visible = true;
                            btiQueroMeCandidatar.Visible = false;
                        }
                        else
                        {
                            pnlJaEnviei.Visible = false;
                            btiQueroMeCandidatar.Visible = true;
                        }
                    }
                    else
                    {
                        pnlJaEnviei.Visible = false;
                        btiQueroMeCandidatar.Visible = true;
                    }

                    upControles.Update();
                    break;
                #endregion
            }
        }
        #endregion

        #region AjustaPerguntaExistente
        /// <summary>
        /// Metodo responsavel por ajustar perguntar existente
        /// </summary>
        /// <param name="objVaga"></param>
        /// <param name="objCurriculo"></param>
        /// <returns>Se existir pergunta retorna false senão true</returns>
        private bool AjustaPerguntaExistente(BLL.Vaga objVaga, Curriculo objCurriculo)
        {
            if (objVaga.ExistePerguntas() && !VagaCandidato.CurriculoJaCandidatouVaga(objCurriculo, objVaga))
            {
                ucModalQuestionarioVagas.Inicializar(objVaga.IdVaga, null);
                ucModalQuestionarioVagas.MostrarModal();
                return false;
            }
            return true;
        }
        #endregion

        #region EsconderBotaoCandidatar
        private void EsconderBotaoCandidatar()
        {
            btiQueroMeCandidatar.Visible = false;
            pnlJaEnviei.Visible = true;

            upControles.Update();
        }
        #endregion

        #endregion
    }
}