using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Data;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Owin;

namespace BNE.Auth.NET45
{

    public static class ByteHelper
    {
        public static String ToHexadecimal(this Byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static byte[] ToBytesFromHexadecimal(this string text)
        {
            byte[] bytes = new byte[text.Length * sizeof(char)];
            System.Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

        }
    }
    public class HexEncoder : ITextEncoder
    {
        public String Encode(Byte[] data)
        {
            return data.ToHexadecimal();
        }

        public Byte[] Decode(String text)
        {
            return text.ToBytesFromHexadecimal();
        }
    }
    public class FormsAuthenticationTicketDataProtector : IDataProtector
    {
        public Byte[] Protect(Byte[] userData)
        {
            FormsAuthenticationTicket ticket;
            using (var memoryStream = new MemoryStream(userData))
            {
                var binaryFormatter = new BinaryFormatter();
                ticket = binaryFormatter.Deserialize(memoryStream) as FormsAuthenticationTicket;
            }

            if (ticket == null)
            {
                return null;
            }

            try
            {
                var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                return encryptedTicket.ToBytesFromHexadecimal();
            }
            catch
            {
                return null;
            }
        }

        public Byte[] Unprotect(Byte[] protectedData)
        {
            FormsAuthenticationTicket ticket;
            try
            {
                ticket = FormsAuthentication.Decrypt(protectedData.ToHexadecimal());
            }
            catch
            {
                return null;
            }

            if (ticket == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, ticket);

                return memoryStream.ToArray();
            }
        }
    }

    public static class BNELoginConfigOwin
    {
        public static void Configure(Owin.IAppBuilder appBuilder)
        {
            BNEAutenticacao.DefaultAuthManagerFactory = (f, b) => f != null ? new LoginPadraoComIdentity(f, b) :
                                                                              new LoginPadraoComIdentity(b);

            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = FormsAuthentication.FormsCookieName,
#if DEBUG
                CookieDomain = FormsAuthentication.CookieDomain,
#else
                CookieDomain = null,
#endif
                CookiePath = FormsAuthentication.FormsCookiePath,
                CookieSecure = CookieSecureOption.SameAsRequest,
                AuthenticationMode = AuthenticationMode.Active,
                ExpireTimeSpan = FormsAuthentication.Timeout,
                SlidingExpiration = true,
                AuthenticationType = "Forms",
                TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(
                    new FormsAuthenticationTicketSerializer(),
                    new FormsAuthenticationTicketDataProtector(),
                    new HexEncoder())
            });
        }
    }

    public class FormsAuthenticationTicketSerializer : IDataSerializer<AuthenticationTicket>
    {
        public Byte[] Serialize(AuthenticationTicket model)
        {
            var toSerialize = model.Identity.Claims.Where(a => a.Type != ClaimsIdentity.DefaultNameClaimType)
                                          .Select(c => new BNE.Auth.LoginPadraoAspNet.SimpleClaim { ClaimType = c.Type, Value = c.Value })
                                          .Concat(new[] { new BNE.Auth.LoginPadraoAspNet.SimpleClaim { 
                                                                            ClaimType = "built", 
                                                                               Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm")}});

            var data = Helper.ClaimsDefaultSerializerHelper.SerializeClaims(toSerialize);

            var userTicket = new FormsAuthenticationTicket(
                2,
                model.Identity.Name,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                model.Properties.IsPersistent,
                data,
                FormsAuthentication.FormsCookiePath);

            using (var dataStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(dataStream, userTicket);

                return dataStream.ToArray();
            }
        }

        public AuthenticationTicket Deserialize(Byte[] data)
        {
            using (var dataStream = new MemoryStream(data))
            {
                var binaryFormatter = new BinaryFormatter();
                var ticket = binaryFormatter.Deserialize(dataStream) as FormsAuthenticationTicket;
                if (ticket == null)
                    return null;


                var simpleClaims = BNE.Auth.Helper.ClaimsDefaultSerializerHelper.DeserializeClaims(ticket.UserData);

                var claims = simpleClaims
                                    .Where(a => a.ClaimType != ClaimsIdentity.DefaultNameClaimType)
                                    .Select(c => new System.Security.Claims.Claim(c.ClaimType, c.Value))
                                    .Concat(new[] { new System.Security.Claims.Claim(ClaimTypes.IsPersistent, ticket.IsPersistent.ToString()) });


                var authTicket = new AuthenticationTicket(new ClaimsIdentity(claims, "Forms"), new AuthenticationProperties() { IsPersistent = ticket.IsPersistent });
                authTicket.Properties.IssuedUtc = new DateTimeOffset(ticket.IssueDate);
                authTicket.Properties.ExpiresUtc = new DateTimeOffset(ticket.Expiration);
                authTicket.Properties.IsPersistent = ticket.IsPersistent;

                return authTicket;
            }
        }

        private Claim CreateClaim(String type, String value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}
