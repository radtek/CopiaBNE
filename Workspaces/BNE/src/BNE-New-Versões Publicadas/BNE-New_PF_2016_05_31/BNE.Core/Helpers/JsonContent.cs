using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Core.Helpers
{
    public class JsonContent : HttpContent
    {
        private readonly MemoryStream _Stream = new MemoryStream();

        public JsonContent(String document)
        {
            StreamWriter writer = new StreamWriter(_Stream);
            writer.Write(document);
            writer.Flush();
            _Stream.Position = 0;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override Task SerializeToStreamAsync(Stream stream, System.Net.TransportContext context)
        {

            _Stream.CopyTo(stream);

            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }
    }
}
