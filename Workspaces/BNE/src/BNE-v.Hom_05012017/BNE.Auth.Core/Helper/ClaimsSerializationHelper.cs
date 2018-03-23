using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BNE.Auth.Core.Helper
{
    public static class ClaimsDefaultSerializerHelper
    {
        public static readonly Func<IEnumerable<LoginPadraoAspNet.SimpleClaim>, string> SerializeClaims = a => JsonConvert.SerializeObject(a);
        public static readonly Func<string, IEnumerable<LoginPadraoAspNet.SimpleClaim>> DeserializeClaims = a => JsonConvert.DeserializeObject<IEnumerable<LoginPadraoAspNet.SimpleClaim>>(a);
    }
}
