using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace APIGateway.Model
{
    public class Endpoint
    {
        private string _destinationRelativePath;

        public string RelativePath { get; set; }

        public string DestinationRelativePath
        {
            get
            {
                if (String.IsNullOrEmpty(_destinationRelativePath))
                    return RelativePath;
                return _destinationRelativePath;
            }
            set { _destinationRelativePath = value; }
        }

        public string MethodString
        {
            get { return Method.ToString(); }
            private set { Method = (Method)Enum.Parse(typeof(Method), value, true); }
        }

        public Method Method { get; set; }

        public bool LogSucesso { get; set; }

        public short Id { get; set; }

        public bool LogErro { get; set; }

        public bool LogResponse { get; set; }

        public bool AllowAnonymous { get; set; }

        public Api Api { get; set; }

        public string ApiUrlSuffix { get; set; }

        private Regex reSegments = new Regex(@"[^/]*(\/|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex reParameterName = new Regex(@"(?<=\{).+(?=\})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex reParameterValue = new Regex(@".+(?=(\/|$))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public string ResolveDestination(Uri receivedUri)
        {
            var receivedSegments = new List<string>(receivedUri.Segments);
            // Removendo ApiUrlSuffix
            receivedSegments.RemoveAt(1);

            var endpointSegments = reSegments.Matches(RelativePath);

            var destination = DestinationRelativePath;
            // Percorrendo url procurando parametros
            for (int i = 0; i < endpointSegments.Count; i++)
            {
                var seg = endpointSegments[i];
                if (reParameterName.IsMatch(seg.Value))
                    destination = destination.Replace($"{{{reParameterName.Match(seg.Value).Value}}}", reParameterValue.Match(receivedSegments[i]).Value);
            }

            return destination;
        }
    }
}
