using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BNE.BLL;
using BNE.BLL.Mensagem;
using log4net;
using MailSender;
using MailSender.Models;
using Quartz;
using System.Configuration;
using System.Threading;
using System.Linq;

namespace BNE.Services.MailSender
{
    /// <summary>
    /// Essa job existe para pegar todos os emails que deram erro e tentar reenviar para a api de envio de email
    /// </summary>
    [DisallowConcurrentExecution]
    public class MensagemCSJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly IMailSenderAPI MailSenderApi = new MailSenderAPI();
        private string TransacionalKey = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.MailSenderAPIKey);
        private static readonly int MaxTask = Convert.ToInt32(ConfigurationManager.AppSettings["MaxTask"].ToString());

        public MensagemCSJob()
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug("Job MensagemCSJob started...");

            while (true)
            {

                try
                {

                    var emails = MensagemCS.GetAllToSend();

                        Parallel.ForEach(emails, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, email =>
                        {
                            {
                                EnviarEmail(email);
                            }
                        });
                  
                       // _logger.Debug("Job MensagemCSJob aguardando mensagens para envio");
                        Thread.Sleep(30000);


                }
                catch (Exception ex)
                {
                    _logger.Error("Job MensagemCSJob erro ao recuperar emails", ex);
                    Thread.Sleep(30000);
                    continue;
                }
            }

            // _logger.Debug("Job MensagemCSJob ended...");
        }

        public void EnviarEmail(BLL.MensagemCS email)
        {
            try
            {
                _logger.Debug($"Enviando para a API a MensagemCS {email.IdMensagemCS}");

                var sw = new Stopwatch();
                sw.Start();
                MailMessageView retorno;
                if (email.PossuiAnexo())
                {
                    retorno = MailSenderApi.Mail.Post(new SendCommand(TransacionalKey, email.DescricaoEmailRemetente, new List<string> { email.DescricaoEmailDestinatario }, null, null, null, email.DescricaoEmailAssunto, email.DescricaoMensagem, null, null, null, new Dictionary<string, string> { { email.NomeAnexo, Convert.ToBase64String(email.ArquivoAnexo) } }));
                }
                else
                {
                    retorno = MailSenderApi.Mail.Post(new SendCommand(TransacionalKey, email.DescricaoEmailRemetente, new List<string> { email.DescricaoEmailDestinatario }, null, null, null, email.DescricaoEmailAssunto, email.DescricaoMensagem));
                }
                sw.Stop();

                var swDel = new Stopwatch();
                swDel.Start();
                if (retorno != null)
                {
                    _logger.Debug($"MensagemCS {email.IdMensagemCS} enviado para API!");
                    email.SalvarDataEnvio();
                    _logger.Debug($"MensagemCS {email.IdMensagemCS} deletado da base!");
                }
                swDel.Stop();

                _logger.Info($"O serviço levou {sw.Elapsed} para enviar e {swDel.Elapsed} para deletar a MensagemCS {email.IdMensagemCS}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Email {email.IdMensagemCS} com erro!", ex);
            }
        }
    }
}