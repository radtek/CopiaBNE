using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using IntegracaoSituacao = BNE.BLL.Enumeradores.IntegracaoSituacao;
using SituacaoCurriculo = BNE.BLL.Enumeradores.SituacaoCurriculo;
using TipoCurriculo = BNE.BLL.Enumeradores.TipoCurriculo;
using TipoVinculoIntegracao = BNE.BLL.TipoVinculoIntegracao;

namespace BNE.Services
{
    internal partial class IntegracaoWebfopag : BaseService
    {
        #region IntegracaoWebfopag
        public IntegracaoWebfopag()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = Settings.Default.IntegracaoWebfopagDelayMinutos;
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(Integrar);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #endregion

        #region Metodos

        #region Integrar
        public void Integrar()
        {
            try
            {
                AjustarThread(DateTime.Now);

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;
                    try
                    {
                        EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                            EventLogEntryType.Information, Event.InicioExecucao);

                        #region Integração na rescisão
                        var listaIntegrao = Integracao.RecuperarIntegracoesRecisao();

                        foreach (var objIntegracao in listaIntegrao)
                        {
                            try
                            {
                                IntegrarRescisao(objIntegracao);
                                objIntegracao.IntegracaoSituacao =
                                    new BLL.IntegracaoSituacao((int)IntegracaoSituacao.Integrado);
                                objIntegracao.DataIntegracao = DateTime.Now;
                                objIntegracao.Save();
                            }
                            catch (Exception ex)
                            {
                                GerenciadorException.GravarExcecao(ex);
                                objIntegracao.IntegracaoSituacao =
                                    new BLL.IntegracaoSituacao((int)IntegracaoSituacao.ComErro);
                                objIntegracao.Save();
                            }
                        }
                        #endregion

                        #region Integração na admissão
                        var listaIntegraoAdmissao = Integracao.RecuperarIntegracoesAdmissao();

                        foreach (var objIntegracao in listaIntegraoAdmissao)
                        {
                            try
                            {
                                IntegrarAdmissao(objIntegracao);
                                objIntegracao.IntegracaoSituacao =
                                    new BLL.IntegracaoSituacao((int)IntegracaoSituacao.Integrado);
                                objIntegracao.DataIntegracao = DateTime.Now;
                                objIntegracao.Save();
                            }
                            catch (Exception ex)
                            {
                                GerenciadorException.GravarExcecao(ex);
                                objIntegracao.IntegracaoSituacao =
                                    new BLL.IntegracaoSituacao((int)IntegracaoSituacao.ComErro);
                                objIntegracao.Save();
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = GerenciadorException.GravarExcecao(ex, out message);
                        message = string.Format("{0} - {1}", id, message);
                        EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                    }

                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.FimExecucao);
                    AjustarThread(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }
        #endregion

        #region IntegrarRescisao
        private void IntegrarRescisao(Integracao objIntegracao)
        {
            if (objIntegracao.NumeroCPF.HasValue)
            {
                const string descricaoIP = "Integraçao WF";

                PessoaFisica objPessoaFisica;
                if (!PessoaFisica.CarregarPorCPF(objIntegracao.NumeroCPF.Value, out objPessoaFisica))
                {
                    objPessoaFisica = new PessoaFisica
                    {
                        CPF = objIntegracao.NumeroCPF.Value,
                        DescricaoIP = descricaoIP,
                        Endereco = new Endereco()
                    };
                }
                else
                {
                    if (objPessoaFisica.Endereco != null)
                        objPessoaFisica.Endereco.CompleteObject();
                    else
                        objPessoaFisica.Endereco = new Endereco();
                }

                CarregarPessoaFisica(objIntegracao, ref objPessoaFisica);

                Curriculo objCurriculo;
                if (!Curriculo.CarregarPorCpf(objIntegracao.NumeroCPF.Value, out objCurriculo))
                {
                    objCurriculo = new Curriculo
                    {
                        PessoaFisica = objPessoaFisica,
                        DescricaoIP = descricaoIP,
                        FlagManha = true,
                        FlagTarde = true,
                        FlagNoite = false
                    };
                }

                var dataAtualizacao = objIntegracao.DataSaidaPrevista.Value;
                objCurriculo.DataAtualizacao = dataAtualizacao;
                objCurriculo.DataModificacaoCV = dataAtualizacao;
                objCurriculo.ValorPretensaoSalarial = objCurriculo.ValorUltimoSalario = objIntegracao.ValorSalario;
                objCurriculo.SituacaoCurriculo = new BLL.SituacaoCurriculo((int)SituacaoCurriculo.ComCritica);
                objCurriculo.TipoCurriculo = new BLL.TipoCurriculo((int)TipoCurriculo.Mini);
                objCurriculo.PessoaFisica = objPessoaFisica;

                PessoaFisicaComplemento objPessoaFisicaComplemento;
                if (
                    !PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica,
                        out objPessoaFisicaComplemento))
                {
                    objPessoaFisicaComplemento = new PessoaFisicaComplemento
                    {
                        PessoaFisica = objPessoaFisica
                    };
                }

                objPessoaFisicaComplemento.NumeroHabilitacao = objIntegracao.NumeroHabilitacao;
                objPessoaFisicaComplemento.CategoriaHabilitacao = objIntegracao.CategoriaHabilitacao;
                objPessoaFisicaComplemento.FlagFilhos = objIntegracao.Flagfilhos;

                PessoaFisicaVeiculo objPessoaFisicaVeiculo = null;
                if (!PessoaFisicaVeiculo.ExisteVeiculo(objPessoaFisica.IdPessoaFisica) &&
                    objIntegracao.TipoVeiculo != null)
                {
                    objPessoaFisicaVeiculo = new PessoaFisicaVeiculo
                    {
                        PessoaFisica = objPessoaFisica,
                        AnoVeiculo =
                            objIntegracao.AnoVeiculo.HasValue
                                ? (short)objIntegracao.AnoVeiculo.Value
                                : (short)DateTime.Now.Year,
                        TipoVeiculo = objIntegracao.TipoVeiculo,
                        FlagInativo = false
                    };
                }

                List<ExperienciaProfissional> listExperienciaProfissionalLocal;
                ExperienciaProfissional objExperienciaProfissional = null;
                if (
                    !ExperienciaProfissional.CarregarExperienciaProfissionalImportada(objPessoaFisica.IdPessoaFisica,
                        out listExperienciaProfissionalLocal))
                {
                    //Inserir ExperienciaProfissional
                    objExperienciaProfissional = new ExperienciaProfissional
                    {
                        DataAdmissao =
                            objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now,
                        DataDemissao = objIntegracao.DataSaidaPrevista,
                        AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE()),
                        Funcao = objIntegracao.Funcao,
                        DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob(),
                        RazaoSocial = objIntegracao.RazaoSocial,
                        PessoaFisica = objCurriculo.PessoaFisica,
                        FlagImportado = true
                    };
                }
                else
                {
                    var dataAdmissao = objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now;
                    var razaoSocial = objIntegracao.RazaoSocial;


                    //Atualizar ou Inserir uma Experiencia Profissional
                    var flgEncontrado = false;
                    var listExperienciaProfissionalImportadaLocal =
                        listExperienciaProfissionalLocal.FindAll(EncontrarImportados);

                    foreach (var objExperienciaProfissionalImportadaLocal in listExperienciaProfissionalImportadaLocal)
                    {
                        if (objExperienciaProfissionalImportadaLocal.DataAdmissao ==
                            dataAdmissao &&
                            objExperienciaProfissionalImportadaLocal.RazaoSocial ==
                            razaoSocial)
                        {
                            objExperienciaProfissional = new ExperienciaProfissional(objExperienciaProfissionalImportadaLocal.IdExperienciaProfissional);
                            objExperienciaProfissional.DataDemissao = objIntegracao.DataSaidaPrevista;
                            objExperienciaProfissional.AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE());
                            objExperienciaProfissional.Funcao = objIntegracao.Funcao;
                            objExperienciaProfissional.DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob();
                            objExperienciaProfissional.PessoaFisica = objCurriculo.PessoaFisica;
                            objExperienciaProfissional.FlagImportado = true;
                            objExperienciaProfissional.RazaoSocial = objExperienciaProfissionalImportadaLocal.RazaoSocial;
                            objExperienciaProfissional.DescricaoFuncaoExercida = objExperienciaProfissionalImportadaLocal.DescricaoFuncaoExercida;
                            objExperienciaProfissional.DataAdmissao = objExperienciaProfissionalImportadaLocal.DataAdmissao;
                            objExperienciaProfissional.FlagInativo = objExperienciaProfissionalImportadaLocal.FlagInativo;
                            objExperienciaProfissional.VlrSalario = objExperienciaProfissionalImportadaLocal.VlrSalario;
                            objExperienciaProfissional.DescricaoNavegador = objExperienciaProfissionalImportadaLocal.DescricaoNavegador;
                            objExperienciaProfissional.DataCadastro = objExperienciaProfissionalImportadaLocal.DataCadastro;

                            flgEncontrado = true;
                        }
                    }
                    if (!flgEncontrado)
                    {
                        objExperienciaProfissional = new ExperienciaProfissional();
                        objExperienciaProfissional.DataAdmissao = objIntegracao.DataAdmissao.HasValue
                            ? objIntegracao.DataAdmissao.Value
                            : DateTime.Now;
                        objExperienciaProfissional.DataDemissao = objIntegracao.DataSaidaPrevista;
                        objExperienciaProfissional.AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE());
                        objExperienciaProfissional.Funcao = objIntegracao.Funcao;
                        objExperienciaProfissional.DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob();
                        objExperienciaProfissional.RazaoSocial = objIntegracao.RazaoSocial;
                        objExperienciaProfissional.PessoaFisica = objCurriculo.PessoaFisica;
                        objExperienciaProfissional.FlagImportado = true;
                    }
                }

