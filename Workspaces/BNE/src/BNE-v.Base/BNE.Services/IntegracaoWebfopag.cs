using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL;
using BNE.Services.Code;
using BNE.Services.Properties;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Services
{
    partial class IntegracaoWebfopag : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static readonly int DelayExecucao = Settings.Default.IntegracaoWebfopagDelayMinutos;
        private const string EventSourceName = "IntegracaoWebfopag";
        #endregion

        #region IntegracaoWebfopag
        public IntegracaoWebfopag()
        {
            InitializeComponent();
        }
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
                        EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                        #region Integração na rescisão
                        var listaIntegrao = Integracao.RecuperarIntegracoes();

                        foreach (var objIntegracao in listaIntegrao)
                        {
                            try
                            {
                                IntegrarRescisao(objIntegracao);
                                objIntegracao.IntegracaoSituacao = new IntegracaoSituacao((int)Enumeradores.IntegracaoSituacao.Integrado);
                                objIntegracao.DataIntegracao = DateTime.Now;
                                objIntegracao.Save();
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex);
                                objIntegracao.IntegracaoSituacao = new IntegracaoSituacao((int)Enumeradores.IntegracaoSituacao.ComErro);
                                objIntegracao.Save();
                            }
                        }
                        #endregion

                        #region Integração na admissão
                        var listaIntegraoAdmissao = IntegracaoAdmissao.RecuperarIntegracoes();

                        foreach (var objIntegracao in listaIntegraoAdmissao)
                        {
                            try
                            {
                                IntegrarAdmissao(objIntegracao);
                                objIntegracao.IntegracaoSituacao = new IntegracaoSituacao((int)Enumeradores.IntegracaoSituacao.Integrado);
                                objIntegracao.DataIntegracao = DateTime.Now;
                                objIntegracao.Save();
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex);
                                objIntegracao.IntegracaoSituacao = new IntegracaoSituacao((int)Enumeradores.IntegracaoSituacao.ComErro);
                                objIntegracao.Save();
                            }
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        string message;
                        var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                        message = string.Format("{0} - {1}", id, message);
                        EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                    }

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);
                    AjustarThread(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region Integrar
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

                objPessoaFisica.DataNascimento = objIntegracao.DataNascimento;
                objPessoaFisica.NomePessoa = BLL.Custom.Helper.AjustarString(objIntegracao.NomePessoa);
                objPessoaFisica.NomePessoaPesquisa = BLL.Custom.Helper.RemoverAcentos(objIntegracao.NomePessoa);
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

                var dataAtualizacao = DateTime.Now;
                objCurriculo.DataAtualizacao = dataAtualizacao;
                objCurriculo.DataModificacaoCV = dataAtualizacao;
                objCurriculo.ValorPretensaoSalarial = objCurriculo.ValorUltimoSalario = objIntegracao.ValorSalario;
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.ComCritica);
                objCurriculo.TipoCurriculo = new TipoCurriculo((int)Enumeradores.TipoCurriculo.Mini);
                objCurriculo.PessoaFisica = objPessoaFisica;

                PessoaFisicaComplemento objPessoaFisicaComplemento;
                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
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
                if (!PessoaFisicaVeiculo.ExisteVeiculo(objPessoaFisica.IdPessoaFisica) && objIntegracao.TipoVeiculo != null)
                {
                    objPessoaFisicaVeiculo = new PessoaFisicaVeiculo
                    {
                        PessoaFisica = objPessoaFisica,
                        AnoVeiculo = objIntegracao.AnoVeiculo.HasValue ? (short)objIntegracao.AnoVeiculo.Value : (short)DateTime.Now.Year,
                        TipoVeiculo = objIntegracao.TipoVeiculo,
                        FlagInativo = false
                    };
                }

                List<ExperienciaProfissional> listExperienciaProfissionalLocal;
                ExperienciaProfissional objExperienciaProfissional;
                if (!ExperienciaProfissional.CarregarExperienciaProfissionalImportada(objPessoaFisica.IdPessoaFisica, out listExperienciaProfissionalLocal))
                {
                    //Inserir ExperienciaProfissional
                    objExperienciaProfissional = new ExperienciaProfissional
                    {
                        DataAdmissao = objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now,
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
                    objExperienciaProfissional = new ExperienciaProfissional
                    {
                        DataAdmissao = objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now,
                        RazaoSocial = objIntegracao.RazaoSocial
                    };

                    //Atualizar ou Inserir uma Experiencia Profissional
                    bool flgEncontrado = false;
                    List<ExperienciaProfissional> listExperienciaProfissionalImportadaLocal = listExperienciaProfissionalLocal.FindAll(EncontrarImportados);
                    foreach (ExperienciaProfissional objExperienciaProfissionalImportadaLocal in listExperienciaProfissionalImportadaLocal)
                    {
                        if (objExperienciaProfissionalImportadaLocal.DataAdmissao == objExperienciaProfissional.DataAdmissao && objExperienciaProfissionalImportadaLocal.RazaoSocial == objExperienciaProfissional.RazaoSocial)
                        {
                            objExperienciaProfissional.DataDemissao = objIntegracao.DataSaidaPrevista;
                            objExperienciaProfissional.AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE());
                            objExperienciaProfissional.Funcao = objIntegracao.Funcao;
                            objExperienciaProfissional.DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob();
                            objExperienciaProfissional.PessoaFisica = objCurriculo.PessoaFisica;
                            objExperienciaProfissional.FlagImportado = true;

                            flgEncontrado = true;
                        }
                    }
                    if (!flgEncontrado)
                    {
                        objExperienciaProfissional.DataAdmissao = objIntegracao.DataAdmissao.HasValue ? objIntegracao.DataAdmissao.Value : DateTime.Now;
                        objExperienciaProfissional.DataDemissao = objIntegracao.DataSaidaPrevista;
                        objExperienciaProfissional.AreaBNE = new AreaBNE(objIntegracao.Funcao.RecuperarAreaBNE());
                        objExperienciaProfissional.Funcao = objIntegracao.Funcao;
                        objExperienciaProfissional.DescricaoAtividade = objIntegracao.Funcao.RecuperarDescricaoJob();
                        objExperienciaProfissional.RazaoSocial = objIntegracao.RazaoSocial;
                        objExperienciaProfissional.PessoaFisica = objCurriculo.PessoaFisica;
                        objExperienciaProfissional.FlagImportado = true;
                    }
                }

                bool liberarVIP = false;

                #region Atualizar Situacao do Currículo

                //Se existe rescisao para o empregado dependendo do tipo de vinculo e do tipo de rescisão ele ganha vip.
                //Sempre que a rescisão ocorrer por morte o currículo é desativado.
                //Se rescisão por Justa Causa Bloqueia o currículo
                //Caso não entre em nenhum filtro altera para Ativa currículo (Aguardando Publicação)
                if (objIntegracao.MotivoRescisao.IdMotivoRescisao.Equals((int)Enumeradores.MotivoRescisaoIntegracao.Morte))
                    DesativarCurriculo(objCurriculo);
                else if (objIntegracao.MotivoRescisao.IdMotivoRescisao.Equals((int)Enumeradores.MotivoRescisaoIntegracao.MorteEmpregadorIndividual))
                    DesativarCurriculo(objCurriculo);
                else if (objIntegracao.MotivoRescisao.IdMotivoRescisao.Equals((int)Enumeradores.MotivoRescisaoIntegracao.ComJustaCausa))
                    BloquearCurriculo(objCurriculo);
                else if (DeveLiberarVIP(objIntegracao.MotivoRescisao, objIntegracao.TipoVinculoIntegracao))
                    liberarVIP = true;
                else
                    AtivarCurriculo(objCurriculo);

                #endregion

                objCurriculo.SalvarIntegracao(objIntegracao, objPessoaFisicaComplemento, objPessoaFisicaVeiculo, objExperienciaProfissional, liberarVIP);
            }
        }
        #endregion

        #region IntegrarAdmissao
        private void IntegrarAdmissao(IntegracaoAdmissao objIntegracao)
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorCpf(objIntegracao.NumeroCPF, out objCurriculo))
            {
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Bloqueado);
                objCurriculo.Save();

                var objCurriculoCorrecao = new CurriculoCorrecao
                {
                    Curriculo = objCurriculo,
                    DescricaoCorrecao = "Currículo bloqueado pela Integração com a Webfopag",
                    UsuarioFilialPerfil = new UsuarioFilialPerfil(1659356)
                };
                objCurriculoCorrecao.Save();
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual)
        {
            TimeSpan tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, DelayExecucao, 0);
            if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(EventSourceName, String.Format("Integração Webfopag - Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(EventSourceName, String.Format("Integração Webfopag - Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
        }
        #endregion

        #region DeveLiberarVIP
        private static bool DeveLiberarVIP(MotivoRescisao objMotivoRescisao, TipoVinculoIntegracao objTipoVinculo)
        {
            switch (objTipoVinculo.IdTipoVinculoIntegracao)
            {
                case (int)Enumeradores.TipoVinculoIntegracao.EmpregadoPorTempoDeterminado:
                case (int)Enumeradores.TipoVinculoIntegracao.EmpregadoPorTempoIndeterminado:
                    switch (objMotivoRescisao.IdMotivoRescisao)
                    {
                        case (int)Enumeradores.MotivoRescisaoIntegracao.SemJustaCausa:
                        case (int)Enumeradores.MotivoRescisaoIntegracao.ContratoExperienciaEmpresa:
                            return true;

                    }
                    break;
                case (int)Enumeradores.TipoVinculoIntegracao.Aprendiz:
                    switch (objMotivoRescisao.IdMotivoRescisao)
                    {
                        case (int)Enumeradores.MotivoRescisaoIntegracao.SemJustaCausa:
                        case (int)Enumeradores.MotivoRescisaoIntegracao.ContratoExperienciaEmpresa:
                        case (int)Enumeradores.MotivoRescisaoIntegracao.TerminoContrato:
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
            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Cancelado);
            objCurriculo.FlagInativo = true;
        }
        #endregion

        #region BloquearCurriculo
        public static void BloquearCurriculo(Curriculo objCurriculo)
        {
            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Bloqueado);
            objCurriculo.FlagInativo = true;
        }
        #endregion

        #region AtivarCurriculo
        public static void AtivarCurriculo(Curriculo objCurriculo)
        {
            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoPublicacao);
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

        #endregion

    }
}
