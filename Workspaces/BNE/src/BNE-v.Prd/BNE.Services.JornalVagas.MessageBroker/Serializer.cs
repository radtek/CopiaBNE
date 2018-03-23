using System;
using System.Text;
using BNE.Services.JornalVagas.MessageBroker.Contracts;
using Newtonsoft.Json;

namespace BNE.Services.JornalVagas.MessageBroker
{
    public class Serializer : ISerializer
    {
        public const string ContentType = "application/json";

        private readonly Encoding _encoding = Encoding.UTF8;

        private readonly JsonSerializerSettings _settings;

        public Serializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public Serializer()
        {
            _settings = new JsonSerializerSettings();
        }

        public byte[] Serialize<T>(T message)
        {
            object data = message;

            if (data.GetType() == typeof(byte[]))
            {
                return data as byte[];
            }

            var body = JsonConvert.SerializeObject(data, _settings);

            return _encoding.GetBytes(body);
        }

        public object Deserialize(Type dataType, byte[] body)
        {
            var sBody = _encoding.GetString(body);
            return JsonConvert.DeserializeObject(sBody, dataType, _settings);
        }
    }
}