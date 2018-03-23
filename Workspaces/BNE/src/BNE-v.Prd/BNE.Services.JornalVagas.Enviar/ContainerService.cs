using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BNE.JornalVagas;
using BNE.Services.JornalVagas.MessageBroker;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using log4net;
using MailSender;
using MailSender.Models;

namespace BNE.Services.JornalVagas.Enviar
{
    public class ContainerService
    {
        private readonly ILog _logger;
        private readonly IListener _listenerEnviar;
        private readonly ISerializer _serializer;
        private readonly IMailSenderAPI _mailSenderApi;
        private const string Remetente = "atendimento@bne.com.br";

        public ContainerService(ILog logger, ISerializer serializer, IBrokerConnection brokerConnection, IMailSenderAPI mailSenderApi)
        {
            _logger = logger;
            _serializer = serializer;
            _mailSenderApi = mailSenderApi;

            _listenerEnviar = new Listener(Constants.ParaEnviarQueuename, brokerConnection, logger);

            ServicePointManager.DefaultConnectionLimit = 10000;
            ServicePointManager.Expect100Continue = false;
        }

        public void Start()
        {
            _logger.Debug($"{GetType().FullName}.Start()");

            var tasks = new List<Task>();
            for (var i = 0; i < MaxWorkers(); i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    _listenerEnviar.Subscribe(args =>
                    {
                        var message = (Message)_serializer.Deserialize(typeof(Message), args.Body);

                        if (message.Mail != null)
                        {
                            try
                            {
                                _mailSenderApi.Mail.Post(new SendCommand(message.Mail.Key, Remetente, new List<string> { message.Mail.Email }, subject: message.Mail.Subject, message: message.Mail.Message));
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(message.Mail.Email, ex);
                            }
                        }

                        if (message.Webpush != null && message.Webpush.Any())
                        {
                            //TODO: Webpush
                            foreach (var webpush in message.Webpush)
                            {
                                _logger.Debug($"Sent webpush to {message.Curriculo} - {webpush.Token}");
                            }
                        }
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