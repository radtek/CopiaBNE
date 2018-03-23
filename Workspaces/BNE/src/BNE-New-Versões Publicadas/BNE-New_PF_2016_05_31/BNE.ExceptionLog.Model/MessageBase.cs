using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BNE.ExceptionLog.Model
{
    public abstract class MessageBase
    {

        [BsonId]
        public ObjectId Id { get; set; }
        public string Aplicacao { get; set; }
        public string Usuario { get; set; }
        public string Message { get; set; }
        public string Payload { get; set; }
        public TipoMensagem TipoMensagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public List<Ocorrencia> Ocorrencias { get; set; }
        public Guid Guid { get; set; }

        protected MessageBase()
        {
            DataCadastro = DateTime.Now;
            Ocorrencias = new List<Ocorrencia>();
            Guid = Guid.NewGuid();
        }

        public class Ocorrencia
        {
            public DateTime DataCadastro { get; set; }
            public string Payload { get; set; }
            public string Usuario { get; set; }
        }

    }
}
