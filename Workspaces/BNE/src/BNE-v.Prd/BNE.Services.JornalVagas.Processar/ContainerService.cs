using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using BNE.BLL.Notificacao;
using BNE.JornalVagas;
using BNE.Services.JornalVagas.MessageBroker;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using FormatWith;
using log4net;

namespace BNE.Services.JornalVagas.Processar
{
    public class ContainerService
    {
        private readonly ILog _logger;
        private readonly IListener _listenerEnfileirar;
        private readonly IListener _listenerProcessar;
        private readonly ISerializer _serializer;
        private readonly JornalVagaService _jornalVagasService;

        public ContainerService(JornalVagaService jornalVagasService, ILog logger, ISerializer serializer, IBrokerConnection brokerConnection)
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;

            _logger = logger;
            _serializer = serializer;
            _jornalVagasService = jornalVagasService;
            _listenerEnfileirar = new Listener(Constants.ParaProcessarQueuename, brokerConnection, logger);
            _listenerProcessar = new Listener(Constants.ParaEnviarQueuename, brokerConnection, logger);
        }

        public void Start()
        {
            _logger.Debug($"{GetType().FullName}.Start()");

            var tasks = new List<Task>();
            for (var i = 0; i < MaxWorkers(); i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    _listenerEnfileirar.Subscribe(args =>
                    {
                        var jornal = (ProcessamentoJornalVagas)_serializer.Deserialize(typeof(ProcessamentoJornalVagas), args.Body);
                        try
                        {
                            var key = jornal.FlagInvisivel ? Constants.MailsenderProcessKeyJornalVagasInvisivel : Constants.MailsenderProcessKeyJornalVagas;

                            var vagas = _jornalVagasService.RecuperarVagas(jornal).GetAwaiter().GetResult();
                            if (vagas.Count > 0)
                            {
                                var cartas = _jornalVagasService.RecuperarCartas(jornal);
                                var utms = _jornalVagasService.RecuperarUtms(jornal);

                                var hmtlFacets = _jornalVagasService.FacetsHTML(vagas, utms).GetAwaiter().GetResult();

                                var stopWatch = new Stopwatch();
                                stopWatch.Start();

                                var curriculo = _jornalVagasService.RecuperarCurriculo(jornal);

                                var htmlVagas = _jornalVagasService.VagasHTML(curriculo, vagas, cartas, utms[BLL.Notificacao.Enumeradores.UtmUrl.Vagas]);

                                var processingResult = jornal.Processar(curriculo, hmtlFacets, htmlVagas, vagas.Count, cartas, utms, _logger);
                                if (processingResult != null)
                                {
                                    //Publicando a mensagem para o robo de envio de notificacao
                                    _listenerProcessar.Publisher.Send(string.Empty, Constants.ParaEnviarQueuename, _serializer.Serialize(new Message(curriculo.IdCurriculo, new MailMessage(key, curriculo.Email, processingResult.Subject, processingResult.Body))), true);
                                    //Disparando mensagem para atualizar a data do ultimo envio para esse currículo
                                    _listenerProcessar.Publisher.Send(string.Empty, Constants.ParaAtualizarAlertaQueuename, _serializer.Serialize(new AtualizarAlerta(curriculo.IdCurriculo)), true);
                                }

                                stopWatch.Stop();
                                if (_logger.IsInfoEnabled)
                                {
                                    _logger.Info($"Tempo processando currículos para o {jornal}:{stopWatch.Elapsed}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(jornal.ToString(), ex);
                        }

                        _jornalVagasService.FinalizarProcessamento(jornal);
                    });
                }, TaskCreationOptions.LongRunning));
            }

            Task.WhenAll(tasks);
        }

        public void Stop()
        {
            _logger.Debug($"{GetType().FullName}.Stop()");
        }

        private static int MaxWorkers()
        {
            try
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["MaxWorkers"]);
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}