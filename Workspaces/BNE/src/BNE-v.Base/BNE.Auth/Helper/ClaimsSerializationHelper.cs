using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Auth.Helper
{
    public static class ClaimsDefaultSerializerHelper
    {
        public static readonly Func<IEnumerable<LoginPadraoAspNet.SimpleClaim>, string> SerializeClaims =
             (a) => JsonConvert.SerializeObject(a);

        public static readonly Func<string, IEnumerable<LoginPadraoAspNet.SimpleClaim>> DeserializeClaims =
            (a) => JsonConvert.DeserializeObject<IEnumerable<BNE.Auth.LoginPadraoAspNet.SimpleClaim>>(a);

    }
}