                var liberarVIP = false;

                #region Atualizar Situacao do Currículo

                //Se existe rescisao para o empregado dependendo do tipo de vinculo e do tipo de rescisão ele ganha vip.
                //Sempre que a rescisão ocorrer por morte o currículo é desativado.
                //Se rescisão por Justa Causa Bloqueia o currículo
                //Caso não entre em nenhum filtro altera para Ativa currículo (Aguardando Publicação)
                if (objIntegracao.MotivoRescisao.IdMotivoRescisao.Equals((int)MotivoRescisaoIntegracao.Morte))
                    DesativarCurriculo(objCurriculo);
                else if (
                    objIntegracao.MotivoRescisao.IdMotivoRescisao.Equals(
                        (int)MotivoRescisaoIntegracao.MorteEmpregadorIndividual))
                    DesativarCurriculo(objCurriculo);
                else if (
                    objIntegracao.MotivoRescisao.IdMotivoRescisao.Equals(
                        (int)MotivoRescisaoIntegracao.ComJustaCausa))
                    BloquearCurriculo(objCurriculo);
                else if (DeveLiberarVIP(objIntegracao.MotivoRescisao, objIntegracao.TipoVinculoIntegracao, objIntegracao))
                    liberarVIP = true;
                else
                    AtivarCurriculo(objCurriculo);
                #endregion

