using System;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using BNE.BLL.Custom.Maps;
using BNE.Services.Code;
using BNE.Services.Properties;
using BNE.BLL;
using Microsoft.SqlServer.Types;

namespace BNE.Services
{
    partial class BuscaCoordenada : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static DateTime _diaExecucao;
        private static readonly int DelayExecucao = Settings.Default.BuscaCoordenadasDelayMinutos;
        private const string EventSourceName = "BuscaCoordenada";
        private static readonly int LimiteRequisicoes = Settings.Default.BuscaCoordenadasLimiteRequisicoes;
        private int _requisicoesEfetuadas;
        #endregion

        #region Construtores
        public BuscaCoordenada()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(IniciarBuscaCoordenadas);
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

        #region Métodos

        #region IniciarBuscaCoordenadas
        public void IniciarBuscaCoordenadas()
        {
            while (true)
            {
                try
                {
                    _requisicoesEfetuadas = 0;
                    _diaExecucao = DateTime.Today;


                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);

                    //Valida se é um novo dia para reiniciar o contador de execuções realizadas
                    if (!_diaExecucao.Date.Equals(DateTime.Today.Date))
                    {
                        _diaExecucao = DateTime.Today;
                        _requisicoesEfetuadas = 0;
                    }

                    if (_requisicoesEfetuadas < LimiteRequisicoes)
                    {
                        #region Currículo
                        try
                        {
                            if (Curriculo.ExisteSemLocalizacao())
                            {
                                try
                                {
                                    //Calculando o limite diário para envio para evitar possíveis time-out em produção
                                    //Se caso a primeira vez que rodar o robo tenha 1 milhao de registros para serem processados, 
                                    //o robo vai pegar no máximo a quantidade equivalente ao limite diário
                                    int limite = LimiteRequisicoes - _requisicoesEfetuadas;

                                    var listaSemLocalizacao = Curriculo.ListarSemLocalizacao(limite);
                                    foreach (DataRow dr in listaSemLocalizacao.Rows)
                                    {
                                        if (_requisicoesEfetuadas < LimiteRequisicoes)
                                        {
                                            try
                                            {
                                                var resultado = GeocodeService.RecuperarCoordenada(dr["Des_Logradouro"].ToString(), dr["Num_Endereco"].ToString(), dr["Num_CEP"].ToString(), dr["Nme_Cidade"].ToString(), dr["Sig_Estado"].ToString(), GeocodeService.Provider.Bing);
                                                //Incrementa a quantidade de requisições feitas no dia.
                                                _requisicoesEfetuadas++;
                                                if (resultado != null)
                                                {
                                                    var objCurriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                                                    objCurriculo.AlterarLocalizacao(SqlGeography.Point(resultado.Latitude, resultado.Longitude, 4326));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                EL.GerenciadorException.GravarExcecao(ex);
                                                EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex);
                                    EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex);
                            EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                        }

                        #endregion
                    }

                    if (_requisicoesEfetuadas < LimiteRequisicoes)
                    {
                        #region Filial
                        try
                        {
                            if (Filial.ExisteSemLocalizacao())
                            {
                                try
                                {
                                    //Calculando o limite diário para envio para evitar possíveis time-out em produção
                                    //Se caso a primeira vez que rodar o robo tenha 1 milhao de registros para serem processados, 
                                    //o robo vai pegar no máximo a quantidade equivalente ao limite diário
                                    int limite = LimiteRequisicoes - _requisicoesEfetuadas;

                                    var listaSemLocalizacao = Filial.ListarSemLocalizacao(limite);
                                    foreach (DataRow dr in listaSemLocalizacao.Rows)
                                    {
                                        if (_requisicoesEfetuadas < LimiteRequisicoes)
                                        {
                                            try
                                            {
                                                var resultado = GeocodeService.RecuperarCoordenada(dr["Des_Logradouro"].ToString(), dr["Num_Endereco"].ToString(), dr["Num_CEP"].ToString(), dr["Nme_Cidade"].ToString(), dr["Sig_Estado"].ToString(), GeocodeService.Provider.Bing);
                                                //Incrementa a quantidade de requisições feitas no dia.
                                                _requisicoesEfetuadas++;
                                                if (resultado != null)
                                                {
                                                    var objFilial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                                                    objFilial.AlterarLocalizacao(SqlGeography.Point(resultado.Latitude, resultado.Longitude, 4326));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                EL.GerenciadorException.GravarExcecao(ex);
                                                EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex);
                                    EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex);
                            EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                        }

                        #endregion
                    }

                    /*
                    if (_requisicoesEfetuadas < LimiteRequisicoes)
                    {
                        #region Vaga
                        try
                        {
                            if (Vaga.ExisteSemLocalizacao())
                            {
                                try
                                {
                                    //Calculando o limite diário para envio para evitar possíveis time-out em produção
                                    //Se caso a primeira vez que rodar o robo tenha 1 milhao de registros para serem processados, 
                                    //o robo vai pegar no máximo a quantidade equivalente ao limite diário
                                    int limite = LimiteRequisicoes - _requisicoesEfetuadas;

                                    var listaSemLocalizacao = Vaga.ListarSemLocalizacao(limite);
                                    foreach (DataRow dr in listaSemLocalizacao.Rows)
                                    {
                                        if (_requisicoesEfetuadas < LimiteRequisicoes)
                                        {
                                            try
                                            {
                                                var resultado = GeocodeService.RecuperarCoordenada(dr["Des_Logradouro"].ToString(), dr["Num_Endereco"].ToString(), dr["Num_CEP"].ToString(), dr["Nme_Cidade"].ToString(), dr["Sig_Estado"].ToString(), GeocodeService.Provider.Bing);
                                                //Incrementa a quantidade de requisições feitas no dia.
                                                _requisicoesEfetuadas++;
                                                if (resultado != null)
                                                {
                                                    var objVaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
                                                    objVaga.AlterarLocalizacao(SqlGeography.Point(resultado.Latitude, resultado.Longitude, 4326));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                EL.GerenciadorException.GravarExcecao(ex);
                                                EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex);
                                    EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex);
                            EventLogWriter.LogEvent(EventSourceName, ex.Message, EventLogEntryType.Warning, (int)EventID.ErroExecucao);
                        }

                        #endregion
                    }
                    */

                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);

                    AjustarThread(DateTime.Now);
                }
                catch (Exception ex)
                {
                    string message;
                    var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                    message = string.Format("{0} - {1}", id, message);
                    EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                }
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaFinal)
        {
            var tempoTotalExecucao = horaFinal - _dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, DelayExecucao, 0);
            if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(EventSourceName, String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
        }
        #endregion

        #endregion

    }
}
