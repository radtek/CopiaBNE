using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BNE.ExceptionLog.LogServer.Model
{
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class LogInfo
    {
        private LogInfo()
        {
            Ocorrencias = new List<Ocorrencia>();
        }

        public LogInfo(Guid guid, string application, string message, List<Ocorrencia> ocorrencias, LogLevel logLevel = LogLevel.Error)
        {
            Guid = guid;
            Level = logLevel;
            Message = message;
            Application = application;
            Ocorrencias = ocorrencias.OrderByDescending(x => x.IncidentTime).ToList();
        }

        public LogInfo(Guid guid, string application, string exceptionDetails, string message, string customMessage, List<Ocorrencia> ocorrencias, LogLevel logLevel = LogLevel.Error)
        {
            Guid = guid;
            Level = logLevel;
            Message = message;
            Application = application;
            Ocorrencias = ocorrencias.OrderByDescending(x => x.IncidentTime).ToList();

            ExceptionDetails = exceptionDetails;
            CustomMessage = customMessage;
        }

        [DataMember(Name = "Nivel")]
        [Newtonsoft.Json.JsonProperty("Nivel")]
        [System.Xml.Serialization.XmlElement("Nivel")]
        public LogLevel Level { get; set; }

        [DataMember(Name = "Aplicacao")]
        [Newtonsoft.Json.JsonProperty("Aplicacao")]
        [System.Xml.Serialization.XmlElement("Aplicacao")]
        public string Application { get; set; }

        [DataMember(Name = "Mensagem")]
        [Newtonsoft.Json.JsonProperty("Mensagem")]
        [System.Xml.Serialization.XmlElement("Mensagem")]
        public string Message { get; set; }

        [DataMember(Name = "MensagemCustomizada")]
        [Newtonsoft.Json.JsonProperty("MensagemCustomizada")]
        [System.Xml.Serialization.XmlElement("MensagemCustomizada")]
        public string CustomMessage { get; set; }

        [DataMember(Name = "Quantidade")]
        [Newtonsoft.Json.JsonProperty("Quantidade")]
        public int TotalIncidents { get { return Ocorrencias.Count; } }

        [DataMember(Name = "Ocorrencias")]
        [Newtonsoft.Json.JsonProperty("Ocorrencias")]
        public List<Ocorrencia> Ocorrencias { get; set; }

        [DataMember(Name = "UltimoIncidente")]
        [Newtonsoft.Json.JsonProperty("UltimoIncidente")]
        public DateTime LastIncident
        {
            get
            {
                var dataOcorrencia = Ocorrencias.OrderByDescending(x => x.IncidentTime).FirstOrDefault();
                if (dataOcorrencia != null)
                    return dataOcorrencia.IncidentTime;

                return default(DateTime);
            }
        }

        [DataMember(Name = "Guid")]
        [System.Xml.Serialization.XmlElement(ElementName = "Guid")]
        [Newtonsoft.Json.JsonProperty("Guid")]
        public Guid Guid { get; set; }

        [DataMember(Name = "DetalhesExcecao")]
        [Newtonsoft.Json.JsonProperty("DetalhesExcecao")]
        [System.Xml.Serialization.XmlElement(ElementName = "DetalhesExcecao")]
        public string ExceptionDetails { get; set; }
       
        public class Ocorrencia
        {
            [DataMember(Name = "Payload")]
            [Newtonsoft.Json.JsonProperty("Payload")]
            [System.Xml.Serialization.XmlElement("Payload")]
            public virtual string Payload { get; set; }

            [DataMember(Name = "DataIncidente")]
            [Newtonsoft.Json.JsonProperty("DataIncidente")]
            [System.Xml.Serialization.XmlElement(ElementName = "DataIncidente")]
            public DateTime IncidentTime { get; set; }

            [DataMember(Name = "Usuario")]
            [Newtonsoft.Json.JsonProperty("Usuario")]
            [System.Xml.Serialization.XmlElement("Usuario")]
            public virtual string Usuario { get; set; }

        }
    }
}