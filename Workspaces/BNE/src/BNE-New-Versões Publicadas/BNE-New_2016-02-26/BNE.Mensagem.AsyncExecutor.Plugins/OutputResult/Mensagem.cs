using System;
using System.Collections.Generic;
using BNE.Mensagem.AsyncExecutor.Plugins.Plugins;
using BNE.Services.AsyncServices.Base.Plugins.Interface;

namespace BNE.Mensagem.AsyncExecutor.Plugins.OutputResult
{
    class Mensagem : IPluginResult
    {
        private readonly string _inputPluginName;
        public List<MensagemEmail> ListaEmail { get; set; }

        public string InputPluginName
        {
            get { return _inputPluginName; }
        }

        public bool FinishTask { get; private set; }

        public Mensagem(InputPlugin objInputPlugin, Boolean finishTask)
        {
            _inputPluginName = objInputPlugin.MetadataName;
            FinishTask = finishTask;
        }

        public Mensagem(InputPlugin objInputPlugin, List<MensagemEmail> mensagens, Boolean finishTask)
        {
            _inputPluginName = objInputPlugin.MetadataName;
            ListaEmail = mensagens;
            FinishTask = finishTask;
        }

        public class MensagemEmail
        {
            public int IdMensagem { get; set; }
            public string Descricao { get; set; }
            public string Assunto { get; set; }
            public string From { get; set; }
            public string To { get; set; }
        }

    }
}
