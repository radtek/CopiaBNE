using System;
using System.Messaging;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.Console.Test
{
    class Program
    {
        private const string CaminhoPadraoQueue1 = "FormatName:Direct=OS:EMPVW0114203\\private$\\";
        private const string CaminhoPadraoQueue2 = "FormatName:Direct=TCP:10.114.113.203\\private$\\";
        private const string CaminhoPadraoQueue3 = "FormatName:Direct=TCP:158.85.106.105\\private$\\";
        private const string CaminhoPadraoQueue4 = "FormatName:Direct=HTTP://158.85.106.105/private$/";
        //private const string CaminhoPadraoQueue5 = "FormatName:Direct=HTTP://158.85.106.105/msmq/";

        static void Main(string[] args)
        {
            const string queueName = "bne_gieyson";
            var xmlAtividade = new MensagemAtividade
            {
                IdfAtividade = 12323
            };

            var m = new Message(xmlAtividade);

            var mq1 = new MessageQueue(CaminhoPadraoQueue1 + queueName);
            mq1.Send(m);

            var mq2 = new MessageQueue(CaminhoPadraoQueue2 + queueName);
            mq2.Send(m);

            var mq3 = new MessageQueue(CaminhoPadraoQueue3 + queueName);
            mq3.Send(m);

            var mq4 = new MessageQueue(CaminhoPadraoQueue4 + queueName);
            mq4.Send(m);

            //var mq5 = new MessageQueue(CaminhoPadraoQueue5 + queueName);
            //mq5.Send(m);


            System.Console.ReadKey();
        }

    }
}
