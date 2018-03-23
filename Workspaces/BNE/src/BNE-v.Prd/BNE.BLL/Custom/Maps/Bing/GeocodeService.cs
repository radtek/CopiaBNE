using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BNE.BLL.BINGGeocodeService;

namespace BNE.BLL.Custom.Maps.Bing
{
    internal class Service
    {
        private static readonly string bingMapsAPIKey = Parametro.RecuperaValorParametro(Enumeradores.Parametro.BingMapsAPIKey);

        public class Request
        {
            public Address Address { get; set; }
        }

        public class Address
        {
            public string Route { get; set; }
            public string StreetNumber { get; set; }
            public string PostalCode { get; set; }
            public string Locality { get; set; }
            public string StateShortName { get; set; }
            public string District { get; set; }

            public override string ToString()
            {
                string postalCode = string.Empty;
                if (!string.IsNullOrWhiteSpace(PostalCode))
                    postalCode = string.Format("{0}-{1}", PostalCode.Substring(0, 5), PostalCode.Substring(5));

                var lista = new List<string>();
                if (!string.IsNullOrEmpty(Route))
                    lista.Add(Route);

                if (!string.IsNullOrEmpty(StreetNumber))
                    lista.Add(StreetNumber);

                if (!string.IsNullOrEmpty(District))
                    lista.Add(District);

                if (!string.IsNullOrEmpty(Locality))
                    lista.Add(Locality);

                if (!string.IsNullOrWhiteSpace(postalCode))
                    return string.Format("{0} - {1}, {2}, Brazil", string.Join(" ,", lista), StateShortName, postalCode);

                return string.Format("{0} - {1}, Brazil", string.Join(" - ", lista), StateShortName);
            }
        }

        #region GetResponse
        public static GeocodeResponse GetResponse(Request geoCodeRequest)
        {
            if (string.IsNullOrEmpty(bingMapsAPIKey))
                throw new Exception("A chave do BING não está configurada.");

            GeocodeResponse resposta;
            using (var client = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService"))
            {
                var request = new GeocodeRequest
                {
                    Credentials = new Credentials { ApplicationId = bingMapsAPIKey },
                    Address = new BINGGeocodeService.Address
                    {
                        Locality = geoCodeRequest.Address.Locality,
                        AdminDistrict = geoCodeRequest.Address.StateShortName,
                        CountryRegion = "Brazil"
                    }
                };

                if (!string.IsNullOrWhiteSpace(geoCodeRequest.Address.Route) && !string.IsNullOrWhiteSpace(geoCodeRequest.Address.StreetNumber))
                    request.Address.AddressLine = string.Format("{0}, {1}", geoCodeRequest.Address.Route, geoCodeRequest.Address.StreetNumber);

                if (!string.IsNullOrWhiteSpace(geoCodeRequest.Address.PostalCode))
                    request.Address.PostalCode = geoCodeRequest.Address.PostalCode;

                if (!string.IsNullOrWhiteSpace(geoCodeRequest.Address.District))
                    request.Address.District = geoCodeRequest.Address.District;
                /*
                var request = new GeocodeRequest
                {
                    Credentials = new Credentials { ApplicationId = bingMapsAPIKey },
                    Address = new BINGGeocodeService.Address
                    {
                        AddressLine = geoCodeRequest.Address.ToString()
                    }
                };
                */
                resposta = client.Geocode(request);
            }

            return resposta;
        }
        #endregion

        #region RecuperarCoordenada
        public static GeocodeLocation RecuperarCoordenada(string descricaoLogradouro, string numeroEndereco, string numeroCEP, string nomeBairro, string nomeCidade, string siglaEstado)
        {
            var apiRequest = new Request
            {
                Address = new Address
                {
                    PostalCode = numeroCEP,
                    Route = descricaoLogradouro,
                    StreetNumber = numeroEndereco,
                    Locality = nomeCidade,
                    StateShortName = siglaEstado,
                    District = nomeBairro
                }
            };

            var apiResponse = GetResponse(apiRequest);

            var retorno = apiResponse.Results.FirstOrDefault();

            //Se não tem retorno, abre para buscar sem o número
            if (retorno == null)
            {
                apiRequest = new Request
                {
                    Address = new Address
                    {
                        PostalCode = numeroCEP,
                        Route = descricaoLogradouro,
                        Locality = nomeCidade,
                        StateShortName = siglaEstado
                    }
                };

                apiResponse = GetResponse(apiRequest);

                retorno = apiResponse.Results.FirstOrDefault();
            }

            if (retorno != null)
            {
                var location = retorno.Locations.FirstOrDefault();
                if (location != null)
                    return location;
            }
            return null;
        }
        #endregion

        #region RecuperarCoordenada
        public static GeocodeLocation RecuperarCoordenada(string bairro, string nomeCidade, string siglaEstado)
        {
            var apiRequest = new Request
            {
                Address = new Address
                {
                    //District = bairro,
                    Locality = nomeCidade,
                    StateShortName = siglaEstado,
                }
            };

            var apiResponse = GetResponse(apiRequest);

            var retorno = apiResponse.Results.FirstOrDefault();

            if (retorno != null)
                return retorno.Locations.FirstOrDefault();

            return null;
        }
        #endregion

    }

}
