using System;
using System.Collections.Generic;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;

namespace BNE.Services.Plugins.PluginResult
{

    public class MensagemPlugin : IPluginResult
    {

        private readonly string _inputPluginName = String.Empty;
        public List<MensagemSMS> ListaSMS { get; set; }
        public List<MensagemEmail> ListaEmail { get; set; }
        public List<MensagemSMSTanque> ListaSMSTanque { get; set; }

        public MensagemPlugin(InputPlugin objInputPlugin, Boolean finishTask)
        {
            _inputPluginName = objInputPlugin.MetadataName;
            FinishTask = finishTask;
        }

        public MensagemPlugin(InputPlugin objInputPlugin, List<MensagemEmail> mensagens, Boolean finishTask)
        {
            ListaEmail = mensagens;
            _inputPluginName = objInputPlugin.MetadataName;
            FinishTask = finishTask;
        }

        public MensagemPlugin(InputPlugin objInputPlugin, List<MensagemSMS> mensagens, Boolean finishTask)
        {
            ListaSMS = mensagens;
            _inputPluginName = objInputPlugin.MetadataName;
            FinishTask = finishTask;
        }
        public MensagemPlugin(InputPlugin objInputPlugin, List<MensagemSMSTanque> mensagens, Boolean finishTask)
        {
            ListaSMSTanque = mensagens;
            _inputPluginName = objInputPlugin.MetadataName;
            FinishTask = finishTask;
        }

        public string InputPluginName
        {
            get { return _inputPluginName; }
        }

        public bool FinishTask
        {
            get;
            private set;
        }

        public bool FinishWithPonteAzul
        {
            get;
            private set;
        }

        public class MensagemSMS
        {
            public int IdMensagem { get; set; }
            public string Descricao { get; set; }
            public string DDDCelular { get; set; }
            public string NumeroCelular { get; set; }
        }

        public class MensagemSMSTanque : MensagemSMS
        {
            public string IdUsuarioTanque { get; set; } //Usuario Filial Perfil ou sistema bne (Usuário tanque)
            public int IdCurriculo { get; set; }
            public string NomePessoa { get; set; }
        }

        public class MensagemEmail
        {
            public int IdMensagem { get; set; }
            public string Descricao { get; set; }
            public string Assunto { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public Dictionary<string, byte[]> Arquivo { get; set; }
            public Tabela TabelaOrigem { get; set; }

            public MensagemEmail()
            {
                this.TabelaOrigem = Tabela.MensagemCS;
            }

            public enum Tabela
            {
                MensagemCS,
                MensagemMailing
            }
        }

    }
}
