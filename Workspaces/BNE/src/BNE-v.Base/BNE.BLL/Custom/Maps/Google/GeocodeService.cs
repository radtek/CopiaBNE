using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace BNE.BLL.Custom.Maps.Google
{
    internal class Service
    {

        public class Request
        {
            public Address Address { get; set; }
            public bool Sensor { get; set; }
        }

        public class Address
        {
            public string Route { get; set; }
            public string StreetNumber { get; set; }
            public string PostalCode { get; set; }
            public string Locality { get; set; }
            public string StateShortName { get; set; }

            public override string ToString()
            {
                string postalCode = string.Format("{0}-{1}", PostalCode.Substring(0, 5), PostalCode.Substring(5));
                string route = string.Empty;
                string streetNumber = string.Empty;

                if (!string.IsNullOrEmpty(Route))
                    route = string.Concat(Route, ",");

                if (!string.IsNullOrEmpty(StreetNumber))
                    streetNumber = string.Concat(StreetNumber, " - ");

                return string.Format("{0}{1}{2} - {3}, {4}, Brazil", route, streetNumber, Locality, StateShortName, postalCode);
            }
        }

        public class Response
        {
            public results[] Results { get; set; }
            public string Status { get; set; }
        }

        public class results
        {
            public string formatted_address;
            public string FormattedAddress { get { return formatted_address; } }
            public geometry Geometry { get; set; }
            public string[] Types { get; set; }
            public address_component[] address_components;
            public address_component[] AddressComponents { get { return address_components;  } }
        }

        public class geometry
        {
            public string location_type;
            public string LocationType { get { return location_type; } }
            public location Location { get; set; }
        }

        public class location
        {
            public string lat;
            public string lng;
            public double Latitude
            {
                get { return Convert.ToDouble(lat); } 
            }
            public double Longitude
            {
                get { return Convert.ToDouble(lng); }
            }

        }

        public class address_component
        {
            public string long_name { get; set; }
            public string LongName { get { return long_name; } }
            public string short_name { get; set; }
            public string ShortName { get { return short_name; } }
            public string[] Types { get; set; }
        }

        #region GetResponse
        public static Response GetResponse(Request geoCodeRequest)
        {
            var googleMapsAPIURL = Parametro.RecuperaValorParametro(Enumeradores.Parametro.GoogleMapsAPIURL);

            string formattedURL = googleMapsAPIURL.Replace("{fullAddress}", geoCodeRequest.Address.ToString()).Replace("{sensor}", geoCodeRequest.Sensor.ToString(CultureInfo.CurrentCulture).ToLower());
            var webRequest = WebRequest.Create(formattedURL);
            var webResponse = webRequest.GetResponse();
            var sr = new StreamReader(webResponse.GetResponseStream());
            try
            {
                string retorno = sr.ReadToEnd();

                /* Apenas a título de informação */
                string path = AppDomain.CurrentDomain.BaseDirectory + "log\\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (var sw = new StreamWriter(string.Format("{0}{1}.txt", path, geoCodeRequest.Address.ToString().Replace("/", "_").Replace(":", "_"))))
                {
                    sw.Write(retorno);
                }
                /* FIM: Apenas a título de informação */


                return JsonConvert.DeserializeObject<Response>(retorno);
            }
            finally
            {
                sr.Close();
            }
        }
        #endregion

        #region RecuperarCoordenada
        public static results RecuperarCoordenada(string descricaoLogradouro, string numeroEndereco, string numeroCEP, string nomeCidade, string siglaEstado)
        {
            var googleMapsAPIRequest = new Request
            {
                Address = new Address
                {
                    PostalCode = numeroCEP,
                    Route = descricaoLogradouro,
                    StreetNumber = numeroEndereco,
                    Locality = nomeCidade,
                    StateShortName = siglaEstado
                },
                Sensor = false
            };

            var googleMapsAPIResponse = GetResponse(googleMapsAPIRequest);

            var retorno = googleMapsAPIResponse.Results.FirstOrDefault();

            //Se não tem retorno, abre para buscar sem o logradouro e número
            if (retorno == null)
            {
                googleMapsAPIRequest = new Request
                {
                    Address = new Address
                    {
                        PostalCode = numeroCEP,
                        Locality = nomeCidade,
                        StateShortName = siglaEstado
                    },
                    Sensor = false
                };

                googleMapsAPIResponse = GetResponse(googleMapsAPIRequest);

                retorno = googleMapsAPIResponse.Results.FirstOrDefault();
            }

            return retorno;
        }
        #endregion

    }
}
