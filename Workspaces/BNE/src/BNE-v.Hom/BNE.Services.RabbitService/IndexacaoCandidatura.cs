using BNE.BLL.Custom.Rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Text;
using Newtonsoft.Json;
using BNE.BLL;
using System.Diagnostics;

namespace BNE.Services.RabbitService
{
    public partial class IndexacaoCandidatura : ServiceBase
    {
        private QueueingBasicConsumer consumer = null;
        private string _hostName = ConfigurationManager.AppSettings["RabbitServe"];
        private string _userName = ConfigurationManager.AppSettings["RabbitUserName"];
        private string _password = ConfigurationManager.AppSettings["RabbitPassword"];
        private System.Diagnostics.EventLog eventLog;


        public IndexacaoCandidatura()
        {
            InitializeComponent();
            eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("IndexacaoCandidatura"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "IndexacaoCandidatura", "IndexacaoCandidaturaEventoLog");
            }
            eventLog.Source = "IndexacaoCandidatura";
            eventLog.Log = "IndexacaoCandidaturaEventoLog";

            eventLog.WriteEntry("Iniciou InitializeComponent ", EventLogEntryType.Information);
        }

        //public void Iniciar()
        //{
        //    OnStart(null);
        //}

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Iniciou", EventLogEntryType.Information);
            var factory = new ConnectionFactory();
            factory.UserName = _userName;
            factory.Password = _password;
            factory.HostName = _hostName;

            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            channel.QueueDeclare(BLL.Enumeradores.RabbitFila.IndexarCandidatura.ToString(), true, false, false, null);

            //while (true)
            //{
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body;
                        String mensagem = Encoding.UTF8.GetString(body);
                        var objIndexar = JsonConvert.DeserializeObject<DTOCandidaturaRabbit>(mensagem);
                        VagaCandidato.IndexarVagaCandidato(objIndexar.IdVaga, objIndexar.IdCurriculo);
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        eventLog.WriteEntry("Error - indexar a candidatura", EventLogEntryType.Error);
                        EL.GerenciadorException.GravarExcecao(ex, "Indexar a candidatura no solr Rabbit");
                    }

                    // channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(BLL.Enumeradores.RabbitFila.IndexarCandidatura.ToString(), false, consumer);

            //}
            //channel.QueueDeclare(BLL.Enumeradores.RabbitFila.IndexarCandidatura.ToString(), true, false, false, null);

            //channel.BasicQos(0, 1, false); //basic quality of service
            // var consumer = new QueueingBasicConsumer(channel);


            //while (true)
            //{
            //    try
            //    {
            //        if (!channel.IsClosed)
            //        {
            //            BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
            //            String mensagem = Encoding.UTF8.GetString(deliveryArguments.Body);
            //            //Indexar no  Solr
            //            try
            //            {
            //                var objIndexar = JsonConvert.DeserializeObject<CandidaturaRabbit>(mensagem);
            //                VagaCandidato.IndexarVagaCandidato(objIndexar.IdVaga, objIndexar.IdCurriculo);
            //                if(4 <= Convert.ToInt32("ds"))
            //                {

            //                }

            //                channel.BasicAck(deliveryArguments.DeliveryTag, false);
            //            }
            //            catch (Exception)
            //            {
            //                //a mensagem que deu erro retornar para a fila.
            //            }
            //        }
            //        channel.BasicConsume(BLL.Enumeradores.RabbitFila.IndexarCandidatura.ToString(), false, consumer);
            //    }
            //    catch (Exception ex)
            //    {
            //        EL.GerenciadorException.GravarExcecao(ex, "Rabbit service ao tentar abrir conexao.");
            //        Thread.Sleep(800000);
            //        OnStart(null);
            //    }
            //}
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("ON STOP", EventLogEntryType.Information);
        }
    }
}
