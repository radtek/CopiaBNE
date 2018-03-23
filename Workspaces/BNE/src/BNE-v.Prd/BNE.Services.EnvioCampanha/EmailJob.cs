using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BNE.BLL.Mensagem;
using log4net;
using MailSender;
using MailSender.Models;
using Microsoft.Rest;
using Quartz;

namespace BNE.Services.EnvioCampanha
{
    /// <summary>
    ///     Essa job existe para pegar todos os emails que deram erro e tentar reenviar para a api de envio de email
    /// </summary>
    [DisallowConcurrentExecution]
    public class EmailJob : IJob
    {
        private static readonly IMailSenderAPI MailSenderApi = new MailSenderAPI();
        private static readonly int MaxTask = Convert.ToInt32(ConfigurationManager.AppSettings["MaxTask"]);
        private readonly EmailService _emailService = new EmailService();
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmailJob()
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug($"{typeof(EmailJob).FullName} started...");

            try
            {
                var tasks = new List<Task>();
                var queue = new BlockingCollection<Email>();
                var enviados = new ConcurrentBag<int>();

                var hasAnyToSend = _emailService.AnyToSend();

                if (hasAnyToSend)
                {
                    var producer = Task.Factory.StartNew(() =>
                    {
                        foreach (var item in _emailService.GetAllToSend())
                        {
                            queue.Add(item);
                        }

                        queue.CompleteAdding();
                    }, TaskCreationOptions.LongRunning);
                    tasks.Add(producer);

                    for (var x = 0; x < MaxTask; x++)
                    {
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            while (!queue.IsCompleted)
                            {
                                Email item;
                                if (queue.TryTake(out item))
                                {
                                    if (EnviarEmail(item))
                                    {
                                        enviados.Add(item.Id);
                                    }
                                }
                            }
                        }, TaskCreationOptions.LongRunning));
                    }

                    Task.WaitAll(tasks.ToArray());

                    _emailService.Enviados(enviados);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao recuperar emails", ex);
            }

            _logger.Debug($"{typeof(EmailJob).FullName} ended...");
        }

        public bool EnviarEmail(Email email)
        {
            try
            {
                Thread.Sleep(1000);
                _logger.Debug($"Enviando para a API o email {email.Id}");

                var sw = new Stopwatch();
                sw.Start();
                HttpOperationResponse<MailMessageView> retorno;
                if (email.PossuiAnexo())
                {
                    //retorno = new HttpOperationResponse<MailMessageView>
                    //{
                    //    Response = new HttpResponseMessage(HttpStatusCode.OK),
                    //    Body = new MailMessageView(Guid.NewGuid())
                    //};
                    retorno = MailSenderApi.Mail.PostWithHttpMessagesAsync(new SendCommand(email.Key, email.De, new List<string> {email.Para}, null, null, null, email.Assunto, email.Mensagem, null, null, null, new Dictionary<string, string> {{email.NomeAnexo, email.Anexo}})).Result;
                }
                else
                {
                    //retorno = new HttpOperationResponse<MailMessageView>
                    //{
                    //    Response = new HttpResponseMessage(HttpStatusCode.OK),
                    //    Body = new MailMessageView(Guid.NewGuid())
                    //};
                    retorno = MailSenderApi.Mail.PostWithHttpMessagesAsync(new SendCommand(email.Key, email.De, new List<string> {email.Para}, null, null, null, email.Assunto, email.Mensagem)).Result;
                }
                sw.Stop();

                if (retorno.Response.IsSuccessStatusCode && retorno.Body.Id != Guid.Empty)
                {
                    _logger.Debug($"Email {email.Id} enviado para API!");
                    return true;
                }

                _logger.Debug($"Email {email.Id} retornou erro ao ser enviado para a para API!");
                _emailService.FalhaAoEnviar(email);
            }
            catch (Exception ex)
            {
                _logger.Error($"Email {email.Id} com erro!", ex);
                _emailService.FalhaAoEnviar(email);
            }
            return false;
        }
    }
}