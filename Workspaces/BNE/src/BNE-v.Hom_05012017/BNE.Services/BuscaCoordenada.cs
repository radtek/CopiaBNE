using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BNE.BLL;
using BNE.BLL.Custom.Maps;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using Microsoft.SqlServer.Types;

namespace BNE.Services
{
    internal partial class BuscaCoordenada : BaseService
    {
        #region Construtores
        public BuscaCoordenada()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static DateTime _diaExecucao;
        private static readonly int DelayExecucao = Settings.Default.BuscaCoordenadasDelayMinutos;
        private static int LimiteRequisicoes = Settings.Default.BuscaCoordenadasLimiteRequisicoes;
        private int _requisicoesEfetuadas;
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
            LimiteRequisicoes = 3000000;

            var dataUltima = DateTime.Now;
            _requisicoesEfetuadas = 0;
            _diaExecucao = DateTime.Today;

            while (true)
            {
                try
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

                    EventLogWriter.LogEvent(string.Format("Iniciou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.InicioExecucao);

                    //Valida se é um novo dia para reiniciar o contador de execuções realizadas
                    if (!_diaExecucao.Date.Equals(DateTime.Today.Date))
                    {
                        dataUltima = DateTime.Now;
                        _diaExecucao = DateTime.Today;
                        _requisicoesEfetuadas = 0;
                    }

                    //var rgoogle = GeocodeService.RecuperarCoordenada(string.Empty, string.Empty, String.Empty, "Centro", "Colombo", "PR", GeocodeService.Provider.Google);
                    //var rbing = GeocodeService.RecuperarCoordenada(string.Empty, string.Empty, String.Empty, "Centro", "Colombo", "PR", GeocodeService.Provider.Bing);

                    if (_requisicoesEfetuadas < LimiteRequisicoes)
                    {
                        #region Currículo
                        try
                        {
                            if (Curriculo.ExisteSemLocalizacao())
                            {
                                try
                                {
                                    var lote = 100;

                                    var listaSemLocalizacao = Curriculo.ListarSemLocalizacao(lote, dataUltima);

                                    do
                                    {
                                        var stopwatch = new Stopwatch();
                                        stopwatch.Start();
                                        //Parallel.ForEach(listaSemLocalizacao.AsEnumerable(), dr =>
                                        Parallel.ForEach(listaSemLocalizacao.AsEnumerable(),
                                            new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount},
                                            dr =>
                                            {
                                                try
                                                {
                                                    var resultado =
                                                        GeocodeService.RecuperarCoordenada(
                                                            dr["Des_Logradouro"].ToString(),
                                                            dr["Num_Endereco"].ToString(), dr["Num_CEP"].ToString(),
                                                            string.Empty, dr["Nme_Cidade"].ToString(),
                                                            dr["Sig_Estado"].ToString(), GeocodeService.Provider.Bing);
                                                    var objCurriculo =
                                                        new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));

                                                    //Incrementa a quantidade de requisições feitas no dia.
                                                    _requisicoesEfetuadas++;

                                                    if (resultado != null)
                                                        objCurriculo.AlterarLocalizacao(
                                                            SqlGeography.Point(resultado.Latitude, resultado.Longitude,
                                                                4326));

                                                    if (Convert.ToDateTime(dr["Dta_Atualizacao"]) < dataUltima)
                                                        dataUltima = Convert.ToDateTime(dr["Dta_Atualizacao"]);
                                                }
                                                catch (Exception ex)
                                                {
                                                    GerenciadorException.GravarExcecao(ex);
                                                    EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning,
                                                        Event.ErroExecucao);
                                                }
                                            });

                                        stopwatch.Stop();

                                        EventLogWriter.LogEvent("Busca coordenada - Tempo: " + stopwatch.Elapsed,
                                            EventLogEntryType.Information, Event.ErroExecucao);
                                        listaSemLocalizacao = Curriculo.ListarSemLocalizacao(lote, dataUltima);
                                    } while (listaSemLocalizacao.Rows.Count > 0 &&
                                             _requisicoesEfetuadas < LimiteRequisicoes);
                                }
                                catch (Exception ex)
                                {
                                    GerenciadorException.GravarExcecao(ex);
                                    EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex);
                            EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                        }
                        #endregion
                    }

                    /*
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
                                                EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex);
                                    EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex);
                            EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                        }

                        #endregion
                    }
                    */

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
                                                EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex);
                                    EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex);
                            EventLogWriter.LogEvent(ex.Message, EventLogEntryType.Warning, Event.ErroExecucao);
                        }

                        #endregion
                    }
                    */

                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.FimExecucao);

                    AjustarThread(DateTime.Now);
                }
                catch (Exception ex)
                {
                    string message;
                    var id = GerenciadorException.GravarExcecao(ex, out message);
                    message = string.Format("{0} - {1}", id, message);
                    EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                }
            }
        }
        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaFinal)
        {
            var tempoTotalExecucao = horaFinal - _dataHoraUltimaExecucao;
            var delay = new TimeSpan(0, DelayExecucao, 0);
            if ((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds > 0)
            {
                EventLogWriter.LogEvent(
                    string.Format(
                        "Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                        delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                    Event.AjusteExecucao);
                Thread.Sleep((int) delay.TotalMilliseconds - (int) tempoTotalExecucao.TotalMilliseconds);
            }
            else
                EventLogWriter.LogEvent(
                    string.Format(
                        "Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                        DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int) delay.TotalMilliseconds),
                    EventLogEntryType.Warning, Event.WarningAjusteExecucao);
        }
        #endregion

        #endregion
    }
}