                objCurriculo.SalvarIntegracao(objIntegracao, objPessoaFisicaComplemento, objPessoaFisicaVeiculo,
                    objExperienciaProfissional, liberarVIP);
            }
        }
        #endregion

        #region IntegrarAdminssao
        private void IntegrarAdmissao(Integracao objIntegracao)
        {

            var excluirCvSolr = false;

            if (objIntegracao.NumeroCPF.HasValue)
            {
                const string descricaoIP = "Integraçao WF";

                PessoaFisica objPessoaFisica;
                if (!PessoaFisica.CarregarPorCPF(objIntegracao.NumeroCPF.Value, out objPessoaFisica))
                {
                    //Task 43574 - Cpf Não existe no BNE não faz nenhuma ação
                    return;

                    //objPessoaFisica = new PessoaFisica
                    //{
                    //    CPF = objIntegracao.NumeroCPF.Value,
                    //    DescricaoIP = descricaoIP,
                    //    Endereco = new Endereco()
                    //};
                }
                else
                {

                    if (objPessoaFisica.Endereco != null)
                        objPessoaFisica.Endereco.CompleteObject();
                    else
                        objPessoaFisica.Endereco = new Endereco();
                }

                //seta os dados da pessoa fisica apenas se ela ainda não tem cadastro ou a admissão for maior que a data de alteracao
                if (objPessoaFisica.DataAlteracao == DateTime.MinValue || objPessoaFisica.DataAlteracao < objIntegracao.DataAdmissao)
                    CarregarPessoaFisica(objIntegracao, ref objPessoaFisica);

                Curriculo objCurriculo;
                if (!Curriculo.CarregarPorCpf(objIntegracao.NumeroCPF.Value, out objCurriculo))
                {
                    objCurriculo = new Curriculo
                    {
                        PessoaFisica = objPessoaFisica,
                        DescricaoIP = descricaoIP
                    };
                }
                else
                {
                    excluirCvSolr = !objCurriculo.FlagInativo;
                    objCurriculo.PessoaFisica = objPessoaFisica;
                }


                if (objCurriculo.DataAtualizacao == DateTime.MinValue || objCurriculo.DataAtualizacao < objIntegracao.DataAdmissao)
                {
                    var dataAtualizacao = objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now;
                    objCurriculo.DataAtualizacao = dataAtualizacao;
                    objCurriculo.DataModificacaoCV = dataAtualizacao;
                    objCurriculo.ValorPretensaoSalarial = objCurriculo.ValorUltimoSalario = objIntegracao.ValorSalario;
                    objCurriculo.TipoCurriculo = new BLL.TipoCurriculo((int)TipoCurriculo.Mini);
                    objCurriculo.PessoaFisica = objPessoaFisica;
                }

                objCurriculo.FlagInativo = true;
                //ja possui cadastro no bne entra no status exclusão Logica - Task 43574
                //Exclusão Lógica: Tira o curriculo das pesquias e dos processos de e-mail e SMS. Quando esse curriculo acessar o BNE ele deve fazer um novo cadastro, a aplicação
                //não deve puxar seus dados pessoais já cadastrados anteriormente, mas devem ser atualizados quando preenchido novamente.
                objCurriculo.SituacaoCurriculo = new BLL.SituacaoCurriculo((int)SituacaoCurriculo.ExclusaoLogica);
                objCurriculo.FlagInativo = true;
                BLL.Notificacao.AlertaCidades.DeletaTodosAlertas(objCurriculo.IdCurriculo);
                BLL.Notificacao.AlertaFuncoes.DeletaTodosAlertas(objCurriculo.IdCurriculo);

                PessoaFisicaComplemento objPessoaFisicaComplemento;
                if (
                    !PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica,
                        out objPessoaFisicaComplemento))
                {
                    objPessoaFisicaComplemento = new PessoaFisicaComplemento
                    {
                        PessoaFisica = objPessoaFisica
                    };
                }

                objPessoaFisicaComplemento.NumeroHabilitacao = objIntegracao.NumeroHabilitacao;
                objPessoaFisicaComplemento.CategoriaHabilitacao = objIntegracao.CategoriaHabilitacao;
                objPessoaFisicaComplemento.FlagFilhos = objIntegracao.Flagfilhos;

                PessoaFisicaVeiculo objPessoaFisicaVeiculo = null;
                if (!PessoaFisicaVeiculo.ExisteVeiculo(objPessoaFisica.IdPessoaFisica) &&
                    objIntegracao.TipoVeiculo != null)
                {
                    objPessoaFisicaVeiculo = new PessoaFisicaVeiculo
                    {
                        PessoaFisica = objPessoaFisica,
                        AnoVeiculo =
                            objIntegracao.AnoVeiculo.HasValue
                                ? (short)objIntegracao.AnoVeiculo.Value
                                : (short)DateTime.Now.Year,
                        TipoVeiculo = objIntegracao.TipoVeiculo,
                        FlagInativo = false
                    };
                }

                List<ExperienciaProfissional> listExperienciaProfissionalLocal;
                ExperienciaProfissional objExperienciaProfissional = null;
                if (
                    !ExperienciaProfissional.CarregarExperienciaProfissionalImportada(objPessoaFisica.IdPessoaFisica,
                        out listExperienciaProfissionalLocal))
                {
                    //Inserir ExperienciaProfissional
                    objExperienciaProfissional = new ExperienciaProfissional
                    {
                        DataAdmissao =
                            objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now,
                        DataDemissao = objIntegracao.DataSaidaPrevista,
                        AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE()),
                        Funcao = objIntegracao.Funcao,
                        DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob(),
                        RazaoSocial = objIntegracao.RazaoSocial,
                        PessoaFisica = objCurriculo.PessoaFisica,
                        FlagImportado = true
                    };
                }
                else
                {
                    var dataAdmissao = objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now;
                    var razaoSocial = objIntegracao.RazaoSocial;


                    //Atualizar ou Inserir uma Experiencia Profissional
                    var flgEncontrado = false;
                    var listExperienciaProfissionalImportadaLocal =
                        listExperienciaProfissionalLocal.FindAll(EncontrarImportados);

                    foreach (var objExperienciaProfissionalImportadaLocal in listExperienciaProfissionalImportadaLocal)
                    {
                        if (objExperienciaProfissionalImportadaLocal.DataAdmissao ==
                            dataAdmissao &&
                            objExperienciaProfissionalImportadaLocal.RazaoSocial ==
                            razaoSocial)
                        {
                            objExperienciaProfissional = new ExperienciaProfissional(objExperienciaProfissionalImportadaLocal.IdExperienciaProfissional);
                            objExperienciaProfissional.DataDemissao = objIntegracao.DataSaidaPrevista;
                            objExperienciaProfissional.AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE());
                            objExperienciaProfissional.Funcao = objIntegracao.Funcao;
                            objExperienciaProfissional.DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob();
                            objExperienciaProfissional.PessoaFisica = objCurriculo.PessoaFisica;
                            objExperienciaProfissional.FlagImportado = true;
                            objExperienciaProfissional.RazaoSocial = objExperienciaProfissionalImportadaLocal.RazaoSocial;
                            objExperienciaProfissional.DescricaoFuncaoExercida = objExperienciaProfissionalImportadaLocal.DescricaoFuncaoExercida;
                            objExperienciaProfissional.DataAdmissao = objExperienciaProfissionalImportadaLocal.DataAdmissao;
                            objExperienciaProfissional.FlagInativo = objExperienciaProfissionalImportadaLocal.FlagInativo;
                            objExperienciaProfissional.VlrSalario = objExperienciaProfissionalImportadaLocal.VlrSalario;
                            objExperienciaProfissional.DescricaoNavegador = objExperienciaProfissionalImportadaLocal.DescricaoNavegador;
                            objExperienciaProfissional.DataCadastro = objExperienciaProfissionalImportadaLocal.DataCadastro;

                            flgEncontrado = true;
                        }
                    }
                    if (!flgEncontrado)
                    {
                        objExperienciaProfissional = new ExperienciaProfissional();
                        objExperienciaProfissional.DataAdmissao = objIntegracao.DataAdmissao.HasValue
                            ? objIntegracao.DataAdmissao.Value
                            : DateTime.Now;
                        objExperienciaProfissional.DataDemissao = objIntegracao.DataSaidaPrevista;
                        objExperienciaProfissional.AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE());
                        objExperienciaProfissional.Funcao = objIntegracao.Funcao;
                        objExperienciaProfissional.DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob();
                        objExperienciaProfissional.RazaoSocial = objIntegracao.RazaoSocial;
                        objExperienciaProfissional.PessoaFisica = objCurriculo.PessoaFisica;
                        objExperienciaProfissional.FlagImportado = true;
                        objExperienciaProfissional.RazaoSocial = objIntegracao.RazaoSocial;
                        objExperienciaProfissional.DataAdmissao = objIntegracao.DataAdmissao.Value;
                        objExperienciaProfissional.FlagInativo = false;
                        objExperienciaProfissional.VlrSalario = objIntegracao.ValorSalario;
                        objExperienciaProfissional.DescricaoNavegador = null;
                        objExperienciaProfissional.DataCadastro = DateTime.Now;
                    }
                }

                objCurriculo.SalvarIntegracao(objIntegracao, objPessoaFisicaComplemento, objPessoaFisicaVeiculo,
                    objExperienciaProfissional, false, false);

                var objObservacaoCurriculo = new CurriculoObservacao
                {
                    Curriculo = objCurriculo,
                    DescricaoObservacao = "Curriculo bloqueado por integração Webfopag. Vinculado a empresa " + objIntegracao.DesCS,
                    FlagSistema = true,
                    FlagInativo = false,
                    UsuarioFilialPerfil = null
                };

                objObservacaoCurriculo.Save();

                if (excluirCvSolr)
                    Curriculo.ExcluirCVSolr(objCurriculo.IdCurriculo);


            }
        }
        #endregion IntegrarAdminssao

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual)
        {
            var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, DelayExecucao, 0);
            if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(
                    string.Format(
                        "Integração Webfopag - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                        delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                    Event.AjusteExecucao);
                Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(
                    string.Format(
                        "Integração Webfopag - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                        DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds),
                    EventLogEntryType.Warning, Event.WarningAjusteExecucao);
        }
        #endregion

        #region DeveLiberarVIP
        private static bool DeveLiberarVIP(MotivoRescisao objMotivoRescisao, TipoVinculoIntegracao objTipoVinculo, Integracao objIntegracao)
        {

            var dataAtual = DateTime.Now;
            if (!objIntegracao.DataSaidaPrevista.HasValue || !objIntegracao.DataSaidaPrevista.Value.Month.Equals(dataAtual.Month)
                || !objIntegracao.DataSaidaPrevista.Value.Year.Equals(dataAtual.Year))
                return false;



            switch (objTipoVinculo.IdTipoVinculoIntegracao)
            {
                case (int)BLL.Enumeradores.TipoVinculoIntegracao.EmpregadoPorTempoDeterminado:
                case (int)BLL.Enumeradores.TipoVinculoIntegracao.EmpregadoPorTempoIndeterminado:
                    switch (objMotivoRescisao.IdMotivoRescisao)
                    {
                        case (int)MotivoRescisaoIntegracao.SemJustaCausa:
                        case (int)MotivoRescisaoIntegracao.ContratoExperienciaEmpresa:
                            return true;
                    }
                    break;
                case (int)BLL.Enumeradores.TipoVinculoIntegracao.Aprendiz:
                    switch (objMotivoRescisao.IdMotivoRescisao)
                    {
                        case (int)MotivoRescisaoIntegracao.SemJustaCausa:
                        case (int)MotivoRescisaoIntegracao.ContratoExperienciaEmpresa:
                        case (int)MotivoRescisaoIntegracao.TerminoContrato:
                            return true;
                    }
                    break;
            }



            return false;
        }
        #endregion

        #region DesativarCurriculo
        public static void DesativarCurriculo(Curriculo objCurriculo)
        {
            objCurriculo.SituacaoCurriculo = new BLL.SituacaoCurriculo((int)SituacaoCurriculo.Cancelado);
            objCurriculo.FlagInativo = true;
        }
        #endregion

        #region BloquearCurriculo
        public static void BloquearCurriculo(Curriculo objCurriculo)
        {
            objCurriculo.SituacaoCurriculo = new BLL.SituacaoCurriculo((int)SituacaoCurriculo.Bloqueado);
            objCurriculo.FlagInativo = true;
        }
        #endregion

        #region AtivarCurriculo
        public static void AtivarCurriculo(Curriculo objCurriculo)
        {
            objCurriculo.SituacaoCurriculo = new BLL.SituacaoCurriculo((int)SituacaoCurriculo.AguardandoPublicacao);
            objCurriculo.FlagInativo = false;
        }
        #endregion

        #region EncontrarImportados
        public static bool EncontrarImportados(ExperienciaProfissional objExperienciaProfissional)
        {
            if (objExperienciaProfissional.FlagImportado.HasValue && objExperienciaProfissional.FlagImportado.Value)
                return true;

            return false;
        }
        #endregion

        #region CarregarPessoaFisica
        private void CarregarPessoaFisica(Integracao objIntegracao, ref PessoaFisica objPessoaFisica)
        {

            objPessoaFisica.DataNascimento = objIntegracao.DataNascimento;
            objPessoaFisica.NomePessoa = Helper.AjustarString(objIntegracao.NomePessoa);
            objPessoaFisica.NomePessoaPesquisa = Helper.RemoverAcentos(objIntegracao.NomePessoa);
            objPessoaFisica.Sexo = objIntegracao.Sexo;
            objPessoaFisica.NumeroDDDCelular = objIntegracao.NumeroDDDCelular;
            objPessoaFisica.NumeroCelular = objIntegracao.NumeroCelular;
            objPessoaFisica.NumeroDDDTelefone = objIntegracao.NumeroDDDTelefone;
            objPessoaFisica.NumeroTelefone = objIntegracao.NumeroTelefone;
            objPessoaFisica.FlagInativo = false;
            objPessoaFisica.EmailPessoa = objIntegracao.EmailPessoa;
            objPessoaFisica.NumeroRG = objIntegracao.NumeroRG;
            objPessoaFisica.NomeOrgaoEmissor = objIntegracao.NomeOrgaoEmissor;
            objPessoaFisica.DataExpedicaoRG = objIntegracao.DataExpedicaoRG;
            objPessoaFisica.NomePai = objIntegracao.NomePai;
            objPessoaFisica.NomeMae = objIntegracao.NomeMae;
            objPessoaFisica.ApelidoPessoa = objIntegracao.ApelidoPessoa;
            objPessoaFisica.SiglaUFEmissaoRG = objIntegracao.SiglaUFEmissaoRG;
            objPessoaFisica.Raca = objIntegracao.Raca;
            objPessoaFisica.Deficiencia = objIntegracao.Deficiencia;
            objPessoaFisica.Cidade = objIntegracao.Cidade;
            objPessoaFisica.EstadoCivil = objIntegracao.EstadoCivil;
            objPessoaFisica.Escolaridade = objIntegracao.Escolaridade;

            objPessoaFisica.Endereco.NumeroCEP = objIntegracao.NumeroCEP;
            objPessoaFisica.Endereco.DescricaoLogradouro = objIntegracao.DescricaoLogradouro;
            objPessoaFisica.Endereco.NumeroEndereco = objIntegracao.NumeroEndereco;
            objPessoaFisica.Endereco.DescricaoComplemento = objIntegracao.DescricaoComplemento;
            objPessoaFisica.Endereco.DescricaoBairro = objIntegracao.DescricaoBairro;
            objPessoaFisica.Endereco.Cidade = objIntegracao.Cidade;
        }
        #endregion

        #endregion
    }
}